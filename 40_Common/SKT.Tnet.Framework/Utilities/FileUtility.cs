using System;

namespace SKT.Tnet.Framework.Utilities
{
    /// <Summary>
    /// 파일 관련 도구 모음 클래스
    /// </Summary>
    /// <Remarks>
    /// - 작  성  자 : 네오플러스, 정재혁<br/>
    /// - 최초작성일 : 2015년 04월 01일<br/>
    /// - 주요변경로그<br/>
    ///   * 2015년 04월 01일 정재혁 최초작성<br/>
    /// </Remarks>
    public class FileUtility
    {
        /// <summary>
        /// 파일 크기의 단위
        /// </summary>
        private static readonly string[] SizeSuffixes = { "B", "KB", "MB", "GB", "TB", "PB" };

        /// <summary>
        /// 파일 크기 숫자 형태로 가져온다.(Byte)
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        /// <returns>파일 크기(long)</returns>
        public static long GetFileSizeByPath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) == true || !System.IO.File.Exists(filePath))
            {
                return 0;
            }

            System.IO.FileInfo fi = new System.IO.FileInfo(filePath);
            return fi.Length;
        }

        /// <summary>
        /// 파일 크기 문자 형태로 가져온다.
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        /// <returns>파일 크기(string)</returns>
        public static string GetFileSizeToStringByPath(string filePath)
        {
            long fileLength = GetFileSizeByPath(filePath);

            return GetFileSizeToString(fileLength);
        }

        /// <summary>
        /// 파일 크기를 문자 형태로 가져온다.
        /// </summary>
        /// <param name="fileSize">파일 크기</param>
        /// <returns>파일 크기 문자열(ex: 500MB)</returns>
        public static string GetFileSizeToString(long fileSize)
        {
            string size = string.Empty;

            try
            {
                //if (fileSize >= 1024 * 1024 * 1024)
                //{
                //    size = Convert.ToString(fileSize / (1024 * 1024 * 1024)) + "GB";
                //}
                //else if (fileSize >= 1024 * 1024)
                //{
                //    size = Convert.ToString(fileSize / (1024 * 1024)) + "MB";
                //}
                //else if (fileSize >= 1024)
                //{
                //    size = Convert.ToString(fileSize / (1024)) + "KB";
                //}
                //else
                //{
                //    size = Convert.ToString(fileSize) + "B";
                //}

                if (fileSize < 0)
                {
                    return "-" + GetFileSizeToString(-fileSize);
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
        public static string GetFileSizeToString(long fileSize, out string dataType)
        {
            string size = string.Empty;
            dataType = "B";

            try
            {
                //if (fileSize >= 1024 * 1024 * 1024)
                //{
                //    size = Convert.ToString(fileSize / (1024 * 1024 * 1024));
                //    dataType = "GB";
                //}
                //else if (fileSize >= 1024 * 1024)
                //{
                //    size = Convert.ToString(fileSize / (1024 * 1024));
                //    dataType = "MB";
                //}
                //else if (fileSize >= 1024)
                //{
                //    size = Convert.ToString(fileSize / (1024));
                //    dataType = "KB";
                //}
                //else
                //{
                //    size = Convert.ToString(fileSize);
                //    dataType = "B";
                //}

                if (fileSize < 0)
                {
                    return "-" + GetFileSizeToString(-fileSize);
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
    }
}