using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Configuration;
using SKT.Common;

namespace SKT.Common
{
    public class NamoUtils
    {
        private static string uploadPath = HttpContext.Current.Server.MapPath("~/") + "NamoFiles";
        private static string uploadURL = "/NamoFiles";

        // Mime decoding
        public static string DecodeMIME(string MIMEContent, string subDir)
        {
            string bodymsg = "";
            string dataFolder;
            string dataUrl;
            DirectoryInfo di;
            FileInfo[] afi;
            NamoMIME.MimeObject mime = new NamoMIME.MimeObject();
            //MIMEObjectClass mime = new MIMEObjectClass();

            // save included file
            dataFolder = uploadPath + "\\" + subDir;
            dataUrl = uploadURL + "/" + subDir.Replace("\\", "/");
            Directory.CreateDirectory(dataFolder);
            mime.Decode(MIMEContent, dataFolder);

            // file reading 
            string filePath = dataFolder + "\\noname";
            if (!File.Exists(filePath))
                filePath += ".htm";
            FileStream freader = File.OpenRead(filePath);
            byte[] buffer = new byte[freader.Length];
            freader.Read(buffer, 0, (int)freader.Length);
            //bodymsg = Encoding.Default.GetString(buffer);
            bodymsg = Encoding.UTF8.GetString(buffer);
            freader.Close();
            File.Delete(filePath);
            di = new DirectoryInfo(dataFolder);
            afi = di.GetFiles();

            //파일이 없는 경우 임시 폴더 삭제
            if (di.GetFiles().Length == 0 && di.GetDirectories().Length == 0)
            {
                //di.Delete();
            }

            foreach (FileInfo fi in afi)
            {
                bodymsg = Replace(bodymsg, fi.FullName, dataUrl + "/" + fi.Name);
            }

            return bodymsg;
        }


        //  Mime decoding
        private static string InlineImageFilePath = ConfigurationManager.AppSettings["InlineImageFilePath"];
        public static string DecodeMIMEMakeFileAndInsertDB(string MIMEContent, string subDir, string BoardID, string ItemID, string UserID
            , out  List<Attach> InlineImageList)
        {
            string bodymsg = "";
            string dataFolder;
            string dataUrl;
            DirectoryInfo di;
            FileInfo[] afi;
            NamoMIME.MimeObject mime = new NamoMIME.MimeObject();
            //MIMEObjectClass mime = new MIMEObjectClass();

            // save included file
            dataFolder = InlineImageFilePath + "\\" + subDir;
            dataUrl = "/Common/Controls/InlineImage/InlineImage.aspx?InlineImageID=";
            Directory.CreateDirectory(dataFolder);
            mime.Decode(MIMEContent, dataFolder);

            // file reading 
            string filePath = dataFolder + "\\noname";
            if (!File.Exists(filePath))
                filePath += ".htm";
            FileStream freader = File.OpenRead(filePath);
            byte[] buffer = new byte[freader.Length];
            freader.Read(buffer, 0, (int)freader.Length);
            //bodymsg = Encoding.Default.GetString(buffer);
            bodymsg = Encoding.UTF8.GetString(buffer);
            freader.Close();
            File.Delete(filePath);
            di = new DirectoryInfo(dataFolder);
            afi = di.GetFiles();

            InlineImageList = AttachmentHelper.InsertInlineImage(dataFolder, BoardID, ItemID, UserID);

            //파일이 없는 경우 임시 폴더 삭제
            if (di.GetFiles().Length == 0 && di.GetDirectories().Length == 0)
            {
                di.Delete();
            }

            foreach (Attach img in InlineImageList)
            {
                bodymsg = Replace(bodymsg, img.ServerFileName, dataUrl + img.AttachID);
            }

            return bodymsg;
        }

        public static string ConvertHtmlChars(string source)
        {
            string convert;
            convert = Replace(source, "<", "&lt;");
            convert = Replace(convert, ">", "&gt;");
            convert = Replace(convert, "\"", "&quot;");
            convert = Replace(convert, "&nbsp;", "&amp;nbsp;");

            return convert;
        }

        public static string Replace(string source, string oldValue, string newValue)
        {
            StringBuilder sb;

            sb = new StringBuilder(source);
            sb.Replace(oldValue, newValue);
            return sb.ToString();
        }

        public static string SetBaseURL(string source, string BaseURL = "")
        {
            if(string.IsNullOrEmpty(BaseURL))
            {
                BaseURL = ConfigurationManager.AppSettings["BaseURL"] ?? string.Empty;
                if (string.IsNullOrEmpty(BaseURL))
                {
                    BaseURL = "http://" + System.Web.HttpContext.Current.Request.Url.Host;
                    if (!System.Web.HttpContext.Current.Request.Url.Port.Equals("80"))
                    {
                        BaseURL += ":" + System.Web.HttpContext.Current.Request.Url.Port;
                    }
                    BaseURL += "/";
                }
                Log4NetHelper.Info(System.Web.HttpContext.Current.Request.Url);
            }

            StringBuilder sb;
            sb = new StringBuilder(source);
            //<img style="width: 800px; height: 480px;" src="/NamoFiles/aa068022-4f59-4f02-97eb-76b585a02192/302262_275713649145921_114440047_n.jpg">
            sb.Replace("src=\"/", "src=\"" + BaseURL);
            return sb.ToString();
        }
    }
}
