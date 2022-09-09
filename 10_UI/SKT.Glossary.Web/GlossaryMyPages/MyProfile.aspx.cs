using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Common;
using SKT.Glossary.Dac;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web.Services;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Data;

using System.Web.UI.HtmlControls;
using SKT.Tnet.Controls;
using SKT.Tnet.Framework.Configuration;
using SKT.Tnet.Framework.Security;
using SKT.Tnet.Framework.Common;
using SKT.Tnet.Framework.Utilities;

namespace SKT.Glossary.Web.Glossary
{
    public partial class MyProfile : System.Web.UI.Page
    {
        protected string UserID = string.Empty;
        protected string RootURL = string.Empty;
        protected string UserName = string.Empty;
        protected string UserTeamCode = string.Empty;
        protected string UserTeam = string.Empty;
        protected string UserEmail = string.Empty;
        protected string saperrator = string.Empty;
        protected string saperrator2 = string.Empty;

        protected string TutorialYN = string.Empty;
        protected string TeamNum = string.Empty;
        protected string DisplayCareer = string.Empty;
        protected bool bOwn = true;
        public string myUserId = string.Empty;
       

        public bool onluNumUser = true;
        public string UserID_Crypt = string.Empty;

        protected ScoreRankingType scoreRankingType;    //2014-06-26 Mr.No

        protected void Page_Load(object sender, EventArgs e)
        {
            //////URL 타고 넘어오는거 구분
            //if (Session["WebcubeUseYN"] == null)
            //{
            //    Session.Add("WebcubereturnURL", Page.Request.Url.ToString());
            //    Response.Redirect("../Main.aspx");
            //}
            //else
            //{
            //    Session.Remove("WebcubereturnURL");
            //}

            RootURL = ConfigurationManager.AppSettings["RootURL"] ?? string.Empty;
            UserID = (Request["UserID"] ?? string.Empty).ToString();
            UserID = string.Empty;
            UserTeamCode = (Request["DeptCode"] ?? string.Empty).ToString();
            UserTeam = (Request["DeptName"] ?? string.Empty).ToString();
            UserInfo u = new UserInfo(this.Page);
            onluNumUser = u.isIdOnlyNum;



            UserID_Crypt = SKT.Common.CryptoHelper.AESEncryptString(u.UserID, "sktelecom_tikle2");

            TutorialCheck();

            UserControlNateOnBizPop.targetBtnYn = true;
            
            // 페이지 접근 계정 본인여부 체크
            if (UserID == string.Empty || u.UserID == UserID)
            {
                bOwn = true;
            }
            else
            {
                bOwn = false;
            }

            if (string.IsNullOrEmpty(UserID) == false) // 이값이 있으면 다른사용자를 본다는뜻이다.
            {

                if (u.UserID == UserID)
                {
                    LoginSelect();
                    btnCarreer.Visible = true;
                }
                else
                {
                    
                    NotLoginSelect(UserID);
                    btnCarreer.Visible = false;
                }
            }
            else if (string.IsNullOrEmpty(UserTeamCode) == false)
            {
                DeptView(UserTeamCode);
                MyProfiles.Style["display"] = "none";
                MyTeam.Style["display"] = "block";
            }
            else //현재 로그인 사용자
            {
                LoginSelect();
                btnCarreer.Visible = true;
            }
            //Editor.Visible = false;

            //20131219 , 관리자 별도 처리를 위해서 메서드 추가
            if (u.isAdmin)
            {
                AdminProcess();
            }

            ////201312123 , 예외 처리자 별도 처리를 위해서 메서드 추가
            //if (u.isDuser || u.isGuser)
            //{
            //    EuserProcess();
            //}

            if (u.isTiklei)
            {
                TikleiProcess();
            }

            if (u.UserID == UserID)
            {
                ScoreRanking(u.UserID);
            }
            else
            {
                ScoreRanking(UserID);
            }
        }

        private void AdminProcess()
        {
            onluNumUser = true;
            btnBeforeCarreer.Visible = false;
            btnAfterCarreer.Visible = false;
        }

        private void EuserProcess()
        {
            //btngoTeam.Visible = false;
            btnBeforeCarreer.Visible = false;
            btnAfterCarreer.Visible = false;
        }

        private void TikleiProcess()
        {

        }

        private void TutorialCheck()
        {
            UserInfo u = new UserInfo(this.Page);

            GlossaryControlBiz biz = new GlossaryControlBiz();
            TutorialInfo data = biz.TutorialSelect(u.UserID);

            if (data.ProfileYN == "N")
            {
                TutorialYN = "N";
            }
            else   //데이타가 없으면 무조건 보여줌.
            {
                TutorialYN = "Y";
            }

        }

        private void DeptView(string UserTeamCode)
        {
            TutorialYN = "N"; //튜토리얼 안보여줌.

            GlossaryProfileType TeamLeader;
            GlossaryProfileBiz biz = new GlossaryProfileBiz();
            //GlossaryProfileType data = biz.GlossaryDeptProfileSelect(UserTeamCode, out TeamLeader);
            ArrayList list = biz.GlossaryDeptProfileSelect(UserTeamCode, out TeamLeader);
            TeamNum = "0";
            saperrator = string.Empty;
            if (list.Count > 0)
            {
                TeamNum = list.Count.ToString();

                ImpersonUserinfo Topdata = biz.UserSelect(TeamLeader.UserID);
                TeamTopUserImg.Text = "<img src=" + Topdata.PhotoUrl + " align=\"" + Topdata.Name + "/" + Topdata.DeptName + "\" />";

                //lblTeamLader.Text = "<abbr title=\"" + EHRHelper.EHRWorkStatus(TeamLeader.UserID) + "\"><a href=\"javascript:fnProfileView('" + TeamLeader.UserID + "')\">" + TeamLeader.UserName + "</a></abbr>";
                lblTeamLader.Text = "<abbr title=\"" + EHRHelper.EHRWorkStatus(TeamLeader.UserID) + "\">" + TeamLeader.UserName + "</abbr>";

                rptInGeneral.DataSource = list;
                rptInGeneral.DataBind();

                this.lblTeam.Visible = false;
                /*
                this.lblEmail.Visible = false;
                this.lblPhone.Visible = false;
                this.lblWorkArea.Visible = false;
                */
                this.imgFace.Visible = false;

                btnModify.Visible = true;
                //btnCommit.Visible = true;
                //btnFoolowInsert.Visible = false; //자기자신은 버튼이 없어짐.
                //btnGoDocRoom.Visible = false;

                //btngoTeam.Visible = false;


                GlossaryProfileType data = biz.GlossaryProfileSelect(UserTeamCode);

                //Iframe 생성 Mostisoft 2015.08.21
                lblDisplayBody.Text = "<p class='text' data-UserID=" + UserID + "><iframe id='StandaloneView' class='StandaloneView" + UserID + "' src='MyProfileIframe_View.aspx?UserID=" + UserTeamCode + "'  scrolling='no' frameborder='0' style='width:100%'><div class='view_ct_area' style='padding-top:10px;'><div class='view_ct' style='margin:0;'></div></div></iframe></p>";
                //this.lblDisplayBody.Text = data.Contents;
                

                Careers.Visible = false;
            }
        }

        private void LoginSelect()
        {
            UserInfo myu = new UserInfo(this.Page);
            UserID = myu.UserID;
           // UserName = u.Name;
           // UserTeam = u.DeptName;
           // UserTeamCode = u.DeptID;
           // UserEmail = u.EmailAddress;
           // saperrator = " / ";
           // saperrator2 = " / ";

            GlossaryProfileBiz biz = new GlossaryProfileBiz();
            ImpersonUserinfo u = biz.UserSelect(UserID);


            UserID = u.UserID;
            UserName = u.Name;
            UserTeam = u.DeptName;
            UserTeamCode = u.DeptID;
            UserEmail = u.EmailAddress;
            saperrator = " / ";
            saperrator2 = " / ";

           //this.litEmail.Text = u.EmailAddress;
            //this.litPhone.Text = u.Phone;
            this.litWorkArea.Text = u.WorkArea;
            this.litpart.Text = u.Part;
            this.imgFace.ImageUrl = u.PhotoUrl;

            this.lblName.Text = UserName;
            this.lblTeam.Text = UserTeam;
            this.lblTeam2.Text = UserTeam;
            this.litPositionName.Text = u.PositionName;
            this.litSosok.Text = UserTeam;
            // = u.EmailAddress;
            // u.Phone;

            



            this.litWorkArea.Text = u.WorkArea;

            this.litpart.Text = "";

            if (u.Part != null && !u.Part.Equals(""))
            {
                this.litpart.Text += u.Part;
            }

            if (u.Part2 != null && !u.Part2.Equals("")) {
                this.litpart.Text  +=  ", " + u.Part2;
            }

            if (u.Part3 != null && !u.Part3.Equals(""))
            {
                this.litpart.Text += ", " + u.Part3;
            }


            //, <a href="mailto:<%=UserEmail%>"><asp:Literal ID="litEmail" runat="server"></asp:Literal></a>
            // = u.EmailAddress;
            // u.Phone;
            this.litPhoneText.Text = "";
            if (u.Phone != null && !u.Phone.Equals(""))
            {
                this.litPhoneText.Text += u.Phone;
            }

            if (u.EmailAddress != null && !u.EmailAddress.Equals(""))
            {
                if (!this.litPhoneText.Text.Equals(""))
                {
                    this.litPhoneText.Text += " , ";
                }
                this.litPhoneText.Text += u.EmailAddress;
            }


     
            this.imgFace.ImageUrl = u.PhotoUrl;
            //this.lblvacation.Text = "근태";

            GlossaryProfileBiz biz1 = new GlossaryProfileBiz();
            GlossaryProfileType data = biz1.GlossaryProfileSelect(UserID);

            //this.lblDisplayBody.Text = data.Contents;
            lblDisplayBody.Text = "<p class='text' data-UserID=" + UserID + "><iframe id='StandaloneView' class='StandaloneView" + UserID + "' src='MyProfileIframe_View.aspx?UserID=" + UserID + "'  scrolling='no' frameborder='0' style='width:100%'><div class='view_ct_area' style='padding-top:10px;'><div class='view_ct' style='margin:0;'></div></div></iframe></p>";


            btnModify.Visible = true;
            //btnCommit.Visible = true;
            //btnFoolowInsert.Visible = false; //자기자신은 버튼이 없어짐.
            //CheckExPeople(u.JobCode);

            BeforeCarreerSelect(UserID);
            AfterCarreerSelect(UserID);
            GlossaryUserGlossaryList(UserID);

            DisplayCareer = "";

            EditNoSKCarrer.Visible = true;
            EditSKCarrer.Visible = true;

            string workstatus = EHRHelper.EHRWorkStatus(UserID);

            if (workstatus.Length == 0)
            {
                this.WorkStatus.Visible = false;
            }
            else
            {
                //this.WorkStatus.Text =  workstatus;
                this.WorkStatus.Text = "<br /><sapn style='padding-top:8px; display:inline-block; font-size:13px;'>" + workstatus + "</sapn>";
            }

        }


        private void NotLoginSelect(string UserID)
        {
            GlossaryProfileBiz biz = new GlossaryProfileBiz();
            ImpersonUserinfo u = biz.UserSelect(UserID);


            UserID = u.UserID;
            UserName = u.Name;
            UserTeam = u.DeptName;
            UserTeamCode = u.DeptID;
            UserEmail = u.EmailAddress;
            saperrator = " / ";
            saperrator2 = " / ";

            this.lblName.Text = UserName;
            this.lblTeam.Text = UserTeam;
            this.lblTeam2.Text = UserTeam;
            this.litPositionName.Text = u.PositionName;
            this.litSosok.Text = UserTeam;
            //this.litEmail.Text = u.EmailAddress;
            //this.litPhone.Text = u.Phone;
            this.litWorkArea.Text = u.WorkArea;
            this.litpart.Text = u.Part;
            this.imgFace.ImageUrl = u.PhotoUrl;

            this.litpart.Text = u.Part;

            if (u.Part2 != null && !u.Part2.Equals(""))
            {
                this.litpart.Text += ", " + u.Part2;
            }

            if (u.Part3 != null && !u.Part3.Equals(""))
            {
                this.litpart.Text += ", " + u.Part3;
            }


            //, <a href="mailto:<%=UserEmail%>"><asp:Literal ID="litEmail" runat="server"></asp:Literal></a>
            // = u.EmailAddress;
            // u.Phone;
            this.litPhoneText.Text = "";
            if (u.Phone != null && !u.Phone.Equals(""))
            {
                this.litPhoneText.Text += u.Phone;
            }

            if (u.EmailAddress != null && !u.EmailAddress.Equals(""))
            {
                if (!this.litPhoneText.Text.Equals(""))
                {
                    this.litPhoneText.Text += " , ";
                }
                this.litPhoneText.Text += u.EmailAddress;
            }



            GlossaryProfileType data = biz.GlossaryProfileSelect(UserID);

            //this.lblDisplayBody.Text = data.Contents;
            lblDisplayBody.Text = "<p class='text' data-UserID=" + UserID + "><iframe id='StandaloneView' class='StandaloneView" + UserID + "' src='MyProfileIframe_View.aspx?UserID=" + UserID + "'  scrolling='no' frameborder='0' style='width:100%'><div class='view_ct_area' style='padding-top:10px;'><div class='view_ct' style='margin:0;'></div></div></iframe></p>";

            btnModify.Visible = false;
            //btnCommit.Visible = false;
            //btnFoolowInsert.Visible = true; //자기자신은 버튼이 없어짐.
            //CheckExPeople(u.JobCode);

            btnBeforeCarreer.Visible = false;
            btnAfterCarreer.Visible = false;

            BeforeCarreerSelect(UserID);
            AfterCarreerSelect(UserID);
            GlossaryUserGlossaryList(UserID);

            DisplayCareer = "none";

            string workstatus = EHRHelper.EHRWorkStatus(UserID);

            if (workstatus.Length == 0)
            {
                this.WorkStatus.Visible = false;
            }
            else
            {
                this.WorkStatus.Text = workstatus;
                this.WorkStatus.Text = "<br /><sapn style='padding-top:8px; display:inline-block; font-size:13px;'>" + workstatus + "</sapn>";
            }


            UserInfo myu = new UserInfo(this.Page);
            myUserId = myu.UserID;
        }



        protected void hdbtnModify_Click(object sender, EventArgs e)
        {
            //Editor 변수명 변경 Mostisoft 2015.08.21
            //NamoEditor.Visible = true;
        }

        protected void btnGoDocRoom_Click(object sender, EventArgs e)
        {
            //location.href = "/GlossaryMyPages/MyDocumentsList.aspx?ReaderUserID=" + m_Empno;
            //Response.Redirect("/" + RootURL + "/GlossaryMyPages/MyDocumentsList.aspx?ReaderUserID="+UserID, false);
            Response.Redirect("/GlossaryMyPages/MyDocumentsList.aspx?ReaderUserID=" + UserID, false);

        }

        //protected void CheckExPeople(string UserID)
        //protected void CheckExPeople(string positioncode)
        //{
        //    int numpcode;
        //    if (int.TryParse(positioncode, out numpcode) == true)
        //    {
        //        //if (UserID == "1070155")  //하성민 사장님 은 부서 정보 제외
        //        if (numpcode < 301)  //301 은 팀장님  이값보다 작으면 직위가 더 높으신분이다 떄문에 숨긴다.
        //        {
        //            //btngoTeam.Visible = false;
        //            //UserTeam = "";
        //            saperrator2 = " / ";
        //            this.lblTeam.Visible = false;
        //            this.lblTeam2.Visible = true;  //링크없는버전
        //        }
        //        else if (numpcode != 305) //더큰값중에서 pl 이 아니면 더높은분이다.
        //        {
        //            //btngoTeam.Visible = false;
        //            //UserTeam = "";
        //            saperrator2 = " / ";
        //            this.lblTeam.Visible = false;
        //            this.lblTeam2.Visible = true;  //링크없는버전  
        //        }
        //    }
        //}

        protected void rptInSKCareer_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
            }
        }

        protected void rptInNotSKCareer_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
            }
        }

        protected void rptDocument_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal WikiClass = (Literal)e.Item.FindControl("ltWiki");
                Literal Num = (Literal)e.Item.FindControl("Num");
                Literal litReply = (Literal)e.Item.FindControl("litReply");
                Literal litPermission = (Literal)e.Item.FindControl("litPermission");

                // 2014-06-16 Mr.No
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Literal litUserInfo = (Literal)e.Item.FindControl("litUserInfo");

                // 2014-06-16 Mr.No
                if (drv["PrivateYN"].ToString().Equals("N"))
                {
                    //litUserInfo.Text = "<a href='javascript:fnProfileView(\"" + drv["UserID"].ToString() + "\");'>";
                    litUserInfo.Text += drv["UserName"].ToString() + "/" + drv["DeptName"].ToString();
                    /*
                     * 2014-08-12 Rank제외
                    litUserInfo.Text += "<img class=\"icon_img\" title=\"" + glossaryType.Rank + "\" width=\"19\" height=\"19\" src=\"";
                    litUserInfo.Text += ConfigurationManager.AppSettings["FrontImageUrl"] + glossaryType.Grade + ConfigurationManager.AppSettings["AftermageUrl"] + "\"/>";
                    */

                }
                else
                {
                    litUserInfo.Text = SKT.Common.SecurityHelper.Clear_XSS_CSRF(SKT.Common.SecurityHelper.Add_XSS_CSRF(drv["UserName"].ToString()));
                }
            }
        }

        #region btnSave_Click
        //저장
        protected void btnSave_Click(object sender, EventArgs e)
        {
            GlossaryProfileBiz biz = new GlossaryProfileBiz();
            GlossaryProfileType data = new GlossaryProfileType();

            UserInfo u = new UserInfo(this.Page);

            data.UserID = u.UserID;

            SKT.Tnet.Controls.WebEditorData wed = new WebEditorData();
            wed = SKT.Common.CommonActiveSquareEditor.GetDecodeMIME(hddActiveBody.Value, "TempImage");

            string strContents = wed.HtmlBody;
            strContents = strContents.Replace("<HTML>", "");
            strContents = strContents.Replace("<HEAD>", "");
            strContents = strContents.Replace("<TITLE>", "");
            strContents = strContents.Replace("</TITLE>", "");
            strContents = strContents.Replace("<META content=\"text/html; charset=utf-8\" http-equiv=Content-Type>", "");
            strContents = strContents.Replace("<META content=IE=5 http-equiv=X-UA-Compatible>", "");
            strContents = strContents.Replace("<META name=GENERATOR content=\"MSHTML 11.00.9600.18427\">", "");
            strContents = strContents.Replace("</HEAD>", "");
            strContents = strContents.Replace("<BODY style=\"FONT-SIZE: 10pt; FONT-FAMILY: 맑은 고딕\">", "");
            strContents = strContents.Replace("<BODY style=\"FONT-FAMILY: 맑은 고딕\">", "");
            strContents = strContents.Replace("</BODY>", "");
            strContents = strContents.Replace("</HTML>", "");

            data.Contents = Utility.WeeklyRemoveBackgroundColor(SKT.Common.SecurityHelper.Clear_XSS_CSRF(strContents).Trim());

            //data.DeptCode = DeptCode;
            //Editor Html의 nbsp 변환  Mostisoft 2015.08.21
            data.DeptCode = u.DeptID;

            //아래 두가지 값은 에디터에따라 다르게될예정...
            data.ContentsModify = string.Empty;
            data.Summary = "PERSON";

            biz.GlossaryProfileInsert(data);
        }
        #endregion

        [WebMethod]
        public static string ChangeProfile(string UserID, string DeptCode, string Description, string Mode)
        {
            string strMessage = string.Empty;

            try
            {
                GlossaryProfileBiz biz = new GlossaryProfileBiz();
                GlossaryProfileType data = new GlossaryProfileType();

                data.UserID = UserID;
                data.Contents = Description;
                //data.DeptCode = DeptCode;
                //Editor Html의 nbsp 변환  Mostisoft 2015.08.21
                data.DeptCode = DeptCode.InsertNBSP();

                //아래 두가지 값은 에디터에따라 다르게될예정...
                data.ContentsModify = string.Empty;
                data.Summary = Mode;

                biz.GlossaryProfileInsert(data);
                strMessage = "Success";
            }
            catch (Exception ex)
            {
                strMessage = "Exception : " + ex.Message;
            }

            return strMessage;
        }

        protected void btnFoolowInsert_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo(this.Page);
            GlossaryFollowDac dac = new GlossaryFollowDac();
            dac.GlossaryFollowInsert(u.UserID, UserID);
            //Response.Redirect("/" + RootURL + "/GlossaryMyPages/MyFollowList.aspx");
            Response.Redirect("/GlossaryMyPages/MyFollowList.aspx?ReaderUserID=" + UserID);
        }


        protected void rpt_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            GlossaryProfileBiz biz = new GlossaryProfileBiz();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal UserImg = (Literal)e.Item.FindControl("UserImg");
                ImpersonUserinfo data = biz.UserSelect(((GlossaryProfileType)e.Item.DataItem).UserID);
                UserImg.Text = "<img src=" + data.PhotoUrl + " align=\"" + data.Name + "/" + data.DeptName + "\" />";

            }
        }

        private string Base64Encode(string src, System.Text.Encoding enc)
        {
            byte[] arr = enc.GetBytes(src);
            return Convert.ToBase64String(arr);
        }

        private string Base64Decode(string src, System.Text.Encoding enc)
        {
            byte[] arr = Convert.FromBase64String(src);
            return enc.GetString(arr);
        }

        #region GetEHRData
        /// <summary>
        /// GetEHRData
        /// </summary>
        /// <param name="Empno"></param>
        /// <param name="CAREER_TYPE"></param>
        /// <returns></returns>
        // I_PERNR : 사원번호
        // I_SELEC  = 1:AFTER(입사후),  2:BEFORE(입사전)
        // I_GUBUN : 1:전체, 2이동기록,3승진기록
        // http://erp-ap2.sktelecom.com:8000/sap/bc/srt/wsdl/flv_10002A111AD1/bndg_url/sap/bc/srt/rfc/sap/zehr_disp_carrer_1/501/zehr_disp_carrer_1/zehr_disp_carrer_1?sap-client=501
        // http://erp-qa2.sktelecom.com:8003/sap/bc/srt/wsdl/flv_10002A111AD1/bndg_url/sap/bc/srt/rfc/sap/zehr_disp_carrer_1/501/zehr_disp_carrer_1/zehr_disp_carrer_1?sap-client=501
        public ArrayList GetEHRData(string Empno, string CAREER_TYPE)
        {
            string serviceURL = string.Empty;
            string serviceID = string.Empty;
            string servieePW = string.Empty;

            ArrayList list = new ArrayList();

            try
            {
                serviceURL = ConfigurationManager.AppSettings["eHRServiceUrl"].ToString();
                serviceID = ConfigurationManager.AppSettings["eHRServiceID"].ToString();
                servieePW = ConfigurationManager.AppSettings["eHRServicePW"].ToString();

                if (string.IsNullOrEmpty(serviceURL) || string.IsNullOrEmpty(serviceID) || string.IsNullOrEmpty(servieePW))
                {
                    throw new Exception("eHR Web service call info not exist");
                }

                ICredentials creds = new NetworkCredential(serviceID, servieePW, "");

                using (eHR.ZEHR_DISP_CARRER_1 webRefObject = new eHR.ZEHR_DISP_CARRER_1())
                {
                    webRefObject.Url = serviceURL;
                    webRefObject.Credentials = creds;
                    webRefObject.Timeout = 10000;

                    eHR.ZehrDispCarrer param = new eHR.ZehrDispCarrer();
                    param.IPernr = Empno;
                    param.ISelec = CAREER_TYPE.Equals("AFTER") ? "1" : "2";
                    param.IGubun = "2";
                    param.TOut1 = new eHR.Zehrs0002[1];
                    param.TOut2 = new eHR.Zehrs0003[1];

                    eHR.ZehrDispCarrerResponse result = webRefObject.ZehrDispCarrer(param);

                    if (result.EReturn.Type.Equals("S"))
                    {
                        eHR.Zehrs0002[] aa = result.TOut1;
                        eHR.Zehrs0003[] bb  = result.TOut2;

                        if (CAREER_TYPE.Equals("AFTER"))
                        {
                            foreach (eHR.Zehrs0002 xn in aa)
                            {
                                GlossaryProfileCareerAfterType data = new GlossaryProfileCareerAfterType();
                                data.ID = "0";
                                data.UserID = Empno;
                                data.Date = xn.Begda;
                                data.Status = xn.Massnt;
                                data.Depart = xn.Mgtxt;
                                list.Add(data);
                            }
                        }
                        else
                        {
                            foreach (eHR.Zehrs0003 xn in bb)
                            {
                                GlossaryProfileCareerBeforeType data = new GlossaryProfileCareerBeforeType();
                                data.ID = "0";
                                data.UserID = Empno;
                                data.Company = xn.Compy;
                                data.BeginDate = xn.Begda;
                                data.EndDate = xn.Endda;
                                data.Position = xn.Lstjw;
                                data.Depart = xn.Zdept;
                                data.Job = xn.Zgmjm;
                                list.Add(data);
                            }
                        }
                    }
                }
            }
            catch 
            {
                return null;
            }

            return list;
        }
        #endregion

        #region OLD GetEHRData
        //private string GetEHRData(string Empno, string CAREER_TYPE)
        //{
        //    //string Empno = "1109677";
        //    //string empno = "1109737";
        //    //string empno = "1108448";
        //    //string CAREER_TYPE ="BEFORE";   //BEFORE/AFTER
        //    //string CAREER_TYPE = "AFTER";   //BEFORE/AFTER

        //    string user_name = Base64Encode(Empno, System.Text.Encoding.Default);

        //    string param = string.Empty;
        //    string url = string.Empty;

        //    if (EHRCareerFG.Value.Equals("OLD"))
        //    {
        //        //AS-IS
        //        //string domain_name = "tikle.sktelecom.com";
        //        string domain_name = "tnet.sktelecom.com";
        //        param = "?user_name=" + user_name + "&domain_name=" + domain_name + "&System=Approval&CAREER_TYPE=" + CAREER_TYPE;
        //        param += "&saml2=disabled";
        //        url = ConfigurationManager.AppSettings["EHRCareerURL"].ToString() + param;
        //    }
        //    else
        //    {
        //        //TO-BE
        //        param = "?CAREER_TYPE=" + CAREER_TYPE;
        //        url = ConfigurationManager.AppSettings["EHRCareerURL"].ToString() + param;
        //    }

        //    // Prepare web request...
        //    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
        //    myRequest.Method = "GET";
        //    myRequest.Accept = "text/html, application/xhtml+xml, */*";

        //    //string thost = ".sktelecom.com";
        //    //Uri target = new Uri(url);
        //    //CookieContainer cookiedata = new CookieContainer();
        //    //cookiedata.Add(target, new Cookie("AuthorizationKey", "ec99907ef46b4a439e189cd2a8782085%7C150.19.7.12", "/", thost));
        //    //cookiedata.Add(target, new Cookie("domainlogon", "N", "/", thost));
        //    //cookiedata.Add(target, new Cookie("SM_USER", Empno, "/", thost));
        //    //cookiedata.Add(target, new Cookie("SM_DEPTCODE", "Z708", "/", thost));
        //    //cookiedata.Add(target, new Cookie("SM_COMPANY", "1000", "/", thost));
        //    //myRequest.CookieContainer = cookiedata;


        //    string resResult = string.Empty;
        //    try
        //    {
        //        using (HttpWebResponse wRes = (HttpWebResponse)myRequest.GetResponse())
        //        {
        //            long length = wRes.ContentLength;
        //            string pp = wRes.ContentType;

        //            Stream respPostStream = wRes.GetResponseStream();
        //            StreamReader readerPost = new StreamReader(respPostStream, Encoding.GetEncoding("UTF-8"), true);

        //            resResult = readerPost.ReadToEnd();

        //            respPostStream.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.ToString());
        //        return ex.ToString();
        //    }
        //    return resResult;
        //}
        #endregion

        public ArrayList GetAfterCareer(string Empno)
        {
            ArrayList list = new ArrayList();
            string CAREER_TYPE = "AFTER";
            list = GetEHRData(Empno, CAREER_TYPE);

            //string CAREER_TYPE = "BEFORE";
            //string XmlStr = GetEHRData(Empno, CAREER_TYPE);

            //XmlReader reader = XmlReader.Create(new StringReader(XmlStr));

            //XmlDocument xml = new XmlDocument();

            //xml.Load(reader);

            //string NodeQuery = "/PERSONAL_CAREER/AFTER_CAREERS/CAREER";
            //XmlNodeList xnList = xml.SelectNodes(NodeQuery); //접근할 노드


            //foreach (XmlNode xn in xnList)
            //{
            //    GlossaryProfileCareerAfterType data = new GlossaryProfileCareerAfterType();
            //    data.ID = "0";
            //    data.UserID = Empno;
            //    data.Date = xn["BEGDA"].InnerText;
            //    data.Status = xn["MASSNT"].InnerText;
            //    data.Depart = xn["MGTXT"].InnerText;

            //    list.Add(data);

            //}

            return list;
        }

        public ArrayList GetBeforeCareer(string Empno)
        {
            ArrayList list = new ArrayList();

            string CAREER_TYPE = "BEFORE";
            list = GetEHRData(Empno, CAREER_TYPE);
            //string XmlStr = GetEHRData(Empno, CAREER_TYPE);

            //XmlReader reader = XmlReader.Create(new StringReader(XmlStr));

            //XmlDocument xml = new XmlDocument();

            //xml.Load(reader);

            //string NodeQuery = "/PERSONAL_CAREER/BEFORE_CAREERS/CAREER";
            //XmlNodeList xnList = xml.SelectNodes(NodeQuery); //접근할 노드


            //foreach (XmlNode xn in xnList)
            //{
            //    GlossaryProfileCareerBeforeType data = new GlossaryProfileCareerBeforeType();
            //    data.ID = "0";
            //    data.UserID = Empno;
            //    data.Company = xn["COMPY"].InnerText;
            //    data.BeginDate = xn["BEGDA"].InnerText;
            //    data.EndDate = xn["ENDDA"].InnerText;
            //    //data.SKGYN = xn["SKGYN"].InnerText;
            //    data.Position = xn["LSTJW"].InnerText;
            //    data.Depart = xn["ZDEPT"].InnerText;
            //    data.Job = xn["ZGMJM"].InnerText;
            //    list.Add(data);

            //}

            return list;
        }

        protected void btnCarreer_Click(object sender, EventArgs e)
        {
            ArrayList listAfterCareer;
            ArrayList listBeforeCareer;
            GlossaryProfileBiz biz = null;
            UserInfo u = new UserInfo(this.Page);


            listAfterCareer = GetAfterCareer(u.UserID);

            //DB reset
            biz = new GlossaryProfileBiz();
            biz.GlossarySKCareerReset(u.UserID);

            //DB 저장
            for (int i = 0; i < listAfterCareer.Count; i++)
            {
                biz.GlossarySKCareerInsert((GlossaryProfileCareerAfterType)listAfterCareer[i]);
            }

            listBeforeCareer = GetBeforeCareer(u.UserID);

            //DB reset
            biz = new GlossaryProfileBiz();
            biz.GlossaryNoSKCareerReset(u.UserID);

            //DB 저장
            for (int i = 0; i < listBeforeCareer.Count; i++)
            {
                biz.GlossaryNoSKCareerInsert((GlossaryProfileCareerBeforeType)listBeforeCareer[i]);
            }

            DisplayCareer = "block";

            Response.Redirect("/GlossaryMyPages/MyProfile.aspx", false);

        }
        protected void btnAfterCarreer_Click(object sender, EventArgs e)
        {
            ArrayList list;

            UserInfo u = new UserInfo(this.Page);
            list = GetAfterCareer(u.UserID);

            //DB reset
            GlossaryProfileBiz biz = new GlossaryProfileBiz();
            biz.GlossarySKCareerReset(u.UserID);

            //DB 저장
            for (int i = 0; i < list.Count; i++)
            {
                biz.GlossarySKCareerInsert((GlossaryProfileCareerAfterType)list[i]);
            }

            rptInSKCareer.DataSource = list;
            rptInSKCareer.DataBind();

            Response.Redirect("/GlossaryMyPages/MyProfile.aspx", false);

        }
        protected void btnBeforeCarreer_Click(object sender, EventArgs e)
        {
            ArrayList list;

            UserInfo u = new UserInfo(this.Page);
            list = GetBeforeCareer(u.UserID);

            //DB reset
            GlossaryProfileBiz biz = new GlossaryProfileBiz();
            biz.GlossaryNoSKCareerReset(u.UserID);

            //DB 저장
            for (int i = 0; i < list.Count; i++)
            {
                biz.GlossaryNoSKCareerInsert((GlossaryProfileCareerBeforeType)list[i]);
            }

            rptInNotSKCareer.DataSource = list;
            rptInNotSKCareer.DataBind();

            Response.Redirect("/GlossaryMyPages/MyProfile.aspx", false);
        }

        protected int BeforeCarreerSelect(string UserID)
        {
            GlossaryProfileBiz biz = new GlossaryProfileBiz();

            ArrayList list = biz.GlossaryNoSKCareerList(UserID);

            rptInNotSKCareer.DataSource = list;
            rptInNotSKCareer.DataBind();

            return list.Count;
        }

        protected int AfterCarreerSelect(string UserID)
        {
            GlossaryProfileBiz biz = new GlossaryProfileBiz();

            ArrayList list = biz.GlossarySKCareerList(UserID);

            rptInSKCareer.DataSource = list;
            rptInSKCareer.DataBind();

            return list.Count;
        }

        protected void GlossaryUserGlossaryList(string UserID)
        {
            GlossaryProfileBiz biz = new GlossaryProfileBiz();
            DataSet ds = new DataSet();
            ds = biz.GlossaryUserGlossaryList(UserID);

            rptDocument.DataSource = ds.Tables[0];
            rptDocument.DataBind();
        }

        [WebMethod]
        public static string ModifyAfterCareerCall(string UserID, string ID, string Text)
        {
            GlossaryProfileBiz biz = new GlossaryProfileBiz();

            GlossaryProfileCareerAfterType data = new GlossaryProfileCareerAfterType();
            data.ID = ID;
            data.UserID = UserID;
            data.Message = Text;

            biz.GlossarySKCareerInsert(data);
            return "SUCCESS";
        }

        [WebMethod]
        public static string DeleteCareerCall(string type, string ID)
        {
            GlossaryProfileDac dac = new GlossaryProfileDac();
            dac.GlossarySKCareerDelete(ID, type);

            return "SUCCESS";
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 개인 랭킹 점수
        /// </summary>
        /// <param name="UserID"></param>
        protected void ScoreRanking(string UserID)
        {
            if (!String.IsNullOrEmpty(UserID))
            {
                scoreRankingType = ScoreRankingDac.Instance.ScoreRankingSelect(UserID);

                int addPercent = (scoreRankingType.TotalScore / 100);

                //if (scoreRankingType.Grade == 1) 
                //{
                //    scoreRankingType.Rank = "지존";
                //    Percent = 74 + addPercent;
                //}
                //else
                if (scoreRankingType.Grade == 0)
                {
                    scoreRankingType.Rank = "지존";
                    //Percent = 49 + addPercent;
                }
                else if (scoreRankingType.Grade == 1)
                {
                    scoreRankingType.Rank = "고수";
                    //Percent = 49 + addPercent;
                }
                else if (scoreRankingType.Grade == 2)
                {
                    scoreRankingType.Rank = "중수";
                    //Percent = 26 + addPercent;
                }
                else
                {
                    scoreRankingType.Rank = "초수";
                    //Percent = addPercent;
                }

                hdCheck.Value = "show";
            }
            else
            {
                scoreRankingType = new ScoreRankingType();
                scoreRankingType.Rank = "";
                scoreRankingType.TotalScore = 0;
            }
        }

        /// <summary>
        /// SK C&C 박준용 과장님 계산법
        /// </summary>
        protected string Calc(int TotalScore)
        {
            double y1 = 20;
            double y2 = 70;
            double y3 = 1500;
            double y4 = 20000;

            double x = TotalScore;
            double rtnValue = 0;

            if (x <= y1)
            {
                rtnValue = 0 + ((x / y1) * 25);
            }
            else if (y1 < x && x <= y2)
            {
                rtnValue = 25 + (((x - y1) / (y2 - y1)) * 25);
            }
            else if (y2 < x && x <= y3)
            {
                rtnValue = 50 + (((x - y2) / (y3 - y2)) * 25);
            }
            else if (x > y3)
            {
                rtnValue = 75 + (((x - y3) / (y4 - y3)) * 25);
            }

            // 그래프 넘어가지 않게 제한함
            if (rtnValue > 100)
            {
                rtnValue = 100;
            }

            // 2014-07-08 Mr.No 주석처리 함
            // 지존은 없으므로 제한함
            //if (rtnValue > 75)
            //{
            //    rtnValue = 75;
            //}

            return rtnValue.ToString();
        }

        
    }
       
}