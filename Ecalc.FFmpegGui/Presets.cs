using System.Collections.Generic;

namespace Ecalc.FFmpegGui
{
    public class Presets: Dictionary<string, string>
    {
        public Presets()
        {
            Add("Test preset", "test preset");
        }
    }
}
