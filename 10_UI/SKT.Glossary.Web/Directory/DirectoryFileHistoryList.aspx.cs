using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Common;
using SKT.Glossary.Dac;
using System.Collections;
using System.Web.Services;
using System.Data;
using System.IO;
using System.Web.Script.Serialization;

using SKT.Common.TikleDocManagerService;
using System.Text.RegularExpressions;
using System.Text;

//using DSAPILib;

using System.ServiceModel.Channels;
using System.ServiceModel;  


namespace SKT.Glossary.Web.Directory
{
    public partial class DirectoryFileHistoryList : System.Web.UI.Page
    {
        protected string RootURL = string.Empty;
        protected string UserID = string.Empty;
        protected string DirID = string.Empty;
        protected string FileID = string.Empty;

        DataTable _dtFileList = null; 

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(this, string.Empty);
            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            DirID = (Request["DirID"] ?? string.Empty).ToString();
            FileID = (Request["FileID"] ?? string.Empty).ToString();

            if (!Page.IsPostBack)
            {
                //추가  
            }

            FileDataTableSchema();

            BindSelect(); 


        }

        /// <summary>
        /// 조회  
        /// </summary>
        private void BindSelect()
        {

            SKT.Common.TikleDocManagerService.DocManagerServiceClient client = new SKT.Common.TikleDocManagerService.DocManagerServiceClient();
            
            using (new OperationContextScope(client.InnerChannel))
            {
                // Add a HTTP Header to an outgoing request
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers["tikle"] = "31163105310731083101";
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                SKT.Common.TikleDocManagerService.T_FileInfo[] resultFiles = client.GetFileHistory(DirID, FileID);

                for (int i = 0; i < resultFiles.Length; i++)
                {
                    //if (resultFiles[i].IS_CURRENT_VERSION == "N")
                    //{
                        DataRow drAdd = _dtFileList.NewRow();

                        drAdd["IS_CURRENT_VERSION"] = resultFiles[i].IS_CURRENT_VERSION;
                        drAdd["EDITOR_ID"] = resultFiles[i].EDITOR_ID;
                        drAdd["EDITOR"] = resultFiles[i].EDITOR;
                        drAdd["EDIT_DATE"] = resultFiles[i].EDIT_DATE;
                        drAdd["EDIT_URL"] = resultFiles[i].EDIT_URL;
                        drAdd["VERSION_NO"] = resultFiles[i].VERSION_NO;

                        _dtFileList.Rows.Add(drAdd);
                    //}

                }
            }

            if (_dtFileList.Rows.Count > 0)
            {
                NoData.Visible = false;
            }

            rptFile.DataSource = _dtFileList;
            rptFile.DataBind();

        }

        /// <summary>
        /// 그리드 바인딩  
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        protected void rptFile_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal Confrim = (Literal)e.Item.FindControl("ilConfrim");

                DataRowView row = e.Item.DataItem as DataRowView;

                if (row["IS_CURRENT_VERSION"].ToString() == "Y")
                {
                    Confrim.Text = "현재버전";
                }
                else
                {
                    //Confrim.Text = "<a href=\"javascript:fnMyConfrim('" + HttpUtility.UrlEncode(row["EDIT_URL"].ToString()) + "')\" class=\"btn_s\" ><b>확인</b></a>";
                    Confrim.Text = "<a href=\"javascript:fnMyConfrim('" + row["EDIT_URL"].ToString() + "')\" class=\"btn_s\" ><b>확인</b></a>";
                }
            }
        }

        // <summary>
        /// 파일임시 테이블  
        /// </summary>
        /// <returns></returns>
        private DataTable FileDataTableSchema()
        {
            //테이블 컬럼 전체 삭제 
            if (_dtFileList != null)
            {
                _dtFileList.Columns.Clear();
            }

            //테이블 컬럼 생성

            _dtFileList = new DataTable();
            _dtFileList.Columns.Add("IS_CURRENT_VERSION");
            _dtFileList.Columns.Add("EDITOR_ID");
            _dtFileList.Columns.Add("EDITOR");
            _dtFileList.Columns.Add("EDIT_DATE");
            _dtFileList.Columns.Add("EDIT_URL");
            _dtFileList.Columns.Add("VERSION_NO");

            return _dtFileList;
        }




    }
}