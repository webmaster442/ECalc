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
                CommandLine = "-i {input} -vc copy -ac copy {output}",
                Extension = "mkv"
            });
        }
    }
}
