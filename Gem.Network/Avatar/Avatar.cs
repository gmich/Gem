////using System.IO;
////using System.Net;
////using Microsoft.Xna.Framework;
////using Microsoft.Xna.Framework.Graphics;

////namespace  Gem.Network.Avatar
////{
////    public class Avatar
////    {

////        public Avatar(string usrname, GraphicsDevice graphicsDevice)
////        {
////            MemoryStream memoryStream = new MemoryStream();
////            byte[] buffer = new byte[2048];
////            SteamApi steamapi = new SteamApi(usrname);

////            if (steamapi.Name != null)
////                UsrName = steamapi.Name;
////            else
////                UsrName = usrname;
////            try
////            {
////                HttpWebRequest avatarRequest = (HttpWebRequest)WebRequest.Create(steamapi.AvatarPath);
////                HttpWebResponse avatarResponse = (HttpWebResponse)avatarRequest.GetResponse();
////                Stream stream = avatarResponse.GetResponseStream();

////                int bytesRead = 1;
////                while (bytesRead != 0)
////                {
////                    bytesRead = stream.Read(buffer, 0, buffer.Length);
////                    memoryStream.Write(buffer, 0, bytesRead);
////                }

////                memoryStream.Position = 0;

////                Texture = Texture2D.FromStream(graphicsDevice, memoryStream);
////                HasFailed = false;
////            }
////            catch
////            {
////                var tex = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
////                tex.SetData<Color>(new Color[] { Color.White });
////                Texture = tex;
////                HasFailed = true;
////            }
////        }

////        public bool HasFailed
////        {
////            get;
////            private set;
////        }

////        public Texture2D Texture
////        {
////            get; 
////            set;
////        }

////        public string UsrName
////        {
////            get;
////            set;
////        }
////    }
////}
