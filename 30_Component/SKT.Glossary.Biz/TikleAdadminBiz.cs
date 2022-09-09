using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKT.Glossary.Dac;
using SKT.Glossary.Type;

namespace SKT.Glossary.Biz
{
    public class TikleAdadminBiz
    {


        public DataSet TikleAdminTotal(string sdate, string edate)
        {

            TikleAdminDac dac = new TikleAdminDac();
            DataSet rtn = dac.TikleAdminTotal(sdate,edate);
            return rtn;
        }

        public DataSet TikleAdminDept(string sdate, string edate)
        {
            TikleAdminDac dac = new TikleAdminDac();
            DataSet rtn = dac.TikleAdminDept(sdate,edate);
            return rtn;
        }

        public DataSet TikleAdminMenu(string sdate, string edate)
        {
            TikleAdminDac dac = new TikleAdminDac();
            DataSet rtn = dac.TikleAdminMenu(sdate, edate);
            return rtn;
        }

        /*
        Author : 개발자-김성환D, 리뷰자-진현빈D
        Create Date : 2016.04.27 
        Desc : 통계화면 추가
        */     
        public DataSet TikleAdminAccess(string sdate, string edate)
        {
            TikleAdminDac dac = new TikleAdminDac();
            DataSet rtn = dac.TikleAdminAccess(sdate, edate);
            return rtn;
        }

        public DataSet TikleAdminWeeklyNoteCount(string sdate, string edate)
        {
            TikleAdminDac dac = new TikleAdminDac();
            DataSet rtn = dac.TikleAdminWeeklyNoteCount(sdate, edate);
            return rtn;
        }


        /*
        Author : 개발자-김성환D, 리뷰자-진현빈D
        Create Date : 2016.05.11
        Desc : Weekly 통계 추가
        */ 
        public DataSet TikleAdminWeeklyData(string sdate, string edate,string deptcode)
        {
            TikleAdminDac dac = new TikleAdminDac();
            DataSet rtn = dac.TikleAdminWeeklyData(sdate, edate, deptcode);
            return rtn;
        }

        /*
        Author : 개발자-김성환D, 리뷰자-진현빈D
        Create Date : 2016.05.11
        Desc : Weekly 통계 대상 부서 조회
        */ 
        public DataSet TikleAdminTargetDept()
        {
            TikleAdminDac dac = new TikleAdminDac();
            DataSet rtn = dac.TikleAdminTargetDept();
            return rtn;
        }

		public DataSet TikleAdminBannerSelect()
		{
			TikleAdminDac dac = new TikleAdminDac();
			DataSet rtn = dac.TikleAdminBannerSelect();
			return rtn;
		}

		public DataSet TikleAdminSiteConfigUpdate(string ATTR_NM, string ATTR_VAL, string ATTR_DESC, string USERID)
		{
			TikleAdminDac dac = new TikleAdminDac();
			DataSet rtn = dac.TikleAdminSiteConfigUpdate(ATTR_NM, ATTR_VAL, ATTR_DESC, USERID);
			return rtn;
		}

		public DataSet TikleAdminBannerUpdate(string NotID, string Title, string ImgFile, string URL, string SeqNo, string USERID)
		{
			TikleAdminDac dac = new TikleAdminDac();
			DataSet rtn = dac.TikleAdminBannerUpdate(NotID, Title, ImgFile, URL, SeqNo, USERID);
			return rtn;
		}

		//메인화면 지식 목록 삭제
		public DataSet TikleAdminMainNoticeDelete(string Gubun, string NotID = null)
		{
			TikleAdminDac dac = new TikleAdminDac();
			DataSet rtn = dac.TikleAdminMainNoticeDelete(Gubun, NotID);

			return rtn;
		}

		//메인화면 지식 등록
		public DataSet TikleAdminMainNoticeInsert(MainNoticeType Data)
		{
			TikleAdminDac dac = new TikleAdminDac();
			DataSet rtn = dac.TikleAdminMainNoticeInsert(Data);

			return rtn;
		}

		//메인화면 지식 조회
		public DataSet TikleAdminMainNoticeSelect(string Gubun)
		{
			TikleAdminDac dac = new TikleAdminDac();
			DataSet rtn = dac.TikleAdminMainNoticeSelect(Gubun);

			return rtn;
		}
        
        //관리자>통계>종합 문서함 개수 2014-11-24 김성환
        public DataSet TikleAdminTotal_DirExcel()
        {
            TikleAdminDac dac = new TikleAdminDac();
            DataSet rtn = dac.TikleAdminTotal_DirExcel();
            return rtn;
        }

        //관리자>통계>종합 의견함 개수 2014-11-24 김성환
        public DataSet TikleAdminTotal_SurveyExcel()
        {
            TikleAdminDac dac = new TikleAdminDac();
            DataSet rtn = dac.TikleAdminTotal_SurveyExcel();
            return rtn;
        }

        //Guest 모드 설정 2015-05-12 김성환
        public DataSet TikleAdmin_GuestSwitch(string UserID)
        {
            TikleAdminDac dac = new TikleAdminDac();
            DataSet rtn = dac.TikleAdmin_GuestSwitch(UserID);
            return rtn;
        }

        //플랫폼 접속자 수 
        public DataSet TikleAdminPlatStat(string sdate, string edate)
        {
            TikleAdminDac dac = new TikleAdminDac();
            DataSet rtn = dac.TikleAdminPlatStat(sdate, edate);
            return rtn;
        }

        public DataSet ArraTrendSelect(int PageNum, int PageSize)
        {
            TikleAdminDac dac = new TikleAdminDac();
            DataSet rtn = dac.ArraTrendSelect(PageNum, PageSize);
            return rtn;
        }
        public DataSet ArraTrendAction(string mode, int ID, string Gubun, string Title, string Url, string UserID)
        {
            TikleAdminDac dac = new TikleAdminDac();
            DataSet rtn = dac.ArraTrendAction(mode, ID, Gubun, Title, Url, UserID);
            return rtn;
        }
    }
}
