using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using SKT.Common;

using System.Web.Services;
using System.ServiceModel;
using System.ServiceModel.Channels;

using SKT.Common.TikleDocManagerService;

namespace SKT.Glossary.Biz
{
    public class GlossaryDirectoryBiz
    {

        public DataSet GlossaryDirectory_List(string Mode, string UserID, int PageNum, int PageSize, string GatheringYN, string GatheringID)
        {
            GlossaryDirectoryDac Dac = new GlossaryDirectoryDac();

            //Public
            DataSet ds = Dac.GlossaryDir_List(Mode, PageNum, PageSize, UserID, GatheringYN, GatheringID);

            return ds;
        }



        public List<GlossaryDirectoryFileType> GlossaryDirFile_List(string ID)
        {
            List<GlossaryDirectoryFileType> listGlossaryFileType = GlossaryDirectoryDac.Instance.GlossaryDirFile_List(ID);
            return listGlossaryFileType;
        }




        public GlossaryDirectoryType DirectoryInsert(GlossaryDirectoryType dirType, string Mode)
        {
            GlossaryDirectoryDac Dac = new GlossaryDirectoryDac();
            DataSet ds = Dac.DirectoryInsert(dirType, Mode);


            dirType.DirID = ds.Tables[0].Rows[0].ItemArray[0].ToString();

            //if (!string.IsNullOrEmpty(dirType.SvID))
            //    Dac.SurveyQstInsert(ds.Tables[0].Rows[0].ItemArray[0].ToString(), dirType.UserID);

            return dirType;
        }


        public GlossaryDirectoryFileType DirectoryFileInsert(GlossaryDirectoryFileType dirFileType, string Mode)
        {
            GlossaryDirectoryDac Dac = new GlossaryDirectoryDac();
            DataSet ds = Dac.DirectoryFileInsert(dirFileType, Mode);

            dirFileType.FileID = ds.Tables[0].Rows[0].ItemArray[0].ToString();

            //if (!string.IsNullOrEmpty(dirType.SvID))
            //    Dac.SurveyQstInsert(ds.Tables[0].Rows[0].ItemArray[0].ToString(), dirType.UserID);

            return dirFileType;
        }

        //2016-11-03 공동 관리자 추가
        public int DirectoryManagerInsert(DirectoryMgrType mgrType,string tkType)
        {
            GlossaryDirectoryDac Dac = new GlossaryDirectoryDac();

            int result = 0;

            string[] ToUser = mgrType.ManagerID.Split('/');
            string[] ToUserName = mgrType.ManagerName.Split('&');

            for (int i = 0; i < ToUser.Length - 1; i++)
            {
                mgrType.ManagerID = ToUser[i];
                mgrType.ManagerName = ToUserName[i];

                result = Dac.DirectoryManagerInsert(mgrType, tkType);
            }
            return result;
        }

        //2016-11-03 공동 관리자 모두제거
        public int DirectoryManagerDelete(string dirID, string tkType)
        {
            GlossaryDirectoryDac Dac = new GlossaryDirectoryDac();

            int result = 0;
            result = Dac.DirectoryManagerDelete(dirID, tkType);
            return result;
        }


        /// <summary>
        /// 문서함 삭제
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string DirectoryDelete(GlossaryDirectoryType dirType)
        {
            GlossaryDirectoryDac Dac = new GlossaryDirectoryDac();
            DataSet ds = Dac.DirectoryDelete(dirType);

            string rtnVal = ds.Tables[0].Rows[0].ItemArray[0].ToString();

            return rtnVal;
        }


        /// <summary>
        /// 조회 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataSet ddlDirectorySelect(string userID)
        {
            GlossaryDirectoryDac Dac = new GlossaryDirectoryDac();

            DataSet ds = Dac.ddlDirectorySelect(userID);

            return ds;
        }

        /// <summary>
        /// 디렉토리 리스트 조회  
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataSet ddlDirectorySelect(string mode, string userID, string GatheringYN, string GatheringID)
        {
            GlossaryDirectoryDac Dac = new GlossaryDirectoryDac();

            //DivType (Pub) (Vis)
            DataSet ds = Dac.ddlDirectorySelect(mode, userID, GatheringYN, GatheringID);

            return ds;
        }

        /// <summary>
        /// 디렉토리 조회  
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="userID"></param>
        /// <param name="divID"></param>
        /// <returns></returns>
        public DataSet ddlDirectorySelect(string mode, string userID, string divID)
        {
            GlossaryDirectoryDac Dac = new GlossaryDirectoryDac();
            DataSet ds = Dac.ddlDirectorySelect(mode, userID, divID);

            return ds;
        }

        /// <summary>
        /// 현재 문서 사용자 정보 조회(Share Point) 
        /// </summary>
        /// <param name="dirID"></param>
        /// <param name="fileID"></param>
        /// <returns></returns>
        /// [WebService]
        public List<string> GetExcelConfirmData(string dirID, string fileID)
        {
            List<string> result = new List<string>();

            DocManagerServiceClient client = new DocManagerServiceClient();

            using (new OperationContextScope(client.InnerChannel))
            {
                // Add a HTTP Header to an outgoing request
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers["tikle"] = "31163105310731083101";
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                T_UserInfo pResult = client.GetCheckedUserInfo(dirID, fileID);

                int iResult = pResult.STATUS;
                if (pResult.STATUS == -1)
                {
                    result.Add("-1");
                    result.Add(pResult.DisplayName);
                    result.Add(pResult.Department);
                    result.Add(pResult.MobilePhone);
                }
                else
                {
                    result.Add("0");
                    result.Add(pResult.DisplayName);
                    result.Add(pResult.Department);
                    result.Add(pResult.MobilePhone);
                }
            }

            return result;
        }

        public T_FileInfo[] GetSearchFileResult(string dir, string kw)
        {
            T_FileInfo[] resultFiles = null;

            DocManagerServiceClient client = new DocManagerServiceClient();

            using (new OperationContextScope(client.InnerChannel))
            {
                // Add a HTTP Header to an outgoing request
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers["tikle"] = "31163105310731083101";
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                resultFiles = client.SearchFiles(dir, kw);

                //if (_dtDirList != null && _dtDirList.Rows.Count > 0)
                //{
                //	for (int i = 0; i < _dtDirList.Rows.Count; i++)
                //	{
                //		for (int k = 0; k < resultFiles.Length; k++)
                //		{

                //			if (_dtDirList.Rows[i]["Dir_ID"].ToString() == resultFiles[k].FOLDER_NAME)
                //			{
                //				DataRow drAdd = _dtFileList.NewRow();

                //				drAdd["Dir_ID"] = resultFiles[k].FOLDER_NAME;
                //				drAdd["FILE_NAME"] = resultFiles[k].FILE_NAME;
                //				drAdd["CREATE_DATE"] = resultFiles[k].CREATE_DATE;
                //				drAdd["EDIT_DATE"] = resultFiles[k].EDIT_DATE;
                //				drAdd["EDIT_URL"] = resultFiles[k].EDIT_URL;
                //				drAdd["EDITOR"] = resultFiles[k].EDITOR;
                //				drAdd["EDITOR_ID"] = resultFiles[k].EDITOR_ID;
                //				drAdd["WRITE_ID"] = resultFiles[k].WRITE_ID;
                //				drAdd["WRITER"] = resultFiles[k].WRITER;

                //				_dtFileList.Rows.Add(drAdd);
                //			}
                //		}
                //	}
                //}
            }

            return resultFiles;
        }

        public string spSetFileReadPermission(string fId, string fileName, List<T_Authority> authorityList, string UserID)
        {
            string rtn = string.Empty;
            //웹서비스  객체 생성  
            DocManagerServiceClient proxy = new DocManagerServiceClient();

            using (new OperationContextScope(proxy.InnerChannel))
            {
                // Add a HTTP Header to an outgoing request
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers["tikle"] = "31163105310731083101";
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                Result result = proxy.SetFileReadPermission(fId, fileName, authorityList.ToArray<SKT.Common.TikleDocManagerService.T_Authority>(), "skt\\" + UserID);
                if (result.STATUS != 0)
                {
                    //Response.Write("<script>alert('성공');</script>"); 
                }
                rtn = result.STATUS.ToString();
            }

            return rtn;
        }

        /// <summary>
        /// 디렉토리 권한 조회 
        /// </summary>
        /// <param name="divID"></param>
        /// <returns></returns>
        public DataSet GetDirectoryAuth(string divID)
        {
            GlossaryDirectoryDac Dac = new GlossaryDirectoryDac();
            DataSet ds = Dac.GetDirectoryAuth(divID);

            return ds;
        }

        /// <summary>
        /// 디렉토리 권한 조회 
        /// </summary>
        /// <param name="divID"></param>
        /// <returns></returns>
        public DataSet GetGatheringAuth(string GatheringID)
        {
            GlossaryDirectoryDac Dac = new GlossaryDirectoryDac();
            DataSet ds = Dac.GetGatheringAuth(GatheringID);

            return ds;
        }

    }
}
