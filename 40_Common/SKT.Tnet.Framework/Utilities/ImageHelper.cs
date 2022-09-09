using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;

namespace SKT.Tnet.Framework.Utilities
{
    public class ImageHelper
    {
        /// <summary>
        /// Resize image with a directory as source
        /// </summary>
        /// <param name="OriginalFileLocation">Image location</param>
        /// <param name="heigth">new height</param>
        /// <param name="width">new width</param>
        /// <param name="keepAspectRatio">keep the aspect ratio</param>
        /// <param name="getCenter">return the center bit of the image</param>
        /// <returns>image with new dimentions</returns>
        public static System.Drawing.Image resizeImageFromFile(String OriginalFileLocation, int heigth, int width, Boolean keepAspectRatio = true, Boolean getCenter = false)
        {
            int newheigth = heigth;
            System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(OriginalFileLocation);

            // Prevent using images internal thumbnail
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            if (keepAspectRatio || getCenter)
            {
                int bmpY = 0;
                double resize = (double)FullsizeImage.Width / (double)width;//get the resize vector
                if (getCenter)
                {
                    bmpY = (int)((FullsizeImage.Height - (heigth * resize)) / 2);// gives the Y value of the part that will be cut off, to show only the part in the center
                    Rectangle section = new Rectangle(new Point(0, bmpY), new Size(FullsizeImage.Width, (int)(heigth * resize)));// create the section to cut of the original image
                    //System.Console.WriteLine("the section that will be cut off: " + section.Size.ToString() + " the Y value is minimized by: " + bmpY);
                    using (Bitmap orImg = new Bitmap((Bitmap)FullsizeImage)) //for the correct effect convert image to bitmap.
                    {
                        FullsizeImage.Dispose();//clear the original image
                        using (Bitmap tempImg = new Bitmap(section.Width, section.Height, PixelFormat.Format64bppArgb))
                        {
                            using (Graphics cutImg = Graphics.FromImage(tempImg)) //              set the file to save the new image to.
                            {
                                cutImg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                cutImg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                cutImg.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                                cutImg.DrawImage(orImg, 0, 0, section, GraphicsUnit.Pixel);// cut the image and save it to tempImg
                            }

                            FullsizeImage = tempImg;//save the tempImg as FullsizeImage for resizing later
                            return FullsizeImage.GetThumbnailImage(width, heigth, null, IntPtr.Zero);
                        }
                    }
                }
                else
                {
                    newheigth = (int)(FullsizeImage.Height / resize);//  set the new heigth of the current image
                }
            }//return the image resized to the given heigth and width
            return FullsizeImage.GetThumbnailImage(width, newheigth, null, IntPtr.Zero);
        }

        /// <summary>
        /// 이미지 사이즈 조정 함수
        /// </summary>
        /// <param name="OriginalFilePath">원본 이미지 파일 경로</param>
        /// <param name="ResizeFilePath">사이즈 변경 이미지 파일 경로</param>
        /// <param name="Size">조정할 사이즈 (X축 또는 Y축) 크기</param>
        /// <returns></returns>
        public static bool ResizeSave(string OriginalFilePath, string ResizeFilePath, int Size)
        {
            bool bRtn = true;

            try
            {
                if (string.IsNullOrEmpty(OriginalFilePath) == true) throw new ArgumentNullException();
                if (string.IsNullOrEmpty(ResizeFilePath) == true) throw new ArgumentNullException();

                if (System.IO.File.Exists(OriginalFilePath) == true)
                {
                    using (System.Drawing.Image OriginalImage = System.Drawing.Image.FromFile(OriginalFilePath))
                    {
                        int _Width = OriginalImage.Width;
                        int _Height = OriginalImage.Height;

                        if (_Width > _Height)
                        {
                            using (System.Drawing.Image ThumbnailImage = ImageHelper.resizeImageFromFile(OriginalFilePath, System.Convert.ToInt32((_Height * (_Width / Size)) / 100), Size))
                            {
                                ThumbnailImage.Save(ResizeFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }
                        else
                        {
                            using (System.Drawing.Image ThumbnailImage = ImageHelper.resizeImageFromFile(OriginalFilePath, Size, System.Convert.ToInt32((_Width * (_Height / Size)) / 100)))
                            {
                                ThumbnailImage.Save(ResizeFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }
                    }
                }
                else
                {
                    bRtn = false;
                }
            }
            catch
            {
                bRtn = false;
            }

            return bRtn;
        }

        /// <summary>
        /// 이미지 사이즈 조정 함수
        /// </summary>
        /// <param name="OriginalFilePath">원본 이미지 파일 경로</param>
        /// <param name="ResizeFilePath">사이즈 변경 이미지 파일 경로</param>
        /// <param name="Height">조정할 이미지 사이트 (높이)</param>
        /// <param name="Width">조정할 이미지 사이트 (폭)</param>
        /// <returns></returns>
        public static bool ResizeSave(string OriginalFilePath, string ResizeFilePath, int Height, int Width)
        {
            bool bRtn = true;

            try
            {
                if (string.IsNullOrEmpty(OriginalFilePath) == true) throw new ArgumentNullException();
                if (string.IsNullOrEmpty(ResizeFilePath) == true) throw new ArgumentNullException();

                if (System.IO.File.Exists(OriginalFilePath) == true)
                {
                    using (System.Drawing.Image ThumbnailImage = ImageHelper.resizeImageFromFile(OriginalFilePath, Height, Width))
                    {
                        ThumbnailImage.Save(ResizeFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
                else
                {
                    bRtn = false;
                }
            }
            catch
            {
                bRtn = false;
            }

            return bRtn;
        }

        /// <summary>
        /// 이미지 사이즈 조정 함수
        /// </summary>
        /// <param name="OriginalFilePath">원본 이ㅣ지 파일 경로</param>
        /// <param name="ResizeFilePath">사이즈 변경 이미지 파일 경로</param>
        /// <param name="Size">조정할 이미지 사이즈</param>
        /// <param name="Gubun">조정할 이미지 사이즈 위치 (높이 : Height, 폭 : Width)</param>
        /// <returns></returns>
        public static bool ResizeSave(string OriginalFilePath, string ResizeFilePath, int Size, string Gubun)
        {
            bool bRtn = true;
            int _Width = 0;
            int _Height = 0;
            int tempWidth = 0;
            int tempHeight = 0;

            try
            {
                if (string.IsNullOrEmpty(OriginalFilePath) == true) throw new ArgumentNullException();
                if (string.IsNullOrEmpty(ResizeFilePath) == true) throw new ArgumentNullException();

                if (System.IO.File.Exists(OriginalFilePath) == true)
                {
                    using (System.Drawing.Image OriginalImage = System.Drawing.Image.FromFile(OriginalFilePath))
                    {
                        _Width = OriginalImage.Width;
                        _Height = OriginalImage.Height;

                        if (Gubun == "Width")
                        {
                            if (Size < _Width)
                            {
                                tempWidth = Size;
                                tempHeight = System.Convert.ToInt32((_Height * (_Width / Size)) / 100);
                            }
                            else
                            {
                                tempWidth = _Width;
                                tempHeight = _Height;
                            }
                        }
                        
                        if (Gubun == "Height")
                        {
                            if (Size < _Height)
                            {
                                tempWidth = System.Convert.ToInt32((_Width * (_Height / Size)) / 100);
                                tempHeight = Size;
                            }
                            else
                            {
                                tempWidth = _Width;
                                tempHeight = _Height;
                            }
                        }
                    }

                    using (System.Drawing.Image ThumbnailImage = ImageHelper.resizeImageFromFile(OriginalFilePath, tempHeight, tempWidth))
                    {
                        ThumbnailImage.Save(ResizeFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
                else
                {
                    bRtn = false;
                }
            }
            catch
            {
                bRtn = false;
            }

            return bRtn;
        }

        #region HTML inline Image
        /// <summary>
        /// html 에서 첫번째 img 태그의 src 속성에 기술된 이미지 url을 반환한다.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetImageURL(string html)
        {
            string IMG_REX_PATTERN = @"<[Ii][Mm][Gg][^>]*src\s*=\s*[\""\']?(?<IMAGE_URL>[^""'>\s]*)[\""\']?[^>]*>";
            Match m = Regex.Match(html, IMG_REX_PATTERN);

            return m.Groups["IMAGE_URL"].ToString();
        }

        /// <summary>
        /// Html 내에 img 태그의 src 속성에 기술된 이미지 url를 반환 (복수형)
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static List<Uri> GetImageURLS(string html)
        {
            return ImageHelper.FetchLinksFromSource(html);
        }

        /// <summary>
        /// Html 에서 img 태스의 src 속성값을 취득하여 반환하는 함수
        /// </summary>
        /// <param name="htmlSource"></param>
        /// <returns></returns>
        private static List<Uri> FetchLinksFromSource(string htmlSource)
        {
            List<Uri> links = null;

            try
            {
                if (string.IsNullOrEmpty(htmlSource) == false)
                {
                    links = new List<Uri>();

                    string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
                    MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    foreach (Match m in matchesImgSrc)
                    {
                        string href = m.Groups[1].Value;

                        bool bChk = true;

                        if (links.Count > 0)
                        {
                            foreach (Uri url in links)
                            {
                                if (url.ToString() == href)
                                {
                                    bChk = false;
                                    break;
                                }
                            }
                        }
                        
                        if (bChk == true)
                        {
                            links.Add(new Uri(href));
                        }
                    }
                }
            }
            catch
            {
                links = null;
            }

            return links;
        }

        /// <summary>
        /// HTML 내에 src 속성값 중에서 도메인 설정이 않된 부분 설정하기.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="Domain"></param>
        /// <returns></returns>
        public static string ReplaceImageUrl(string html, string Domain)
        {
            List<string> CheckUrl = new List<string>();

            try
            {
                if (string.IsNullOrEmpty(html) == false)
                {
                    string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
                    MatchCollection matchesImgSrc = Regex.Matches(html, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    foreach (Match m in matchesImgSrc)
                    {
                        bool bChk = true;
                        string href = m.Groups[1].Value;
                        string tempUrl = string.Empty;

                        if (href.Substring(0, 1) == "/")
                        {
                            tempUrl = Domain + href;
                            tempUrl = tempUrl.Replace("//", "/");
                        }
                        else
                        {
                            tempUrl = href;
                        }

                        if (CheckUrl.Count > 0)
                        {
                            foreach (string url in CheckUrl)
                            {
                                if (url == href)
                                {
                                    bChk = false;
                                }
                            }
                        }
                        else
                        {
                            CheckUrl.Add(href);
                        }

                        if (bChk == true)
                        {
                            html = html.Replace(href, tempUrl);
                        }

                    }
                }
            }
            catch { }

            return html;
        }

        #endregion

        #region 사용자 사진 이미지
        /// <summary>
        /// 사용자 Photo 이미지 취득 함수
        /// </summary>
        /// <param name="EmpID">사번</param>
        /// <param name="Width">이미지의 폭</param>
        /// <param name="Height">이미지의 높이</param>
        /// <returns></returns>
        public static string GetPhotoImage(string EmpID, int Width = 40, int Height = 40)
        {
            string strRtn = string.Empty;

            if (string.IsNullOrEmpty(EmpID) == true)
            {
                strRtn = string.Format("<img src='http://{0}/Images/noimage.gif' widht={1} height={2} /> "
                    , HttpContext.Current.Request.Url.Authority
                    , Width.ToString()
                    , Height.ToString()
                    );
            }
            else
            {
                strRtn = string.Format("<img src='http://con1.toktok.sk.com/HRProfile/SKT/0{0}.jpg' widht={1} height={2} OnError=\"this.src='http://{3}/images/board/no_img_pro40x40.jpg'\" /> "
                    , EmpID
                    , Width.ToString()
                    , Height.ToString()
                    , HttpContext.Current.Request.Url.Authority
                    );
            }

            return strRtn;
        }

        /// <summary>
        /// 사용자 Photo 이미지 취득 함수
        /// </summary>
        /// <param name="EmpID">사번</param>
        /// <returns></returns>
        public static string GetPhotoImageUrl(string EmpID)
        {
            return string.Format("http://con1.toktok.sk.com/HRProfile/SKT/0{0}.jpg", EmpID);
        }
        #endregion
    }
}
