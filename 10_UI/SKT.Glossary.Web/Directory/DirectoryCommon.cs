using System;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;

using System.ServiceModel;
using System.ServiceModel.Channels;

using System.Web.Script.Serialization;

using SKT.Common;
using SKT.Common.TikleDocManagerService;
using SKT.Glossary.Web.Common.Controls;
using SKT.Glossary.Type;
using SKT.Glossary.Biz;
using SKT.Tnet.Framework.Security;


namespace SKT.Glossary.Web.Directory
{
    public class DirectoryCommon
    {
		//private string DSAPI_KEYFILE_PAHT;	// 문서보안 암/복호화 Key 파일 경로
		//private string DSAPI_PROPFILE_PATH;	// Service Linker 설정파일 경로
		//private string DSAPI_DEFAULT_USER;	// 문서보안 암/복호화 시 사용할 기본계정
		//private string DSAPI_AUTH_STR;		// 문서보안 암호화 시 보안설정 문자열

		//private string DIR_UPLOAD_FILE_PATH;	// 문서업로드 경로
		//private string DIR_ENC_FILE_PATH;		// 문서 암호화 시 저장 경로
		//private string DIR_DEC_FILE_PATH;		// 문서 복호화 시 저장 경로
        //private string DIR_SAMPLE_FILE_PATH;	// 기본문서 경로

        #region # Get Set
        // 문서보안 암/복호화 Key 파일 경로
		private string DSAPI_KEYFILE_PAHT
		{
			get
			{
				return ConfigurationManager.AppSettings["DSAPI_KEYFILE_PAHT"].ToString();
			}
		}

		// Service Linker 설정파일 경로
		private string DSAPI_PROPFILE_PATH
		{
			get
			{
				return ConfigurationManager.AppSettings["DSAPI_PROPFILE_PATH"].ToString();
			}
		}

		// 문서보안 암/복호화 시 사용할 기본계정
		private string DSAPI_DEFAULT_USER
		{
			get
			{
				return ConfigurationManager.AppSettings["DSAPI_DEFAULT_USER"].ToString();
			}
		}

		// 문서보안 암호화 시 보안설정 문자열
		private string DSAPI_AUTH_STR
		{
			get
			{
				return ConfigurationManager.AppSettings["DSAPI_AUTH_STR"].ToString();
			}
		}

        // 문서업로드 경로
        public string DIR_UPLOAD_FILE_PATH_back
        {
            get
            {
                //return ConfigurationManager.AppSettings["DIR_UPLOAD_FILE_PATH"].ToString();


                
                return ConfigurationManager.AppSettings["AttachFilePath"].ToString();
                
            }
        }

		// 문서업로드 경로
        /*
            Author : 개발자-장찬우G, 리뷰자-진현빈D 
            Create Date :  2016.06.29 
            Desc : nas장비로 변경
        */
		public string DIR_UPLOAD_FILE_PATH
		{
			get
			{
				//return ConfigurationManager.AppSettings["DIR_UPLOAD_FILE_PATH"].ToString();
                                
                string NAS_VirtualDirectory = SKT.Tnet.Framework.Configuration.ConfigReader.GetString("NAS_VirtualDirectory");
                string NAS_PhysicalPath = HttpContext.Current.Server.MapPath("/" + NAS_VirtualDirectory);
                return NAS_PhysicalPath;
			}
		}

		// 문서 암호화 시 저장 경로
		public string DIR_ENC_FILE_PATH
		{
			get
			{
				return ConfigurationManager.AppSettings["DIR_ENC_FILE_PATH"].ToString();
			}
		}

        // 문서 복호화 시 저장 경로
        public string DIR_DEC_FILE_PATH_back
        {
            get
            {

                
                return ConfigurationManager.AppSettings["DIR_DEC_FILE_PATH"].ToString();
                
            }
        }

		// 문서 복호화 시 저장 경로
        /*
            Author : 개발자-장찬우G, 리뷰자-진현빈D
            Create Date : 2016.06.29 
            Desc : nas장비로 변경
        */
        public string DIR_DEC_FILE_PATH
		{
			get
			{                
                string NAS_VirtualDirectory = SKT.Tnet.Framework.Configuration.ConfigReader.GetString("NAS_VirtualDirectory");
                string NAS_PhysicalPath = HttpContext.Current.Server.MapPath("/" + NAS_VirtualDirectory);
                return NAS_PhysicalPath;
			}
		}

        // 기본문서 경로
        public string DIR_SAMPLE_FILE_PATH_back
        {
            get
            {
               
                return ConfigurationManager.AppSettings["DIR_SAMPLE_FILE_PATH"].ToString();
                
            }
        }

		// 기본문서 경로
        /*
            Author : 개발자-장찬우G, 리뷰자-진현빈D 
            Create Date : 2016.06.29 
            Desc : nas장비로 변경
        */
        public string DIR_SAMPLE_FILE_PATH
		{
			get
			{
                string NAS_VirtualDirectory = SKT.Tnet.Framework.Configuration.ConfigReader.GetString("NAS_VirtualDirectory");
                string NAS_PhysicalPath = HttpContext.Current.Server.MapPath("/" + NAS_VirtualDirectory);
                NAS_PhysicalPath = NAS_PhysicalPath + @"/ShareFiles/Sample";
                return NAS_PhysicalPath;
			}
		}
        #endregion

        ///<summary>
		/// 디렉토리에 권한이 없는 구성원에게 조회권한 부여
		///</summary>
		///<param name="str">디렉토리ID</param>
		///<param name="str">파일명</param>
		///<param name="List<T_Authority>">대상자</param>
		///<param name="string">대상자</param>
		///<returns>string</returns>
        public string spSetFileReadPermission(string DirID, string strFileName, List<T_Authority> authorityList, string UserID)
        {

            string errMsg = string.Empty;

            //웹서비스  객체 생성  
            DocManagerServiceClient proxy = new DocManagerServiceClient();
            try
            {

                using (new OperationContextScope(proxy.InnerChannel))
                {
                    // Add a HTTP Header to an outgoing request
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["tikle"] = "31163105310731083101";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    Result result = proxy.SetFileReadPermission(DirID, strFileName, authorityList.ToArray<SKT.Common.TikleDocManagerService.T_Authority>(), "skt\\" + UserID);

                    
                    if (result.STATUS != 0)
                    {
                        //Response.Write("<script>alert('성공');</script>"); 
                    }
                }
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }
            return "";
        }

        /// <summary>
        /// 20141020 그룹 사용자 조회
        /// </summary>
        public List<CommonAuthType> DirectoryGroupUserList(string registerid, string groupid)
        {
            List<CommonAuthType> glossaryGroupUserlist = new List<CommonAuthType>();
            GlossaryMyGroupBiz biz = new GlossaryMyGroupBiz();
            DataSet ds = new DataSet();

            ds = biz.MyGroupListSelect2(registerid, groupid, "MyGroup");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    CommonAuthType temp = new CommonAuthType();
                    temp.AuthID = dr["ToUserID"].ToString();
                    temp.AuthName = dr["ToUserName"].ToString();
                    temp.AuthType = dr["ToUserType"].ToString();
                    temp.RegID = dr["REG_ID"].ToString();
                    temp.RegDTM = Convert.ToDateTime(dr["REG_DTM"]);
                    temp.DeptNumber = dr["DeptNumber"].ToString();

                    glossaryGroupUserlist.Add(temp);
                }
            }

            return glossaryGroupUserlist;
        }

        /// <summary>
        /// 20141021 접속자가 초대자 목록에 포함되는지 여부 체크
        /// </summary>
        public bool checkUserInGroup(List<CommonAuthType> glossaryAuthlist, UserInfo u)
        {
            for (int i = 0; i < glossaryAuthlist.Count; i++)
            {
                CommonAuthType auth = glossaryAuthlist[i];

                if (auth.AuthType == "U")
                {
                    if (auth.AuthID == u.UserID) { return true; }
                }
                else if (auth.AuthType == "G")
                {
                    List<CommonAuthType> glossaryGroupUserlist = new List<CommonAuthType>();
                    glossaryGroupUserlist = DirectoryGroupUserList(auth.RegID, auth.AuthID);

                    for (int j = 0; j < glossaryGroupUserlist.Count; j++)
                    {
                        CommonAuthType authSub = glossaryGroupUserlist[j];

                        if (authSub.AuthType == "U") { if (authSub.AuthID == u.UserID) { return true; } }
                        else if (authSub.AuthType == "O")
                        {
                            string[] arrDeptNumber = u.DeptPath.Split('/');
                            if (Array.IndexOf(arrDeptNumber, authSub.DeptNumber) >= 0) { return true; }
                        }
                    }
                }
                else if (auth.AuthType == "O")
                {
                    string[] arrDeptNumber = u.DeptPath.Split('/');
                    if (Array.IndexOf(arrDeptNumber, auth.AuthID) >= 0) { return true; }
                }
            }

            return false;
        }


        


        /// <summary>
        /// 문서함 사용자 저장
        /// </summary>
        /// public bool SaveUserList(string DIR_ID, string strUserID, UserAndDepartmentList UDList, string strMode = "Insert")
        public bool SaveUserList(string DIR_ID, string strUserID, string ToUserStr, string ToUserTypeStr, string strMode = "Insert")
        {
            string errMsg = string.Empty;
            string WorkType = (strMode == "Insert") ? "C" : "M"; // SharePoint용 변수

            GlossaryDirectoryAuthBiz DirAuth = new GlossaryDirectoryAuthBiz();

            //******************//
            //조직도 저장  
            //******************//
            //삭제후 인서트   
            try
            {
                //삭제   
                DirAuth.GlossaryDirectoryAuthDelete(DIR_ID, "Auth");

                //인서트
                //UDList.AuthCL은 auth_type 이다  
                DirAuth.GlossaryDirectoryAuthInsert(DIR_ID, strUserID, ToUserStr, ToUserTypeStr, "RW", strMode);

            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }

            //****************************//
            // Share Point 서버 저장   
            //***************************//

            List<T_Authority> authorityList = new List<T_Authority>();
            string[] ToUser = ToUserStr.Split('/');
            string[] ToUserType = ToUserTypeStr.Split('/');

            for (int i = 0; i < ToUser.Length; i++)
            {
                switch (ToUserType[i])
                {
                    case "U": // 사용자
                        authorityList.Add(new T_Authority() { AD_ID = "skt\\" + ToUser[i].ToString(), TYPE = "P" }); //개인 
                        break;
                    case "O": // 조직
                        authorityList.Add(new T_Authority() { AD_ID = "skt\\ORG" + ToUser[i].ToString(), TYPE = "G" });   //그룹 
                        break;
                    case "G":  // 그룹
                        List<string> MyGrpList = AjaxPage.GetMyGroupList("MyGroup", strUserID, ToUser[i].ToString());

                        if (MyGrpList.Count > 0)
                        {
                            foreach (String tmp in MyGrpList)
                            {
                                AjaxMyGroupList grpList = new JavaScriptSerializer().Deserialize<AjaxMyGroupList>(tmp);

                                switch (grpList.ToUserType)
                                {
                                    case "U": // 사용자
                                        authorityList.Add(new T_Authority() { AD_ID = "skt\\" + grpList.ToUserID.ToString(), TYPE = "P" }); //개인 
                                        break;
                                    case "O": // 조직
                                        authorityList.Add(new T_Authority() { AD_ID = "skt\\ORG" + grpList.ToUserID.ToString(), TYPE = "G" });   //그룹 
                                        break;
                                }
                            }
                        }
                        break;
                }
            }

            //웹서비스  객체 생성  
            SKT.Common.TikleDocManagerService.DocManagerServiceClient proxy = new DocManagerServiceClient();

            try
            {
                //**************//
                //1.폴더 생성
                //**************//
                using (new OperationContextScope(proxy.InnerChannel))
                {
                    // Add a HTTP Header to an outgoing request
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["tikle"] = "31163105310731083101";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    Result result = proxy.CreateFolder(DIR_ID, "skt\\" + strUserID, WorkType, authorityList.ToArray<T_Authority>());
                }


            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }

            //사용자 카운트 update
            UpdateUserCount(DIR_ID);

            // 추후 수정 필요 예외 발생 여부에 따라 처리
            return true;
        }

        /// <summary>
        /// 사용자 카운트 update
        /// </summary>
        public void UpdateUserCount(string DIR_ID)
        {
            GlossaryControlBiz commBiz = new GlossaryControlBiz();
            commBiz.commAuthUserCntUpdate(DIR_ID, "Directory");
        }

        //파일 리스트 조회
        public static T_FileInfo[] GetFileList(string DIR_ID, int topCnt = 5)
        {
            T_FileInfo[] arrFileLsit = null;
            string m = string.Empty;

            try
            {
                //웹서비스  객체 생성  
                SKT.Common.TikleDocManagerService.DocManagerServiceClient proxy = new DocManagerServiceClient();
                using (new OperationContextScope(proxy.InnerChannel))
                {
                    // Add a HTTP Header to an outgoing request
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["tikle"] = "31163105310731083101";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    arrFileLsit = proxy.GetFolderItems(DIR_ID, "", topCnt);
                }
            }
            catch (System.Exception ex)
            {
                m = ex.Message;
            }

            return arrFileLsit;
        }

		/// <summary>
		/// 폴더별 파일 갯수 조회
		/// </summary>
		public static string GetFileCount(string DIR_ID)
		{
			string FileCount = "0";

			try
			{
				//웹서비스  객체 생성  
				SKT.Common.TikleDocManagerService.DocManagerServiceClient proxy = new DocManagerServiceClient();

				using (new OperationContextScope(proxy.InnerChannel))
				{
					// Add a HTTP Header to an outgoing request
					HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
					requestMessage.Headers["tikle"] = "31163105310731083101";
					OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

					FileCount = proxy.GetFolderItemCount(DIR_ID, "").ToString();
				}

			}
			catch (System.ExecutionEngineException ex)
			{
				//Response.Write("<script>alert('" + ex.Message + "'");
				//Response.End();
			}

			return FileCount;

		}

        /// <summary>
        /// 파일 업로드
        /// </summary>
        public string FileUpload(string DIR_ID, string strUploadType, string strGUID, string strUserID, string strUserName, string strNewFileName = null)
        {
            string UploadFilePath = DIR_UPLOAD_FILE_PATH;
            string DecFilePath = DIR_DEC_FILE_PATH;

            string UploadFullFilePath = string.Empty;
            string DecFullFilePath = string.Empty;

            string errMsg = string.Empty;

            // 기본파일 선택일 경우 사용
            string srcFileName = string.Empty;
            string fileExt = string.Empty;
            string tgtFileName = string.Empty;

            FileStream freader = null;

            GlossaryDirectoryBiz biz = new GlossaryDirectoryBiz();

            GlossaryDirectoryFileType FileType = new GlossaryDirectoryFileType();
            List<Attach> dirList = new List<Attach>();

            if (strUploadType == "upload")
            {
                //**************************//
                //DEXT Upload 올리기 
                //**************************//
                Guid itemGUID = Guid.Empty;

                GuidTryParse(strGUID, out itemGUID);

                AttachmentHelper.UpdateByItemGuid(itemGUID, Convert.ToInt64(DIR_ID), 100);

                dirList = AttachmentHelper.Select(Convert.ToInt64(DIR_ID), Convert.ToInt64(10), 100);
            }
            else
            {
                //**************************//
                //기본파일 등록
                //**************************//
                Attach sampleAttach = new Attach();

                srcFileName = ConfigurationManager.AppSettings["Dir_SampleFile_Name_" + strUploadType].ToString();
                fileExt = ConfigurationManager.AppSettings["Dir_SampleFile_Ext_" + strUploadType].ToString();
                tgtFileName = strNewFileName + "." + fileExt;

                sampleAttach.FileName = srcFileName + "." + fileExt;
                sampleAttach.Extension = fileExt;
                sampleAttach.FileSize = 0;

                dirList.Add(sampleAttach);
            }

            for (int i = 0; i < dirList.Count; i++)
            {
                if (strUploadType == "upload")
                {
                    UploadFullFilePath = UploadFilePath + @"\" + dirList[i].Folder + @"\" + ReplaceFileName(dirList[i].FileName);
                    DecFullFilePath = DecFilePath + @"\" + ReplaceFileName(dirList[i].FileName);

                    tgtFileName = ReplaceFileName(dirList[i].FileName);
                }
                else
                {
                    UploadFullFilePath = DIR_SAMPLE_FILE_PATH + @"\" + ReplaceFileName(dirList[i].FileName);
                    DecFullFilePath = DecFilePath + @"\" + ReplaceFileName(tgtFileName);
                }

                //**************************//
                //업로드 파일 DB 저장 
                //**************************//
                FileType.DirID = DIR_ID;
                FileType.FileID = string.Empty;
                FileType.FileNM = ReplaceFileName(dirList[i].FileName);
                FileType.FileSize = dirList[i].FileSize.ToString();
                FileType.FileExt = dirList[i].Extension;
                FileType.RegID = strUserID;
                FileType.RegNM = strUserName;

                try
                {
                    biz.DirectoryFileInsert(FileType, "Insert");
                }
                catch (System.Exception exp)
                {
                    errMsg = exp.Message;
                }

                //2015-11-09 복호화는 진행하지 않는다.
                //업로드 파일일 경우 복호화를 하고 아닐경우 파일 카피
                //if (strUploadType == "upload")
                //{
                //    //**************************//
                //    //암호화/복호화  
                //    //**************************//

                //    //DSDecFileDAC(DSAPI_DEFAULT_USER, UploadFullFilePath, DecFullFilePath);

                //    if (ConfigurationManager.AppSettings["IsTestServer"].ToString().Equals("Y"))
                //    {
                //        File.Copy(UploadFullFilePath, DecFullFilePath, true);
                //    }
                //    else
                //    {
                //        DSDecFileDAC(DSAPI_DEFAULT_USER, UploadFullFilePath, DecFullFilePath);
                //    }



                //    //if (!DSDecFileDAC(DSAPI_DEFAULT_USER, UploadFullFilePath, DecFullFilePath))
                //    //{
                //    //  Response.Write("<script>alert('복호화과정에서 오류가 발생했습니다.');</script>");
                //    //}
                //}
                //else
                //{
                //File.Copy(UploadFullFilePath, DecFullFilePath, true);                  
                //}

                //****************//
                //3.파일저장  
                //****************//

                /*
                Author : 개발자-장찬우G, 리뷰자-진현빈D 
                Create Date : 2016.06.29 
                Desc : Impersonation
                */
                Impersonation im = new Impersonation();
                im.ImpersonationStart();
                try
                {
                    File.Copy(UploadFullFilePath, DecFullFilePath, true);
                    freader = File.OpenRead(DecFullFilePath);

                }
                catch (Exception ex)
                {
                    im.ImpersonationEnd();
                    throw ex;
                }
                //2017-11-13 아래 파일삭제 기능때문에
                //im.ImpersonationEnd();

                //freader = File.OpenRead(DecFullFilePath);
                byte[] buffer = new byte[freader.Length];
                freader.Read(buffer, 0, (int)freader.Length);


                //웹서비스  객체 생성  
                SKT.Common.TikleDocManagerService.DocManagerServiceClient proxy1 = new DocManagerServiceClient();

                try
                {
                    using (new OperationContextScope(proxy1.InnerChannel))
                    {
                        // Add a HTTP Header to an outgoing request
                        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                        requestMessage.Headers["tikle"] = "31163105310731083101";
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                        Result result = proxy1.UploadFile(DIR_ID, "skt\\" + strUserID, tgtFileName, buffer);
                    }
                }
                catch (System.Exception ex)
                {
                    errMsg = ex.Message;
                }
                finally
                {
                    freader.Close();
                }

                //파일삭제
                if (File.Exists(DecFullFilePath))
                {
                    File.Delete(DecFullFilePath);
                }

                if (strUploadType == "upload")
                {
                    ///New 파일삭제
                    if (File.Exists(UploadFullFilePath))
                    {
                        File.Delete(UploadFullFilePath);
                    }
                }

                im.ImpersonationEnd();

            }

            //파일삭제(DextUpload)
            AttachmentHelper.Delete2(DIR_ID);

            if (strUploadType == "upload")
            {
                ///New 폴더삭제
                if (dirList.Count > 0)
                {
                    if (!String.IsNullOrEmpty(dirList[0].Folder))
                    {
                        Impersonation im1 = new Impersonation();
                        im1.ImpersonationStart();

                        string FolderName = UploadFilePath + @"\" + dirList[0].Folder;

                        DirectoryInfo dir = new DirectoryInfo(FolderName);
                        if (dir.Exists)
                            dir.Delete();

                        im1.ImpersonationEnd();
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// 파일 다운로드
        /// </summary>
        public byte[] FileDownload(string DIR_ID, string FileName) 
        {
            string strMsg = string.Empty;
            string DecFileFullPath = DIR_DEC_FILE_PATH + @"\" + FileName;  // 원본 파일 전체 경로
            string EncFileFullPath = DIR_ENC_FILE_PATH + @"\" + FileName;  // 암호화 파일 전체 경로
            byte[] fstrem = null;
            int fileLen = 0;


            SKT.Common.TikleDocManagerService.DocManagerServiceClient proxy = new DocManagerServiceClient();

            /*
                Author : 개발자-장찬우G, 리뷰자-진현빈D
                Create Date : 2016.06.29 
                Desc : Impersonation
            */
            Impersonation im = new Impersonation();
            im.ImpersonationStart();

            try
            {
                using (new OperationContextScope(proxy.InnerChannel))
                {
                    // Add a HTTP Header to an outgoing request
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["tikle"] = "31163105310731083101";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    byte[] buffer = proxy.DownloadFile(DIR_ID, FileName);                    
                    
                    FileStream fs = new FileStream(DecFileFullPath, FileMode.Create);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();                    
                }

                //파일 암호화 
                //DSEncFileDAC(DSAPI_DEFAULT_USER, DecFileFullPath, EncFileFullPath);

                //암호화된 파일을 bytes로 읽어들인다
                //FileStream newfs = new FileStream(EncFileFullPath, FileMode.Open);
                FileStream newfs = new FileStream(DecFileFullPath, FileMode.Open);

                fileLen = (int)newfs.Length;
                fstrem = new byte[fileLen];

                int numBytesToRead = fileLen;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    int n = newfs.Read(fstrem, numBytesRead, numBytesToRead);

                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }

                newfs.Close();

                //파일삭제 
                if (File.Exists(DecFileFullPath))
                {
                    File.Delete(DecFileFullPath);
                }
                if (File.Exists(EncFileFullPath))
                {
                    File.Delete(EncFileFullPath);
                }
                
            }
            catch (System.Exception ex)
            {
                strMsg = ex.Message;
                im.ImpersonationEnd();
            }
            im.ImpersonationEnd();

            return fstrem;
        }

        // Guid.TryParse 를 닷넷 4.5 부터 지원하여 새로 만듬
        public bool GuidTryParse(string s, out Guid result)
        {
            if (string.IsNullOrEmpty(s))
            {
                result = Guid.Empty;
                return false;
            }
            else
            {
                Regex format = new Regex(
                "^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
                Match match = format.Match(s);
                if (match.Success)
                {
                    result = new Guid(s);
                    return true;
                }
                else
                {
                    result = Guid.Empty;
                    return false;
                }
            }
        }

		public static string ReplaceFileName(string strFileName)
		{
			/*
			 * 쉐어포인트에서 허용하지 않는 특수문자를 언더바로 치환한다.
			 * \ / : * ? " < > | # { } % ~ & '
			 */
			string removeSimbol = strFileName.Replace("\\", "_");

			removeSimbol = removeSimbol.Replace("/", "_");
			removeSimbol = removeSimbol.Replace(":", "_");
			removeSimbol = removeSimbol.Replace("*", "_");
			removeSimbol = removeSimbol.Replace("?", "_");
			removeSimbol = removeSimbol.Replace("\"", "_");
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

		/// <summary>
        /// DAC방식의 파일 암호화 함수
        /// </summary>
        /// <param name="keyFileName">키파일 이름(경로포함) </param>
        /// <param name="pId">권한을 구분 적용하는 그룹ID 개인ID도 가능</param>
        /// <param name="srcPath">원본파일 이름(경로포함) 복호화 문서</param>
        /// <param name="destPath">암호화한 파일 이름(경로포함) 암호화 문서</param>
        /// <returns></returns>
        public bool DSEncFileDAC(string pId, string srcPath, string destPath)
        {
            int iResult = -1;
            bool bResult = false;
            DSAPILib.SCSLClass DSApi = new DSAPILib.SCSLClass();
            try
            {
                DSApi.SettingPathForProperty(DSAPI_PROPFILE_PATH);

                DSApi.DSAddUserDAC(pId, DSAPI_AUTH_STR, 0, 0, 0);
                iResult = DSApi.DSEncFileDACV2(DSAPI_KEYFILE_PAHT, pId, srcPath, destPath, 1);//DAC암호화
                //결과값 0:성공 0이외의 수는 실패로 간주
                if (iResult == 0) { bResult = true; }
                else { bResult = false; }

            }
            catch (Exception ex)
            {
                //에러처리 각 사이트에 맞게 처리
                throw ex;
            }

            //Response.Write("iResult : " + iResult);

            return bResult;
        }
        
        /// <summary>
        /// DAC방식의 파일 복호화 함수
        /// </summary>
        /// <param name="keyFileName">키파일 이름(경로포함) </param>
        /// <param name="pId">권한을 구분 적용하는 그룹ID 개인ID도 가능</param>
        /// <param name="srcPath">원본파일 이름(경로포함) 암호화 문서</param>
        /// <param name="destPath">복호화한 파일 이름(경로포함) 복호화 문서</param>
        /// <returns></returns>
        public bool DSDecFileDAC(string pId, string srcPath, string destPath)
        {
            int iResult = -1;
            bool bResult = false;
            DSAPILib.SCSLClass DSApi = new DSAPILib.SCSLClass();
            
            try
            {
                DSApi.SettingPathForProperty(DSAPI_PROPFILE_PATH);
                iResult = DSApi.DSDecFileDAC(DSAPI_KEYFILE_PAHT, pId, srcPath, destPath);

                //결과값 0:성공 0이외의 수는 실패로 간주
                //결과값 -36 : 원본 파일이 암호화 파일이 아님 성공으로 간주 
                
                if (iResult == 0) { bResult = true; }
                else if (iResult == -36) { bResult = true; }
                else { bResult = false; }
            }
            catch (Exception ex)
            {
                //에러처리 각 사이트에 맞게 처리
                throw ex;
            }
            //Response.Write("iResult : " + iResult);

            return bResult;
        }

    }
}