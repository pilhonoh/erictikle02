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

using System.ServiceModel.Channels;
using System.ServiceModel;

namespace SKT.Glossary.Biz
{
    public class GlossaryControlBiz
    {
        
        //좋아요 추가
		public GlossaryLikeType GlossaryLikeInsert(GlossaryControlType Board)
        {
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = Dac.GlossaryLikeInsert(Board);

			return GlossaryLikeSelect(Board.GlossaryID, "Info");
        }

        //좋아요 카운트
        public GlossaryLikeType GlossaryLikeSelect(string GlossaryID, string Mode)
        {
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = Dac.GlossaryLikeSelect(GlossaryID, Mode);
            GlossaryLikeType likeType = new GlossaryLikeType();
            
            if(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (Mode == "Info")
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        likeType.ID = dr["ID"].ToString();
                        likeType.GlossaryID = dr["GlossaryID"].ToString();
                        likeType.LatestUserID = dr["LatestUserID"].ToString();
                        likeType.LatestUserName = dr["LatestUserName"].ToString();
                        if (Convert.ToInt32(dr["TotalCount"]) < 1)
                        {
                            likeType.TotalCount = dr["TotalCount"].ToString();
                        }
                        else
                        {
                            likeType.TotalCount = (Convert.ToInt32(dr["TotalCount"]) - 1).ToString();
                        }                     
                    }
                }
            }
            return likeType;
        }     

        public ArrayList GlossaryLikeList(string GlossaryID, string Mode)
        {
            ArrayList list = new ArrayList();
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = Dac.GlossaryLikeSelect(GlossaryID, Mode);
            
            if (Mode == "Total")
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryLikeType likeType = new GlossaryLikeType();
                    likeType.ID = dr["ID"].ToString();
                    likeType.GlossaryID = dr["GlossaryID"].ToString();
                    likeType.UserID = dr["UserID"].ToString();
                    likeType.UserName = dr["UserName"].ToString();
                    likeType.DeptName = dr["DeptName"].ToString();
                    likeType.LikeY = dr["LikeY"].ToString();
                    likeType.CreateDate = dr["CreateDate"].ToString();
                    list.Add(likeType);
                }
            }
            return list;
        }

        //뷰페이지 메뉴들 Select
        public GlossaryControlType GlossaryViewMenuSelect(string UserID, string CommonID)
        {
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = Dac.GlossaryViewMenuSelect(UserID, CommonID);
            GlossaryControlType Board = new GlossaryControlType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ScrapsYN = dr["ScrapsYN"].ToString();
                    Board.ReadYN = dr["ReadYN"].ToString();
                    Board.LikeCount = dr["LikeCount"].ToString();
                    Board.MailYN = dr["MailYN"].ToString();
                    Board.NoteYN = dr["NoteYN"].ToString();
                    Board.LikeY = dr["LikeYN"].ToString();
                    Board.Historycount = dr["Historycount"].ToString();
                    Board.ScrapCount = dr["ScrapCount"].ToString();
                }
            }
            return Board;
        }

        //사용자 이름으로 사용자 정보 리스트를 가져온다.
        public ArrayList UserNameList(string UserName, out int TotalCount)
        {
            ArrayList list = new ArrayList();
            TotalCount = 0;
            GlossaryControlDac dac = new GlossaryControlDac();
            DataSet ds = new DataSet();
            ds = dac.UserNameList(UserName);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ImpersonUserinfo Info = new ImpersonUserinfo();
                    Info.UserID = dr["EmpID"].ToString();
                    Info.Name = dr["Name"].ToString();
                    Info.DeptName = dr["DeptName"].ToString();
                    Info.EmailAddress = dr["EmailAddress"].ToString();
                    Info.WorkArea = dr["WorkArea"].ToString();
                    
                    list.Add(Info);
                }
            }
            TotalCount = list.Count;
            return list;
        }

        //태그 리스트
        public ArrayList GlossaryTagList(string CommonID, string Title, string UserID)
        {
            ArrayList list = new ArrayList();
            GlossaryControlDac dac = new GlossaryControlDac();
            DataSet ds = new DataSet();
            ds = dac.GlossaryTagList(CommonID, UserID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["Title"].ToString() != Title)
                    {
                        GlossaryControlType Board = new GlossaryControlType();
                        Board.ID = dr["ID"].ToString();
                        Board.Title = dr["Title"].ToString();
                        list.Add(Board);
                    }
                }
            }
            return list;
        }

        public TutorialInfo TutorialSelect(string UserID)
        {
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = Dac.TutorialSelect(UserID);
            TutorialInfo Board = new TutorialInfo();

            Board.ProfileYN = "Y";
            Board.QNAYN = "Y";
            Board.ResultYN = "Y";
            Board.FirstWrite = "Y";

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.UserID = dr["UserID"].ToString();
                    Board.ProfileYN = dr["ProfileYN"].ToString();
                    Board.QNAYN = dr["QNAYN"].ToString();
                    Board.ResultYN = dr["ResultYN"].ToString();
                    Board.FirstWrite = dr["FirstWrite"].ToString();
                }
            }
            return Board;
        }

        public void TutorialInsert(TutorialInfo data)
        {
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = Dac.TutorialInsert(data);
        }

        //
        public string ExistTitle(string Title, string ID)
        {
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = Dac.ExistTitle(Title, ID);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    return dr["ID"].ToString();
                }
            }
            return "";
        }

        public string ExistTitle(string Title, string ID, string GatheringYN, string GatheringID)
        {
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = Dac.ExistTitle(Title, ID, GatheringYN, GatheringID);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    return dr["ID"].ToString();
                }
            }
            return "";
        }

		//
        public string ExistTitleEtc(string Title, string itemID, string UserID, string mode)
        {
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = Dac.ExistTitleEtc(Title, itemID, UserID, mode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    return dr["ID"].ToString();
                }
            }
            return "";
        }

        /*
        * 대상자 카운트 가져오기
        */
        public string GlossaryAuthUserCnt(string idx, string type, string uid)
        {

            DataSet ds = new DataSet();
            GlossaryMyGroupDac Dac = new GlossaryMyGroupDac();
            ds = Dac.CommAuthCount(idx, type);
            Int32 ucnt = 0;
            string userCnt = string.Empty;
            string ToUserNameU = string.Empty;

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userCnt = dr["USERCNT"].ToString();
                    ToUserNameU = dr["USERNAME"].ToString();
 
                }
            }

            ucnt = int.Parse(userCnt);

            ucnt--;
            if (ucnt > 1)
            {
                ToUserNameU = ToUserNameU + "님 외 " + ucnt-- + "명";
            }

            return ToUserNameU;
        }


        /*
       * 대상자 카운트 가져오기
       */
        public DataSet commAuthUserCntUpdate(string idx, string type)
        {
            DataSet ds = new DataSet();
            GlossaryControlDac Dac = new GlossaryControlDac();
            return Dac.CommAuthCountUpdate(idx, type);
        }


        /*
         * 댓글 목록 가져오기
         */
        public DataSet commCommentListSelect(string commType, string commIdx, string userID, int pageNum, int pageSize)
        {

            DataSet ds = new DataSet();
            GlossaryControlDac Dac = new GlossaryControlDac();
            ds = Dac.CommCommentListSelect(commType, commIdx, userID, pageNum, pageSize);
            return ds;
        
        }

        /*
         * 댓글 상세데이터 가져오기
         */
        public DataSet commCommentSelect(string commType, string commIdx, string userID, string ID)
        {

            DataSet ds = new DataSet();
            GlossaryControlDac Dac = new GlossaryControlDac();
            ds = Dac.CommCommentSelect(commType, commIdx, userID, ID);
            return ds;

        }

        /*
         * 댓글 등록하기
         */
        public CommCommentType commCommentInsert(CommCommentType Board)
        {
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = Dac.CommCommentInsert(Board);
            Board.ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            return Board;
        }

        /*
         * 댓글 댓글 답변
         */
        public DataSet commCommentSupInsert(CommCommentType Board)
        {
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = Dac.CommCommentSupInsert(Board);
            return ds;
        }



        /*
         * 댓글 수정하기
         */
        public DataSet commCommentUpdate(CommCommentType Board)
        {
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = Dac.CommCommentUpdate(Board);
            return ds;
        }

        /*
          * 댓글 삭제하기
          */
        public DataSet commCommentDelete(CommCommentType Board)
        {
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = Dac.CommCommentDelete(Board);
            return ds;
        }

        /*
         * 댓글 상세카운트 가져오기
         */
        public DataSet commCommentCountSelect(string commType, string commIdx)
        {

            DataSet ds = new DataSet();
            GlossaryControlDac Dac = new GlossaryControlDac();
            ds = Dac.CommCommentCountSelect(commType, commIdx);
            return ds;

        }


        

        //추천  추가
        public DataSet commCommentCountLike(CommCommentType Comment)
        {
            DataSet ds = new DataSet();
            GlossaryControlDac Dac = new GlossaryControlDac();
            ds = Dac.CommCommentCountLike(Comment);
            return ds;
        }


       

        // 베스트 댓글  추가
        public DataSet commCommentCountBest(CommCommentType Comment)
        {
            GlossaryControlDac Dac = new GlossaryControlDac();
            DataSet ds = Dac.CommCommentCountBest(Comment);
            return ds;
        }
        /*
       // 2014-07-09 Mr.No
       public string CommentBest_Check(string CommonID)
       {
           GlossarySurveyCommentDac Dac = new GlossarySurveyCommentDac();
           string BestReplyYN = Dac.CommentBest_Check(CommonID);
           return BestReplyYN;
       }
       */

        /*
        * 댓글 목록 가져오기
        */
        public DataSet commCommentListExcel(string commType, string commIdx)
        {

            DataSet ds = new DataSet();
            GlossaryControlDac Dac = new GlossaryControlDac();
            ds = Dac.CommCommentListExcel(commType, commIdx);



            if (ds != null && ds.Tables.Count > 0)
            {
                string changeTag = string.Empty;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    changeTag = dr["CONTENTS"].ToString();
                    changeTag = Utility.BREncode(changeTag);
                    changeTag = SecurityHelper.Add_XSS_CSRF(changeTag);
                    dr["CONTENTS"] = changeTag;
                }

                ds.Tables[0].AcceptChanges();
            }
            return ds;

        }

        //쪽지 발송
        public string commNateOnBizSendCall(NateOnBizSendType nateonbiz)
        {

            //발송정보가 있다면
            if (nateonbiz != null)
            {
                //발송자ID가 있다면
                if (nateonbiz.NateOnBizTargetIDS != null)
                {
                    ArrayList list = new ArrayList();
                    ArrayList listMail = new ArrayList();
                    string dirLink = "";
                    string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
                    string NoteBody = "<html><body>"; //보낼 메세지 만들기.
                    //<font face=\"맑은고딕\" size=\"2\">" + nateonbiz.NateOnBizSenderNM + "님께서  쪽지를 보내셨습니다.</font><br/><br/>
                    NoteBody += "<font face='맑은고딕' size='2'>" + nateonbiz.NateOnBizCONTENTS + "</font><br/><br/>";

                    if (nateonbiz.NateOnBizLInkType.Equals("fileSend"))
                    {

						dirLink = BaseURL + "Directory/FileOpenTransfer.aspx?file=" + HttpUtility.UrlEncode(nateonbiz.NateOnBizLInkUrl) + "&tikle=1";
                        NoteBody += "<font face=\"맑은고딕\" size=\"2\">▶ 끌.문서 바로가기: ＇<a href=\"" + dirLink + "\" target=\"_docs\" >" + nateonbiz.fileName + "</a><br /></font>";
                    }
                    else if (nateonbiz.NateOnBizLInkUrl != null)
                    {
                        if (nateonbiz.NateOnBizLInkNm == null)
                        {
                            nateonbiz.NateOnBizLInkNm = nateonbiz.NateOnBizLInkUrl;
                        }

                        if (nateonbiz.NateOnBizLInkNm != null && !nateonbiz.NateOnBizLInkNm.Equals(""))
                        {
                            NoteBody += "<font face=\"맑은고딕\" size=\"2\"><a href=\"" + nateonbiz.NateOnBizLInkUrl + "\" target=\"_docs\" >" + nateonbiz.NateOnBizLInkNm + "</a><br /></font>";
                        }

                        
                        
                    }
                    NoteBody += "</body></html>";

                    
                    

                    /*
                    U : 개인 : 이메일 찾아봐야함
                    O : 조직 : sk.org.조직코드
                    G : 그룹 : 리스트 가져와야함
                    */
                    GlossaryProfileBiz biz_ = new GlossaryProfileBiz();
                    DataSet ds = new DataSet();
                    ImpersonUserinfo imu = new ImpersonUserinfo();
                    GlossaryMyGroupDac dac = new GlossaryMyGroupDac();

                    List<T_Authority> authorityList = new List<T_Authority>();

                    CBHNoteType dataFirst = new CBHNoteType();
                    CBHNoteType data;
                    dataFirst = new CBHNoteType();
                    dataFirst.Content = NoteBody;
                    dataFirst.Kind = "3"; //일반쪽지.
                    dataFirst.URL = null;
                    dataFirst.SendUserName = nateonbiz.NateOnBizSenderNM;
                    dataFirst.SendUserID = nateonbiz.NateOnBizSenderID;

                    CBHMailType dataFirstMail = new CBHMailType();
                    CBHMailType dataMail ;
                    dataFirstMail = new CBHMailType();
                    dataFirstMail.Content = NoteBody;
                    dataFirstMail.Subject = "T.끌 알림 메일입니다.";
                    dataFirstMail.SenderEmail = nateonbiz.NateOnBizSenderMail;

                    for (int i = 0; i < nateonbiz.NateOnBizTargetIDS.Length - 1; i++)
                    {
                        
                        if (nateonbiz.NateOnBizTargetTYS[i].Equals("U"))
                        { 
                            
                            imu = biz_.UserSelect(nateonbiz.NateOnBizTargetIDS[i]);

                            /*
                            Author : 개발자-김성환D, 리뷰자-진현빈D
                            Create Date : 2016.04.20 
                            Desc : 유저 email값이 없으면 메일 보내지 않음
                            */
                            if (imu.EmailAddress != null)
                            {
                                data = createTypeTargetUser(dataFirst, imu.EmailAddress);
                                list.Add(data);

                                dataMail = createTypeTargetUserMail(dataFirstMail, imu.EmailAddress);
                                listMail.Add(dataMail);

                                if (nateonbiz.NateOnBizLInkType.Equals("fileSend"))
                                {
                                    authorityList.Add(new T_Authority() { AD_ID = "skt\\" + imu.UserID, TYPE = "P" }); //개인
                                }
                            }
                            //**helper.SendNoteToQueue(data);

                        }
                        else if(nateonbiz.NateOnBizTargetTYS[i].Equals("O"))
                        {
                           // data.TargetUser = "skt.org."+nateonbiz.NateOnBizTargetIDS[i];
                            data = createTypeTargetUser(dataFirst, "skt.org." + nateonbiz.NateOnBizTargetIDS[i]);
                            list.Add(data);

                            dataMail = createTypeTargetUserMail(dataFirstMail, "skt.org." + nateonbiz.NateOnBizTargetIDS[i]);
                            listMail.Add(dataMail);

                            if (nateonbiz.NateOnBizLInkType.Equals("fileSend"))
                            {
                                authorityList.Add(new T_Authority() { AD_ID = "skt\\ORG" + nateonbiz.NateOnBizTargetIDS[i], TYPE = "G" });   //그룹
                            }
                        }
                        else if(nateonbiz.NateOnBizTargetTYS[i].Equals("G"))
                        {
                            ds = dac.MyGroupListSelect2(nateonbiz.NateOnBizUserID, nateonbiz.NateOnBizTargetIDS[i], "MyGroup");

                             if (ds != null && ds.Tables.Count > 0)
                            {
                                string changeTag = string.Empty;
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    if (dr["ToUserType"].ToString().Equals("U")) {
                                        imu = biz_.UserSelect(dr["ToUserID"].ToString());
                                       // data.TargetUser = imu.EmailAddress.Remove(imu.EmailAddress.ToString().IndexOf('@'));
                                        data = createTypeTargetUser(dataFirst, "skt.org." + nateonbiz.NateOnBizTargetIDS);
                                        list.Add(data);

                                        dataMail = createTypeTargetUserMail(dataFirstMail, "skt.org." + nateonbiz.NateOnBizTargetIDS);
                                        listMail.Add(dataMail);

                                        if (nateonbiz.NateOnBizLInkType.Equals("fileSend"))
                                        {
                                            authorityList.Add(new T_Authority() { AD_ID = "skt\\" + imu.UserID, TYPE = "P" }); //개인
                                        }
                                    }
                                    else if (dr["ToUserType"].ToString().Equals("O"))
                                    {
                                       // data.TargetUser = "sk.org." + nateonbiz.NateOnBizTargetIDS;
                                        data = createTypeTargetUser(dataFirst, "skt.org." + nateonbiz.NateOnBizTargetIDS);
                                        list.Add(data);

                                        dataMail = createTypeTargetUserMail(dataFirstMail, "skt.org." + nateonbiz.NateOnBizTargetIDS);
                                        listMail.Add(dataMail);

                                        //**helper.SendNoteToQueue(data);
                                        if (nateonbiz.NateOnBizLInkType.Equals("fileSend"))
                                        {
                                            authorityList.Add(new T_Authority() { AD_ID = "skt\\ORG" + nateonbiz.NateOnBizTargetIDS[i], TYPE = "G" });   //그룹
                                        }
                                    }
                                    
                                }

                            }
                        }
                        
                    }
                    //실제발송하기
                    if (list != null)
                    {
                        if (nateonbiz.NateOnBizLInkType.Equals("fileSend"))
                        {
                            GlossaryDirectoryBiz spBiz = new GlossaryDirectoryBiz();
                            spBiz.spSetFileReadPermission(nateonbiz.folderName, nateonbiz.fileName, authorityList, nateonbiz.NateOnBizUserID);
                        }

                        //CBHMSMQHelper helper = new CBHMSMQHelper();
                        for (int i = 0; i < list.Count; i++)
                        {
                            //쪽지 20170802
                            CBHNoteType sendData = (CBHNoteType)list[i];
                            CBHInterface.CBHNoteSend(sendData);
                            //OK//helper.SendNoteToQueue(sendData);

                            //메일 20170802
                            CBHMailType sendDataMail = (CBHMailType)listMail[i];
                            CBHInterface.CBHMailSend(sendDataMail);
                        }
                    }

                }
            }

            return "OK";
        }

        //public CBHNoteType createTypeTargetUser(CBHNoteType type, string targetId) {
        //    CBHNoteType rtnTyp = new CBHNoteType();

        //    rtnTyp.Content = type.Content;
        //    rtnTyp.Kind = type.Kind; //일반쪽지.
        //    rtnTyp.URL = type.URL;
        //    rtnTyp.SendUserName = type.SendUserName;
        //    rtnTyp.SendUserID = type.SendUserID;

        //    if (targetId.IndexOf("@") > 0)
        //    {
        //        rtnTyp.TargetUser = targetId.Remove(targetId.IndexOf('@'));
        //    }else{
        //        rtnTyp.TargetUser = targetId;
        //    }

            

        //    return rtnTyp;
        //}

        public CBHNoteType createTypeTargetUser(CBHNoteType type, string targetId)
        {
            CBHNoteType rtnTyp = new CBHNoteType();

            rtnTyp.Content = type.Content;
            rtnTyp.Kind = type.Kind; //일반쪽지.
            rtnTyp.URL = type.URL;
            rtnTyp.SendUserName = type.SendUserName;
            rtnTyp.SendUserID = type.SendUserID;

            if (targetId.IndexOf("@") > 0)
            {
                rtnTyp.TargetUser = targetId.Remove(targetId.IndexOf('@'));
            }
            else
            {
                rtnTyp.TargetUser = targetId;
            }

            return rtnTyp;
        }

        public CBHMailType createTypeTargetUserMail(CBHMailType type, string targetId)
        {
            CBHMailType rtnTyp = new CBHMailType();

            rtnTyp.Content = type.Content;
            rtnTyp.Subject = type.Subject;
            rtnTyp.SenderEmail = type.SenderEmail;
            rtnTyp.ReceiverEmail = targetId;

            return rtnTyp;
        }

    }

    
}