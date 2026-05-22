using System.Text.Json.Serialization;

namespace OdtwarzaczAudiobookow
{
    public class AudioBook
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public double Duration { get; set; }
        public double LastPosition { get; set; }
        public DateTime LastPlayed { get; set; }
        public string? CoverImagePath { get; set; }
        public List<Chapter> Chapters { get; set; } = new();

        [JsonIgnore]
        public string DisplayDuration => TimeSpan.FromSeconds(Duration).ToString(@"hh\:mm\:ss");

        [JsonIgnore]
        public string DisplayPosition => TimeSpan.FromSeconds(LastPosition).ToString(@"hh\:mm\:ss");

        [JsonIgnore]
        public double ProgressPercent => Duration > 0 ? (LastPosition / Duration) * 100 : 0;
    }

    public class Chapter
    {
        public string Title { get; set; } = string.Empty;
        public double StartTime { get; set; }
        public double EndTime { get; set; }

        [JsonIgnore]
        public string DisplayTime => TimeSpan.FromSeconds(StartTime).ToString(@"hh\:mm\:ss");
    }

    public class SyncSettings
    {
        public bool Enabled { get; set; } = false;
        public bool IsServerMode { get; set; } = false;
        public string ServerIP { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 5555;

        [JsonIgnore]
        public Dictionary<string, SyncPosition> LastPositions { get; set; } = new();
    }

    public class SyncPosition
    {
        public double Position { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class SyncData
    {
        public string MessageType { get; set; } = "UPDATE"; // UPDATE, REQUEST, RESPONSE
        public string BookId { get; set; } = string.Empty;
        public double Position { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Success { get; set; } = true;
    }

    public class AppSettings
    {
        public List<AudioBook> AudioBooks { get; set; } = new();
        public SyncSettings SyncSettings { get; set; } = new();
        public float LastSpeed { get; set; } = 1.0f;
        public float BackgroundVolume { get; set; } = 0.2f;
        public float MainVolume { get; set; } = 1.0f;
        public string? LastPlayedBookId { get; set; }
        public string? BackgroundMusicPath { get; set; }
        public bool BackgroundMusicEnabled { get; set; } = false;
        public bool AutoResume { get; set; } = true;
        public int SeekSeconds { get; set; } = 30;
        public string SortBy { get; set; } = "Title"; // Title, Author, LastPlayed, Duration
        public bool SortAscending { get; set; } = true;
    }

    /// <summary>
    /// Predefiniowany dźwięk ambient
    /// </summary>
    public class AmbientSound
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public bool IsBuiltIn { get; set; } = true;
    }
}