////using System;
////using System.Threading;
////using Microsoft.Xna.Framework;
////using Microsoft.Xna.Framework.Graphics;

////namespace  Gem.Network.Avatar
////{
////    class LoadAvatarThread
////    {   
////        public Thread thread;

////        #region Properties

////        public Avatar Avatar
////        {
////            get;
////            set;
////        }

////        #endregion

////        public LoadAvatarThread(string name, GraphicsDevice gDevice)
////        {
////            thread = new Thread(() => LoadAvatar(name, gDevice));
////            thread.Name = name;
////            thread.Start();
////        }

////        public void Dispose()
////        {
////            thread.Abort();
////            Avatar = null;
////        }

////        #region Thread Method

////        void LoadAvatar(string name,GraphicsDevice gDevice)
////        {
////            Avatar = new Avatar(name, gDevice);
////        }

////        #endregion
////    }
////}
