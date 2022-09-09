using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SKT.Glossary.Type;
using SKT.Glossary.Biz;
using SKT.Glossary.Dac;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace SKT.Glossary.Web.Interface
{
    public partial class LearningWorld : System.Web.UI.Page
    {
        protected string Mode = "TikleView";
        protected string sqlMode = "selectGlossary";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Request
            string EmpNo = (Request["EmpNo"] ?? string.Empty).ToString();
            string ID = (Request["ID"] ?? string.Empty).ToString();
            string Mode = (Request["Command"] ?? string.Empty).ToString();
            
//            string responseHeader = @"
//                <responseHeader>
//	                <command>TikleView</command>
//	                <resultCode>{0}</resultCode>
//	                <resultMessage>{1}</resultMessage>
//                </responseHeader>
//            ";

            //string ResponseMessage = string.Empty;
            //ResponseMessage = "<?xml version='1.0' encoding='EUC-KR'?>\r\n";

            //System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(this.Response.OutputStream, System.Text.Encoding.UTF8);
            System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(this.Response.OutputStream, System.Text.Encoding.Default);
            writer.Formatting = System.Xml.Formatting.Indented;

            writer.WriteStartDocument(); //문서시작
            writer.Indentation = 4;

            try
            {
                // 파라미터가 전달 안 됬을 경우
                if (EmpNo == string.Empty || ID == string.Empty || !Mode.Equals(Mode))
                {
                    writer.WriteStartElement("Tikle");

                    writer.WriteElementString("command", "TikleView");
                    writer.WriteElementString("resultCode", "1001");
                    writer.WriteElementString("resultMessage", "PARAMETER IS EMPTY"); 

                    writer.WriteEndElement(); 

                    return;
                }
                else
                {
                    writer.WriteStartElement("Tikle");

                    writer.WriteElementString("command", "TikleView");
                    writer.WriteElementString("resultCode", "1000");
                    writer.WriteElementString("resultMessage", "SUCCESS");

                    // Glossary Infomation Select
                    GlossaryType Board = new GlossaryType();
                    GlossaryBiz biz = new GlossaryBiz();
                    Board = biz.GlossarySelect(ID, EmpNo, "");    // ID : CommonID 

                    // Category Name Select
                    GlossaryCategoryType glossaryCategoryType = GlossaryCategoryDac.Instance.GlossaryCategorySelect(Board.CategoryID);

                    writer.WriteStartElement("responseBody");

                    writer.WriteElementString("ID", Board.ID);
                    writer.WriteElementString("Title", Board.Title);
                    
                    writer.WriteStartElement("Contents");
                    writer.WriteCData(Board.Contents.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;nbsp;", " ").Replace("&quot;", "\"").Replace("&amp;amp;", "&"));
                    writer.WriteEndElement();
                    
                    writer.WriteStartElement("Summary");
                    writer.WriteCData(Board.Summary.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;nbsp;", " ").Replace("&quot;", "\"").Replace("&amp;amp;", "&"));
                    writer.WriteEndElement();

                    writer.WriteElementString("Hit", Board.Hits);
                    writer.WriteElementString("UserID", Board.UserID);
                    writer.WriteElementString("UserName", Board.UserName);
                    writer.WriteElementString("DeptName", Board.DeptName);
                    writer.WriteElementString("UserEmail", Board.UserEmail);
                    writer.WriteElementString("CreateDate", Board.CreateDate);
                    writer.WriteElementString("Category", glossaryCategoryType.CategoryTitle);

                    writer.WriteEndElement();

                    writer.WriteEndElement();


                    //ResponseMessage += "<responseBody>\r\n";
                    //// set XML
                    //ResponseMessage += "<ID>" + Board.ID + "</ID>\r\n";
                    //ResponseMessage += "<Title>" + Board.Title + "</Title>\r\n";
                    //ResponseMessage += "<Contents>" + Board.Contents.ToString() + "</Contents>\r\n";
                    //ResponseMessage += "<Summary>" + Board.Summary.ToString() + "</Summary>\r\n";
                    //ResponseMessage += "<Hit>" + Board.Hits + "</Hit>\r\n";
                    //ResponseMessage += "<UserID>" + Board.UserID + "</UserID>\r\n";
                    //ResponseMessage += "<UserName>" + Board.UserName + "</UserName>\r\n";
                    //ResponseMessage += "<DeptName>" + Board.DeptName + "</DeptName>\r\n";
                    //ResponseMessage += "<UserEmail>" + Board.UserEmail + "</UserEmail>\r\n";
                    //ResponseMessage += "<CreateDate>" + Board.CreateDate + "</CreateDate>\r\n";
                    //ResponseMessage += "<Category>" + glossaryCategoryType.CategoryTitle + "</Category>\r\n";

                    //ResponseMessage += "</responseBody>\r\n";
                    //ResponseMessage += "</Tikle>\r\n";

                }

            }
            // 다른 예외 처리
            catch (Exception ex)
            {
                //ResponseMessage += "<sktpeServer>\r\n";
                //ResponseMessage += string.Format(responseHeader, "1002", ex.Message);
                //ResponseMessage += "</sktpeServer>\r\n";

                writer.WriteStartElement("Tikle");

                writer.WriteElementString("command", "TikleView");
                writer.WriteElementString("resultCode", "1002");
                writer.WriteElementString("resultMessage", ex.Message);

                writer.WriteEndElement();
            }
            finally
            {
                // 방문자 수 증가
                IncreaseOfVisitors(EmpNo);

                //Response.ContentType = "text/xml";
                //Response.Write(ResponseMessage);
                //Response.End();

                //Response.Buffer = false;
                
                //Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.ContentType = "text/xml";
                writer.Flush(); //버퍼닫기
                writer.Close(); //문서종료
            }
        }


        /// <summary>
        /// 방문자 수 증가
        /// </summary>
        protected void IncreaseOfVisitors(string EmpNo)
        {
            string UserName = string.Empty;

            // 사번이 있으면 사원 이름 가져오기
            if (!string.IsNullOrEmpty(EmpNo))
            {
                Database db = DatabaseFactory.CreateDatabase("ConnGlossary");
                DbCommand dbCommand = db.GetSqlStringCommand(@"
                SELECT [USER_NAME] FROM [Glossary].[dbo].[View_User] AS VU WHERE VU.[USER_ID] = @UserID");

                db.AddInParameter(dbCommand, "UserID", DbType.String, EmpNo);
                using (DataSet ds = db.ExecuteDataSet(dbCommand))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            UserName = (dr["USER_NAME"] == DBNull.Value) ? null : dr.Field<string>("USER_NAME");
                        }
                    }
                }
            }

            // 사원이름이 있으면 방문자 수 증가
            // SP 에서 그날 방문을 한번이라도 했다면 방문자 수에 증가하지 않는 로직이 포함되어 있음
            if (!string.IsNullOrEmpty(UserName))
            {
                GlossaryLoginType glt = new GlossaryLoginType();

                glt.UserID = string.Empty;
                glt.Name = string.Empty;
                glt.SessionID = string.Empty;
                glt.UrlBefore = string.Empty;
                glt.UrlCurrent = string.Empty;
                glt.PathCurrent = string.Empty;
                glt.LoginType = string.Empty;
                
                try
                {
                    glt.UserID = EmpNo;
                    glt.Name = UserName;
                    glt.SessionID = Session.SessionID.ToString();
                    glt.LoginType = "LW";
                    glt.UrlBefore = (Request.UrlReferrer == null? "": Request.UrlReferrer.ToString());
                    glt.UrlCurrent = (Request.Url == null ? "" : Request.Url.ToString());
                    glt.PathCurrent = (Request.Url.LocalPath == null ? "" : Request.Url.LocalPath.ToString());
                }catch { }
                finally
                {
                    GlossaryDac dac = new GlossaryDac();
                    dac.Insert_LW_EventAttendance(glt);
                }
            }
        }
    }
}