using System;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Web.Script.Serialization;
using SKT.Tnet;
using SKT.Tnet.Framework.Security;
using SKT.Tnet.Framework.Configuration;
using SKT.Tnet.Framework.Common;
using SKT.Tnet.Framework.Utilities;
using System.Security.Principal;



namespace SKT.Tnet.Controls
{

	public enum FileCtrlMode
	{
		EDIT,
		VIEW
	}

	/// <summary>
	/// 파일 업로드 컴포넌트
	/// </summary>
	[DefaultProperty("ID"), ToolboxData("<{0}:FileCtrl runat=server></{0}:FileCtrl>")]
	public class FileCtrl : HiddenField
	{

		// 저장 경로 지정
		public string SavePath { get; set; }

		// 삭제 되어진 파일을 반환
		public string DelFiles;

		// 파일 정보
		public List<Dictionary<string, object>> files;

		// 컨트롤 Mode
		public FileCtrlMode ViewType; 

		// 제한 확장자
		public string DeniedExt { get; set; }        

		#region >> 페이지 로드 이벤트

		/// <summary>
		///  Control이 로드 될때 Hidden의 값을 객체로 변환하여 준다. 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.Page.Form.Enctype = "multipart/form-data";

			if (!string.IsNullOrEmpty(this.Value))
			{
				files = this.Value.JsonHelper_Deserialize();
			}
		}

		#endregion

		#region >> File Data 초기화
		/// <summary>
		///  File Data를 바인딩 하여 준다. 
		/// </summary>
		/// <param name="fileData"></param>
		public void FileDataBind(DataTable fileData)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();
			Dictionary<string, object> file;
			files = new List<Dictionary<string, object>>();

			foreach (DataRow dr in fileData.Rows)
			{
				file = new Dictionary<string, object>();

				file.Add("FileKey",		dr["FileKey"].ToString());
				file.Add("FileName",	dr["FileName"].ToString());
				file.Add("FileExt",		dr["FileExt"].ToString());
				file.Add("FileSize",	dr["FileSize"].ToString());
				file.Add("FilePath",	dr["FilePath"].ToString());
				file.Add("FileMode", "");

				files.Add(file);
			}

			this.Value = files.JsonHelper_Serialization();
		}
		#endregion

        // 파일 저장 관련 Method

        #region >> IsFileSizeLimitOver : 파일 용량 제한 Over 확인
        /// <summary>
        /// 파일 용량 제한 Over 확인
        /// </summary>
        /// <returns></returns>
        public bool IsFileSizeLimitOver()
        {

            float curFileSize = 0;  
            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                if (HttpContext.Current.Request.Files.GetKey(i) == string.Format(this.ClientID + "_Files"))
                {
                    curFileSize = curFileSize + HttpContext.Current.Request.Files[i].ContentLength; 
                }
            }

            return curFileSize > int.Parse(ConfigReader.GetString("SKTSection", "DefaultSettings", "FileUploadLimitSize")); 
        }
        #endregion 

        // 파일 저장 관련 Method

        #region >> IsUploadFileCheck : 파일 용량 제한 Over 확인
        /// <summary>
        /// 파일 용량 제한 Over 확인
        /// </summary>
        /// <returns></returns>
        public string IsUploadFileCheck()
        {
            string returnValue = string.Empty; 
            string fileExt = string.Empty;
            string fileName = string.Empty;

            string DeniedExt_01 = string.IsNullOrEmpty(DeniedExt) && ConfigReader.ExistValue("SKTSection", "DefaultSettings", "FileUploadDeniedExt") ? ConfigReader.GetString("SKTSection", "DefaultSettings", "FileUploadDeniedExt") : DeniedExt;


            float curFileSize = 0;
            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {


                if (HttpContext.Current.Request.Files.GetKey(i) == string.Format(this.ClientID + "_Files"))
                {
                    HttpPostedFile nowFile = HttpContext.Current.Request.Files[i];
                    if (nowFile.ContentLength > 0)
                    {
                        fileName = nowFile.FileName.Split('\\')[nowFile.FileName.Split('\\').Length - 1].ToString();
                        fileExt = fileName.Substring(fileName.LastIndexOf('.') + 1).ToUpper();

                        if (fileExt != "" && DeniedExt_01.ToUpper().IndexOf(fileExt.ToUpper()) >= 0)
                        {
                            returnValue = "EXTDENIED";
                            break;
                        }
                        curFileSize = curFileSize + HttpContext.Current.Request.Files[i].ContentLength;
                    }
                }
            }

            if (curFileSize > int.Parse(ConfigReader.GetString("SKTSection", "DefaultSettings", "FileUploadLimitSize")))
            {
                returnValue = "SIZEOVER"; 
            }

            return returnValue; 
        }
        #endregion 

        #region >> Save : 파일 저장
        /// <summary>
		///  신규 파일을 저장 하고 삭제할 파일을 삭제하여 준다.         
		/// </summary>
		/// <returns></returns>
		public List<Dictionary<string, object>> Save()
		{
            Impersonation im = new Impersonation();
            im.ImpersonationStart();
			
            //if (files == null) files = new List<Dictionary<string, object>>();
            files = new List<Dictionary<string, object>>();

			// 신규 등록 파일을 저장하고 files 객체를 갱신하여 준다. 
			foreach (Dictionary<string, object> addFile in this._SaveFile())
			{
				files.Add(addFile);	 
            }

            im.ImpersonationEnd();

			return files;
		}

        

		#endregion

		#region >> _SaveFile : 신규 파일을 등록
		/// <summary>
		///		신규 파일을 등록하고 등록한 정보를 반환
		/// </summary>
		/// <returns></returns>
        private List<Dictionary<string, object>> _SaveFile()
        {

            Dictionary<string, object> file;
            string fileKey = string.Empty;
            string saveFullPath = string.Empty;
            string fileName = string.Empty;
            string fileExt = string.Empty;

            // 저장 디렉토리를 생성 하여 준다. 
            string saveDir = string.Format(ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, "NamoWebEditor", "FileRootKOR"), SavePath);


            if (HttpContext.Current.Request.Files.Count > 0 && !Directory.Exists(HttpContext.Current.Server.MapPath(saveDir)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(saveDir));
            }

            // 삭제되어진 정보 정리
            List<Dictionary<string, object>> newfiles = new List<Dictionary<string, object>>();
            if (files == null) files = new List<Dictionary<string, object>>();
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i]["FileMode"].ToString() != "D")
                {
                    newfiles.Add(files[i]);
                }
            }
            files = newfiles;


            // 추가된 파일 정보를 저장
            newfiles = new List<Dictionary<string, object>>();
            try
            {

                for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    if (HttpContext.Current.Request.Files.GetKey(i) == string.Format(this.ClientID + "_Files"))
                    {
                        HttpPostedFile nowFile = HttpContext.Current.Request.Files[i];
                        if (nowFile.ContentLength > 0)
                        {
                            fileKey = Guid.NewGuid().ToString("N");

                            fileName = nowFile.FileName.Split('\\')[nowFile.FileName.Split('\\').Length - 1].ToString();
                            fileExt = fileName.Substring(fileName.LastIndexOf('.') + 1).ToUpper();


                            //saveFullPath = saveDir + @"\" + fileKey + "." + fileExt;

                            saveFullPath = saveDir + @"\" + fileName; 
                            nowFile.SaveAs(HttpContext.Current.Server.MapPath(saveFullPath));

                            file = new Dictionary<string, object>();
                            file.Add("FileKey", fileKey);
                            file.Add("FileName", fileName);
                            file.Add("FileExt", fileExt);
                            file.Add("FileSize", nowFile.ContentLength);
                            file.Add("FilePath", saveFullPath);
                            file.Add("FileMode", "N");



                            newfiles.Add(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                foreach (Dictionary<string, object> delFile in newfiles)
                {
                    FileInfo fi = new FileInfo(HttpContext.Current.Server.MapPath(delFile["FilePath"].ToString()));
                    fi.Delete();
                }
                throw new Exception("File 업로드중 오류가 발생했습니다.\r\n" + ex.Message);
            }


            return newfiles;
        }




        private List<Dictionary<string, object>> _SaveFile(string FlagExist)
        {

            Dictionary<string, object> file;
            string fileKey = string.Empty;
            string saveFullPath = string.Empty;
            string fileName = string.Empty;
            string fileExt = string.Empty;

            // 저장 디렉토리를 생성 하여 준다. 
            string saveDir = string.Format(ConfigReader.GetString(CoreContants.DEFAULT_SECTION_NAME, "NamoWebEditor", "FileRootKOR"), SavePath);



            if (HttpContext.Current.Request.Files.Count > 0 && !Directory.Exists(HttpContext.Current.Server.MapPath(saveDir)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(saveDir));
            }



            // 삭제되어진 정보 정리
            List<Dictionary<string, object>> newfiles = new List<Dictionary<string, object>>();
            if (files == null) files = new List<Dictionary<string, object>>();
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i]["FileMode"].ToString() != "D")
                {
                    newfiles.Add(files[i]);
                }
            }
            files = newfiles;


            // 추가된 파일 정보를 저장
            newfiles = new List<Dictionary<string, object>>();
            try
            {

                for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    if (HttpContext.Current.Request.Files.GetKey(i) == string.Format(this.ClientID + "_Files"))
                    {
                        HttpPostedFile nowFile = HttpContext.Current.Request.Files[i];
                        if (nowFile.ContentLength > 0)
                        {
                            fileKey = Guid.NewGuid().ToString("N");

                            fileName = nowFile.FileName.Split('\\')[nowFile.FileName.Split('\\').Length - 1].ToString();
                            fileExt = fileName.Substring(fileName.LastIndexOf('.') + 1).ToUpper();


                            //saveFullPath = saveDir + @"\" + fileKey + "." + fileExt;

                            saveFullPath = saveDir + @"\" + fileName;
                            nowFile.SaveAs(HttpContext.Current.Server.MapPath(saveFullPath));

                            file = new Dictionary<string, object>();
                            file.Add("FileKey", fileKey);
                            file.Add("FileName", fileName);
                            file.Add("FileExt", fileExt);
                            file.Add("FileSize", nowFile.ContentLength);
                            file.Add("FilePath", saveFullPath);
                            file.Add("FileMode", "N");



                            newfiles.Add(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                foreach (Dictionary<string, object> delFile in newfiles)
                {
                    FileInfo fi = new FileInfo(HttpContext.Current.Server.MapPath(delFile["FilePath"].ToString()));
                    fi.Delete();
                }
                throw new Exception("File 업로드중 오류가 발생했습니다.\r\n" + ex.Message);
            }


            return newfiles;
        }











		#endregion

		/// <summary>
		///  파일 업로드 컴포넌트를 그려준다.
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(HtmlTextWriter writer)
		{

            string DeniedExt_01 = string.IsNullOrEmpty(DeniedExt) && ConfigReader.ExistValue("SKTSection", "DefaultSettings", "FileUploadDeniedExt") ? ConfigReader.GetString("SKTSection", "DefaultSettings", "FileUploadDeniedExt") : DeniedExt;

            writer.Write(string.Format("<div class='FileUploadCtrl'{0}>", string.IsNullOrEmpty(DeniedExt_01) ? "" : " DeniedExt='" + DeniedExt_01 + "'"));

			this.Value = files.JsonHelper_Serialization();
			base.Render(writer);

			StringBuilder currentFilesStr = new StringBuilder();
			if (files != null && files.Count > 0)
			{
				int i = 0;
				string fileLink;

				foreach (Dictionary<string, object> file in files)
				{
					fileLink = string.Format("<a href='javascript:void(0);' onClick='FileCtrl_FileDownload(\"{0}\", \"{1}\")'>{0}</a>", HttpUtility.HtmlEncode(file["FileName"]), HttpUtility.HtmlEncode(file["FilePath"]));

					currentFilesStr.AppendFormat("<tr FileName='{0}'>", file["FileName"]);
					currentFilesStr.AppendFormat("<td class='fileIcon'><img class='fileUploadTypeIcon' src='/Images/ICON/IC{0}.gif'></td>", file["FileExt"].ToString());
					currentFilesStr.AppendFormat("<td>{0}</td>", fileLink);
					currentFilesStr.AppendFormat("<td class='fileSize'>{0}</td>", FileUtility.GetFileSizeToString(long.Parse(file["FileSize"].ToString())));




					if (ViewType != FileCtrlMode.VIEW)
					{
						currentFilesStr.Append("<td  class='fileDel'>");
                        currentFilesStr.AppendFormat("<img src='/Images/ICON/DELETE.gif'  class='fileUploadDelIcon' onclick='javascript:FileCtrl_CurFileDelete(this, \"{0}\", \"{1}\",\"{2}\" )'>", this.ClientID, i.ToString(), file["FileKey"].ToString());
                        currentFilesStr.Append("</td>");
					}



					currentFilesStr.Append("</tr>");
					i++;
				}
			}

			writer.Write(string.Format(@"
			<div class='FileUloadTable'>
				<table id='{0}_dtFiles'>
					{1}
				</table>
			</div>", this.ClientID, currentFilesStr.ToString()));


			if (ViewType != FileCtrlMode.VIEW)
			{
				writer.Write(string.Format(@"
			<div class='FileUploadBtn'>
				<div id='{0}_divFiles' FileUploadObj='{0}' style='text-align: right;'>
                    <span>찾아보기</span>
					<input type='file' name='{0}_Files' id='{0}_File_0'  onChange='FileCtrl_FileChange();'/>
				</div>
			</div>
			<input type='hidden' ID='{0}_DelFiles' value=''/>
			", this.ClientID));
			};


			writer.Write("<script>FileCtrl_FileIconEmpty();</script></div>");
		}
	}



}
