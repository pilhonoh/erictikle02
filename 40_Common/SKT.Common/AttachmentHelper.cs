using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;
using System.Drawing.Imaging;
using System.Security.Principal;
using System.Web;
namespace SKT.Common
{
    public static class AttachmentHelper
    {
        #region 썸네일
        private static string ThumbnailFilePath = ConfigurationManager.AppSettings["ThumbnailFilePath"];
        private static int ThumbnailWidth = Convert.ToInt32(ConfigurationManager.AppSettings["ThumbnailWidth"] ?? "190");


        /// <summary>
        /// 파일 크기의 단위
        /// </summary>
        private static readonly string[] SizeSuffixes = { "B", "KB", "MB", "GB", "TB", "PB" };
        
        //폭이 190px고정된 썸네일을 만든다
        //public static void ProcessThumbnail(BoardType board, string UniqueItemPath)
        public static void ProcessThumbnail(string Body, string UniqueItemPath, List<Attach> AttachLis, string ItemID,  string Mode = "INSERT")
        {
            string ThumbnailTargetImgPath = string.Empty;
            string ThumbnailFileFullPath = string.Empty;

            Attach attach = new Attach();
            attach.ItemID = ItemID;

            int ThumbnailCnt = 0;

            //1. 썸네일 구하기
            if (Mode == "INSERT")
            {
                string regexImgSrc = "<img.+?src=[\"'](.+?)[\"'].+?>";
                Match matchesImgSrc = Regex.Match(Body, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                if (!string.IsNullOrEmpty(matchesImgSrc.Groups[1].Value) && matchesImgSrc.Groups[1].Value.Contains("InlineImageID="))
                {
                    string InlineImageID = matchesImgSrc.Groups[1].Value.Split('=')[1];
                    foreach (Attach att in AttachLis)
                    {
                        if (att.AttachID.Equals(InlineImageID))
                        {
                            attach = att;
                            ThumbnailTargetImgPath = att.ServerFileName;
                            ThumbnailCnt = 1;

                            MakeThumNameFile(ThumbnailCnt,ThumbnailTargetImgPath, attach, UniqueItemPath);
                        }
                    }
                    //ThumbnailTargetImgPath = HttpContext.Current.Server.MapPath("~/") + matchesImgSrc.Groups[1].Value;
                    //ThumbnailCnt = 1;
                }
            }
            else
            {
                //일루오면 무조건 AttachList 값이 있다고 간주한다.
                string regexImgSrc = "<img.+?src=[\"'](.+?)[\"'].+?>";

                //Match[] matches = Regex.Matches(Body, regexImgSrc).Cast<Match>().ToArray();

                Match matchesImgSrc = Regex.Match(Body, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                //foreach (Match matchesImgSrc in matches)
                {
                    if (!string.IsNullOrEmpty(matchesImgSrc.Groups[1].Value) && matchesImgSrc.Groups[1].Value.Contains("InlineImageID="))
                    {
                        string InlineImageID = matchesImgSrc.Groups[1].Value.Split('=')[1];

                        long result;
                        if (long.TryParse(InlineImageID, out result) == true)
                        {
                            Attach inlineattach = AttachmentHelper.Select(long.Parse(InlineImageID), true);
                            ThumbnailTargetImgPath = inlineattach.ServerFileName;
                            MakeThumNameFile(1, ThumbnailTargetImgPath, inlineattach, UniqueItemPath);
                        }
                        else //여기는 마이그레이션 항목일테니.... 처리를 따로 해주어야한다...
                        {
                            Attach inlineattach = AttachmentHelper.GetAttachFromOldFileName(InlineImageID, AttachType.InlineImage);
                            ThumbnailTargetImgPath = inlineattach.ServerFileName;
                            MakeThumNameFile(1, ThumbnailTargetImgPath, inlineattach, UniqueItemPath);
                        }

                    }
                    else
                    {
                        Attach inlineattach = new Attach();
                        MakeThumNameFile(0, ThumbnailTargetImgPath, inlineattach, UniqueItemPath);  //이건지우기가 될듯..
                    }
                }
                               
                
            }
            
        }
        public static void MakeThumNameFile(int ThumbnailCnt,string ThumbnailTargetImgPath, Attach attach,  string UniqueItemPath)
        {
            string ThumbnailFileFullPath = string.Empty;
            if (ThumbnailCnt == 1 && File.Exists(ThumbnailTargetImgPath))
                using (System.Drawing.Image imgFull = System.Drawing.Image.FromFile(ThumbnailTargetImgPath))
                {
                    //2. 이미지 크기변경
                    decimal dOrigWidth = imgFull.Width;
                    decimal dOrigHeight = imgFull.Height;
                    decimal dRatio;
                    int iNewX;
                    int iNewY;

                    dRatio = ThumbnailWidth / dOrigWidth;

                    iNewX = Convert.ToInt32(dOrigWidth * dRatio);
                    iNewY = Convert.ToInt32(dOrigHeight * dRatio);

                    //3. 확장자 구하기
                    System.Drawing.Imaging.ImageFormat ifFormat = imgFull.RawFormat;
                    string ThumbnailExtension = GetImageExtension(ifFormat);

                    //4. 파일 저장
                    using (System.Drawing.Bitmap imgOutput = new System.Drawing.Bitmap(imgFull, iNewX, iNewY))
                    {
                        using (System.Drawing.Graphics gfxResizer = System.Drawing.Graphics.FromImage(imgOutput))
                        {
                            gfxResizer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            gfxResizer.DrawImage(imgFull, 0, 0, iNewX, iNewY);

                            string sSaveName = Path.GetFileName(ThumbnailTargetImgPath);
                            string sSavePath = AttachmentHelper.GetTargetDirectory(ThumbnailFilePath, UniqueItemPath);
                            ThumbnailFileFullPath = sSavePath + "\\" + sSaveName;
                            imgOutput.Save(ThumbnailFileFullPath, ifFormat);
                        }
                    }
                }

            //5. DB기록

            attach.AttachType = AttachType.Thumbnail;
            if (ThumbnailCnt != 0)
            {
                //attach.FileName = fileUpload["DextuploadX"][i].FileName;//클라이언트
                attach.FileName = Path.GetFileName(ThumbnailFileFullPath);
                attach.ServerFileName = Path.GetFullPath(ThumbnailFileFullPath);
                attach.Extension = Path.GetExtension(ThumbnailFileFullPath);
                attach.Folder = Path.GetDirectoryName(ThumbnailFileFullPath);
                attach.FileSize = new FileInfo(ThumbnailFileFullPath).Length;
            }
            else
            {
                attach.DeleteYN = "Y";
            }


            string OldThumbnailFolder = AttachmentHelper.InsertThumbnail(attach);

            //6. 이전 썸네일이 있다면 지운다
            if (!string.IsNullOrEmpty(OldThumbnailFolder) && Directory.Exists(OldThumbnailFolder))
            {
                Directory.Delete(OldThumbnailFolder, true);
            }
        }

        public static string GetImageExtension(ImageFormat format)
        {
            return ImageCodecInfo.GetImageEncoders()
                                 .First(x => x.FormatID == format.Guid)
                                 .FilenameExtension
                                 .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                 .First()
                                 .Trim('*');
        }
        public static string GetImageMimeType(ImageFormat format)
        {
            return ImageCodecInfo.GetImageEncoders()
                                 .First(x => x.FormatID == format.Guid)
                                 .MimeType
                                 .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                 .First()
                                 .Trim('*');
        }
        public static string InsertThumbnail(Attach attach)
        {
            using (AttachDac dac = new AttachDac())
            {
                DataSet ds = dac.Insert(attach);
                if (!ds.Tables[0].Rows[0].IsNull("OldThumbnailFolder"))
                {
                    return ds.Tables[0].Rows[0]["OldThumbnailFolder"].ToString();
                }
                return string.Empty;
            }
        }

        public static List<Attach> SelectThumbnail(Int64 ItemID, int BoardID)
        {
            List<Attach> list;

            using (AttachDac dac = new AttachDac())
            {
                DataSet ds = dac.Select(ItemID, BoardID, Convert.ToInt32(AttachType.Thumbnail));
                list = GetAttachListFromDataSet(ds, true);
            }
            return list;
        }
        #endregion 썸네일 끝

        #region 인라인 이미지 처리
        //인라인이미지 DB기록
        public static List<Attach> InsertInlineImage(string dataFolder, string BoardID, string ItemID, string UserID)
        {
            List<Attach> AttachList = new List<Attach>();
            DirectoryInfo di = new DirectoryInfo(dataFolder);
            if (di.Exists)
            { 
                FileInfo[] afi = di.GetFiles();

                foreach (FileInfo fi in afi)
                {
                    Attach attach = new Attach();

                    attach.AttachID = string.Empty;
                    attach.BoardID = BoardID;
                    attach.ItemID = ItemID;
                    attach.AttachType = AttachType.InlineImage;
                    //attach.FileName = fileUpload["DextuploadX"][i].FileName;//클라이언트
                    attach.FileName = Path.GetFileName(fi.FullName);
                    attach.ServerFileName = Path.GetFullPath(fi.FullName);
                    attach.Extension = Path.GetExtension(fi.FullName);
                    attach.Folder = Path.GetDirectoryName(fi.FullName);
                    attach.FileSize = new FileInfo(fi.FullName).Length;
                    attach.AuthorID = UserID;

                    attach.AttachID = AttachmentHelper.Insert(attach).ToString();
                    AttachList.Add(attach);
                }
            }
            return AttachList;
        }


        public static string GetInlineImagePath(Int64 AttachID)
        {
            string InlineImagePath = string.Empty;
            Attach InlineImage = Select(AttachID, true);
            if (InlineImage.AttachType == AttachType.InlineImage)
                InlineImagePath = InlineImage.ServerFileName;
            return InlineImagePath;
        }
        #endregion 인라인 이미지 처리 끝

        // 이하 첨부 공통 관련 부분
        
        /// <summary>
        /// 게시물과 관련된 모든 파일 삭제하기 
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="BoardID"></param>
        public static void DeleteItemAllAttachment(Int64 ItemID, Int64 CommonID, int BoardID = 0)
        {
            //1. 게시물에 해당되는 첨부/썸네일/인라인 조회
            List<Attach> AllAttachment = AttachmentHelper.SelectAllAttachment(ItemID, CommonID, BoardID);
            for (int i = 0; i < AllAttachment.Count; i++)
            {
                string AttachServerFileName = AllAttachment[i].ServerFileName;
                string AttachFolder = AllAttachment[i].Folder;
                if (AttachServerFileName != "")
                {
                    DirectoryInfo di = new DirectoryInfo(AttachFolder);
                    //2. 디렉토리가 있다면 디렉토리채로 지운다
                    if (di.Exists)
                    {
                        Directory.Delete(AttachFolder, true);
                    }
                    //3. DB에서 삭제 표시한다
                    AttachmentHelper.Delete(AllAttachment[i].AttachID);

                }
            }
        }
        public static long Insert(Attach attach)
        {
            using (AttachDac dac = new AttachDac())
            {
                DataSet ds = dac.Insert(attach);
                return  Convert.ToInt64(ds.Tables[0].Rows[0]["AttachID"]);;
            }
        }

        /// <summary>
        /// 하위 호환을 위해 ItemGuid 파라미터를 사용하는 별도의 Insert 메서드를 구현함.
        /// </summary>
        /// <param name="attach"></param>
        /// <returns></returns>
        public static long InsertWithItemGuid(Attach attach)
        {
            using (AttachDac dac = new AttachDac())
            {
                using (DataSet ds = dac.InsertWithItemGuid(attach))
                {
                    return Convert.ToInt64(ds.Tables[0].Rows[0]["AttachID"]);
                }
            }
        }

        public static void Update(string AttachIDs, Int64 ItemID, int BoardID = 0)
        {
            using (AttachDac dac = new AttachDac())
            {
                dac.Update(AttachIDs,ItemID,BoardID);
            }
        }

        /// <summary>
        /// ItemGuid 값에 해당하는 Attach의 ItemID 값 갱신
        /// </summary>
        /// <param name="ItemGuid"></param>
        /// <param name="ItemID"></param>
        /// <param name="BoardID"></param>
        public static void UpdateByItemGuid(Guid ItemGuid, long ItemID, int BoardID = 0)
        {
            using (AttachDac dac = new AttachDac())
            {
                dac.UpdateByItemGuid(ItemGuid, ItemID, BoardID);
            }
        }

        public static void Delete(string AttachIDs)
        {
            using (AttachDac dac = new AttachDac())
            {
                dac.Delete(AttachIDs);
            }
        }

        //이철수 추가 
        public static void Delete2(string AttachIDs)
        {
            using (AttachDac dac = new AttachDac())
            {
                dac.Delete2(AttachIDs);
            }
        }


        public static List<Attach> Select(Int64 ItemID, Int64 CommonID, int BoardID)
        {
            List<Attach> list;

            using (AttachDac dac = new AttachDac())
            {
                DataSet ds = dac.Select(ItemID, CommonID, BoardID);
                //list = GetAttachListFromDataSet(ds, false);

                list = GetAttachListFromDataSet(ds, true); 
            }
            return list;
        }


        //2015.10.27 zz17779 : 첨부파일 가져오기
        public static DataSet SelectAttach(Int64 ItemID, Int64 CommonID, int BoardID)
        {

            DataSet ds = new DataSet();

            using (AttachDac dac = new AttachDac())
            {
                 ds = dac.Select(ItemID, CommonID, BoardID);
            }

            return ds;
        }


        public static List<Attach> Select(Int64 ItemID, int BoardID)
        {
            List<Attach> list;

            using (AttachDac dac = new AttachDac())
            {
                DataSet ds = dac.Select(ItemID, ItemID, BoardID);
                list = GetAttachListFromDataSet(ds, false);
            }
            return list;
        }

        public static Attach Select(Int64 AttachID, bool RealFileInfo = false)
        {
            Attach attach = new Attach();

            using (AttachDac dac = new AttachDac())
            {
                DataSet ds = dac.Select(AttachID);
                List<Attach> list = GetAttachListFromDataSet(ds, RealFileInfo);
                if (list.Count > 0)
                {
                    attach = list[0];
                }
            }
            return attach;
        }
        public static List<Attach> SelectAllAttachment(Int64 ItemID, Int64 CommonID, int BoardID)
        {
            List<Attach> list;

            using (AttachDac dac = new AttachDac())
            {
                DataSet ds = dac.SelectAllAttachment(ItemID, CommonID, BoardID);
                list = GetAttachListFromDataSet(ds, true);
            }
            return list;
        }

        public static string ReplaceFileName(string strFileName)
        {
            /*
             * 쉐어포인트에서 허용하지 않는 특수문자를 언더바로 치환한다.
             * \ / : * ? " < > | # { } % ~ & '
             */
            //string removeSimbol = strFileName.Replace("\\", "_");

            //removeSimbol = removeSimbol.Replace("/", "_");
            //removeSimbol = removeSimbol.Replace(":", "_");
            //removeSimbol = removeSimbol.Replace("\"", "_");
            string removeSimbol = strFileName.Replace("*", "_");
            removeSimbol = removeSimbol.Replace("?", "_");
            removeSimbol = removeSimbol.Replace("<", "_");
            removeSimbol = removeSimbol.Replace(">", "_");
            removeSimbol = removeSimbol.Replace("|", "_");
            removeSimbol = removeSimbol.Replace("#", "_");
            removeSimbol = removeSimbol.Replace("{", "_");
            removeSimbol = removeSimbol.Replace("}", "_");
            removeSimbol = removeSimbol.Replace("%", "_");
            removeSimbol = removeSimbol.Replace("~", "_");
            removeSimbol = removeSimbol.Replace("&", "_");
            removeSimbol = removeSimbol.Replace("'", "_");

            return removeSimbol;
        }

        private static List<Attach> GetAttachListFromDataSet(DataSet ds, bool RealFileInfo = true)
       // private static List<Attach> GetAttachListFromDataSet(DataSet ds, bool RealFileInfo = false)
        {
            List<Attach> list = new List<Attach>();
            Attach attach;
            long allFileSize = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    attach = new Attach();

                    attach.AttachID = dr["AttachID"].ToString();
                    attach.BoardID = dr["BoardID"].ToString();
                    attach.ItemID = dr["ItemID"].ToString();

                    attach.AttachType = (AttachType)Enum.Parse(typeof(AttachType), dr["AttachType"].ToString());
                    //attach.FileName = dr["FileName"].ToString();

                    //attach.ServerFileName = RealFileInfo ? dr["ServerFileName"].ToString() : string.Empty;


                    attach.FileName = ReplaceFileName(dr["FileName"].ToString());
                    attach.ServerFileName = RealFileInfo ? (ReplaceFileName(dr["ServerFileName"].ToString())) : string.Empty;

                    attach.Extension = dr["Extension"].ToString();
                    attach.Folder = RealFileInfo ? dr["Folder"].ToString() : string.Empty;
                    attach.FileSize = Convert.ToInt64(dr["FileSize"]);
                    attach.AuthorID = dr["AuthorID"].ToString();
                    attach.DeleteYN = dr["DeleteYN"].ToString();

                    if (dr.Table.Columns.Contains("ItemGuid"))
                    {
                        attach.ItemGuid = dr["ItemGuid"] == DBNull.Value ? Guid.Empty : dr.Field<Guid>("ItemGuid");
                    }

                    //2015.10.26 zz17779 : 파일업로드 변경관련 추가
                    if ((attach.BoardID == "315" || attach.BoardID == "314" || attach.BoardID == "316") && dr.Table.Columns.Contains("ItemGuid"))
                    {
                        attach.FileSizeString = GetFileSizeToString2(long.Parse(dr["FileSize"].ToString()));
                    }

                    allFileSize  += long.Parse(dr["FileSize"].ToString());

                    attach.TotalFileSize = GetFileSizeToString2(allFileSize);




                    // Mr.No 2015-07-01
                    // Mobile Download Full Path Setting
                    string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
                    string DownloadPath = System.Configuration.ConfigurationManager.AppSettings["DownloadControlServerHandlerUrl"];
                    // Folder=Glossary 고정
                    attach.DownloadUrl = BaseURL + DownloadPath + "?" + "Folder=Glossary" + "&ItemGuid=" + attach.ItemGuid + "&FileName=" + System.Web.HttpUtility.UrlEncode(attach.FileName);
                    
                    list.Add(attach);
                }
            }
            return list;
        }


        /// <summary>
        /// 파일 크기를 문자 형태로 가져온다.
        /// </summary>
        /// <param name="fileSize">파일 크기</param>
        /// <returns>파일 크기 문자열(ex: 500MB)</returns>
        public static string GetFileSizeToString2(long fileSize)
        {
            string size = string.Empty;

            try
            {
    

                if (fileSize < 0)
                {
                    return "-" + GetFileSizeToString2(-fileSize);
                }
                if (fileSize == 0)
                {
                    return "0 B";
                }

                int mag = (int)Math.Log(fileSize, 1024);
                decimal adjustedSize = (decimal)fileSize / (1L << (mag * 10));

                size = string.Format("{0:n2} {1}", adjustedSize, SizeSuffixes[mag]);

                size = size.Replace(".00", ""); 
            }
            catch
            {
                throw;
            }

            return size;
        }

        /// <summary>
        /// 파일 크기를 문자 형태로 가져온다.
        /// </summary>
        /// <param name="fileSize">파일 크기</param>
        /// <param name="dataType"></param>
        /// <returns>파일 크기 문자열</returns>
        public static string GetFileSizeToString2(long fileSize, out string dataType)
        {
            string size = string.Empty;
            dataType = "B";

            try
            {

                if (fileSize < 0)
                {
                    return "-" + GetFileSizeToString2(-fileSize);
                }
                if (fileSize == 0)
                {
                    dataType = "B";

                    return "0";
                }

                int mag = (int)Math.Log(fileSize, 1024);
                decimal adjustedSize = (decimal)fileSize / (1L << (mag * 10));

                dataType = SizeSuffixes[mag];

                size = string.Format("{0:n0}", adjustedSize);
            }
            catch
            {
                throw;
            }

            return size;
        }







        public static Attach GetAttachFromOldFileName(string FileName,Common.AttachType type)
        {
            Attach ret = new Attach();
            List<Attach> list = new List<Attach>();

            using (AttachDac dac = new AttachDac())
            {
                DataSet ds = dac.GetImageServerFileName(FileName);

                list = GetAttachListFromDataSet(ds, true);

                for (int i = 0; i < list.Count; i++)
                {
                    Attach pp = list[i];
                    if (pp.AttachType == type)
                    {
                        return pp;
                    }
                }
            }
            return ret;
        }

        public static string GetTargetDirectory(string ParentFilePath, string UniqueItemPath)
        {
            string TargetDirectory = string.Empty;

            TargetDirectory = Path.Combine(ParentFilePath, UniqueItemPath);

            if (!System.IO.Directory.Exists(TargetDirectory))
            {
                Directory.CreateDirectory(TargetDirectory);
                /*
                try
                {
                    //권한없을시 에러발생으로 임시권한부여 로직추가
                    
                    DirectoryInfo info = new DirectoryInfo(TargetDirectory);
                    DirectorySecurity security = info.GetAccessControl();

                    security.AddAccessRule(new FileSystemAccessRule(ConfigurationManager.AppSettings["DomainName"] + "\\Domain Users", FileSystemRights.FullControl, AccessControlType.Allow));

                    info.SetAccessControl(security);
                     
                }
                catch (IdentityNotMappedException ex)
                {
                    throw ex;
                }
                */
            }
            return TargetDirectory;
        }
    }

    public class Attach //Type
    {
        public string AttachID = string.Empty;
        public string BoardID = string.Empty;
        public string ItemID = string.Empty;
        public AttachType AttachType = AttachType.AttachmentFile;
        public string FileName = string.Empty;
        public string ServerFileName = string.Empty;
        public string Extension = string.Empty;
        public string Folder = string.Empty;
        public long FileSize = 0;
        public string AuthorID = string.Empty;
        public string DeleteYN = "N";
        // 2013-10-17 ItemGuid 추가. 게시글 고유키 값.
        public Guid ItemGuid = Guid.Empty;
        // Mr.No 2015-07-01
        public string DownloadUrl = string.Empty;

        //2015.10.26 zz17779 : 파일업로드 변경관련 추가
        public string FileSizeString = string.Empty;
        public string TotalFileSize = string.Empty;
    }

    public enum AttachType
    { 
        AttachmentFile,
        Thumbnail,
        InlineImage,
        InlineMove
    }

    public class AttachDac : IDisposable
    {
        private static string _connectionStringName = null;

        private string connectionStringName
        {
            get
            {
                string connectionStringName = string.Empty;
                if (_connectionStringName == null)
                {
                    connectionStringName = ConfigurationManager.AppSettings["AttachConnName"] ?? "ConnAttach";
                    _connectionStringName = connectionStringName;
                }
                else
                {
                    connectionStringName = _connectionStringName;
                }
                return connectionStringName;
            }
        }

        public DataSet Insert(Attach attach)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Attach_Insert");
            db.AddInParameter(cmd, "BoardID", DbType.String, attach.BoardID);
            db.AddInParameter(cmd, "ItemID", DbType.String, string.IsNullOrEmpty(attach.ItemID) ? (object)DBNull.Value : attach.ItemID);
            db.AddInParameter(cmd, "AttachType", DbType.Int32, attach.AttachType);
            db.AddInParameter(cmd, "FileName", DbType.String, string.IsNullOrEmpty(AttachmentHelper.ReplaceFileName(attach.FileName)) ? (object)DBNull.Value : AttachmentHelper.ReplaceFileName(attach.FileName));
            db.AddInParameter(cmd, "ServerFileName", DbType.String, string.IsNullOrEmpty(AttachmentHelper.ReplaceFileName(attach.ServerFileName)) ? (object)DBNull.Value : AttachmentHelper.ReplaceFileName(attach.ServerFileName));
            db.AddInParameter(cmd, "Extension", DbType.String, string.IsNullOrEmpty(attach.Extension) ? (object)DBNull.Value : attach.Extension);
            db.AddInParameter(cmd, "Folder", DbType.String, string.IsNullOrEmpty(attach.Folder) ? (object)DBNull.Value : attach.Folder);
            db.AddInParameter(cmd, "FileSize", DbType.Int64, attach.FileSize);
            db.AddInParameter(cmd, "AuthorID", DbType.String, string.IsNullOrEmpty(attach.AuthorID) ? (object)DBNull.Value : attach.AuthorID);
            db.AddInParameter(cmd, "DeleteYN", DbType.String, string.IsNullOrEmpty(attach.DeleteYN) ? (object)DBNull.Value : attach.DeleteYN);

            return db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 하위 호환을 위해 ItemGuid 파라미터를 사용하는 별도의 Insert 메서드를 구현함.
        /// </summary>
        /// <param name="attach"></param>
        /// <returns></returns>
        public DataSet InsertWithItemGuid(Attach attach)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Attach_Insert");
            db.AddInParameter(cmd, "BoardID", DbType.String, attach.BoardID);
            db.AddInParameter(cmd, "ItemID", DbType.String, string.IsNullOrEmpty(attach.ItemID) ? (object)DBNull.Value : attach.ItemID);
            db.AddInParameter(cmd, "AttachType", DbType.Int32, attach.AttachType);
            db.AddInParameter(cmd, "FileName", DbType.String, string.IsNullOrEmpty(AttachmentHelper.ReplaceFileName(attach.FileName)) ? (object)DBNull.Value : AttachmentHelper.ReplaceFileName(attach.FileName));
            db.AddInParameter(cmd, "ServerFileName", DbType.String, string.IsNullOrEmpty(AttachmentHelper.ReplaceFileName(attach.ServerFileName)) ? (object)DBNull.Value : AttachmentHelper.ReplaceFileName(attach.ServerFileName));
            db.AddInParameter(cmd, "Extension", DbType.String, string.IsNullOrEmpty(attach.Extension) ? (object)DBNull.Value : attach.Extension);
            db.AddInParameter(cmd, "Folder", DbType.String, string.IsNullOrEmpty(attach.Folder) ? (object)DBNull.Value : attach.Folder);
            db.AddInParameter(cmd, "FileSize", DbType.Int64, attach.FileSize);
            db.AddInParameter(cmd, "AuthorID", DbType.String, string.IsNullOrEmpty(attach.AuthorID) ? (object)DBNull.Value : attach.AuthorID);
            db.AddInParameter(cmd, "DeleteYN", DbType.String, string.IsNullOrEmpty(attach.DeleteYN) ? (object)DBNull.Value : attach.DeleteYN);
            db.AddInParameter(cmd, "ItemGuid", DbType.Guid, attach.ItemGuid == Guid.Empty ? (object)DBNull.Value : attach.ItemGuid);
            return db.ExecuteDataSet(cmd);
        }

        public void Update(string AttachIDs, Int64 ItemID, int BoardID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Attach_Update");
            db.AddInParameter(cmd, "AttachIDs", DbType.String, AttachIDs);
            db.AddInParameter(cmd, "ItemID", DbType.Int64, ItemID);
            db.AddInParameter(cmd, "BoardID", DbType.Int32, BoardID == 0 ? (object)DBNull.Value : BoardID);
            db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// ItemGuid 값에 해당하는 Attach의 ItemID 값 갱신
        /// </summary>
        /// <param name="ItemGuid"></param>
        /// <param name="ItemID"></param>
        /// <param name="BoardID"></param>
        public void UpdateByItemGuid(Guid ItemGuid, long ItemID, int BoardID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Attach_UpdateByItemGuid");
            db.AddInParameter(cmd, "ItemGuid", DbType.Guid, ItemGuid);
            db.AddInParameter(cmd, "ItemID", DbType.Int64, ItemID);
            db.AddInParameter(cmd, "BoardID", DbType.Int32, BoardID == 0 ? (object)DBNull.Value : BoardID);
            db.ExecuteDataSet(cmd);
        }

        public void Delete(string AttachIDs)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Attach_Delete");
            db.AddInParameter(cmd, "AttachIDs", DbType.String, AttachIDs);
            db.ExecuteDataSet(cmd);
        }

        //이철수 추가  
        public void Delete2(string AttachIDs)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Attach_Delete2");
            db.AddInParameter(cmd, "ItemID", DbType.String, AttachIDs);
            db.ExecuteDataSet(cmd);
        } 

        public DataSet Select(Int64 ItemID, Int64 CommonID, int BoardID, int AttachType = 0)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Attach_Select");
            db.AddInParameter(cmd, "ItemID", DbType.Int64, ItemID);
            db.AddInParameter(cmd, "CommonID", DbType.Int64, CommonID);
            db.AddInParameter(cmd, "BoardID", DbType.Int32, BoardID);
            db.AddInParameter(cmd, "AttachType", DbType.Int32, AttachType);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet Select(Int64 AttachID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Attach_SelectByAttachID");
            db.AddInParameter(cmd, "AttachID", DbType.Int64, AttachID);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet SelectAllAttachment(Int64 ItemID, Int64 CommonID, int BoardID)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_AllAttachment_Select");
            db.AddInParameter(cmd, "ItemID", DbType.Int64, ItemID);
            db.AddInParameter(cmd, "CommonID", DbType.Int64, CommonID);
            db.AddInParameter(cmd, "BoardID", DbType.Int32, BoardID);
            return db.ExecuteDataSet(cmd);
        }

        public DataSet GetImageServerFileName(string FileName)
        {
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd = db.GetStoredProcCommand("up_Attach_SelectByMigrationFileName");
            db.AddInParameter(cmd, "OldAttachFileName", DbType.String, FileName);
            return db.ExecuteDataSet(cmd);
        } 

        public void Dispose()
        {
        }
    }
}
