using System.Collections.Generic;

namespace Ecalc.FFmpegGui
{
    public class Preset
    {
        public string Description { get; set; }
        public string CommandLine { get; set; }
        public string Extension { get; set; }
    }

    public class Presets: List<Preset>
    {
        public Presets()
        {
            Add(new Preset
            {
                Description = "Remux input to MKV container",
                CommandLine = "-i %input% -vcodec copy -acodec copy %output%",
                Extension = "mkv"
            });
            Add(new Preset
            {
                Description = "Create CD compatible WAV",
                CommandLine = "-i %input% -vn -f wav -ar 44100 %output%",
                Extension = "wav"
            });
            Add(new Preset
            {
                Description = "Extract M4A audio from MP4",
                CommandLine = "-i %input% -vn -acodec copy %output%",
                Extension = "m4a"
            });
            Add(new Preset
            {
                Description = "Convert Audio to WavPack (Normal Compression)",
                CommandLine = "-i %input% -vn -acodec wavpack %output%",
                Extension = "wv"
            });
            Add(new Preset
            {
                Description = "Convert Audio to WavPack (High Compression)",
                CommandLine = "-i %input% -vn -acodec wavpack -compression_level 3 %output%",
                Extension = "wv"
            });
            Add(new Preset
            {
                Description = "Convert Audio to Flac (Normal Compression)",
                CommandLine = "-i %input% -vn -acodec flac %output%",
                Extension = "flac"
            });
            Add(new Preset
            {
                Description = "Convert Audio to Flac (High Compression)",
                CommandLine = "-i %input% -vn -acodec flac -compression_level 12 %output%",
                Extension = "flac"
            });
            Add(new Preset
            {
                Description = "Convert Audio to MP3 (Highest Quality)",
                CommandLine = "-i %input% -vn -acodec mp3 -b:a 320k %output%",
                Extension = "mp3"
            });
            Add(new Preset
            {
                Description = "Convert Audio to MP3 (High Quality)",
                CommandLine = "-i %input% -vn -acodec mp3 -b:a 256k %output%",
                Extension = "mp3"
            });
            Add(new Preset
            {
                Description = "Convert Audio to MP3 (Midle Quality)",
                CommandLine = "-i %input% -vn -acodec mp3 -b:a 192k %output%",
                Extension = "mp3"
            });
            Add(new Preset
            {
                Description = "Convert Audio to MP3 (Low Quality)",
                CommandLine = "-i %input% -vn -acodec mp3 -b:a 128k %output%",
                Extension = "mp3"
            });
            Sort(Compare);

        }

        private int Compare(Preset x, Preset y)
        {
            return x.Description.CompareTo(y.Description);
        }
    }
}
