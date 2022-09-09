using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Net;
using System.IO;
using SKT.Common;
using System.Web;

namespace SKT.Common
{
    public class UserInfo
    {
        public enum UserLevelEnum
        {
            User                    //일반 구성원
            ,
            Admin                 //전체 관리자
                ,
            CEO                   //CEO
                ,
            Writer                //작성자
                //아래는 정리해야 한다...
                ,
            PlaLabAdmin           //PlaLab 관리자
                ,
            ODOMAdmin             //ODOM 관리자
                ,
            PlanetXAdmin          //PlanetX 관리자
                ,
            OpenAranaAdmin        //OpenArena 관리자
                ,
            WIKIAdmin             //WIKI 관리자
                , Chief
        }
        #region #####_멤버변수_#####
        protected Page Page = null;
        protected System.Web.SessionState.HttpSessionState Session = null;
        #endregion

        public UserInfo(System.Web.UI.Page Page)
        {
            this.Page = Page;
            this.Session = Page.Session;
        }

        public UserInfo()
        {
            // TODO: Complete member initialization
        }
        public string UserID
        {
            get { return GetSessionObject("UserID"); }
            set
            {
                //외부에서 UserID를 정해 주면 그에 따른다.
                SetUserInfo(value);
            }
        }
        public string Name { get { return GetSessionObject("Name"); } }
        public string DeptID
        {
            get { return GetSessionObject("DeptID"); }
            set { SetSessionObject("DeptID", value); }
        }
        public string DeptName
        {
            get { return GetSessionObject("DeptName"); }
            set { SetSessionObject("DeptName", value); }
        }
        public string DeptPath { get { return GetSessionObject("DeptPath"); } }
        public string CompanyID { get { return GetSessionObject("CompanyID"); } }
        public string EmailAddress { get { return GetSessionObject("EmailAddress"); } }
        public string WorkArea { get { return GetSessionObject("WorkArea"); } }
        public string TEL { get { return GetSessionObject("TEL"); } } //직책
        public string Phone { get { return GetSessionObject("Phone"); } } //직위
        public string PhotoUrl { get { return GetSessionObject("PhotoUrl"); } }
        public string Part { get { return GetSessionObject("Part"); } }
        public string Part2 { get { return GetSessionObject("Part2"); } }
        public string Part3 { get { return GetSessionObject("Part3"); } }
        public string JobCode { get { return GetSessionObject("JobCode"); } } //직무
        public string JobCodeName { get { return GetSessionObject("JobCodeName"); } } //직무
        //public string Nickname { get { return GetSessionObject("Nickname"); } }
        //public string AdminLevel { get { return GetSessionObject("AdminLevel"); } }
        public string Level { get { return GetSessionObject("LEVEL"); } }
        public string UserLang { get { return GetSessionObject("UserLang"); } } //다국어



        public UserLevelEnum UserLevel
        {
            get
            {
                switch (GetSessionObject("UserLevel").ToUpper())
                {
                    case "USER": return UserLevelEnum.User;
                    case "ADMIN": return UserLevelEnum.Admin;
                    case "CEO": return UserLevelEnum.CEO;
                    case "WRITER": return UserLevelEnum.Writer;
                    //아래는 정리해야 한다...
                    case "PLALAB_ADMIN": return UserLevelEnum.PlaLabAdmin;
                    case "ODOM_ADMIN": return UserLevelEnum.ODOMAdmin;
                    case "PLANETX_ADMIN": return UserLevelEnum.PlanetXAdmin;
                    case "OPENARENA_ADMIN": return UserLevelEnum.OpenAranaAdmin;
                    case "WIKI_ADMIN": return UserLevelEnum.WIKIAdmin;
                    default: return UserLevelEnum.User;
                }
            }
        }

        //20140107 , 로그인 별도 처리 로직 변경
        public bool isIdOnlyNum { get { return SetIdOnlyNum(); } }
        public bool isTiklei { get { return SetSpecialUserInfo("1"); } }  //1 티끌이 
        public bool isAdmin { get { return SetSpecialUserInfo("2"); } }   //2 관리자
        public bool isManager { get { return SetSpecialUserInfo("3"); } } //3 매니저
        
        //public bool isGuser { get { return SetSpecialUserInfo("G"); } }   //G 끌지식 예외접속자
        //public bool isDuser { get { return SetSpecialUserInfo("D"); } }   //D 끌문서 예외접속자
        //public bool isDTuser { get { return SetSpecialUserInfo("DT"); } } //DT블로그 - DT센터 예외접속자

        public bool IsGlossaryPermission { get { return SetRole("G"); } } //G 끌지식 예외접속자
        public bool IsDirectoryPermission { get { return SetRole("D"); } } //D 끌문서 예외접속자
        public bool IsDTPermission { get { return SetRole("DT"); } } //DT블로그 - DT센터 예외접속자 

        // CHG610000076956 / 20181206 / 끌지식권한체크
        public bool SetRole(string kind)
        {
            bool returnData = false;
            string RoleString = string.Empty;

            //끌지식
            if (kind.Equals("G"))
            {
                RoleString = ConfigurationManager.AppSettings["RoleGlossary"].ToString();
            }
            else if (kind.Equals("D"))
            {
                RoleString = ConfigurationManager.AppSettings["RoleDirectory"].ToString();
            }
            else if (kind.Equals("DT"))
            {
                RoleString = ConfigurationManager.AppSettings["RoleDT"].ToString();
            }

            string[] arr = RoleString.Split(',');
            var target = UserID.Substring(0, 2);
            string match = Array.Find(arr, n => n.Equals(target));

            if (!string.IsNullOrEmpty(match))
            {
                returnData = true;
            }
            else if (isTiklei || isAdmin || SetSpecialUserInfo(kind))
            {
                returnData = true;
            }
            
            return returnData;
        }

        public bool SetSpecialUserInfo(string kind)
        {
            string[] arr = this.Level.Split(',');
            string match = string.Empty;

            if (arr.Length == 0)
            {
                if (this.Level.Trim().Equals(kind))
                    match = kind.ToString();
            }
            else
            {
                match = Array.Find(arr, n => n.Equals(kind));
            }

            bool ssui = false;
            if (!string.IsNullOrEmpty(match))
            {
                ssui = true;
            }
            return ssui;
        }

        

        //20140123 , 번호사번 식별 추가
        int uuid;
        public bool SetIdOnlyNum()
        {
            bool idon = false;
            if (int.TryParse(UserID, out uuid) == true) 
            {
                idon = true;
            }
            return idon;
        }


        public string userIp
        {
            get
            {
                string userIp;
                userIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                return userIp;
            }
        }

        public string userMachineName
        {
            get
            {
                string userMachineName;
                userMachineName = System.Net.Dns.GetHostName();
                return userMachineName;
            }
            
        }

        // 20150306 팀장여부 및 PositionCode 추가
        public string PositionCode { get { return GetSessionObject("PositionCode"); } }
        public string TeamChiefYN { get { return GetSessionObject("PositionCode") == "301" ? "Y" : "N"; } } //팀장여부
        public string PositionName { get { return GetSessionObject("PositionName"); } }

        // 20150312 ViewLEVEL 추가
        public string ViewLevel
        {
            get { return GetSessionObject("ViewLevel"); }
            set { SetSessionObject("ViewLevel", value); }
        }

        // 20150311 UpperDepartmentNumber 추가
        public string UpperDepartmentNumber
        {
            get { return GetSessionObject("UpperDepartmentNumber"); }
            set { SetSessionObject("UpperDepartmentNumber", value); }
        }
        public string UpperDepartmentName
        {
            get { return GetSessionObject("UpperDepartmentName"); }
            set { SetSessionObject("UpperDepartmentName", value); }
        }

        // 20150325 겸직 추가
        public string AdditionalJobCode
        {
            get { return GetSessionObject("AdditionalJobCode"); }
            set { SetSessionObject("AdditionalJobCode", value); }
        }

        public string AdditionalJobName
        {
            get { return GetSessionObject("AdditionalJobName"); }
            set { SetSessionObject("AdditionalJobName", value); }
        }



        //04월20일 : 부문에 팀원이 속해있는경우 탭 1만 보이도록 수정
        public string HasChild {
            get { return GetSessionObject("hasChild"); }
            set { SetSessionObject("hasChild", value); }
        }

        public string RealViewLevel
        {
            get { return GetSessionObject("RealViewLevel"); }
            set { SetSessionObject("RealViewLevel", value); }
        }


        // 20150401 Weekly 인증사용자 여부 추가
        public string WeeklyAuthUserYN
        {
            get { return GetSessionObject("WeeklyAuthUserYN"); }
        }

        //외부에서 UserID가 정해지지 않으면 현재 시스템에 맞게 구한다. SSO나 HttpContext.
        private string GetUserID()
        {
            string UserID = string.Empty;
            ////HTTP Context
            //{
            //    UserID = System.Web.HttpContext.Current.User.Identity.Name;//string.Empty;
            //    if (UserID.Contains("\\\\"))
            //    {
            //        UserID = UserID.Split(new string[] { "\\\\" }, StringSplitOptions.None)[1];
            //    }
            //    else if (UserID.Contains("\\"))
            //    {
            //        UserID = UserID.Split(new string[] { "\\" }, StringSplitOptions.None)[1];
            //    }
            //    else { }
            //}

            
            //SSO
            UserID = System.Web.HttpContext.Current.Request.Headers["SM_USER"];

            /*
            Author : 개발자-최현미C, 리뷰자-진현빈D
            Create Date : 2016.12.08 
            Desc : Request.Headers가 없을 경우 Request.Cookies로 다시 확인
            */

            if (String.IsNullOrEmpty(UserID))
            {
                if (System.Web.HttpContext.Current.Request.Cookies["SM_USER"] != null)
                {
                    UserID = System.Web.HttpContext.Current.Request.Cookies["SM_USER"].Value;
                }
            }

            if (!String.IsNullOrEmpty(UserID))
                UserID = UserID.ToUpper().Replace(@"SKT\", "");

            //UserID = System.Web.HttpContext.Current.Request["SM_USER"];

            //vpn에서 AD 인증 받고 Toktok 을 먼저 로긴해서 사용할경우는 
            // SM_USER가 모두 없으므로 url에서 사번을 얻는다. (즉 각페이지에서 값을 받도록되어있음...)

            //테스트 아이디

            string IsTestServer = ConfigurationManager.AppSettings["IsTestServer"];
            if (IsTestServer == "Y")
            {
<<<<<<< HEAD
                UserID = "1901081";
=======
                //UserID = "1107160";
>>>>>>> 58671c0c20e24bdacd11e4e4a4c54898af7ff265
            }

            //Alert("UserInfo.cs : GetUserID() : loginSabun => " + UserID, "alert_GetUserID()");

            return UserID;
        }

        public void SetViewLevel(string viewLevel)
        {
            SetSessionObject("ViewLevel", viewLevel);
        }


        /*
        Author : 개발자-장찬우G, 리뷰자-이정선G 
        Create Date : 2016.02.17 
        Desc : 페이지 디버깅용으로 사용할 Alert함수 생성
        */        
        public static void Alert(string message)
        {
            // Clean the message to allow the single quotation mark 
            string cleanMessage = message.Replace("'", "\\'");
            string script = string.Format("alert('{0}');", cleanMessage);
            // Gets the executing web page 
            Page page = HttpContext.Current.CurrentHandler as Page;
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true);
            }
        }

        /*
        Author : 개발자-장찬우G, 리뷰자-이정선G 
        Create Date : 2016.02.17 
        Desc : 페이지 디버깅용으로 사용할 Alert함수 overloading
        */  
        public static void Alert(string message, string key)
        {
            // Clean the message to allow the single quotation mark 
            string cleanMessage = message.Replace("'", "\\'");
            string script = string.Format("alert('{0}');", cleanMessage);
            // Gets the executing web page 
            Page page = HttpContext.Current.CurrentHandler as Page;
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered(key))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), key, script, true);
            }
        }

        public void SetUserInfo()
        {
            string UserID = GetUserID();
            if (!string.IsNullOrEmpty(UserID))
            {
                //Alert("UserInfo.cs : SetUserInfo() : loginSabun => " + UserID, "alert_SetUserInfo1");
                //Alert("UserInfo.cs : SetUserInfo() : SessionID => " + Session.SessionID.ToString(), "alert_SetUserInfo2");

                /*
                Author : 개발자-장찬우G, 리뷰자-이정선G 
                Create Date : 2016.02.17 
                Desc : Log4Net 로그 생성
                */
                Log4NetHelper.Info("--------------------------------------------------------------");
                Log4NetHelper.Info("UserInfo.cs : SetUserInfo() : loginSabun => " + UserID);
                Log4NetHelper.Info("UserInfo.cs : SetUserInfo() : Headers[SM_USER] => " + HttpContext.Current.Request.Headers["SM_USER"]);
                //Log4NetHelper.Info("UserInfo.cs : SetUserInfo() : Headers[SMSESSION] => " + HttpContext.Current.Request["SMSESSION"]);
<<<<<<< HEAD
                //Log4NetHelper.Info("UserInfo.cs : SetUserInfo() : Cookies[SM_USER] => " + HttpContext.Current.Request.Cookies["SM_USER"].Value);
=======
                Log4NetHelper.Info("UserInfo.cs : SetUserInfo() : Cookies[SM_USER] => " + HttpContext.Current.Request.Cookies["SM_USER"].Value);
>>>>>>> 58671c0c20e24bdacd11e4e4a4c54898af7ff265
                //Log4NetHelper.Info("UserInfo.cs : SetUserInfo() : Cookies[SMSESSION ] => " + HttpContext.Current.Request.Cookies["SMSESSION"].Value);
                Log4NetHelper.Info("UserInfo.cs : SetUserInfo() : Session.SessionID => " + Session.SessionID.ToString());
                Log4NetHelper.Info("UserInfo.cs : SetUserInfo() : Session.IsNewSession => " + Session.IsNewSession.ToString());
                Log4NetHelper.Info("UserInfo.cs : SetUserInfo() : IP Address => " + HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR"));
                Log4NetHelper.Info("--------------------------------------------------------------");
                SetUserInfo(GetUserID());
            }
            else
            {
                /*
                Author : 개발자-장찬우G, 리뷰자-이정선G 
                Create Date : 2016.02.17 
                Desc : Log4Net 로그 생성
                */
                Log4NetHelper.Error("UserInfo.cs : SetUserInfo() : Headers > SM_USER 값이 없습니다.");
                //Alert("Headers > SM_USER 값이 없습니다.", "alert_SetUserInfo3");

            }
        }
        public void SetUserInfo(string UserID)
        {
            if (string.IsNullOrEmpty(UserID))
            {
                SetSessionObject("UserID", UserID);
                return;
            }
            string UserHash = CryptoHelper.GetSHA512(UserID);
            //string connectionStringName = "ConnPIN";
            string connectionStringName = "ConnGlossary";
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);                      
            DbCommand cmd = db.GetStoredProcCommand("up_UserInfo_Select");
            db.AddInParameter(cmd, "UserID", DbType.String, UserID);
            DataSet dsUserInfo = db.ExecuteDataSet(cmd);

            SetSessionObject("UserID", UserID);
            SetSessionObject("CompanyID", "SKT");
            if (dsUserInfo.Tables.Count > 0 && dsUserInfo.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsUserInfo.Tables[0].Rows[0];
                SetSessionObject("Name", dr["Name"].ToString());
                SetSessionObject("DeptID", dr["DeptID"].ToString());
                SetSessionObject("DeptName", dr["DeptName"].ToString());
                SetSessionObject("DeptPath", dr["DeptPath"].ToString());
                SetSessionObject("EmailAddress", dr["EmailAddress"].ToString());
                SetSessionObject("WorkArea", dr["WorkArea"].ToString());//"/Common/images/min.jpg");
                SetSessionObject("Part", dr["Part"].ToString());//"/Common/images/min.jpg");
                SetSessionObject("Part2", dr["Part2"].ToString());//"/Common/images/min.jpg");
                SetSessionObject("Part3", dr["Part3"].ToString());//"/Common/images/min.jpg");
                //SetSessionObject("Title", dr["Title"].ToString());
                //SetSessionObject("Duty", dr["Duty"].ToString());
                //SetSessionObject("Job", dr["Job"].ToString());
                SetSessionObject("TEL", dr["TEL"].ToString());
                SetSessionObject("Phone", dr["Phone"].ToString());
                SetSessionObject("JobCode", dr["JobCode"].ToString());
                SetSessionObject("JobCodeName", dr["JobCodeName"].ToString());
                SetSessionObject("AdminLevel", dr["AdminLevel"].ToString());
                //20140116 , 특별 사용자 로직 변경
                SetSessionObject("Level", dr["LEVEL"].ToString());
                //SetSessionObject("Nickname", dr["Nickname"].ToString());

                SetSessionObject("PositionCode", dr["PositionCode"].ToString());
                SetSessionObject("ViewLevel", dr["ViewLevel"].ToString());
                SetSessionObject("PositionName", dr["PositionName"].ToString());

                SetSessionObject("UpperDepartmentNumber", dr["UpperDepartmentNumber"].ToString());
                SetSessionObject("UpperDepartmentName", dr["UpperDepartmentName"].ToString());

                SetSessionObject("AdditionalJobCode", dr["AdditionalJobCode"].ToString());
                SetSessionObject("AdditionalJobName", dr["AdditionalJobName"].ToString());

                SetSessionObject("WeeklyAuthUserYN", dr["WeeklyAuthUserYN"].ToString());

                //04월20일 : 부문에 팀원이 속해있는경우 탭 1만 보이도록 수정
                SetSessionObject("HasChild", dr["hasChild"].ToString());

                SetSessionObject("RealViewLevel", dr["RealViewLevel"].ToString());
         
            }
            if (dsUserInfo.Tables.Count > 1 && dsUserInfo.Tables[1].Rows.Count > 0)
            {
                DataRow dr = dsUserInfo.Tables[1].Rows[0];
                SetSessionObject("UserLevel", dr["UserLevel"].ToString());
            }
            SetSessionObject("UserLang", GetUserLangByWebRequest(UserID));


            ////OrgChart에서 사진 정보 가져오기
            connectionStringName = "ConnGlossary";
            Database db2 = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand cmd2 = db.GetStoredProcCommand("up_UserInfo_Photo");
            db2.AddInParameter(cmd2, "UserID", DbType.String, UserID);
            //DbCommand cmd2 = db2.GetSqlStringCommand("SELECT [photoURL] FROM [OrgChart].[dbo].[tb_person] WHERE employeeID = '" + UserID + "'");
            DataSet dsPhotoUrl = db2.ExecuteDataSet(cmd2);
            if (dsPhotoUrl.Tables.Count > 0 && dsPhotoUrl.Tables[0].Rows.Count > 0)
            {                
                DataRow dr = dsPhotoUrl.Tables[0].Rows[0];
                SetSessionObject("PhotoUrl", dr["photoURL"].ToString());                
            }
            ////OrgChart에서 사진 정보 가져오기 끝
#if DEBUG
#else
            
            connectionStringName = "ConnOrgChart";
            //Database db2 = DatabaseFactory.CreateDatabase(connectionStringName);
            //DbCommand cmd2 = db2.GetSqlStringCommand("SELECT [photoURL] FROM [OrgChart_SKP].[dbo].[tb_person] WHERE employeeID = '" + UserID + "'");
            //DataSet dsPhotoUrl = db2.ExecuteDataSet(cmd2);
            //if (dsPhotoUrl.Tables.Count > 0 && dsPhotoUrl.Tables[0].Rows.Count > 0)
            //{
            //    DataRow dr = dsPhotoUrl.Tables[0].Rows[0];
            //    SetSessionObject("PhotoUrl", dr["photoURL"].ToString());
            //}
            ////OrgChart_SKP에서 사진 정보 가져오기 끝
#endif

        }

        
        /// <summary>세션 데이터 조회, (템플릿)객체 리턴</summary>        
        protected string GetSessionObject(string objName)
        {
            if (this.Session["UserID"] == null) 
            {
                //Alert("UserInfo.cs : GetSessionObject(string objName) : Session > UserID 값이 없을때 호출됨...", "alert_GetSessionObject1");                
                SetUserInfo(); 
            }
            return (this.Session[objName] ?? "").ToString();
        }

        protected void SetSessionObject(string objName, string value)
        {
            this.Session[objName] = value;
        }


        #region #####_보조메소드_#####
        /// <summary>쿼리 샐행결과 가져오기</summary>
        protected DataSet GetDataSet(SqlConnection conn, SqlCommand cmd)
        {
            DataSet rtVal = new DataSet();
            IDataAdapter oAdpter = GetDataAdapter(conn, cmd);
            oAdpter.Fill(rtVal);
            return rtVal;
        }
        /// <summary>데이터 실행을 위한 어댑터 생성</summary>    
        protected IDataAdapter GetDataAdapter(SqlConnection conn, SqlCommand cmd)
        {
            IDataAdapter rtVal = null;
            rtVal = new SqlDataAdapter(cmd.CommandText, conn);
            ((SqlDataAdapter)rtVal).SelectCommand = cmd;
            return rtVal;
        }
        protected string GetUserLangByWebRequest(string UserID)
        {
            string responseFromServer = "KOR";

            try
            {
                // Create a request for the URL. 
                string url = (ConfigurationManager.AppSettings["IFPnetMultilangURL"] ?? "").ToString();
                WebRequest request = WebRequest.Create(url + "?empNo=" + UserID);
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Clean up the streams and the response.
                reader.Close();
                response.Close();

                if (string.IsNullOrEmpty(responseFromServer))
                {
                    responseFromServer = "KOR";
                }
            }
            catch (Exception)
            {
                return "KOR";
            }

            return responseFromServer;
        }

        public void SetUserLang(string lang)
        {
            SetSessionObject("UserLang", lang);
        }
        public void SetUserLang()
        {
            SetSessionObject("UserLang", GetUserLangByWebRequest(UserID));
        }
        #endregion
    }
}
