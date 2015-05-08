using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace Gem.Network.Chat.Client
{
    internal class YoutubeSearch : IDisposable
    {
        private readonly Action<string> Appender;
        private readonly YouTubeService youtubeService;
        private Dictionary<int, VideoInfo> IndexAndVideo;

        internal class VideoInfo
        {
            public string Title { get; set; }
            public string URL { get; set; }
        }

        #region Construct / Dispose

        public YoutubeSearch(Action<string> appender)
        {
            this.Appender = appender;
            youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyAaVegjDqfPLzEqKpw1gkIYeuHJmr_cMfY",
                ApplicationName = this.GetType().ToString()
            });
            IndexAndVideo = new Dictionary<int, VideoInfo>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool isDisposed = false;
        private void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                youtubeService.Dispose();
                isDisposed = true;
            }
        }

        #endregion

        public async Task Run(string key, int entries)
        {
            entries = entries < 25 ? entries : 25;
            
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = key;
            searchListRequest.MaxResults = entries;

            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();
            IndexAndVideo = new Dictionary<int, VideoInfo>();
            int counter = 1;
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format("[{0}]  {1}", counter,searchResult.Snippet.Title));
                        IndexAndVideo.Add(counter++, new VideoInfo { URL = searchResult.Id.VideoId, Title = searchResult.Snippet.Title });
                        break;
                }
            }
            Appender(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));

        }

        public bool Play(int index,out string title)
        {
            if (IndexAndVideo.ContainsKey(index))
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=" + IndexAndVideo[index].URL);
                title = IndexAndVideo[index].Title;
                return true;
            }
            else
            {
                Appender("Invalid index");
                title = null;
                return false;
            }
        }
    }
}