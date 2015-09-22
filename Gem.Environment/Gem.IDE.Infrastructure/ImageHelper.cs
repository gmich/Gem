using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Gem.IDE.Infrastructure
{
    public static class ImageHelper
    {
        public static bool AreBitmapsEqual(Bitmap first, Bitmap second)
        {
            bool equals = true;
            Rectangle rect = new Rectangle(0, 0, first.Width, first.Height);
            BitmapData bmpData1 = first.LockBits(rect, ImageLockMode.ReadOnly, first.PixelFormat);
            BitmapData bmpData2 = second.LockBits(rect, ImageLockMode.ReadOnly, second.PixelFormat);
            unsafe
            {
                byte* ptr1 = (byte*)bmpData1.Scan0.ToPointer();
                byte* ptr2 = (byte*)bmpData2.Scan0.ToPointer();
                int width = rect.Width * 3;
                for (int y = 0; equals && y < rect.Height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (*ptr1 != *ptr2)
                        {
                            return false;
                        }
                        ptr1++;
                        ptr2++;
                    }
                    ptr1 += bmpData1.Stride - width;
                    ptr2 += bmpData2.Stride - width;
                }
            }
            first.UnlockBits(bmpData1);
            second.UnlockBits(bmpData2);
            return true;
        }

        public static Task<Texture2D> LoadAsTexture2D(GraphicsDevice graphicsDevice, string path)
        {
            return Task.Run(() =>
            {
                return LoadTexture2DFromFile(graphicsDevice, path);              
            });
        }
        
        public static Texture2D LoadTexture2DFromBitmap(GraphicsDevice graphicsDevice, string path)
        {
            var bmp = Image.FromFile(path) as Bitmap;
            var texture = new Texture2D(graphicsDevice, bmp.Width, bmp.Height);
            var pixels = new Microsoft.Xna.Framework.Color[bmp.Width * bmp.Height];
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var c = bmp.GetPixel(x, y);
                    pixels[(y * bmp.Width) + x] = new Microsoft.Xna.Framework.Color(c.R, c.G, c.B, c.A);
                }
            }
            texture.SetData(pixels);

            return texture;
        }

        private static Texture2D LoadTexture2DFromFile(GraphicsDevice device,string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return Texture2D.FromStream(device, fileStream);
            }
        }

        public static string ImageToString(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            Image im = Image.FromFile(path);
            var ms = new MemoryStream();

            im.Save(ms, im.RawFormat);

            var array = ms.ToArray();

            return System.Convert.ToBase64String(array);
        }

        public static Image StringToImage(string imageString)
        {
            if (imageString == null)
            {
                throw new ArgumentNullException("imageString");
            }
            var array = System.Convert.FromBase64String(imageString);

            var image = Image.FromStream(new MemoryStream(array));

            return image;
        }

    }

}
