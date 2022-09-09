using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.Services;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using SKT.Common.TikleDocManagerService;


using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;

using SKT.Common;

namespace SKT.Glossary.Web.Directory
{
    public partial class DirectorySearchResult : System.Web.UI.Page
    {
        UserInfo u;

        protected string DivType = string.Empty;
        protected string m_pub = string.Empty;
        protected string m_vis = string.Empty;
        protected string m_pri = string.Empty;

        // 끌.모임 설정(기본값:모임지식이 아님)
        protected string GatheringYN;
        protected string GatheringID;

        string folers = string.Empty;
        public string SearchKeyword = string.Empty;
        public int iTotalCnt = 0;
        DataSet ds = null;
        Dictionary<string, string> dicDirectory = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(this, string.Empty);

            // 끌.모임 설정
            GatheringYN = (Request["GatheringYN"] ?? string.Empty).ToString();
            GatheringID = (Request["GatheringID"] ?? string.Empty).ToString();

            u = new UserInfo(this.Page);

            SearchKeyword = (Request["q"] ?? string.Empty).ToString();

            DivType = string.IsNullOrEmpty(DivType) ? "Pub" : DivType;

            if (DivType == "Pri")
            {
                m_pub = "";
                m_vis = "";
                m_pri = "class=\"on\"";
            }
            else if (DivType == "Vis")
            {
                m_pub = "";
                m_vis = "class=\"on\"";
                m_pri = "";
            }
            else
            {
                m_pub = "class=\"on\"";
                m_vis = "";
                m_pri = "";
            }

            //litSearchKeyword.Text = SearchKeyword;

            // 디렉토리 목록 조회
            GlossaryDirectoryBiz biz = new GlossaryDirectoryBiz();
            ds = biz.ddlDirectorySelect("Pub", u.UserID, GatheringYN, GatheringID);

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dicDirectory.Add(dr["DIR_ID"].ToString(), dr["DIR_NM"].ToString());
                    folers += dr["DIR_ID"].ToString() + "|";
                }
                if (folers.Length > 0)
                {
                    folers = folers.Substring(0, folers.Length - 1);
                }
            }


            // 검색결과 조회/Bind
            SearchResultBind();

        }

        /// <summary>
        /// Share Point 데이터 조회  
        /// </summary>
        public void SearchResultBind()
        {
            GlossaryDirectoryBiz DirBiz = new GlossaryDirectoryBiz();
            SKT.Common.TikleDocManagerService.T_FileInfo[] resultFiles = null;

            if (folers.Length == 0) return;

            resultFiles = DirBiz.GetSearchFileResult(folers, HttpUtility.UrlDecode(SearchKeyword));

            rptSearchResult.DataSource = resultFiles;
            rptSearchResult.DataBind();

            iTotalCnt = resultFiles.Length;
          
            
        }

        /// <summary>
        /// 바인딩  
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        protected void rptSearchResult_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            GlossaryDirectoryBiz DirBiz = new GlossaryDirectoryBiz();

            //if (rptSearchResult.Items.Count < 1)
            //{
            //    if (e.Item.ItemType == ListItemType.Footer)
            //    {
            //        Literal lblFooter = (Literal)e.Item.FindControl("lblEmptyData");

            //        lblFooter.Visible = true;
            //    }
            //}

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SKT.Common.TikleDocManagerService.T_FileInfo glossaryFileType = (SKT.Common.TikleDocManagerService.T_FileInfo)e.Item.DataItem;
                Literal litDirectory = (Literal)e.Item.FindControl("litDirectory");

                litDirectory.Text = "<li>";

                string DirID = glossaryFileType.FOLDER_NAME;
                string FileNM = glossaryFileType.FILE_NAME;
                string fileExt = "";
                string confirmUser = "";
                List<string> result = new List<string>();

                string[] extArr = FileNM.Split('.');

                fileExt = extArr[extArr.Length - 1];


                if (fileExt == "pptx" || fileExt == "ppt")
                {
                    fileExt = "ms_ppt.png";
                }
                else if (fileExt == "docx" || fileExt == "doc")
                {
                    fileExt = "ms_word.png";
                }
                else if (fileExt == "xlsx" || fileExt == "xls")
                {
                    fileExt = "ms_excel.png";

                    /*
                    Author : 개발자-최현미C, 리뷰자-진현빈D
                    Create Date : 2016.07.21
                    Desc : 사용안함 (엑셀파일일 경우 현재 사용자 조회)
                    */
                    //result = DirBiz.GetExcelConfirmData(DirID, FileNM);

                    //if (result[0] != "-1")
                    //{
                    //    confirmUser = result[1]; // + "(" + result[3] + ")";
                    //}
                }
                else if (fileExt == "one")
                {
                    fileExt = "ms_onenote.png";
                }
                else
                {
                    fileExt = "ms_pc.png";
                }

                litDirectory.Text += " <img src=\"/common/images/icon/" + fileExt + "\" alt=\"\" onclick=\"fileDownload('" + DirID + "','" + FileNM + "')\" style='cursor:pointer' />";

                litDirectory.Text += " <dl>";
                //파일명  
                litDirectory.Text += " <dt><a href=\"javascript:fileOpen('" + DirID + "','" + FileNM + "') \">" + FileNM + "</a></dt>";

                litDirectory.Text += " <dd>문서함 : <span class='pr'><a href=\"DirectoryView.aspx?DivType=Pub&DivID=" + DirID + "&GatheringYN="+ GatheringYN +"&GatheringID="+GatheringID+"\">" + dicDirectory[DirID] + "</a></span>";
                litDirectory.Text += "최종수정일 : <span class='pr'>" + glossaryFileType.EDIT_DATE + " </span>";

                if (confirmUser == "")
                {
                    litDirectory.Text += "최종수정자 : <span>" + glossaryFileType.EDITOR + "</span>";
                }
                else
                {
                    litDirectory.Text += "<span class=\"point_red\">현재사용자</span> : " + confirmUser;
                }

                //litDirectory.Text += " <a href=\"javascript:fileVerCheck('" + DirID + "','" + FileNM + "') \"> [편집이력보기]</a>";
                litDirectory.Text += "</dd>";

                litDirectory.Text += " </dl> ";

                //버튼
                litDirectory.Text += " <span class=\"btns\">";


                //-----------------------------------------------
                //관리자만 삭제와 쪽지 가능
                //수정 : 파일삭제는 관리자와 파일생성자가 삭제가능 
                //    : 파일을 수정한 이력이 있으면 삭제를 못하게 
                //-----------------------------------------------

                //1.Share Point 에서 파일이력을 조회해서 이력이 있으면 삭제불가  
                if (glossaryFileType.HAS_EDITED_VERSION == "N")
                {
                    //파일생성자 
                    if (glossaryFileType.WRITE_ID.Replace(@"SKT\", "") == u.UserID)
                    {
                        //삭제 
                        litDirectory.Text += "   <a href=\"javascript:fnDeleteFile('" + DirID + "','" + FileNM + "')\"  class=\"btn1\"><span>삭제하기</span></a>";
                    }
                }

                //쪽지는 폴더관리자만 사용
                //if (_dtDirList.Rows[0]["Reg_ID"].ToString() == u.UserID)
                ////if (glossaryFileType.RegID.Replace(@"SKT\", "")  == u.UserID)
                //{
                //	litDirectory.Text += "   <a href=\"javascript:fnMeno('" + DirID + "','" + FileNM + "')\"  class=\"btn1\"><span>쪽지보내기</span></a>";
                //}


                //공통사용버튼  
                litDirectory.Text += "   <a href=\"javascript:fileDownload('" + DirID + "','" + FileNM + "')\"  class=\"btn1\"><span>다운로드</span></a>";
                //열기버튼 없애기 
                //litDirectory.Text += "   <a href=\"javascript:fileOpen('" + DirID + "','" + FileNM + "')\"  class=\"btn1\"><b>열기</b></a>"; 
                //엑셀확인버튼 추가  
                //litDirectory.Text += "   <a href=\"javascript:fileExcelConfirm('" + DirID + "','" + FileNM + "')\"  class=\"btn1\"><span>사용자확인</span></a>";

                //버전체크
                //litDirectory.Text += "   <a href=\"javascript:fileVerCheck('" + DirID + "','" + FileNM + "')\"  class=\"btn1\"><span>편집이력보기</span></a>";

                litDirectory.Text += " </span>";
                litDirectory.Text += "</li> ";
            }

        }

        /// <summary>
        /// 다운로드  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty;
            SKT.Common.TikleDocManagerService.DocManagerServiceClient proxy = new DocManagerServiceClient();

            try
            {
                //엑셀다운로드
                string appPath = ConfigurationManager.AppSettings["AttachFilePath"].ToString();
                string appPath2 = ConfigurationManager.AppSettings["AttachFilePath2"].ToString();
                string filePath = appPath2 + @"\" + hdFileID.Value;
                string filePath2 = string.Empty;

                using (new OperationContextScope(proxy.InnerChannel))
                {
                    // Add a HTTP Header to an outgoing request
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["tikle"] = "31163105310731083101";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    byte[] buffer = proxy.DownloadFile(hdDirectoryID.Value, hdFileID.Value);

                    filePath2 = appPath2 + @"\" + @"Glossary\" + hdFileID.Value;

                    FileStream fs = new FileStream(filePath2, FileMode.Create);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();
                }

                //파일 암호화 
                //DSEecFileDAC("c:\\Softcamp\\04_KeyFile\\keyDAC_SVR0.sc", "SECURITYDOMAIN", filePath2, filePath);
                DSEecFileDAC("c:\\Softcamp\\04_KeyFile\\keyDAC_SVR0.sc", "SECURITYDOMAIN", filePath2, filePath);

                Response.ClearHeaders();
                Response.ClearContent();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", Server.UrlEncode(hdFileID.Value).Replace("+", "%20")));
                Response.TransmitFile(filePath);
                Response.End();

                /*
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + hdFileID.Value);
                Response.ContentType = this.MineTypeByFileExtension(hdFileID.Value);
                string path = string.Format(@"C:\SKT_MultiUploadedFilesQA\Glossary\Glossary\enc\{0}", hdFileID.Value);
                Response.TransmitFile(path);
                Response.End();
                */

                /*
                
                Response.ClearHeaders();
                Response.ClearContent();
                Response.Charset = "utf-8";
                Response.ContentType = "application/octet-stream";
                
                if (Request.UserAgent.IndexOf("MSIE") > 0)
                {
                    Response.AppendHeader("Content-Disposition", String.Format("attachment; filename=\"{0}\"", HttpUtility.UrlPathEncode(hdFileID.Value)));
                }
                else
                {
                    Response.AppendHeader("Content-Disposition", String.Format("attachment; filename=\"{0}\"", hdFileID.Value));
                }
                
                Response.TransmitFile(hdFileID.Value);
                Response.Flush();
                Response.End(); 
                */

                //파일삭제 
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                if (File.Exists(filePath2))
                {
                    File.Delete(filePath2);
                }
            }
            catch (System.Exception ex)
            {
                strMsg = ex.Message;
            }

        }

        /// <summary>
        /// 파일 삭제  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {

            string AuthType = string.Empty;
            string errMsg = string.Empty;

            UserInfo u = new UserInfo(this.Page);
            string UserID = u.UserID;

            //***********************//
            //1. 로컬DB에서 파일 삭제 
            //***********************//

            GlossaryDirectoryBiz biz = new GlossaryDirectoryBiz();
            GlossaryDirectoryFileType FileType = null;
            string fileName = string.Empty;

            try
            {
                FileType = new GlossaryDirectoryFileType();
                FileType.DirID = hdDirectoryID.Value;
                FileType.FileID = "0";      //(Share Point 연동문제로 이렇게 쓴다)      
                FileType.FileNM = hdFileID.Value;  //로컬DB의 파일id (Share Point 연동문제로 이렇게 쓴다) 
                FileType.FileNM = hdFileName.Value;  //로컬DB의 파일id 


                biz.DirectoryFileInsert(FileType, "Delete");
            }
            catch (System.Exception exp)
            {
                errMsg = exp.Message;
            }

            //****************************//
            //2. Share Point 파일 삭제
            //****************************// 

            //웹서비스  객체 생성  
            SKT.Common.TikleDocManagerService.DocManagerServiceClient proxy = new DocManagerServiceClient();

            try
            {
                using (new OperationContextScope(proxy.InnerChannel))
                {
                    // Add a HTTP Header to an outgoing request
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["tikle"] = "31163105310731083101";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    Result result = proxy.DeleteFile(hdDirectoryID.Value, hdFileID.Value, "skt\\" + u.UserID, "Y");

                    if (result.STATUS == 0) //성공 
                    {
                        Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
                    }
                }
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }

            //Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri); 


        }

        /// <summary>
        /// DAC방식의 파일 암호화 함수 샘플
        /// </summary>
        /// <param name="keyFileName">키파일 이름(경로포함) </param>
        /// <param name="pId">권한을 구분 적용하는 그룹ID 개인ID도 가능</param>
        /// <param name="srcPath">원본파일 이름(경로포함) 암호화 문서</param>
        /// <param name="destPath">복호화한 파일 이름(경로포함) 복호화 문서</param>
        /// <returns></returns>
        public bool DSEecFileDAC(string keyFileName, string pId, string srcPath, string destPath)
        {
            int iResult = -1;
            bool bResult = false;
            DSAPILib.SCSLClass DSApi = new DSAPILib.SCSLClass();
            try
            {
                DSApi.SettingPathForProperty("c:\\Softcamp\\02_Module\\02_ServiceLinker\\DSServiceLinker.ini");

                DSApi.DSAddUserDAC("SECURITYDOMAIN", "111000000", 0, 0, 0);
                iResult = DSApi.DSEncFileDACV2("c:\\Softcamp\\04_KeyFile\\keyDAC_SVR0.sc", "SECURITYDOMAIN", srcPath, destPath, 1);//DAC암호화
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
    }
}