using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using SKT.Glossary.Biz;
using System.Collections;
using System.Data;
using SKT.Glossary.Dac;
using SKT.Glossary.Type;
using SKT.Common;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using SKT.Glossary.Web.Directory;

namespace SKT.Glossary.Web.Common.Controls
{
    public class AjaxUserInfo
    {
        public string Name;
        public string DeptName;
        public string EmpID;
    }

    public class AjaxTagList
    {
        public string id;
        public string name;
    }

    public class AjaxTag
    {
        public string id;
        public string name;
        public ArrayList adjacencies = new ArrayList();
        public ArrayList data = new ArrayList();
    }

    public class AjaxUrlList
    {
        public string Title;
        public string linkurl;
    }

    public class AjaxMyGroupList
    {
        public string ToUserID;
        public string ToUserName;
		public string ToUserType;
    }



    public partial class AjaxPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string Type = (Request["Type"] ?? string.Empty).ToString();
            string TagTitle = (Request["TagTitle"] ?? string.Empty).ToString();
            string CommonID = (Request["CommonID"] ?? string.Empty).ToString();
            if (Type == "GlossaryTagList")
            {
                List<string> result = new List<string>();

                AjaxTagList TagList = new AjaxTagList();
                AjaxTag temp = new AjaxTag();
                ArrayList TotalAdd = new ArrayList();

                DataSet ds = new DataSet();
                GlossaryControlDac dac = new GlossaryControlDac();
                ds = dac.GlossaryTagList(TagTitle, null);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        temp.adjacencies.Add("*" + dr["ID"] + "*");

                    }
                    temp.data.Add("'$color': '#D9EBFA'");
                    temp.data.Add("'$type': 'rectangle'");
                    temp.data.Add("'$width': 60");
                    temp.data.Add("'$height': 18");
                    temp.id = CommonID;
                    temp.name = TagTitle;

                    string json1 = new JavaScriptSerializer().Serialize(temp);
                    TotalAdd.Add(json1);



                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        TagList.id = dr["ID"].ToString();
                        TagList.name = dr["Title"].ToString();
                        //TotalAdd.Add(TagList);
                        string json = new JavaScriptSerializer().Serialize(TagList);
                        TotalAdd.Add(json);
                    }
                }

                Response.Write(new JavaScriptSerializer().Serialize(TotalAdd).Replace("\\", "").Replace("[\"", "[").Replace("\"]", "]").Replace("\"{", "{").Replace("}\"", "}").Replace("\"*", "\"").Replace("*\"", "\"").Replace("*", "\"")
                    .Replace("u00027", "\"").Replace("u0027", "\"").Replace("\"\"", "\"").Replace("60\"", "60").Replace("[\"$color", "{\"$color").Replace("18]", "18}"));


                Response.End();
            }
        }


        //GNB 자동완성 기능
        [WebMethod]
        public static List<string> GetAutoCompleteData(string username)
        {
            List<string> result = new List<string>();
			
            DataSet ds = new DataSet();
            GlossaryDac dac = new GlossaryDac();
			ds = dac.SearchAutoList(username, "MainList");
			
			if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					//result.Add(dr["Title"].ToString());
                    /*
                    Author : 개발자-김성환D, 리뷰자-이정선G
                    Create Date : 2016.02.24
                    Desc : 검색 자동완성 특수문자 변환
                    */
                    string deCodeStr = System.Web.HttpUtility.HtmlDecode(dr["Title"].ToString());
                    result.Add(deCodeStr);
				}
			}
			return result;
        }

		//GNB 자동완성 기능
		[WebMethod]
		public static List<string> GetAutoCompleteDataAdmin(string username)
		{
			List<string> result = new List<string>();
			DataSet ds = new DataSet();
			GlossaryDac dac = new GlossaryDac();
			ds = dac.SearchAutoList(username, "Admin");

			if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					result.Add(dr["Title"].ToString());
					result.Add(dr["CommonID"].ToString());
				}
			}

			return result;
		}

        //GNB 에디터에 제공할 문서 제목및 url....
        [WebMethod]
		public static List<string> GetTileLink(string KeyWord)
        {
            List<string> result = new List<string>();
            DataSet ds = new DataSet();
            GlossaryDac dac = new GlossaryDac();
            ds = dac.SearchAutoList(KeyWord, "MainList");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //result.Add(dr["Title"].ToString());
                    //result.Add("/Main.aspx");  //임시 코드 메인 코드로 넘김.
                    //result.Add(dr["linkurl"].ToString());

                    AjaxUrlList temp = new AjaxUrlList();
                    temp.Title = dr["Title"].ToString();
                    
                    if (dr["CommonID"].ToString() !="0") 
                    {
                        temp.linkurl = "/Glossary/GlossaryView.aspx?mode=Histroy&ItemID=" + dr["CommonID"].ToString();
                    }
                    else if (dr["QNAID"].ToString()!="0")
                    {
                        temp.linkurl = "/QnA/QnAView.aspx?ItemID=" + dr["QNAID"].ToString();
                    }

                    if (dr["UserID"].ToString().Length > 0)
                    {
                        temp.linkurl = "/GlossaryMyPages/MyProfile.aspx?UserID=" + dr["UserID"].ToString();
                    }

                    string json = new JavaScriptSerializer().Serialize(temp);
                    result.Add(json);
                }
            }
            return result;
        }


        //추가 
        [WebMethod]
        public static List<string> GetMyGroupList(string Type, string UserID, string MyGrpID)
        {
            //List<GlossaryGroupAuthType> result = new List<GlossaryGroupAuthType>();
            List<string> result = new List<string>();
            DataSet ds = new DataSet();
            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();
            ds = Dac.MyGroupListSelect2(UserID, MyGrpID, Type);


            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //result.Add(dr["Title"].ToString());
                    //result.Add("/Main.aspx");  //임시 코드 메인 코드로 넘김.
                    //result.Add(dr["linkurl"].ToString());

                    AjaxMyGroupList temp = new AjaxMyGroupList();
                    temp.ToUserID = dr["ToUserID"].ToString();
                    temp.ToUserName = dr["ToUserName"].ToString();
					temp.ToUserType = dr["ToUserType"].ToString();

					if (temp.ToUserType == "G") {
						temp.ToUserName = "[그룹]" + temp.ToUserName;
					}

                    string json = new JavaScriptSerializer().Serialize(temp);
                    result.Add(json);
                }
            }
            return result;
        }

        //2016-11-03 문서 공동관리자 설정
        [WebMethod]
        public static List<string> GetManagerList(string commonID, string tkType)
        {
            List<string> result = new List<string>();
            if (commonID != null && commonID != "") { 
                DataSet ds = new DataSet();
                GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();
                ds = Dac.MyGroupListManagerSelect(commonID, tkType);

                //방장 추가
                AjaxMyGroupList temp1 = new AjaxMyGroupList();
                temp1.ToUserID = ds.Tables[1].Rows[0]["CreaterID"].ToString();
                temp1.ToUserName = ds.Tables[1].Rows[0]["CreaterNM"].ToString();
                temp1.ToUserType = "U";
                string json1 = new JavaScriptSerializer().Serialize(temp1);
                result.Add(json1);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        AjaxMyGroupList temp = new AjaxMyGroupList();
                        temp.ToUserID = dr["ManagerID"].ToString();
                        temp.ToUserName = dr["ManagerName"].ToString();
                        temp.ToUserType = "U";

                        string json = new JavaScriptSerializer().Serialize(temp);
                        result.Add(json);
                    }
                }
            }
            return result;
        }
        //2016-11-03 문서 공동관리자 설정 1901080/1901080/1901080/
        [WebMethod]
        public static bool GetDirectoryManagerCheck(string commonID, string arrUserID)
        {
            DataSet ds = new DataSet();
            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();
            ds = Dac.GetDirectoryManagerCheck(commonID);

            string[] useridlist = arrUserID.Split('/');

            int i = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (string drr in useridlist)
                {
                    DataRow[] drs = ds.Tables[0].Select("EMPNO IN ('" + drr + "')");

                    i += drs.Length;

                }
            }

            if (i == useridlist.Length - 1)
                return true;
            else
                return false;
        }



        //이철수 추가  
        //타이틀 검색.체크
        [WebMethod]
		public static List<string> ExistTitleMyGroup(string Title, string ID)
        {
            List<string> result = new List<string>();
            GlossaryControlBiz biz = new GlossaryControlBiz();
			//string pp = biz.ExistTitleMyGroup(Title, ID);

            //result.Add(pp);
            return result;
        }


        //사용자 자동 검색 (김성환 20150121)
        [WebMethod]
        public static List<string> GetAutoCompleteUserData(string username)
        {
            List<string> result = new List<string>();
            DataSet ds = new DataSet();
            GlossaryControlDac dac = new GlossaryControlDac();
            ds = dac.UserNameList(username);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    result.Add(dr["Name"].ToString() + "/" + dr["DeptName"].ToString() + "@" + dr["EmpID"].ToString());
                    //result.Add(dr["Name"].ToString() + "/" + dr["DeptName"].ToString());
                    //result.Add(dr["EmpID"].ToString());
                }
            }
            return result;
        }

        //에디터 orgchart
        [WebMethod]
        public static List<string> GetAutoCompleteUserDatas(string username)
        {
            List<string> result = new List<string>();
            DataSet ds = new DataSet();
            GlossaryControlDac dac = new GlossaryControlDac();
            ds = dac.UserNameList(username);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AjaxUserInfo temp = new AjaxUserInfo();
                    temp.Name = dr["Name"].ToString();
                    temp.DeptName = dr["DeptName"].ToString();
                    temp.EmpID = dr["EmpID"].ToString();

                    string json = new JavaScriptSerializer().Serialize(temp);
                    result.Add(json);
                }
            }
            return result;
        }

        //WritePage Title 자동완성기능.
        [WebMethod]
        public static List<string> GetAutoCompleteWriteTitleData(string username)
        {
            List<string> result = new List<string>();
            DataSet ds = new DataSet();
            GlossaryDac dac = new GlossaryDac();
            ds = dac.SearchAutoList(username, "MainList");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //result.Add(dr["Title"].ToString());
                    if (dr["CommonID"].ToString() != "0")
                    {
                        //result.Add(dr["Title"].ToString());
                        result.Add(System.Web.HttpUtility.HtmlDecode(dr["Title"].ToString()));
                    }
                }
            }
            return result;
        }

        //Tag 자동완성 기능
        [WebMethod]
        public static List<string> GetAutoCompleteTagData(string username)
        {
            List<string> result = new List<string>();
            DataSet ds = new DataSet();
            GlossaryDac dac = new GlossaryDac();
            ds = dac.SearchAutoList(username, "Tag");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    result.Add(System.Web.HttpUtility.HtmlDecode(dr["TagTitle"].ToString()));
                }
            }
            return result;
        }

        //수정 check in, check out
        [WebMethod]
        public static string GlossaryModifyYN(string ItemID)
        {
            DataSet ds = new DataSet();
            GlossaryControlDac dac = new GlossaryControlDac();
            ds = dac.GlossaryModifyYN(ItemID);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();
                }
            }
            return ds.Tables.Count.ToString();
        }

        //히스토리 check in, check out
        [WebMethod]
        public static string HistoryModifyYN(string ItemID)
        {
            DataSet ds = new DataSet();
            GlossaryHistoryDac dac = new GlossaryHistoryDac();
            ds = dac.GlossaryHistoryModifyYN(ItemID);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryType Board = new GlossaryType();
                }
            }
            return ds.Tables.Count.ToString();
        }

        //check in, out 그냥 빠져나왔을때 업데이트
        [WebMethod]
        public static string GlossaryModifyYNUpdate(string GlossaryModifyYN)
        {
            GlossaryDac Dac = new GlossaryDac();
            Dac.GlossaryModifyYNUpdate(GlossaryModifyYN, "ModifyY");
            return "";
        }

        //스크랩 추가
        [WebMethod]
        public static string GlossaryScrapInsert(string Title, string UserID, string YouUserID, string GlossaryID, string ScrapsYN)
        {
            
            GlossaryScrapType Board = new GlossaryScrapType();
            GlossaryScrapBiz Biz = new GlossaryScrapBiz();
            Board.GlossaryID = GlossaryID;
            Board.Title = Title;
            Board.UserID = UserID;
            Board.YouUserID = YouUserID;
            Board.ScrapsYN = ScrapsYN;
            Biz.GlossaryScrapInsert(Board);
            return "";
        }

        //좋아요 추가
        [WebMethod]
		public static GlossaryLikeType GlossaryLikeInsert(string UserID, string GlossaryID)
        {
            GlossaryControlType Board = new GlossaryControlType();
            GlossaryControlBiz Biz = new GlossaryControlBiz();
            Board.GlossaryID = GlossaryID;
            Board.UserID = UserID;
            Board.LikeY = "Y";

            //Biz.GlossaryLikeInsert(Board);

			return Biz.GlossaryLikeInsert(Board);
        }

        //좋아요 조회
        [WebMethod]
        public static GlossaryLikeType GlossaryLikeSelect(string GlossaryID)
        {
            List<string> result = new List<string>();

            GlossaryControlBiz Biz = new GlossaryControlBiz();

            return Biz.GlossaryLikeSelect(GlossaryID, "Info"); 
        }
       
        [WebMethod]
        public static string GlossaryLikeUserList(string GlossaryID, string Mode)
        {
            string result = "<table class='likeBodyTable'>";               
            
            ArrayList list = new ArrayList();                        
            GlossaryControlBiz Biz = new GlossaryControlBiz();
            GlossaryLikeType Board = new GlossaryLikeType();
            list= Biz.GlossaryLikeList(GlossaryID, Mode);
            
            for(int i=0; i< list.Count; i++)
            {
                Board = (GlossaryLikeType)list[i];
                string reDeptName = string.Empty;
                string reUserName = string.Empty;
                if(Board.DeptName.Length>10)
                {
                    reDeptName = Board.DeptName.Substring(0, 10) + "..";
                }
                else
                {
                    reDeptName = Board.DeptName;
                }

                if (Board.UserName.Length > 4)
                {
                    reUserName = Board.UserName.Substring(0, 4) + "..";
                }
                else
                {
                    reUserName = Board.UserName;
                }

                result += "<tr><td class='likeTdnum'>"
                            +(list.Count-i)
                            + "</td><td class='likeTdName'><a href='/GlossaryMyPages/MyProfile.aspx?UserID="
                            + Board.UserID
                            + "' target='_self'>"
                            + reUserName
                            + "</a></td><td class='likeTdDept'>"
                            + reDeptName
                            + "</td><td class='likeTdDate'>"
                            + Board.CreateDate.ToString()
                            +"</td></tr>";                  
            }
            result += "</table>";
            return result;
        }

        //알람 기능
        [WebMethod]
        public static string GlossaryAlarm(string CommonID, string UserID, string MailSet, string NoteSet)
        {

            GlossaryControlType Board = new GlossaryControlType();
            GlossaryControlDac Dac = new GlossaryControlDac();
            Board.UserID = UserID;
            Board.MailYN = MailSet;
            Board.NoteYN = NoteSet;
            Board.CommonID = CommonID;
            Dac.GlossaryAlarmInsert(Board);
            DataSet ds = new DataSet();
            return NoteSet;
        }


        //공유 저장
        [WebMethod]
        public static string GlossaryShareInsert(string ItemID, string UserID, string ToUserID, string Title, string UserName)
        {
            string[] ToUser = ToUserID.Split('/');
            GlossaryShareType Board = new GlossaryShareType();
            GlossaryShareDac Dac = new GlossaryShareDac();
            GlossaryBiz biz = new GlossaryBiz();
            GlossaryType Board_ = biz.GlossarySelect(ItemID, "", "");
            Board.FromUserID = UserID;
            Board.GlossaryID = Board_.CommonID;  //common 아이디를 넘겨줘야하므로 넘겨주자.
            Board.Title = Title;

            // 2014-05-15 Mr.No
            // 읽기 권한 추가
            GlossaryPermissionsBiz permissionsBiz = new GlossaryPermissionsBiz();
            permissionsBiz.PermissionsInsert(ItemID, ToUserID, UserName);

            Board.UserName = Board_.UserName;
            Board.DeptName = Board_.DeptName;
            Board.BoardUserID = Board_.UserID;
            for (int i = 0; i < ToUser.Length; i++)
            {
                // 2014-06-12 Mr.No
                GlossaryProfileBiz biz_ = new GlossaryProfileBiz();
                ImpersonUserinfo u = biz_.UserSelect(ToUser[i]);

                //if (ToUser[i] != "undefined" && ToUser[i] != "")
                if (ToUser[i] != "" && !String.IsNullOrEmpty(u.UserID))
                {

                    Board.ToUserID = ToUser[i];

                    //CBHMSMQHelper helper = new CBHMSMQHelper();
                    CBHNoteType data = new CBHNoteType();
                    //DataSet ds = new DataSet();
                    //GlossaryControlDac Dac_ = new GlossaryControlDac();
                    //ds = Dac_.GlossaryLikeNote(ToUser[i], Board_.CommonID);

                    string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
                    string NoteLink = BaseURL + "GlossaryMyPages/MyShareList.aspx";

                    //GlossaryProfileBiz biz_ = new GlossaryProfileBiz();
                    //ImpersonUserinfo u = biz_.UserSelect(ToUser[i]);

                    string ContentsStr = "<html><body><font face=\"맑은고딕\" size=\"2\">"
                        //+ "[공유자] 님께서 [공유받은자] 님께 [티끌제목]을 공유 하셨습니다.";
                    + biz_.UserSelect(UserID).Name + "님께서 " + u.Name + " 님께 <STRONG>&lt;" + Title + "&gt;</STRONG>을 공유 하셨습니다."
                        //+ " 티끌이 많은 도움이 되었습니다.<br/> 감사합니다. ^^<br /><br />"
                    + "▶ 티끌 바로가기: ＇<a href=\"" + NoteLink + " \">" + Title + "</a><br /></font></body></html>";

                    data.Content = ContentsStr;
                    data.Kind = "3"; //일반쪽지.
                    data.SendUserName = "티끌이";
                    string userID = u.EmailAddress.Remove(u.EmailAddress.ToString().IndexOf('@')); //이메일 앞부분이 note id 값이다.
                    data.SendUserID = "tikle"; //보내는사람과 받는사람을 같게한다..쪽지에 한해서... 티끌이가 보내자.
                    data.TargetUser = userID;
                    //OK//helper.SendNoteToQueue(data);

                    //쪽지 20170802
                    CBHInterface.CBHNoteSend(data);

                    //메일 20170802
                    CBHInterface.CBHMailSend(u.EmailAddress, "tikle@sk.com", "T.끌 알림 메일입니다.", ContentsStr);

                    Dac.GlossaryShareInsert(Board);
                }

                // 부서인 경우
                // 2014-06-12 Mr.No
                else
                {
                    //string connectionStringName = "ConnOrgChart";
                    string connectionStringName = "ConnGlossary";
                    Database db = DatabaseFactory.CreateDatabase(connectionStringName);
                    DbCommand dbCommand = db.GetStoredProcCommand("up_list_department_person");

                    db.AddInParameter(dbCommand, "departmentNumber", DbType.String, ToUser[i].ToString());

                    using (DataSet ds = db.ExecuteDataSet(dbCommand))
                    {
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                string employeeID = (dr["employeeID"] == DBNull.Value) ? null : dr.Field<string>("employeeID");
                                string displayName = (dr["displayName"] == DBNull.Value) ? null : dr.Field<string>("displayName");
                                if (!String.IsNullOrEmpty(employeeID)) { GlossaryShareInsert(ItemID, UserID, employeeID, Title, displayName); }
                            }
                        }
                    }
                }

            }
            return "";
        }



        //메인 페이지에서 보이기/안보이기 환경설정
        [WebMethod]
        public static string GlossaryFloatingSetInsert(string UserID, string FloatingSet)
        {
            FloatingMenuUserSettingDac Dac = new FloatingMenuUserSettingDac();
            FloatingMenuUserSettingType Board = new FloatingMenuUserSettingType();
            Dac.GlossaryFloatingSetInsert(UserID, FloatingSet);
            return "";
        }

        //메인 페이지에서 쪽지 보내기/안 보내기 환경설정
        [WebMethod]
        public static string GlossaryTikleNoteSetInsert(string UserID, string TikleNoteSet)
        {
            FloatingMenuUserSettingDac Dac = new FloatingMenuUserSettingDac();
            FloatingMenuUserSettingType Board = new FloatingMenuUserSettingType();
            Dac.GlossaryTikleNoteSetInsert(UserID, TikleNoteSet);
            return "";
        }

        //팔로우 삭제
        [WebMethod]
        public static string GlossaryMyFollowDelete(string ItemID)
        {
            GlossaryFollowDac Dac = new GlossaryFollowDac();
            Dac.GlossaryFollowDelete(ItemID);
            return "";
        }

        //url 을 넘겼을떄 내부 문서 일떄 제목을 리턴하기
        [WebMethod]
        public static string GetTitleFormUrl(string url)
        {
            string ret = string.Empty;

            if (url.IndexOf("MyProfile.aspx") >= 0)
            {
                int useridindex = url.IndexOf("UserID=");
                if (useridindex > 0)
                {
                    useridindex = useridindex + 7;

                    string userid = string.Empty;
                    int nextindex = url.IndexOf("&", useridindex);
                    if (nextindex > 0)
                    {
                        userid = url.Substring(useridindex, nextindex - useridindex);
                    }
                    else
                    {
                        userid = url.Substring(useridindex, url.Length - useridindex);
                    }

                    GlossaryProfileBiz biz = new GlossaryProfileBiz();
                    ImpersonUserinfo data = biz.UserSelect(userid);
                    ret = data.Name;
                }
            }
            else if (url.IndexOf("GlossaryView.aspx") >= 0)
            {
                int useridindex = url.IndexOf("ItemID=");
                if (useridindex > 0)
                {
                    useridindex = useridindex + 7;

                    string Itemid = string.Empty;
                    int nextindex = url.IndexOf("&", useridindex);
                    if (nextindex > 0)
                    {
                        Itemid = url.Substring(useridindex, nextindex - useridindex);
                    }
                    else
                    {
                        Itemid = url.Substring(useridindex, url.Length - useridindex);
                    }

                    GlossaryBiz biz = new GlossaryBiz();
                    GlossaryType data = biz.GlossarySelect(Itemid, "", "");
                    ret = data.Title;
                }
            }
            return ret;
        }

        //좋아요 추가
        [WebMethod]
        public static string GlossaryLikeNote(string UserID, string GlossaryID, string UserName)
        {
            //CBHMSMQHelper helper = new CBHMSMQHelper();
            CBHNoteType data = new CBHNoteType();
            DataSet ds = new DataSet();
            GlossaryControlDac Dac = new GlossaryControlDac();
            ds = Dac.GlossaryLikeNote(UserID, GlossaryID);

            string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
            string NoteLink = BaseURL + "Glossary/GlossaryView.aspx?mode=Histroy&ItemID=" + GlossaryID;
            string FristUserID = null;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["UserID"].ToString() != FristUserID || dr["UserName"].ToString() != "비공개")
                    {
                        if (dr["TikleNoteYN"].ToString() == "Y" || string.IsNullOrEmpty(dr["TikleNoteYN"].ToString()))
                        {
                            FristUserID = dr["UserID"].ToString();
                            /* 20140219 변경
                            string ContentsStr = "<html><body><font face=\"맑은고딕\" size=\"2\">"
                                                + dr["UserName"].ToString() + "님께서 올려주신<br/>"
                                                + "<STRONG>&lt;" + dr["Title"].ToString() + "&gt;</STRONG>"
                                                + " 티끌이 많은 도움이 되었습니다.<br/> 감사합니다. ^^<br /><br />"
                                                + "▶ 티끌 바로가기: ＇<a href=\"" + NoteLink + " \">" + dr["Title"].ToString() + "</a><br /></font></body></html>";
                            */
                            string ContentsStr = "<html><body><font face=\"맑은고딕\" size=\"2\">"
                                                + "<STRONG>&lt;" + ds.Tables[0].Rows[0]["Title"].ToString() + "&gt;</STRONG>"
                                                + " 티끌에 대해 "
                                                + UserName
                                                + "이 감사의 마음을 전하셨습니다.<br /><br />"
                                                + "▶ 티끌 바로가기: ＇<a href=\"" + NoteLink + " \">" + dr["Title"].ToString() + "</a><br /></font></body></html>";



                            data.Content = ContentsStr;
                            data.Kind = "3"; //일반쪽지.
                            data.SendUserName = "티끌이";
                            string userID = dr["UserEmail"].ToString().Remove(dr["UserEmail"].ToString().IndexOf('@')); //이메일 앞부분이 note id 값이다.
                            data.SendUserID = "tikle"; //보내는사람과 받는사람을 같게한다..쪽지에 한해서... 티끌이가 보내자.                            
                            data.TargetUser = userID;
                            //OK//helper.SendNoteToQueue(data);

                            //쪽지 20170802
                            CBHInterface.CBHNoteSend(data);

                            //메일 20170802
                            CBHInterface.CBHMailSend(dr["UserEmail"].ToString().Trim(), "tikle@sk.com", "T.끌 알림 메일입니다.", ContentsStr);
                        }
                    }
                }
            }
            
            return "";
        }

         //tutorial 체크
        [WebMethod]
        public static string TutorialStop(string UserID, string TutoType)
        {
            GlossaryControlBiz biz = new GlossaryControlBiz();
            TutorialInfo data = biz.TutorialSelect(UserID);
            
            data.UserID = UserID;
            if (TutoType == "Result")
            {
                data.ResultYN = "N";
            }
            else if (TutoType == "Profile")
            {
                data.ProfileYN = "N";
            }
            else if (TutoType == "QNA")
            {
                data.QNAYN = "N";
            }
            else if (TutoType == "FirstWrite")
            {
                data.FirstWrite = "N";
            }
                        
            biz.TutorialInsert(data);

            return "SUCCESS";
        }

        //타이틀 검색.체크
        [WebMethod]
        public static List<string> ExistTitle(string Title,string ID)
        {
            List<string> result = new List<string>();
            GlossaryControlBiz biz = new GlossaryControlBiz();
            string pp = biz.ExistTitle(Title, ID);
           
            result.Add(pp);
            return result;
        }

        //타이틀 검색.체크 - 모임
        [WebMethod]
        public static List<string> ExistTitleGathering(string Title, string ID, string GatheringYN, string GatheringID)
        {
            List<string> result = new List<string>();
            GlossaryControlBiz biz = new GlossaryControlBiz();
            string pp = biz.ExistTitle(Title, ID, GatheringYN, GatheringID);

            result.Add(pp);
            return result;
        }

		//타이틀 검색.체크(MyGroup, 문서함)
		[WebMethod]
		public static List<string> ExistTitleEtc(string Title, string itemID, string UserID, string mode)
		{
			List<string> result = new List<string>();
			GlossaryControlBiz biz = new GlossaryControlBiz();
			string pp = biz.ExistTitleEtc(Title.Trim(), itemID, UserID, mode);

			result.Add(pp);
			return result;
		}

        [WebMethod]
        public static List<string> EHRWorkStatus(string UserID)
        {
            List<string> result = new List<string>();

            string pp = EHRHelper.EHRWorkStatus(UserID);
            if (pp.Length == 0)
            {
                pp = "근무중";
            }
            result.Add(pp);

            return result;
        }

        // 2014-04-28 Mr.No Category Infomation Selectd
        [WebMethod]
        public static List<string> CategoryContents(string ID)
        {
            GlossaryCategoryType glossaryCategoryType = GlossaryCategoryDac.Instance.GlossaryCategorySelect(Convert.ToInt32(ID));
            List<string> result = new List<string>();
            result.Add(SecurityHelper.ReClear_XSS_CSRF(SecurityHelper.Add_XSS_CSRF(glossaryCategoryType.CategoryContents.Replace("\r\n", string.Empty))));
            return result;
        }

        // 2014-06-03
        // View Page 에서 연관단어 실시간 삭제 
        [WebMethod]
        public static string GlossaryTagDelete_One(string ID)
        {
            GlossaryControlDac glossaryControlDac = new GlossaryControlDac();
            glossaryControlDac.GlossaryTagDelete_One(ID);
            return "";
        }
        // 연관단어 실시간 추가
        [WebMethod]
        public static List<string> GlossaryTagInsert_One(string TagTitle, string CommonID, string UserID, string Title)
        {
            GlossaryControlDac glossaryControlDac = new GlossaryControlDac();
            GlossaryControlType Board = new GlossaryControlType();

            Board.TagTitle = SecurityHelper.Add_XSS_CSRF(TagTitle);
            Board.CommonID = CommonID;
            Board.UserID = UserID;
            Board.Title = Title;

            List<string> result = new List<string>();
            GlossaryControlType glossaryControlType = glossaryControlDac.GlossaryTagInsert_One(Board);
            result.Add(glossaryControlType.ID);
            result.Add(SecurityHelper.Clear_XSS_CSRF(SecurityHelper.Add_XSS_CSRF(glossaryControlType.TagTitle)));
            return result;
        }


        // QnA 토스 저장
        [WebMethod]
        public static string QnAShareInsert(string ItemID, string UserID, string ToUserID, string Title)
        {
            string[] ToUser = ToUserID.Split('/');
            GlossaryQnAShareType Board = new GlossaryQnAShareType();
            GlossaryQnADac Dac = new GlossaryQnADac();
            GlossaryQnABiz biz = new GlossaryQnABiz();
            GlossaryQnAType Board_ = biz.GlossaryQnASelect(ItemID);
            Board.FromUserID = UserID;
            Board.QnAID = Convert.ToInt32(Board_.ID);  
            Board.Title = Title;
            Board.UserName = Board_.UserName;
            Board.DeptName = Board_.DeptName;
            Board.BoardUserID = Board_.UserID;

            for (int i = 0; i < ToUser.Length; i++)
            {
                if (ToUser[i] != "undefined" && ToUser[i] != "")
                {

                    Board.ToUserID = ToUser[i];

                    //CBHMSMQHelper helper = new CBHMSMQHelper();
                    CBHNoteType data = new CBHNoteType();

                    string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
                    string NoteLink = BaseURL + "QnA/QnAView.aspx?ItemID=" + ItemID;

                    GlossaryProfileBiz biz_ = new GlossaryProfileBiz();
                    ImpersonUserinfo u = biz_.UserSelect(ToUser[i]);

                    // old
                    //string ContentsStr = "<html><body><font face=\"맑은고딕\" size=\"2\">"
                    //+ biz_.UserSelect(UserID).Name + "님께서 " + u.Name + " 님께 <STRONG>&lt;" + Title + "&gt;</STRONG>을 토스 하셨습니다."
                    //+"▶ 질문 바로가기: ＇<a href=\"" + NoteLink + " \">" + Title + "</a><br /></font></body></html>";

                    // new
                    string ContentsStr = "<html><body><font face=\"맑은고딕\" size=\"2\">";
                    ContentsStr += "<STRONG>&lt;" + Title + "&gt;</STRONG> 질문이 배달되었습니다.";
                    ContentsStr += u.Name + "님의 친절한 답변이 함께하는 행복으로 이어질 것입니다.";
                    ContentsStr += "▶ 티끌 바로가기: ＇<a href=\"" + NoteLink + " \">" + Title + "</a><br /></font></body></html>";

                    data.Content = ContentsStr;
                    data.Kind = "3"; //일반쪽지.
                    data.SendUserName = "티끌이";
                    string userID = u.EmailAddress.Remove(u.EmailAddress.ToString().IndexOf('@')); //이메일 앞부분이 note id 값이다.
                    data.SendUserID = "tikle"; //보내는사람과 받는사람을 같게한다..쪽지에 한해서... 티끌이가 보내자.
                    data.TargetUser = userID;
                    //OK//helper.SendNoteToQueue(data);

                    //쪽지 20170802
                    CBHInterface.CBHNoteSend(data);

                    //메일 20170802
                    CBHInterface.CBHMailSend(u.EmailAddress, "tikle@sk.com", "T.끌 알림 메일입니다.", ContentsStr);

                    Dac.QnAShareInsert(Board);
                }
            }
            return "";
        }



        // 연관단어 중복 확인
        [WebMethod]
        public static List<string> Tag_Redundancy_Check(string TagTitle, string CommonID)
        {
            long ID = 0;
            GlossaryControlDac glossaryControlDac = new GlossaryControlDac();
            
            TagTitle = SecurityHelper.Add_XSS_CSRF(TagTitle);
            List<string> result = new List<string>();

            if (CommonID != "")
            {
                GlossaryControlType glossaryControlType = glossaryControlDac.GlossaryTagRedundancy_Check(TagTitle, CommonID);
                result.Add(glossaryControlType.ID);
                result.Add(SecurityHelper.Clear_XSS_CSRF(TagTitle));
            }
            else
            {
                result.Add(SecurityHelper.Clear_XSS_CSRF(TagTitle));
            }
            return result;
        }

        [WebMethod]
        public static List<string> CategoryTitle_Check(string CategoryTitle)
        {
            string returnTitle = GlossaryCategoryDac.Instance.CategoryTitle_Check(CategoryTitle);

            List<string> result = new List<string>();
            result.Add(SecurityHelper.Clear_XSS_CSRF(returnTitle));
            return result;
        }

        //쪽지보내기
        [WebMethod(EnableSession = true)]
        public static string CommNateOnBizSend(string contentText, string SendIds, string SendNMs, string SendTYs, string SendLinkNm, string SendLinkLink, string SendLinkType, string SendDirId, string SendFileName)
        {

            string SendMail = HttpContext.Current.Session["EmailAddress"].ToString();

            NateOnBizSendType nateonbiz = new NateOnBizSendType();
            nateonbiz.NateOnBizSenderID = SendMail.Remove(SendMail.IndexOf('@'));
            nateonbiz.NateOnBizSenderMail = SendMail;

            contentText = SecurityHelper.Clear_XSS_CSRF(contentText);

            contentText = Utility.BREncode(contentText);

            nateonbiz.NateOnBizCONTENTS = contentText;
            nateonbiz.NateOnBizLInkNm = SendLinkNm;
            nateonbiz.NateOnBizLInkUrl = SendLinkLink;
            nateonbiz.NateOnBizLInkType = SendLinkType;
            nateonbiz.NateOnBizTargetIDS = SendIds.Split('/');
            nateonbiz.NateOnBizTargetNMS = SendNMs.Split('&');
            nateonbiz.NateOnBizTargetTYS = SendTYs.Split('/');
            nateonbiz.NateOnBizSenderNM = HttpContext.Current.Session["Name"].ToString() + "/" + HttpContext.Current.Session["DeptName"].ToString();
            nateonbiz.NateOnBizUserID = HttpContext.Current.Session["UserID"].ToString();
            
            nateonbiz.folderName = SendDirId;
            nateonbiz.fileName = SendFileName;

            GlossaryControlBiz biz = new GlossaryControlBiz();
            return biz.commNateOnBizSendCall(nateonbiz); 
        }


        // 모임 권한 변경
        [WebMethod]
        public static string SetGatheringInfomation(string uID, string gID, string gName, string authID, string authCL)
        {
            string AuthType = string.Empty;
            string errMsg = string.Empty;
            string mode = "Update";

            string UserID = uID;


            GlossaryGatheringType Board = new GlossaryGatheringType();
            GlossaryGatheringBiz biz = new GlossaryGatheringBiz();

            Board.GatheringID = gID;
            Board.GatheringName = SecurityHelper.Clear_XSS_CSRF(gName).Trim(); //제목            
            Board.Author = UserID;
            Board.Editor = UserID;
            Board.UseYN = "Y";

            //******************//
            // 1. 끌.모임 정보 저장
            //******************//
            Board = biz.GlossaryGathering_Insert(Board, mode);
            string GatheringID = Board.GatheringID;


            //******************//
            // 2. 권한정보 저장
            //******************//
            string AuthID = authID;
            string AuthCL = authCL;

            //삭제후 인서트   
            try
            {
                // 1. 기존 권한 정보 제거   
                biz.GlossaryGatheringAuth_Delete(Board.GatheringID);

                // 2. 새 권한 정보 등록(UDList.AuthCL은 실제로 AuthType 값을 갖음. U / O / G)
                biz.GlossaryGatheringAuth_Insert(GatheringID, UserID, AuthID, AuthCL, "RW", mode);

                // 3. 문서함 & 쉐어포인트 권한 처리
                GlossaryGatheringBiz ggBiz = new GlossaryGatheringBiz();
                DataSet dirList = ggBiz.GlossaryGatheringMenu_List(Board.GatheringID, "Dir");

                if (dirList.Tables.Count > 0 && dirList.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dirList.Tables[0].Rows)
                    {
                        DirectoryCommon dirCommon = new DirectoryCommon();
                        dirCommon.SaveUserList(dr["CommonID"].ToString(), UserID, AuthID, AuthCL, mode);
                    }
                }
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
            }

            //새로고침location.href = "/Glossary/GlossaryNewsList.aspx?TagTitle=&SearchSort=CreateDate&GatheringYN=Y&GatheringID=" + GatheringID;
            string strURL = "/Glossary/GlossaryNewsList.aspx";
            return strURL + "?TagTitle=&SearchSort=CreateDate&GatheringYN=Y&GatheringID=" + GatheringID;

        }



        #region 2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 -----------------------------------
        /// <summary>
        /// 공통코드 조회
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static Dictionary<string, object> GetCommonCode(string pcode)
        {
            WeeklyBiz biz = new WeeklyBiz();

            DataSet ds = biz.CommonCode_Select(pcode);

            return Utility.ToJson(ds);
        }

        #endregion //2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 ------------------------------


        //	- 전년도 위클리 데이터 조회
        //[WebMethod]
        //public static Dictionary<string, object> GetWeeklyOldYear()
        //{
        //    WeeklyBiz biz = new WeeklyBiz();

        //    DataSet ds = biz.Weekly_OldYearWeekly_Year_Select();

        //    return Utility.ToJson(ds);
        //}

        //Author : 개발자-김성환D, 리뷰자-진현빈D
        //Create Date : 2016.08.11
        //Desc : 조회수, 추천수 보이도록 처리
        [System.Web.Services.WebMethod]
        public static int GetGlossaryPermissions(string commonID, string userID)
        {
            UserInfo u = new UserInfo();
            GlossaryPermissionsBiz permissionsBiz = new GlossaryPermissionsBiz();
            int rtnValue = permissionsBiz.Permissions_Check(commonID, userID);
            return rtnValue;
        }
    }
}