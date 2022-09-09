using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using SKT.Common;

namespace SKT.Glossary.Biz
{
    public class GlossaryCategoryBiz
    {
        public void GlossaryCategoryUpdateAdmin(string mode, GlossaryCategoryType board)
        {   
            GlossaryCategoryDac.Instance.GlossaryCategoryUpdateAdmin(mode, board);
        }
        public void GlossaryCategoryDeleteAdmin(GlossaryCategoryType GlossaryCategoryType)
        {
            GlossaryCategoryDac.Instance.GlossaryCategoryDeleteAdmin(GlossaryCategoryType);
        }

        /// <summary>
        /// 메인 화면에서 사용자에 따른 카테고리와 해당 부문의 ID, CategoryName 을 리턴함
        /// </summary>
        /// <param name="GlossaryCategoryType"></param>
        public List<GlossaryCategoryType> GlossaryCategory_Main_User_List(string USER_ID)
        {
            List<GlossaryCategoryType> listGlossaryCategoryType = GlossaryCategoryDac.Instance.GlossaryCategory_Main_User_List(USER_ID);
            return listGlossaryCategoryType;
        }

        /// <summary>
        /// 메인 화면에서 사용자에 따른 카테고리와 해당 부문의 ID, CategoryName 을 리턴함
        /// </summary>
        /// <param name="GlossaryCategoryType"></param>
        public List<GlossaryType> GlossaryCategory_Main_Category_List(int ID)
        {
            List<GlossaryType> listGlossaryCategoryType = GlossaryCategoryDac.Instance.GlossaryCategory_Main_Category_List(ID);
            return listGlossaryCategoryType;
        }

        public int GlossaryCategory_Check(int CategoryID)
        {
            int returnCount = GlossaryCategoryDac.Instance.GlossaryCategory_Check(CategoryID);
            return returnCount;
        }

    }
}
