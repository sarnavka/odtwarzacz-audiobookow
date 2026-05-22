using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Text.Json;

namespace OdtwarzaczAudiobookow
{
    public partial class Form1 : Form
    {
        // Ustawienia i dane
        private AppSettings appSettings = new();
        private AudioBook? currentAudioBook;
        private readonly string settingsPath;

        // Odtwarzacz główny
        private WaveOutEvent? waveOut;
        private AudioFileReader? audioFileReader;
        private SpeedController? speedController;

        // Podkład muzyczny - osobny odtwarzacz z niezależną głośnością
        private WaveOutEvent? backgroundMusicOut;
        private AudioFileReader? backgroundMusicReader;
        private VolumeSampleProvider? backgroundVolumeProvider;

        // Timery
        private System.Windows.Forms.Timer positionTimer = new();
        private System.Windows.Forms.Timer syncTimer = new();
        private System.Windows.Forms.Timer searchTimer = new();

        // Synchronizacja sieciowa
        private NetworkSyncService? syncService;

        // Zasobnik systemowy
        private NotifyIcon? trayIcon;

        // Flagi stanu
        private bool isUserSeeking = false;
        private bool isClosing = false;
        private bool isSyncingPosition = false; // Zapobiega pętli synchronizacji

        // Predefiniowane dźwięki ambient
        private readonly List<AmbientSound> ambientSounds = new()
        {
            new AmbientSound { Name = "Deszcz", Path = "ambient_rain.wav", IsBuiltIn = true },
            new AmbientSound { Name = "Las", Path = "ambient_forest.wav", IsBuiltIn = true },
            new AmbientSound { Name = "Kominek", Path = "ambient_fire.wav", IsBuiltIn = true },
            new AmbientSound { Name = "Ocean", Path = "ambient_ocean.wav", IsBuiltIn = true },
            new AmbientSound { Name = "Kawiarnia", Path = "ambient_cafe.wav", IsBuiltIn = true }
        };

        public Form1()
        {
            InitializeComponent();

            settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

            InitializeTimers();
            InitializeTrayIcon();
            InitializeAmbientSounds();
            LoadSettings();
            ApplySettings();
            RefreshLibraryList();

            if (appSettings.AutoResume)
            {
                AutoResumeLastBook();
            }
        }

        #region Initialization

        private void InitializeTimers()
        {
            positionTimer = new System.Windows.Forms.Timer { Interval = 100 };
            positionTimer.Tick += PositionTimer_Tick;

            // Zwiększono częstotliwość synchronizacji do 2 sekund dla lepszej responsywności
            syncTimer = new System.Windows.Forms.Timer { Interval = 2000 };
            syncTimer.Tick += SyncTimer_Tick;

            searchTimer = new System.Windows.Forms.Timer { Interval = 300 };
            searchTimer.Tick += SearchTimer_Tick;
        }

        private void InitializeTrayIcon()
        {
            trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Text = "Odtwarzacz Audiobooków",
                Visible = true
            };

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Pokaż okno", null, (s, e) => RestoreWindow());
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("▶ Odtwarzaj/Wstrzymaj", null, (s, e) => TogglePlayPause());
            contextMenu.Items.Add("⏹ Zatrzymaj", null, (s, e) => StopPlayback());
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("⏪ -30s", null, (s, e) => Seek(-appSettings.SeekSeconds));
            contextMenu.Items.Add("⏩ +30s", null, (s, e) => Seek(appSettings.SeekSeconds));
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Zakończ", null, (s, e) => Close());

            trayIcon.ContextMenuStrip = contextMenu;
            trayIcon.DoubleClick += (s, e) => RestoreWindow();
        }

        private void InitializeAmbientSounds()
        {
            cmbAmbientSound.Items.Clear();
            cmbAmbientSound.Items.Add("-- Wybierz dźwięk ambient --");

            // Dodaj predefiniowane
            foreach (var sound in ambientSounds)
            {
                cmbAmbientSound.Items.Add(sound.Name);
            }

            cmbAmbientSound.Items.Add("-- Własny plik... --");
            cmbAmbientSound.SelectedIndex = 0;
        }

        private void RestoreWindow()
        {
            Show();
            WindowState = FormWindowState.Normal;
            BringToFront();
            Activate();
        }

        #endregion

        #region Settings

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(settingsPath))
                {
                    var json = File.ReadAllText(settingsPath);
                    appSettings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd wczytywania ustawień: {ex.Message}", "Błąd",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                appSettings = new AppSettings();
            }
        }

        private void SaveSettings()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(appSettings, options);
                File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd zapisu ustawień: {ex.Message}");
            }
        }

        private void ApplySettings()
        {
            // Prędkość
            trackBarSpeed.Value = Math.Clamp((int)(appSettings.LastSpeed * 100),
                trackBarSpeed.Minimum, trackBarSpeed.Maximum);
            UpdateSpeedDisplay();

            // Głośność główna
            trackBarVolume.Value = (int)(appSettings.MainVolume * 100);
            lblVolumeValue.Text = $"{trackBarVolume.Value}%";

            // Głośność tła
            trackBarBackgroundVolume.Value = (int)(appSettings.BackgroundVolume * 100);
            lblBgVolumeValue.Text = $"{trackBarBackgroundVolume.Value}%";

            // Synchronizacja
            txtServerIP.Text = appSettings.SyncSettings.ServerIP;
            txtPort.Text = appSettings.SyncSettings.Port.ToString();
            chkSyncEnabled.Checked = appSettings.SyncSettings.Enabled;
            chkServerMode.Checked = appSettings.SyncSettings.IsServerMode;

            // Inne
            chkAutoResume.Checked = appSettings.AutoResume;
            chkBackgroundMusic.Checked = appSettings.BackgroundMusicEnabled;
            numSeekSeconds.Value = appSettings.SeekSeconds;

            // Sortowanie
            cmbSortBy.SelectedIndex = appSettings.SortBy switch
            {
                "Author" => 1,
                "LastPlayed" => 2,
                "Duration" => 3,
                _ => 0
            };
            chkSortAscending.Checked = appSettings.SortAscending;

            // Uruchom synchronizację jeśli włączona
            if (appSettings.SyncSettings.Enabled)
            {
                InitializeSync();
            }

            // Uruchom podkład jeśli włączony
            if (appSettings.BackgroundMusicEnabled && !string.IsNullOrEmpty(appSettings.BackgroundMusicPath))
            {
                StartBackgroundMusic(appSettings.BackgroundMusicPath);
            }
        }

        #endregion

        #region Library Management

        private void RefreshLibraryList()
        {
            if (InvokeRequired)
            {
                Invoke(RefreshLibraryList);
                return;
            }

            listViewBooks.BeginUpdate();
            listViewBooks.Items.Clear();

            var sortedBooks = SortBooks(appSettings.AudioBooks);

            foreach (var book in sortedBooks)
            {
                var item = new ListViewItem(book.Title);
                item.SubItems.Add(book.Author);
                item.SubItems.Add(book.DisplayDuration);
                item.SubItems.Add(book.DisplayPosition);
                item.SubItems.Add($"{book.ProgressPercent:F0}%");
                item.Tag = book;

                // Podświetl obecnie odtwarzany
                if (currentAudioBook != null && book.Id == currentAudioBook.Id)
                {
                    item.BackColor = Color.LightBlue;
                }

                listViewBooks.Items.Add(item);
            }

            listViewBooks.EndUpdate();
        }

        private IEnumerable<AudioBook> SortBooks(List<AudioBook> books)
        {
            var sorted = appSettings.SortBy switch
            {
                "Author" => books.OrderBy(b => b.Author),
                "LastPlayed" => books.OrderBy(b => b.LastPlayed),
                "Duration" => books.OrderBy(b => b.Duration),
                _ => books.OrderBy(b => b.Title)
            };

            return appSettings.SortAscending ? sorted : sorted.Reverse();
        }

        private void SearchBooks(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                RefreshLibraryList();
                return;
            }

            searchText = searchText.ToLower();
            var filtered = appSettings.AudioBooks
                .Where(b => b.Title.ToLower().Contains(searchText) ||
                           b.Author.ToLower().Contains(searchText))
                .ToList();

            listViewBooks.BeginUpdate();
            listViewBooks.Items.Clear();

            foreach (var book in SortBooks(filtered))
            {
                var item = new ListViewItem(book.Title);
                item.SubItems.Add(book.Author);
                item.SubItems.Add(book.DisplayDuration);
                item.SubItems.Add(book.DisplayPosition);
                item.SubItems.Add($"{book.ProgressPercent:F0}%");
                item.Tag = book;

                if (currentAudioBook != null && book.Id == currentAudioBook.Id)
                {
                    item.BackColor = Color.LightBlue;
                }

                listViewBooks.Items.Add(item);
            }

            listViewBooks.EndUpdate();
        }

        private void AutoResumeLastBook()
        {
            if (string.IsNullOrEmpty(appSettings.LastPlayedBookId)) return;

            var book = appSettings.AudioBooks.FirstOrDefault(b => b.Id == appSettings.LastPlayedBookId);
            if (book != null && book.LastPosition > 0)
            {
                // Wybierz w liście
                foreach (ListViewItem item in listViewBooks.Items)
                {
                    if ((item.Tag as AudioBook)?.Id == book.Id)
                    {
                        item.Selected = true;
                        item.EnsureVisible();
                        break;
                    }
                }

                // Nie odtwarzaj automatycznie, tylko przygotuj
                PlayAudioBook(book, false);
            }
        }

        #endregion

        #region Playback

        private void PlayAudioBook(AudioBook book, bool autoPlay = true)
        {
            try
            {
                StopPlayback();

                if (!File.Exists(book.FilePath))
                {
                    MessageBox.Show("Plik nie istnieje", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                currentAudioBook = book;

                // Inicjalizacja odtwarzacza
                audioFileReader = new AudioFileReader(book.FilePath);
                speedController = new SpeedController(audioFileReader);
                speedController.PlaybackRate = appSettings.LastSpeed;

                waveOut = new WaveOutEvent();
                waveOut.Init(speedController);
                waveOut.Volume = appSettings.MainVolume;
                waveOut.PlaybackStopped += WaveOut_PlaybackStopped;

                // Ustaw pozycję początkową
                if (book.LastPosition > 0 && book.LastPosition < audioFileReader.TotalTime.TotalSeconds)
                {
                    audioFileReader.CurrentTime = TimeSpan.FromSeconds(book.LastPosition);
                }

                // Sprawdź synchronizację przed odtwarzaniem
                if (syncService != null && !syncService.IsServerMode && currentAudioBook != null)
                {
                    var serverPosition = syncService.RequestPosition(currentAudioBook.Id);
                    if (serverPosition.HasValue && Math.Abs(serverPosition.Value - book.LastPosition) > 5)
                    {
                        var result = MessageBox.Show(
                            $"Serwer ma inną pozycję dla tego audiobooka:\n" +
                            $"Lokalna: {TimeSpan.FromSeconds(book.LastPosition):hh\\:mm\\:ss}\n" +
                            $"Serwer: {TimeSpan.FromSeconds(serverPosition.Value):hh\\:mm\\:ss}\n\n" +
                            $"Czy chcesz użyć pozycji z serwera?",
                            "Synchronizacja pozycji",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            audioFileReader.CurrentTime = TimeSpan.FromSeconds(serverPosition.Value);
                            book.LastPosition = serverPosition.Value;
                        }
                    }
                }

                if (autoPlay)
                {
                    waveOut.Play();
                    positionTimer.Start();
                    btnPlay.Text = "⏸";
                }
                else
                {
                    btnPlay.Text = "▶";
                }

                lblNowPlaying.Text = $"{book.Title} - {book.Author}";
                trackBarPosition.Maximum = Math.Max(1, (int)audioFileReader.TotalTime.TotalSeconds);
                UpdatePositionDisplay();

                // Pokaż okładkę
                ShowCoverImage(book);

                // Załaduj rozdziały
                LoadChapters(book);

                // Tray notification
                trayIcon?.ShowBalloonTip(1000, "Odtwarzacz Audiobooków",
                    $"Odtwarzanie: {book.Title}", ToolTipIcon.Info);

                RefreshLibraryList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd odtwarzania: {ex.Message}", "Błąd",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                StopPlayback();
            }
        }

        private void ShowCoverImage(AudioBook book)
        {
            try
            {
                if (!string.IsNullOrEmpty(book.CoverImagePath) && File.Exists(book.CoverImagePath))
                {
                    pictureBoxCover.Image?.Dispose();
                    pictureBoxCover.Image = Image.FromFile(book.CoverImagePath);
                }
                else
                {
                    pictureBoxCover.Image = null;
                }
            }
            catch
            {
                pictureBoxCover.Image = null;
            }
        }

        private void LoadChapters(AudioBook book)
        {
            if (InvokeRequired)
            {
                Invoke(() => LoadChapters(book));
                return;
            }

            listViewChapters.Items.Clear();

            foreach (var chapter in book.Chapters)
            {
                var item = new ListViewItem(chapter.Title);
                item.SubItems.Add(chapter.DisplayTime);
                item.Tag = chapter;
                listViewChapters.Items.Add(item);
            }
        }

        private void StopPlayback()
        {
            positionTimer.Stop();

            SaveCurrentPosition();

            if (waveOut != null)
            {
                waveOut.PlaybackStopped -= WaveOut_PlaybackStopped;
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }

            if (speedController != null)
            {
                speedController.Dispose();
                speedController = null;
            }

            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
            }

            currentAudioBook = null;
            btnPlay.Text = "▶";
            lblNowPlaying.Text = "Brak odtwarzania";
            pictureBoxCover.Image = null;
            listViewChapters.Items.Clear();

            RefreshLibraryList();
        }

        private void TogglePlayPause()
        {
            if (waveOut == null)
            {
                // Rozpocznij odtwarzanie wybranego
                if (listViewBooks.SelectedItems.Count > 0)
                {
                    var book = listViewBooks.SelectedItems[0].Tag as AudioBook;
                    if (book != null) PlayAudioBook(book);
                }
                return;
            }

            if (waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Pause();
                positionTimer.Stop();
                btnPlay.Text = "▶";
            }
            else
            {
                waveOut.Play();
                positionTimer.Start();
                btnPlay.Text = "⏸";
            }
        }

        private void Seek(int seconds)
        {
            if (audioFileReader == null) return;

            var newPosition = audioFileReader.CurrentTime.TotalSeconds + seconds;
            newPosition = Math.Clamp(newPosition, 0, audioFileReader.TotalTime.TotalSeconds);
            audioFileReader.CurrentTime = TimeSpan.FromSeconds(newPosition);
            UpdatePositionDisplay();

            // Natychmiast wyślij nową pozycję
            SaveCurrentPosition();
        }

        private void SeekToPosition(double seconds)
        {
            if (audioFileReader == null) return;

            seconds = Math.Clamp(seconds, 0, audioFileReader.TotalTime.TotalSeconds);
            audioFileReader.CurrentTime = TimeSpan.FromSeconds(seconds);
            UpdatePositionDisplay();

            // Natychmiast wyślij nową pozycję
            SaveCurrentPosition();
        }

        private void WaveOut_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            if (isClosing) return;

            if (InvokeRequired)
            {
                Invoke(() => WaveOut_PlaybackStopped(sender, e));
                return;
            }

            // Sprawdź czy to koniec pliku
            if (audioFileReader != null &&
                audioFileReader.CurrentTime >= audioFileReader.TotalTime - TimeSpan.FromSeconds(1))
            {
                // Reset pozycji na początek
                if (currentAudioBook != null)
                {
                    currentAudioBook.LastPosition = 0;
                    SaveSettings();
                }
            }

            StopPlayback();
        }

        #endregion

        #region Position & Display Updates

        private void UpdatePositionDisplay()
        {
            if (audioFileReader == null) return;

            if (InvokeRequired)
            {
                Invoke(UpdatePositionDisplay);
                return;
            }

            try
            {
                int totalSeconds = (int)audioFileReader.TotalTime.TotalSeconds;
                int currentSeconds = (int)audioFileReader.CurrentTime.TotalSeconds;

                if (trackBarPosition.Maximum != totalSeconds)
                {
                    trackBarPosition.Maximum = Math.Max(1, totalSeconds);
                }

                if (!isUserSeeking)
                {
                    trackBarPosition.Value = Math.Min(currentSeconds, trackBarPosition.Maximum);
                }

                lblCurrentTime.Text = audioFileReader.CurrentTime.ToString(@"hh\:mm\:ss");
                lblTotalTime.Text = audioFileReader.TotalTime.ToString(@"hh\:mm\:ss");
            }
            catch { }
        }

        private void UpdateSpeedDisplay()
        {
            float speed = trackBarSpeed.Value / 100f;
            lblSpeed.Text = $"{speed:F2}x";

            if (speedController != null)
            {
                speedController.PlaybackRate = speed;
            }

            appSettings.LastSpeed = speed;
        }

        private void SaveCurrentPosition()
        {
            if (currentAudioBook == null || audioFileReader == null || isSyncingPosition) return;

            currentAudioBook.LastPosition = audioFileReader.CurrentTime.TotalSeconds;
            currentAudioBook.LastPlayed = DateTime.Now;
            appSettings.LastPlayedBookId = currentAudioBook.Id;

            // Wyślij do serwera/klienta
            if (syncService != null && syncService.IsConnected)
            {
                if (syncService.IsServerMode)
                {
                    syncService.UpdateLocalPosition(currentAudioBook.Id, currentAudioBook.LastPosition);
                }
                else
                {
                    syncService.SendPosition(currentAudioBook.Id, currentAudioBook.LastPosition);
                }
            }

            SaveSettings();
        }

        #endregion

        #region Background Music

        private void StartBackgroundMusic(string? path = null)
        {
            StopBackgroundMusic();

            try
            {
                path ??= appSettings.BackgroundMusicPath;

                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    // Sprawdź folder ambient
                    string ambientDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ambient");
                    if (Directory.Exists(ambientDir))
                    {
                        var files = Directory.GetFiles(ambientDir, "*.wav")
                            .Concat(Directory.GetFiles(ambientDir, "*.mp3"))
                            .ToArray();
                        if (files.Length > 0)
                        {
                            path = files[0];
                        }
                    }

                    // Użyj domyślnych dźwięków Windows
                    if (string.IsNullOrEmpty(path) || !File.Exists(path))
                    {
                        string[] defaultPaths = {
                            @"C:\Windows\Media\Ring05.wav",
                            @"C:\Windows\Media\chimes.wav",
                            @"C:\Windows\Media\notify.wav"
                        };

                        foreach (var defaultPath in defaultPaths)
                        {
                            if (File.Exists(defaultPath))
                            {
                                path = defaultPath;
                                break;
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    MessageBox.Show("Nie znaleziono pliku muzyki tła", "Informacja",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Utwórz zapętlony reader
                var loopingProvider = new LoopingSampleProvider(path);
                backgroundVolumeProvider = new VolumeSampleProvider(loopingProvider);
                backgroundVolumeProvider.Volume = appSettings.BackgroundVolume;

                backgroundMusicOut = new WaveOutEvent();
                backgroundMusicOut.Init(backgroundVolumeProvider);
                backgroundMusicOut.Play();

                appSettings.BackgroundMusicPath = path;
                lblCurrentBgMusic.Text = Path.GetFileName(path);
                SaveSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd odtwarzania muzyki tła: {ex.Message}", "Błąd",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void StopBackgroundMusic()
        {
            if (backgroundMusicOut != null)
            {
                backgroundMusicOut.Stop();
                backgroundMusicOut.Dispose();
                backgroundMusicOut = null;
            }

            if (backgroundMusicReader != null)
            {
                backgroundMusicReader.Dispose();
                backgroundMusicReader = null;
            }

            backgroundVolumeProvider = null;
            lblCurrentBgMusic.Text = "Brak";
        }

        private void UpdateBackgroundVolume()
        {
            appSettings.BackgroundVolume = trackBarBackgroundVolume.Value / 100f;
            lblBgVolumeValue.Text = $"{trackBarBackgroundVolume.Value}%";

            // Ustaw głośność przez VolumeSampleProvider - NIEZALEŻNIE od głównej głośności
            if (backgroundVolumeProvider != null)
            {
                backgroundVolumeProvider.Volume = appSettings.BackgroundVolume;
            }

            SaveSettings();
        }

        #endregion

        #region Network Sync

        private void InitializeSync()
        {
            syncService?.Dispose();
            syncService = new NetworkSyncService();

            // Obsługa statusu połączenia
            syncService.StatusChanged += (s, status) =>
            {
                if (InvokeRequired)
                    Invoke(() => lblSyncStatus.Text = status);
                else
                    lblSyncStatus.Text = status;
            };

            // KLUCZOWE: Obsługa otrzymanych pozycji z serwera
            syncService.PositionReceived += SyncService_PositionReceived;

            if (appSettings.SyncSettings.IsServerMode)
            {
                syncService.StartServer(appSettings.SyncSettings.Port);
            }
            else
            {
                syncService.ConnectAsClient(appSettings.SyncSettings.ServerIP, appSettings.SyncSettings.Port);
            }

            syncTimer.Start();
        }

        private void SyncService_PositionReceived(object? sender, SyncData syncData)
        {
            if (currentAudioBook == null || syncData.BookId != currentAudioBook.Id || isSyncingPosition)
                return;

            // Sprawdź czy otrzymana pozycja jest znacząco różna (różnica > 3 sekundy)
            var currentPos = audioFileReader?.CurrentTime.TotalSeconds ?? 0;
            var timeDiff = Math.Abs(syncData.Position - currentPos);

            if (timeDiff > 3.0 && audioFileReader != null)
            {
                if (InvokeRequired)
                {
                    Invoke(() => ApplySyncedPosition(syncData.Position));
                }
                else
                {
                    ApplySyncedPosition(syncData.Position);
                }
            }
        }

        private void ApplySyncedPosition(double position)
        {
            if (audioFileReader == null || currentAudioBook == null) return;

            try
            {
                isSyncingPosition = true; // Zapobiega pętli synchronizacji

                audioFileReader.CurrentTime = TimeSpan.FromSeconds(position);
                currentAudioBook.LastPosition = position;
                UpdatePositionDisplay();

                System.Diagnostics.Debug.WriteLine($"Zsynchronizowano pozycję: {position:F1}s");
            }
            finally
            {
                isSyncingPosition = false;
            }
        }

        private void StopSync()
        {
            syncTimer.Stop();
            syncService?.Stop();
            syncService?.Dispose();
            syncService = null;
            lblSyncStatus.Text = "Synchronizacja wyłączona";
        }

        #endregion

        #region Timer Events

        private void PositionTimer_Tick(object? sender, EventArgs e)
        {
            if (audioFileReader == null || waveOut == null) return;

            if (waveOut.PlaybackState == PlaybackState.Playing)
            {
                UpdatePositionDisplay();
            }
        }

        private void SyncTimer_Tick(object? sender, EventArgs e)
        {
            if (syncService == null || !syncService.IsConnected)
                return;

            // Klient: pobierz pozycję z serwera
            if (!syncService.IsServerMode && currentAudioBook != null)
            {
                try
                {
                    var serverPosition = syncService.RequestPosition(currentAudioBook.Id);
                    if (serverPosition.HasValue)
                    {
                        var currentPos = audioFileReader?.CurrentTime.TotalSeconds ?? currentAudioBook.LastPosition;
                        var timeDiff = Math.Abs(serverPosition.Value - currentPos);

                        // Jeśli różnica > 5 sekund, zaktualizuj
                        if (timeDiff > 5.0 && audioFileReader != null)
                        {
                            ApplySyncedPosition(serverPosition.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Błąd synchronizacji: {ex.Message}");
                }
            }

            // Zapisz/wyślij aktualną pozycję
            if (currentAudioBook != null && audioFileReader != null && waveOut?.PlaybackState == PlaybackState.Playing)
            {
                SaveCurrentPosition();
            }
        }

        private void SearchTimer_Tick(object? sender, EventArgs e)
        {
            searchTimer.Stop();
            SearchBooks(txtSearch.Text);
        }

        #endregion

        #region Event Handlers - Import

        private void btnImport_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "Pliki audio|*.mp3;*.m4a;*.m4b;*.wav;*.flac;*.ogg;*.wma|Wszystkie pliki|*.*",
                Multiselect = true,
                Title = "Wybierz pliki audio"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            int added = 0;
            foreach (var filePath in dialog.FileNames)
            {
                if (appSettings.AudioBooks.Any(b => b.FilePath == filePath))
                    continue;

                var book = MetadataService.ExtractMetadata(filePath);
                appSettings.AudioBooks.Add(book);
                added++;
            }

            SaveSettings();
            RefreshLibraryList();

            if (added > 0)
            {
                MessageBox.Show($"Dodano {added} audiobooków", "Import zakończony",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnImportFolder_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "Wybierz folder z audiobookami"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            string[] extensions = { ".mp3", ".m4a", ".m4b", ".wav", ".flac", ".ogg", ".wma" };
            var files = Directory.GetFiles(dialog.SelectedPath, "*.*", SearchOption.AllDirectories)
                .Where(f => extensions.Contains(Path.GetExtension(f).ToLower()))
                .ToArray();

            int added = 0;
            foreach (var filePath in files)
            {
                if (appSettings.AudioBooks.Any(b => b.FilePath == filePath))
                    continue;

                var book = MetadataService.ExtractMetadata(filePath);
                appSettings.AudioBooks.Add(book);
                added++;
            }

            SaveSettings();
            RefreshLibraryList();

            MessageBox.Show($"Dodano {added} audiobooków z {files.Length} plików", "Import zakończony",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Event Handlers - Playback Controls

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (waveOut == null)
            {
                if (listViewBooks.SelectedItems.Count > 0)
                {
                    var book = listViewBooks.SelectedItems[0].Tag as AudioBook;
                    if (book != null) PlayAudioBook(book);
                }
                else
                {
                    MessageBox.Show("Wybierz audiobook z listy", "Informacja",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                TogglePlayPause();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopPlayback();
            trackBarPosition.Value = 0;
            lblCurrentTime.Text = "00:00:00";
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            Seek(-appSettings.SeekSeconds);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Seek(appSettings.SeekSeconds);
        }

        private void listViewBooks_DoubleClick(object sender, EventArgs e)
        {
            if (listViewBooks.SelectedItems.Count > 0)
            {
                var book = listViewBooks.SelectedItems[0].Tag as AudioBook;
                if (book != null) PlayAudioBook(book);
            }
        }

        private void listViewChapters_DoubleClick(object sender, EventArgs e)
        {
            if (listViewChapters.SelectedItems.Count == 0 || audioFileReader == null)
                return;

            var chapter = listViewChapters.SelectedItems[0].Tag as Chapter;
            if (chapter != null)
            {
                SeekToPosition(chapter.StartTime);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listViewBooks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Wybierz audiobook do edycji", "Informacja",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var book = listViewBooks.SelectedItems[0].Tag as AudioBook;
            if (book == null) return;

            using var editForm = new EditBookForm(book);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                if (editForm.SaveToFile)
                {
                    MetadataService.UpdateMetadata(book);
                }

                SaveSettings();
                RefreshLibraryList();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewBooks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Wybierz audiobook do usunięcia", "Informacja",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var book = listViewBooks.SelectedItems[0].Tag as AudioBook;
            if (book == null) return;

            var result = MessageBox.Show(
                $"Czy na pewno usunąć '{book.Title}' z biblioteki?\n\n(Plik nie zostanie usunięty z dysku)",
                "Potwierdzenie",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (currentAudioBook?.Id == book.Id)
                {
                    StopPlayback();
                }

                appSettings.AudioBooks.Remove(book);
                SaveSettings();
                RefreshLibraryList();
            }
        }

        private void trackBarPosition_MouseDown(object sender, MouseEventArgs e)
        {
            isUserSeeking = true;
        }

        private void trackBarPosition_Scroll(object sender, EventArgs e)
        {
            // Opcjonalnie: możesz dodać kod do aktualizacji pozycji podczas przeciągania
            // Ale zazwyczaj aktualizujemy dopiero w MouseUp
        }

        private void trackBarPosition_MouseUp(object sender, MouseEventArgs e)
        {
            isUserSeeking = false;

            if (audioFileReader != null)
            {
                SeekToPosition(trackBarPosition.Value);
            }
        }

        private void trackBarSpeed_Scroll(object sender, EventArgs e)
        {
            UpdateSpeedDisplay();
            SaveSettings();
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            appSettings.MainVolume = trackBarVolume.Value / 100f;
            lblVolumeValue.Text = $"{trackBarVolume.Value}%";

            if (waveOut != null)
            {
                waveOut.Volume = appSettings.MainVolume;
            }

            SaveSettings();
        }

        private void trackBarBackgroundVolume_Scroll(object sender, EventArgs e)
        {
            UpdateBackgroundVolume();
        }

        #endregion

        #region Event Handlers - Search & Sort

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void cmbSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            appSettings.SortBy = cmbSortBy.SelectedIndex switch
            {
                1 => "Author",
                2 => "LastPlayed",
                3 => "Duration",
                _ => "Title"
            };
            SaveSettings();
            RefreshLibraryList();
        }

        private void chkSortAscending_CheckedChanged(object sender, EventArgs e)
        {
            appSettings.SortAscending = chkSortAscending.Checked;
            SaveSettings();
            RefreshLibraryList();
        }

        #endregion

        #region Event Handlers - Sync

        private void chkSyncEnabled_CheckedChanged(object sender, EventArgs e)
        {
            appSettings.SyncSettings.Enabled = chkSyncEnabled.Checked;
            SaveSettings();

            if (chkSyncEnabled.Checked)
                InitializeSync();
            else
                StopSync();
        }

        private void chkServerMode_CheckedChanged(object sender, EventArgs e)
        {
            appSettings.SyncSettings.IsServerMode = chkServerMode.Checked;
            SaveSettings();

            if (appSettings.SyncSettings.Enabled)
                InitializeSync();
        }

        private void btnApplySync_Click(object sender, EventArgs e)
        {
            appSettings.SyncSettings.ServerIP = txtServerIP.Text;
            if (int.TryParse(txtPort.Text, out int port))
                appSettings.SyncSettings.Port = port;

            SaveSettings();

            if (appSettings.SyncSettings.Enabled)
                InitializeSync();
        }

        #endregion

        #region Event Handlers - Background Music

        private void chkBackgroundMusic_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBackgroundMusic.Checked)
                StartBackgroundMusic();
            else
                StopBackgroundMusic();

            appSettings.BackgroundMusicEnabled = chkBackgroundMusic.Checked;
            SaveSettings();
        }

        private void btnSelectMusic_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "Pliki audio|*.mp3;*.wav;*.m4a;*.ogg|Wszystkie pliki|*.*",
                Title = "Wybierz plik muzyki tła"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            StartBackgroundMusic(dialog.FileName);
            chkBackgroundMusic.Checked = true;
        }

        private void cmbAmbientSound_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAmbientSound.SelectedIndex <= 0) return;

            // Własny plik
            if (cmbAmbientSound.SelectedIndex == cmbAmbientSound.Items.Count - 1)
            {
                btnSelectMusic_Click(sender, e);
                cmbAmbientSound.SelectedIndex = 0;
                return;
            }

            // Predefiniowany dźwięk
            int soundIndex = cmbAmbientSound.SelectedIndex - 1;
            if (soundIndex >= 0 && soundIndex < ambientSounds.Count)
            {
                var sound = ambientSounds[soundIndex];
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ambient", sound.Path);

                if (File.Exists(path))
                {
                    StartBackgroundMusic(path);
                    chkBackgroundMusic.Checked = true;
                }
                else
                {
                    MessageBox.Show($"Plik dźwięku ambient nie istnieje: {path}\n\nUtwórz folder 'ambient' i dodaj pliki .wav",
                        "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            cmbAmbientSound.SelectedIndex = 0;
        }

        #endregion

        #region Event Handlers - Settings

        private void chkAutoResume_CheckedChanged(object sender, EventArgs e)
        {
            appSettings.AutoResume = chkAutoResume.Checked;
            SaveSettings();
        }

        private void numSeekSeconds_ValueChanged(object sender, EventArgs e)
        {
            appSettings.SeekSeconds = (int)numSeekSeconds.Value;
            SaveSettings();
        }

        #endregion

        #region Event Handlers - Form

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClosing = true;

            positionTimer.Stop();
            syncTimer.Stop();
            searchTimer.Stop();

            SaveCurrentPosition();
            SaveSettings();

            StopPlayback();
            StopBackgroundMusic();
            StopSync();

            trayIcon?.Dispose();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Space:
                    TogglePlayPause();
                    return true;

                case Keys.Escape:
                    StopPlayback();
                    return true;

                case Keys.Left:
                    Seek(-appSettings.SeekSeconds);
                    return true;

                case Keys.Right:
                    Seek(appSettings.SeekSeconds);
                    return true;

                case Keys.Up:
                    if (trackBarSpeed.Value < trackBarSpeed.Maximum)
                    {
                        trackBarSpeed.Value = Math.Min(trackBarSpeed.Value + 5, trackBarSpeed.Maximum);
                        UpdateSpeedDisplay();
                        SaveSettings();
                    }
                    return true;

                case Keys.Down:
                    if (trackBarSpeed.Value > trackBarSpeed.Minimum)
                    {
                        trackBarSpeed.Value = Math.Max(trackBarSpeed.Value - 5, trackBarSpeed.Minimum);
                        UpdateSpeedDisplay();
                        SaveSettings();
                    }
                    return true;

                case Keys.M:
                    chkBackgroundMusic.Checked = !chkBackgroundMusic.Checked;
                    return true;

                case Keys.Control | Keys.Up:
                    if (trackBarVolume.Value < trackBarVolume.Maximum)
                    {
                        trackBarVolume.Value = Math.Min(trackBarVolume.Value + 5, trackBarVolume.Maximum);
                        trackBarVolume_Scroll(this, EventArgs.Empty);
                    }
                    return true;

                case Keys.Control | Keys.Down:
                    if (trackBarVolume.Value > trackBarVolume.Minimum)
                    {
                        trackBarVolume.Value = Math.Max(trackBarVolume.Value - 5, trackBarVolume.Minimum);
                        trackBarVolume_Scroll(this, EventArgs.Empty);
                    }
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}