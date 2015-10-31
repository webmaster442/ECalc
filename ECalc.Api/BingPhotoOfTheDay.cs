using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace ECalc.Api
{
    /// <summary>
    /// A class designed to download the bing photo of the day
    /// </summary>
    public class BingWallPaperClient
    {
        private readonly string _feed;
        private readonly string _tempfilename;
        private readonly string _tempcoppyright;
        private bool _loadcalled;

        /// <summary>
        /// Creates a new instance of the Bing photo downloader
        /// </summary>
        public BingWallPaperClient()
        {
            var tempdir = Environment.ExpandEnvironmentVariables("%temp%");
            _tempfilename = Path.Combine(tempdir, "bingphotooftheday.jpg");
            _tempcoppyright = Path.Combine(tempdir, "bingphotooftheday.txt");
            _loadcalled = false;

            //photo of the day data in xml format
            _feed = "http://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=hu-HU";
        }

        /// <summary>
        /// Downloads the photo of the day synchronously
        /// </summary>
        public void DownLoad()
        {
            bool downloadneeded = true;
            if (File.Exists(_tempfilename))
            {
                FileInfo fi = new FileInfo(_tempfilename);
                if (DateTime.UtcNow.DayOfYear == fi.LastWriteTimeUtc.DayOfYear)
                {
                    downloadneeded = false;
                }
            }

            if (File.Exists(_tempcoppyright))
            {
                var cpy = File.ReadAllText(_tempcoppyright).Split(';');
                CoppyRightData = cpy[0];
                CoppyRightLink = cpy[1];
                downloadneeded = false;
            }
            else downloadneeded = true;

            if (!downloadneeded)
            {
                _loadcalled = true;
                return;
            }

            var document = XDocument.Load(_feed).Elements().Elements().FirstOrDefault();

            var url = (from i in document.Elements()
                       where i.Name == "url"
                       select i.Value.ToString()).FirstOrDefault();

            var imgurl = "http://www.bing.com" + url;

            CoppyRightData = (from i in document.Elements()
                              where i.Name == "copyright"
                              select i.Value.ToString()).FirstOrDefault();

            CoppyRightLink = (from i in document.Elements()
                              where i.Name == "copyrightlink"
                              select i.Value.ToString()).FirstOrDefault();

            File.WriteAllText(_tempcoppyright, CoppyRightData + ";" + CoppyRightLink);

            using (var client = new WebClient())
            {
                client.DownloadFile(imgurl, _tempfilename);
            }
            _loadcalled = true;
        }

        /// <summary>
        /// Asyncronous & awaitable version of the download routine
        /// </summary>
        /// <returns>An awaitable task</returns>
        public Task DownloadAsync()
        {
            return Task.Run(() =>
            {
                DownLoad();
            });
        }

        /// <summary>
        /// Gets the Photo of the day in a WPF complaint ImageSource
        /// </summary>
        public ImageSource WPFPhotoOfTheDay
        {
            get
            {
                if (!_loadcalled)
                    throw new InvalidOperationException("Call the DownLoad() method first");
                return new BitmapImage(new Uri(_tempfilename));
            }
        }

        /// <summary>
        /// CoppyRight data information
        /// </summary>
        public string CoppyRightData
        {
            get;
            private set;
        }

        /// <summary>
        /// CoppyRightLink
        /// </summary>
        public string CoppyRightLink
        {
            get;
            private set;
        }
    }
}
