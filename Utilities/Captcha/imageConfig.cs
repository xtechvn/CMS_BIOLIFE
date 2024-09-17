using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;

namespace Utilities.Captcha
{
    public static class imageConfig
    {
        #region phan_tich_hinh_anh
        public static readonly Color black = Color.FromArgb(255, 40, 40, 40);
        public static readonly Color gray = Color.FromArgb(255, 154, 154, 154);
        public static readonly int maxiswhite = 1;
        public static Image DownloadImage(string fromUrl)
        {
            using (WebClient webClient = new WebClient())
            {
                using (Stream stream = webClient.OpenRead(fromUrl))
                {
                    return Image.FromStream(stream);
                }
            }
        }
        public static List<int> FixError(List<int> lwidth, Bitmap bmp)
        {
            int max = lwidth[1] - lwidth[0];
            int lmax = lwidth[0];
            int rmax = lwidth[1];
            for (int i = 1; i < lwidth.Count; i++)
            {
                if (lwidth[i] - lwidth[i - 1] > max)
                {
                    max = lwidth[i] - lwidth[i - 1];
                    lmax = lwidth[i - 1];
                    rmax = lwidth[i];
                }
            }
            if (max >= 45)
            {
                int fromindex = lmax + max / 2 - 10;
                int toindex = rmax - max / 2 + 10;
                int mincolorblack = 100;
                int mincolorblackindex = 0;
                for (int x = fromindex; x <= toindex; x++)
                {
                    int countblack = 0;
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        Color p = p = bmp.GetPixel(x, y);

                        Color whilte = Color.FromArgb(255, 255, 255, 255);

                        if (black.R < p.R)
                        {
                        }
                        else
                        {
                            countblack += 1;

                        }
                    }

                    if (countblack < mincolorblack)
                    {
                        mincolorblack = countblack;
                        mincolorblackindex = x;
                    }
                }
                lwidth.Add(mincolorblackindex);
                lwidth.Sort();
                return lwidth;
            }
            return lwidth;
        }
        public static Bitmap JoinImage(Bitmap sourceimage, Bitmap content, int x, int y, int number)
        {
            using (Graphics grfx = Graphics.FromImage(sourceimage))
            {
                grfx.DrawImage(content, x + (number + 1) * 15, y);
                return sourceimage;
            }
        }
        public static Bitmap RotateImage(Bitmap img, float rotationAngle, int index)
        {
            Bitmap bmp = new Bitmap(img.Width + 6, img.Height);

            Graphics gfx = Graphics.FromImage(bmp);
            gfx.Clear(Color.White);

            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            gfx.RotateTransform(rotationAngle);

            //gfx.TranslateTransform(-(float)bmp.Width, -(float)bmp.Height);
            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            gfx.DrawImage(img, 0, 0, img.Width, img.Height - 8);

            if (index % 2 == 0)
            {
                for (int i = bmp.Height - 1; i >= 0; i--)
                {
                    for (int j = bmp.Width / 2; j < bmp.Width; j++)
                    {
                        if (i == 0)
                        {
                            Color p = Color.FromArgb(255, 255, 255, 255);
                            bmp.SetPixel(j, i, p);
                        }
                        else
                        {
                            Color p = bmp.GetPixel(j, i - 1);
                            bmp.SetPixel(j, i, p);
                        }
                    }
                }
            }
            else
            {
                if (index != 3)
                {
                    for (int i = 0; i < bmp.Height; i++)
                    {
                        for (int j = bmp.Width / 2; j < bmp.Width; j++)
                        {
                            if (i == bmp.Height - 1)
                            {
                                Color p = Color.FromArgb(255, 255, 255, 255);
                                bmp.SetPixel(j, i, p);
                            }
                            else
                            {
                                Color p = bmp.GetPixel(j, i + 1);
                                bmp.SetPixel(j, i, p);
                            }
                        }
                    }
                }

            }


            gfx.Dispose();
            int bottom = bmp.Height - 1;
            int imgbottom = bmp.Height - 1;
            for (int y = bmp.Height - 1; y >= 0; y--)
            {
                int countblack = 0;
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color p = bmp.GetPixel(x, y);

                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    int rgb = (r + g + b) / 3;
                    p = Color.FromArgb(255, rgb, rgb, rgb);

                    if (black.R < p.R)
                    {
                    }
                    else
                    {
                        countblack += 1;
                    }
                }
                if (countblack > maxiswhite)
                {
                    imgbottom = y;
                    break;
                }
            }
            if (imgbottom != bottom)
            {
                int height = bottom - imgbottom;
                for (int y = imgbottom; y >= 0; y--)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        Color p = bmp.GetPixel(x, y);
                        bmp.SetPixel(x, y + height, p);
                        if (y < height)
                        {
                            p = Color.FromArgb(255, 255, 255, 255);
                            bmp.SetPixel(x, y, p);
                        }
                    }

                }
            }
            return bmp;
        }

        /// <summary>
        /// Xoay cac chu cai trong anh captcha
        /// </summary>
        /// <param name="link_static_image"></param>
        /// <returns></returns>
        public static Bitmap analyticsImgCaptcha(string link_static_image)
        {
            try
            {


                Bitmap rootimg = new Bitmap(imageConfig.DownloadImage(link_static_image));
                Bitmap img = new Bitmap(rootimg.Width + 10, rootimg.Height);
                List<int> lcharwith = new List<int>();
                List<int> lcharheigth = new List<int>();
                int lastvaluewhite = 0;



                for (int y = 0; y < img.Height; y++)
                {
                    //bool iswhite = true;
                    int countblack = 0;
                    for (int x = 0; x < img.Width; x++)
                    {
                        Color p = Color.FromArgb(255, 255, 255, 255);
                        if (x >= rootimg.Width)
                        {
                            img.SetPixel(x, y, Color.FromArgb(255, 255, 255, 255));
                            continue;
                        }
                        else
                        {
                            p = rootimg.GetPixel(x, y);
                        }
                        // Color p = rootimg.GetPixel(x, y);
                        //int a = p.A;
                        int r = p.R;
                        int g = p.G;
                        int b = p.B;

                        int rgb = (r + g + b) / 3;
                        img.SetPixel(x, y, Color.FromArgb(255, rgb, rgb, rgb));

                        if (imageConfig.black.R < p.R)
                        {
                        }
                        else
                        {
                            //iswhite = false;
                            countblack += 1;
                            if (lcharheigth.Count == 0)
                            {
                                lcharheigth.Add(y);
                            }
                        }
                    }
                    if (countblack <= imageConfig.maxiswhite)
                    {
                        if (y - lastvaluewhite > 15)
                        {
                            lcharheigth.Add(y);
                        }
                        lastvaluewhite = y;
                    }
                }
                lastvaluewhite = 18;
                int lastx = 18;
                bool haveerror = false;
                for (int x = 18; x < img.Width; x++)
                {
                    int countblack = 0;
                    for (int y = 0; y < img.Height; y++)
                    {
                        Color p = Color.FromArgb(255, 255, 255, 255);
                        if (x >= rootimg.Width)
                        {
                            img.SetPixel(x, y, Color.FromArgb(255, 255, 255, 255));
                            continue;
                        }
                        else
                        {
                            p = rootimg.GetPixel(x, y);
                        }

                        Color whilte = Color.FromArgb(255, 255, 255, 255);


                        if (imageConfig.black.R < p.R)
                        {
                            if (imageConfig.gray.R < p.R)
                                img.SetPixel(x, y, Color.FromArgb(255, 255, 255, 255));
                            else
                            {
                                img.SetPixel(x, y, imageConfig.gray);
                            }
                        }
                        else
                        {
                            img.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                            countblack += 1;
                            if (lcharwith.Count == 0)
                            {
                                lcharwith.Add(x - 1);
                                lastvaluewhite = x - 1;
                                lastx = x - 1;
                            }
                        }
                    }

                    if (countblack <= imageConfig.maxiswhite)
                    {
                        if (x - lastvaluewhite > 16 || x - lastx > 16)
                        {
                            if (x - lastx >= 45) haveerror = true;
                            lcharwith.Add(x);
                            lastx = x;
                            lastvaluewhite = x;

                        }
                        else
                        {
                            //lastvaluewhite = lastx;
                        }
                        lastvaluewhite = x;

                    }

                }
                if (lcharwith.Count < 7)
                {
                    lcharwith = imageConfig.FixError(lcharwith, img);
                }
                else
                {
                    if (lcharwith.Count > 7)
                    {
                        lcharwith = imageConfig.FixError(lcharwith, img);
                    }
                    else
                    {
                        if (haveerror)
                        {
                            lcharwith = imageConfig.FixError(lcharwith, img);
                        }
                    }
                }
                if (lcharwith[6] > rootimg.Width)
                {
                    lcharwith[6] = rootimg.Width;
                }
                Bitmap lastimage = new Bitmap(img.Width + 90 + 36, img.Height);
                Bitmap newimg = new Bitmap(100, 100);
                for (int i = 0; i < 6; i++)
                {
                    newimg = rootimg.Clone(new Rectangle(lcharwith[i], lcharheigth[0], lcharwith[i + 1] - lcharwith[i], lcharheigth[1] - lcharheigth[0]), img.PixelFormat);
                    newimg = imageConfig.RotateImage(newimg, i % 2 == 0 ? -16 : 16, i);
                    lastimage = imageConfig.JoinImage(lastimage, newimg, lcharwith[i], lcharheigth[0], i);
                }


                for (int x = 0; x < lastimage.Width; x++)
                {
                    for (int y = 0; y < lastimage.Height; y++)
                    {
                        Color p = lastimage.GetPixel(x, y);
                        Color whilte = Color.FromArgb(255, 255, 255, 255);


                        if (imageConfig.black.R < p.R || p.A < 255)
                        {
                            if (imageConfig.gray.R < p.R || p.A < 255)
                                lastimage.SetPixel(x, y, Color.FromArgb(255, 255, 255, 255));
                            else
                                lastimage.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                        }
                        else
                        {
                            lastimage.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                        }
                    }

                }


                return lastimage;

            }
            catch (Exception ex)
            {
                //ErrorWriter.WriteLog(Environment.CurrentDirectory, "analyticsImgCaptcha(link_static_image =" + link_static_image + ") error" + ex.ToString());
                return null;

            }
        }


        #endregion
    }
}
