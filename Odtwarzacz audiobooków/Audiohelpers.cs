using NAudio.Wave;

namespace OdtwarzaczAudiobookow
{
    /// <summary>
    /// Stream z zapętlaniem dla podkładu muzycznego.
    /// </summary>
    public class LoopingWaveStream : WaveStream
    {
        private readonly WaveStream sourceStream;

        public LoopingWaveStream(WaveStream source)
        {
            sourceStream = source;
        }

        public override WaveFormat WaveFormat => sourceStream.WaveFormat;
        public override long Length => sourceStream.Length;
        public override long Position
        {
            get => sourceStream.Position;
            set => sourceStream.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalRead = 0;

            while (totalRead < count)
            {
                int read = sourceStream.Read(buffer, offset + totalRead, count - totalRead);
                if (read == 0)
                {
                    sourceStream.Position = 0;
                    // Bezpieczeństwo przed nieskończoną pętlą
                    if (sourceStream.Position != 0)
                        break;
                }
                totalRead += read;
            }

            return totalRead;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                sourceStream?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Sample provider z regulacją głośności - do niezależnej kontroli głośności tła.
    /// </summary>
    public class VolumeSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider source;
        private float volume = 1.0f;

        public VolumeSampleProvider(ISampleProvider source)
        {
            this.source = source;
        }

        public WaveFormat WaveFormat => source.WaveFormat;

        public float Volume
        {
            get => volume;
            set => volume = Math.Clamp(value, 0f, 1f);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = source.Read(buffer, offset, count);

            if (volume != 1.0f)
            {
                for (int i = 0; i < samplesRead; i++)
                {
                    buffer[offset + i] *= volume;
                }
            }

            return samplesRead;
        }
    }

    /// <summary>
    /// Looping sample provider - zapętlony strumień jako ISampleProvider
    /// </summary>
    public class LoopingSampleProvider : ISampleProvider
    {
        private readonly AudioFileReader sourceReader;
        private readonly string filePath;

        public LoopingSampleProvider(string filePath)
        {
            this.filePath = filePath;
            sourceReader = new AudioFileReader(filePath);
        }

        public WaveFormat WaveFormat => sourceReader.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            int totalRead = 0;

            while (totalRead < count)
            {
                int read = sourceReader.Read(buffer, offset + totalRead, count - totalRead);
                if (read == 0)
                {
                    sourceReader.Position = 0;
                }
                totalRead += read;
            }

            return totalRead;
        }

        public void Dispose()
        {
            sourceReader?.Dispose();
        }
    }
}