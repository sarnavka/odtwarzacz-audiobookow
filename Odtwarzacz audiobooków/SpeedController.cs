using NAudio.Wave;
using SoundTouch;

namespace OdtwarzaczAudiobookow
{
    /// <summary>
    /// Kontroler prędkości audio z zachowaniem oryginalnego tonu głosu.
    /// Używa biblioteki SoundTouch.
    /// </summary>
    public class SpeedController : ISampleProvider, IDisposable
    {
        private readonly ISampleProvider source;
        private readonly SoundTouchProcessor soundTouch;
        private readonly float[] sourceBuffer;
        private readonly float[] tempBuffer;
        private readonly Queue<float> outputQueue;
        private float _playbackRate = 1.0f;
        private bool disposed;
        private readonly int channels;

        private const int BufferSize = 4096;

        public WaveFormat WaveFormat => source.WaveFormat;

        public float PlaybackRate
        {
            get => _playbackRate;
            set
            {
                _playbackRate = Math.Clamp(value, 0.5f, 3.0f);
                soundTouch.Tempo = _playbackRate;
            }
        }

        public SpeedController(ISampleProvider source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.channels = source.WaveFormat.Channels;

            soundTouch = new SoundTouchProcessor();
            soundTouch.SampleRate = source.WaveFormat.SampleRate;
            soundTouch.Channels = channels;
            soundTouch.Tempo = 1.0f;
            soundTouch.Pitch = 1.0f;
            soundTouch.Rate = 1.0f;

            sourceBuffer = new float[BufferSize];
            tempBuffer = new float[BufferSize * 4];
            outputQueue = new Queue<float>();
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesWritten = 0;

            // Najpierw użyj kolejki
            while (samplesWritten < count && outputQueue.Count > 0)
            {
                buffer[offset + samplesWritten] = outputQueue.Dequeue();
                samplesWritten++;
            }

            while (samplesWritten < count)
            {
                // Odczytaj próbki źródłowe
                int toRead = Math.Min(sourceBuffer.Length, (count - samplesWritten) * 2);
                toRead = (toRead / channels) * channels; // wyrównaj

                int sourceSamples = source.Read(sourceBuffer, 0, toRead);

                if (sourceSamples == 0)
                {
                    soundTouch.Flush();
                    int flushed = soundTouch.ReceiveSamples(tempBuffer, tempBuffer.Length / channels);
                    for (int i = 0; i < flushed * channels && samplesWritten < count; i++)
                    {
                        buffer[offset + samplesWritten] = tempBuffer[i];
                        samplesWritten++;
                    }
                    break;
                }

                // Przetwórz przez SoundTouch
                soundTouch.PutSamples(sourceBuffer, sourceSamples / channels);

                // Odbierz przetworzone próbki
                int received = soundTouch.ReceiveSamples(tempBuffer, tempBuffer.Length / channels);
                int totalReceived = received * channels;

                for (int i = 0; i < totalReceived; i++)
                {
                    if (samplesWritten < count)
                    {
                        buffer[offset + samplesWritten] = tempBuffer[i];
                        samplesWritten++;
                    }
                    else
                    {
                        outputQueue.Enqueue(tempBuffer[i]);
                    }
                }
            }

            return samplesWritten;
        }

        public void Dispose()
        {
            if (!disposed)
            {
                soundTouch?.Clear();
                disposed = true;
            }
        }
    }
}