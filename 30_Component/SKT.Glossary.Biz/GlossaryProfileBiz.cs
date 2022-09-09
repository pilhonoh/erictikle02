using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SKT.Glossary.Dac;
using System.Data;
using SKT.Glossary.Type;
using SKT.Common;


namespace SKT.Glossary.Biz
{
   public class GlossaryProfileBiz
    {

        //사용자 사번으로 사용자정보를 가져온다.
        public ImpersonUserinfo UserSelect(string UserID)
        {
            GlossaryProfileDac dac = new GlossaryProfileDac();
            DataSet ds = new DataSet();
            ds = dac.UserSelect(UserID);

            ImpersonUserinfo Info = new ImpersonUserinfo();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                //Info.UserID = dr["EmpID"].ToString();
                // 2014-05-19 Mr.No 수정
                Info.UserID = (dr["EmpID"] == DBNull.Value) ? null : dr.Field<string>("EmpID");
                Info.Name = dr["Name"].ToString();
                Info.DeptID = dr["DeptID"].ToString();
                Info.DeptName = dr["DeptName"].ToString();
                Info.EmailAddress = dr["EmailAddress"].ToString();
                Info.WorkArea = dr["WorkArea"].ToString();
                Info.Part = dr["Part"].ToString();
                
                Info.TEL = dr["TEL"].ToString();
                Info.Phone = dr["Phone"].ToString();

                Info.PhotoUrl  = dac.GetPicture(UserID);
                Info.JobCode = dr["JobCode"].ToString();
                Info.JobCodeName = dr["JobCodeName"].ToString();

                Info.Part2 = dr["Part2"].ToString();
                Info.Part3 = dr["Part3"].ToString();
                Info.PositionName = dr["PositionName"].ToString();
            }

            return Info;
        }

         //사용자 사번으로 사용자정보를 가져온다.
        public List<ImpersonUserinfo> UserSelectList(string UserIDs)
        {
            GlossaryProfileDac dac = new GlossaryProfileDac();
            DataSet ds = new DataSet();
            ds = dac.UserSelectList(UserIDs);

            List<ImpersonUserinfo> infolist = new List<ImpersonUserinfo>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ImpersonUserinfo Info = new ImpersonUserinfo();
                Info.Name = dr["USER_NAME"].ToString();
                Info.EmailAddress = dr["EMAIL_ALIAS"].ToString();
                Info.JobCodeName = dr["POSITION_NAME"].ToString();

                infolist.Add(Info);
            }

            return infolist;
        }



        //프로필 뷰
        public GlossaryProfileType GlossaryProfileSelect(string UserID)
        {
            GlossaryProfileDac Dac = new GlossaryProfileDac();
            DataSet ds = Dac.GlossaryProfileSelect(UserID);
            GlossaryProfileType Board = new GlossaryProfileType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Board.ID = dr["ID"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.CreateDate = dr["CreateDate"].ToString();
                }
            }
            return Board;
        }

        //프로필 추가
        public void GlossaryProfileInsert(GlossaryProfileType Board)
        {
            GlossaryProfileDac Dac = new GlossaryProfileDac();
            DataSet ds = Dac.GlossaryProfileInsert(Board);
        }

        //부서 프로필 뷰
        public ArrayList GlossaryDeptProfileSelect(string DeptCode, out GlossaryProfileType TeamLeader)
        {
            ArrayList list = new ArrayList();

            GlossaryProfileDac Dac = new GlossaryProfileDac();
            DataSet ds = Dac.GlossaryDeptProfileSelect(DeptCode);

            TeamLeader = new GlossaryProfileType();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                
                int i=0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryProfileType Board = new GlossaryProfileType();
                    Board.ID = dr["ID"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.UserName = dr["UserName"].ToString();
                    Board.DeptCode = dr["DeptCode"].ToString();
                    Board.DeptName = dr["DeptName"].ToString();
                    Board.Contents = dr["Contents"].ToString();
                    Board.ContentsModify = dr["ContentsModify"].ToString();
                    Board.Summary = dr["Summary"].ToString();
                    Board.CreateDate = dr["CreateDate"].ToString();
                    Board.PositionCode =  dr["POSITION_CODE"].ToString();

                    Board.WorkStatus = EHRHelper.EHRWorkStatus(Board.UserID);
                    //if (Board.PositionCode.Trim() == "301")
                    
                    if(i==0)  //가장위에 있는사람이 팀에 장이다.
                    {
                        TeamLeader = Board;
                    }
                    else
                    {
                        list.Add(Board);
                    }
                    i++;
                }
            }
            return list;
        }


       //이메일주소로 사용자정보 얻기
        public ImpersonUserinfo GetProfileFromEmail(string Email)
        {
            GlossaryProfileDac Dac = new GlossaryProfileDac();
            DataSet ds = Dac.GetProfileFromEmail(Email);
            GlossaryProfileType Board = new GlossaryProfileType();


            ImpersonUserinfo Info = new ImpersonUserinfo();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Info.UserID = dr["EmpID"].ToString();
                Info.Name = dr["Name"].ToString();
                Info.DeptID = dr["DeptID"].ToString();
                Info.DeptName = dr["DeptName"].ToString();
                Info.EmailAddress = dr["EmailAddress"].ToString();
                Info.WorkArea = dr["WorkArea"].ToString();
                Info.Part = dr["Part"].ToString();

                Info.TEL = dr["TEL"].ToString();
                Info.Phone = dr["Phone"].ToString();

                Info.PhotoUrl = Dac.GetPicture(Info.UserID);
            }

            return Info;
        }
       

       //profile 정보얻기
        public ArrayList GlossaryProfileList(int PageNum, int PageSize, out int TotalCount, string SearchKeyword)
        {
            ArrayList list = new ArrayList();
            TotalCount = 0;
            GlossaryProfileDac dac = new GlossaryProfileDac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryProfileList(PageNum, PageSize, SearchKeyword);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryProfileType Board = new GlossaryProfileType();
                    Board.UserID = dr["UUID"].ToString();
                    Board.UserName = dr["USER_NAME"].ToString();
                    Board.DeptName = dr["DEPT_NAME"].ToString();

                    list.Add(Board);
                }
            }
            return list;
        }

        // 담당자 정보얻기
        public ArrayList GlossaryJobDescriptionList(int PageNum, int PageSize, out int TotalCount, string SearchKeyword)
        {
            ArrayList list = new ArrayList();
            TotalCount = 0;
            GlossaryProfileDac dac = new GlossaryProfileDac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryJobDescriptionList(PageNum, PageSize, SearchKeyword);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryProfileType Board = new GlossaryProfileType();
                    Board.UserID = dr["UUID"].ToString();
                    Board.UserName = dr["USER_NAME"].ToString();
                    Board.DeptName = dr["DEPT_NAME"].ToString();

                    list.Add(Board);
                }
            }
            return list;
        }

        //DB 경력 초기화 시키기 
        public void GlossarySKCareerReset(string UserID)
        {
            GlossaryProfileDac dac = new GlossaryProfileDac();
            dac.GlossarySKCareerReset(UserID);
        }

        public void GlossaryNoSKCareerReset(string UserID)
        {
            GlossaryProfileDac dac = new GlossaryProfileDac();
            dac.GlossaryNoSKCareerReset(UserID);
        }

       //사내 경력 가져오기 
        public ArrayList GlossarySKCareerList(string UserID)
        {
            ArrayList list = new ArrayList();
            //TotalCount = 0;
            GlossaryProfileDac dac = new GlossaryProfileDac();

            DataSet ds = new DataSet();
            ds = dac.GlossarySKCareerList(UserID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryProfileCareerAfterType Board = new GlossaryProfileCareerAfterType();
                    Board.ID = dr["ID"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.Date = Convert.ToDateTime(dr["Date"]).ToString("yyyy-MM-dd");
                    Board.Status = dr["Status"].ToString();
                    Board.Depart = dr["Depart"].ToString();
                    Board.Message = dr["Message"].ToString();
                    list.Add(Board);
                }
            }
            return list;
        }

        public DataSet GlossaryUserGlossaryList(string UserID)
        {
            GlossaryProfileDac dac = new GlossaryProfileDac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryUserGlossaryList(UserID);

            return ds;
        }
       


        //사외 경력 가져오기 
        public ArrayList GlossaryNoSKCareerList(string UserID)
        {
            ArrayList list = new ArrayList();
            //TotalCount = 0;
            GlossaryProfileDac dac = new GlossaryProfileDac();

            DataSet ds = new DataSet();
            ds = dac.GlossaryNoSKCareerList(UserID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //TotalCount = (int)DatabaseMethod.GetDataRow(ds.Tables[0].Rows[0], "TotalCount", 0);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GlossaryProfileCareerBeforeType Board = new GlossaryProfileCareerBeforeType();
                    Board.ID = dr["ID"].ToString();
                    Board.UserID = dr["UserID"].ToString();
                    Board.BeginDate =  Convert.ToDateTime(dr["BeginDate"]).ToString("yyyy-MM-dd");
                    Board.EndDate =  Convert.ToDateTime(dr["EndDate"]).ToString("yyyy-MM-dd");
                    Board.Company = dr["Company"].ToString();
                    Board.Depart = dr["Depart"].ToString();
                    Board.Position = dr["Position"].ToString();
                    Board.Job = dr["Job"].ToString();
                    
                    list.Add(Board);
                }
            }
            return list;
        }

        public void GlossarySKCareerInsert(GlossaryProfileCareerAfterType Data)
        {
            GlossaryProfileDac dac = new GlossaryProfileDac();
            DataSet  ds = dac.GlossarySKCareerInsert(Data);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Data.ID = dr["ID"].ToString();
                }

            }


        }
        public void GlossaryNoSKCareerInsert(GlossaryProfileCareerBeforeType Data)
        {
            GlossaryProfileDac dac = new GlossaryProfileDac();
            DataSet ds = dac.GlossaryNoSKCareerInsert(Data);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Data.ID = dr["ID"].ToString();
                }

            }
        }
    }
}
