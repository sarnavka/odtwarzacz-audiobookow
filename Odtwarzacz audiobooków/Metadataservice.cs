using System.Diagnostics;
using TagLib;

namespace OdtwarzaczAudiobookow
{
    /// <summary>
    /// Serwis do odczytu metadanych plików audio.
    /// Wykorzystuje bibliotekę TagLibSharp do obsługi ID3/MP4 tags.
    /// </summary>
    public static class MetadataService
    {
        /// <summary>
        /// Pobiera metadane z pliku audio.
        /// </summary>
        public static AudioBook ExtractMetadata(string filePath)
        {
            var audioBook = new AudioBook
            {
                FilePath = filePath,
                Title = Path.GetFileNameWithoutExtension(filePath),
                LastPlayed = DateTime.Now
            };

            try
            {
                using var tagFile = TagLib.File.Create(filePath);

                // Tytuł
                if (!string.IsNullOrWhiteSpace(tagFile.Tag.Title))
                {
                    audioBook.Title = tagFile.Tag.Title;
                }

                // Autor (najpierw albumArtist dla audiobooków, potem zwykły artist)
                if (tagFile.Tag.AlbumArtists?.Length > 0 && !string.IsNullOrWhiteSpace(tagFile.Tag.AlbumArtists[0]))
                {
                    audioBook.Author = string.Join(", ", tagFile.Tag.AlbumArtists);
                }
                else if (tagFile.Tag.Performers?.Length > 0 && !string.IsNullOrWhiteSpace(tagFile.Tag.Performers[0]))
                {
                    audioBook.Author = string.Join(", ", tagFile.Tag.Performers);
                }

                // Czas trwania
                audioBook.Duration = tagFile.Properties.Duration.TotalSeconds;

                // Rozdziały (dla długich plików tworzymy sztuczne co 30 min)
                if (audioBook.Duration > 1800)
                {
                    double chapterDuration = 1800; // 30 minut
                    int chapterCount = (int)Math.Ceiling(audioBook.Duration / chapterDuration);

                    for (int i = 0; i < chapterCount; i++)
                    {
                        double start = i * chapterDuration;
                        double end = Math.Min((i + 1) * chapterDuration, audioBook.Duration);

                        audioBook.Chapters.Add(new Chapter
                        {
                            Title = $"Rozdział {i + 1}",
                            StartTime = start,
                            EndTime = end
                        });
                    }
                }

                // Okładka
                ExtractCoverImage(audioBook, tagFile);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd odczytu metadanych: {ex.Message}");

                // Próba odczytu nazwy pliku jako autor - tytuł
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var parts = fileName.Split(new[] { '-', '_' }, 2);
                if (parts.Length >= 2)
                {
                    audioBook.Author = parts[0].Trim();
                    audioBook.Title = parts[1].Trim();
                }

                // Spróbuj pobrać czas trwania przez NAudio
                try
                {
                    using var reader = new NAudio.Wave.AudioFileReader(filePath);
                    audioBook.Duration = reader.TotalTime.TotalSeconds;
                }
                catch { }
            }

            return audioBook;
        }

        /// <summary>
        /// Wyodrębnia okładkę z pliku i zapisuje ją lokalnie.
        /// </summary>
        private static void ExtractCoverImage(AudioBook audioBook, TagLib.File tagFile)
        {
            try
            {
                if (tagFile.Tag.Pictures?.Length > 0)
                {
                    var picture = tagFile.Tag.Pictures[0];
                    string coversDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "covers");
                    Directory.CreateDirectory(coversDir);

                    string extension = picture.MimeType switch
                    {
                        "image/png" => ".png",
                        "image/gif" => ".gif",
                        _ => ".jpg"
                    };

                    string coverPath = Path.Combine(coversDir, $"{audioBook.Id}{extension}");
                    System.IO.File.WriteAllBytes(coverPath, picture.Data.Data);
                    audioBook.CoverImagePath = coverPath;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd wyodrębniania okładki: {ex.Message}");
            }
        }

        /// <summary>
        /// Aktualizuje metadane w pliku.
        /// </summary>
        public static void UpdateMetadata(AudioBook audioBook)
        {
            try
            {
                using var tagFile = TagLib.File.Create(audioBook.FilePath);
                tagFile.Tag.Title = audioBook.Title;
                tagFile.Tag.AlbumArtists = new[] { audioBook.Author };
                tagFile.Save();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd zapisu metadanych: {ex.Message}");
            }
        }

        /// <summary>
        /// Pobiera czas trwania pliku audio.
        /// </summary>
        public static double GetDuration(string filePath)
        {
            try
            {
                using var tagFile = TagLib.File.Create(filePath);
                return tagFile.Properties.Duration.TotalSeconds;
            }
            catch
            {
                try
                {
                    using var reader = new NAudio.Wave.AudioFileReader(filePath);
                    return reader.TotalTime.TotalSeconds;
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}