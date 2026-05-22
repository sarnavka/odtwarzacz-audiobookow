using NetMQ;
using NetMQ.Sockets;
using System.Text.Json;

namespace OdtwarzaczAudiobookow
{
    /// <summary>
    /// Serwis do synchronizacji pozycji odtwarzania między urządzeniami w sieci LAN.
    /// Wykorzystuje NetMQ (implementacja ZeroMQ dla .NET) do komunikacji TCP.
    /// </summary>
    public class NetworkSyncService : IDisposable
    {
        private ResponseSocket? serverSocket;
        private RequestSocket? clientSocket;
        private Thread? serverThread;
        private readonly object syncLock = new();
        private bool isRunning;
        private bool disposed;

        // Przechowywanie pozycji dla serwera
        private readonly Dictionary<string, SyncPosition> positions = new();

        public event EventHandler<string>? StatusChanged;
        public event EventHandler<SyncData>? PositionReceived;

        public bool IsServerMode { get; private set; }
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Uruchamia serwer synchronizacji na podanym porcie.
        /// </summary>
        public void StartServer(int port)
        {
            Stop();

            try
            {
                isRunning = true;
                IsServerMode = true;

                serverThread = new Thread(() => RunServer(port))
                {
                    IsBackground = true,
                    Name = "SyncServerThread"
                };
                serverThread.Start();

                StatusChanged?.Invoke(this, $"Serwer uruchomiony na porcie {port}");
                IsConnected = true;
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Błąd serwera: {ex.Message}");
                IsConnected = false;
            }
        }

        private void RunServer(int port)
        {
            try
            {
                using var socket = new ResponseSocket();
                serverSocket = socket;
                socket.Bind($"tcp://*:{port}");

                while (isRunning)
                {
                    try
                    {
                        if (socket.TryReceiveFrameString(TimeSpan.FromMilliseconds(100), out string? message))
                        {
                            if (!string.IsNullOrEmpty(message))
                            {
                                var response = ProcessServerMessage(message);
                                socket.SendFrame(response);
                            }
                        }
                    }
                    catch (TerminatingException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Błąd serwera: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Błąd serwera: {ex.Message}");
            }
        }

        private string ProcessServerMessage(string message)
        {
            try
            {
                var syncData = JsonSerializer.Deserialize<SyncData>(message);
                if (syncData == null)
                {
                    return JsonSerializer.Serialize(new SyncData { Success = false });
                }

                switch (syncData.MessageType)
                {
                    case "UPDATE":
                        // Klient wysyła swoją pozycję
                        lock (syncLock)
                        {
                            positions[syncData.BookId] = new SyncPosition
                            {
                                Position = syncData.Position,
                                Timestamp = syncData.Timestamp
                            };
                        }

                        // Informuj UI o otrzymanej pozycji
                        PositionReceived?.Invoke(this, syncData);

                        return JsonSerializer.Serialize(new SyncData
                        {
                            MessageType = "RESPONSE",
                            BookId = syncData.BookId,
                            Position = syncData.Position,
                            Success = true
                        });

                    case "REQUEST":
                        // Klient pyta o pozycję dla konkretnej książki
                        lock (syncLock)
                        {
                            if (positions.TryGetValue(syncData.BookId, out var pos))
                            {
                                return JsonSerializer.Serialize(new SyncData
                                {
                                    MessageType = "RESPONSE",
                                    BookId = syncData.BookId,
                                    Position = pos.Position,
                                    Timestamp = pos.Timestamp,
                                    Success = true
                                });
                            }
                        }

                        return JsonSerializer.Serialize(new SyncData
                        {
                            MessageType = "RESPONSE",
                            BookId = syncData.BookId,
                            Position = 0,
                            Success = false
                        });

                    default:
                        return JsonSerializer.Serialize(new SyncData { Success = false });
                }
            }
            catch
            {
                return JsonSerializer.Serialize(new SyncData { Success = false });
            }
        }

        /// <summary>
        /// Łączy się z serwerem jako klient.
        /// </summary>
        public void ConnectAsClient(string serverIP, int port)
        {
            Stop();

            try
            {
                IsServerMode = false;
                clientSocket = new RequestSocket();
                clientSocket.Options.Linger = TimeSpan.Zero;
                clientSocket.Connect($"tcp://{serverIP}:{port}");

                isRunning = true;
                IsConnected = true;
                StatusChanged?.Invoke(this, $"Połączono z {serverIP}:{port}");
            }
            catch (Exception ex)
            {
                IsConnected = false;
                StatusChanged?.Invoke(this, $"Błąd połączenia: {ex.Message}");
            }
        }

        /// <summary>
        /// Wysyła aktualną pozycję do serwera (tryb klienta).
        /// </summary>
        public bool SendPosition(string bookId, double position)
        {
            if (IsServerMode || clientSocket == null || !isRunning)
                return false;

            try
            {
                var syncData = new SyncData
                {
                    MessageType = "UPDATE",
                    BookId = bookId,
                    Position = position,
                    Timestamp = DateTime.Now
                };

                clientSocket.TrySendFrame(TimeSpan.FromMilliseconds(500), JsonSerializer.Serialize(syncData));

                if (clientSocket.TryReceiveFrameString(TimeSpan.FromMilliseconds(500), out string? response))
                {
                    var result = JsonSerializer.Deserialize<SyncData>(response);
                    return result?.Success ?? false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd wysyłania pozycji: {ex.Message}");
            }

            return false;
        }

        /// <summary>
        /// Pobiera pozycję z serwera dla danej książki (tryb klienta).
        /// </summary>
        public double? RequestPosition(string bookId)
        {
            if (IsServerMode || clientSocket == null || !isRunning)
                return null;

            try
            {
                var request = new SyncData
                {
                    MessageType = "REQUEST",
                    BookId = bookId
                };

                clientSocket.TrySendFrame(TimeSpan.FromMilliseconds(500), JsonSerializer.Serialize(request));

                if (clientSocket.TryReceiveFrameString(TimeSpan.FromMilliseconds(500), out string? response))
                {
                    var result = JsonSerializer.Deserialize<SyncData>(response);
                    if (result?.Success == true)
                    {
                        return result.Position;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd pobierania pozycji: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Aktualizuje pozycję lokalnie na serwerze.
        /// </summary>
        public void UpdateLocalPosition(string bookId, double position)
        {
            if (!IsServerMode) return;

            lock (syncLock)
            {
                positions[bookId] = new SyncPosition
                {
                    Position = position,
                    Timestamp = DateTime.Now
                };
            }
        }

        /// <summary>
        /// Zatrzymuje synchronizację.
        /// </summary>
        public void Stop()
        {
            isRunning = false;
            IsConnected = false;

            try
            {
                if (clientSocket != null)
                {
                    clientSocket.Close();
                    clientSocket.Dispose();
                    clientSocket = null;
                }

                if (serverSocket != null)
                {
                    serverSocket.Close();
                    serverSocket.Dispose();
                    serverSocket = null;
                }

                if (serverThread != null && serverThread.IsAlive)
                {
                    serverThread.Join(1000);
                    serverThread = null;
                }

                StatusChanged?.Invoke(this, "Synchronizacja zatrzymana");
            }
            catch { }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                Stop();
                disposed = true;
            }
        }
    }
}