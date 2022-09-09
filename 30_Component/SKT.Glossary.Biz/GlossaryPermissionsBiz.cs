using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace SKT.Glossary.Biz
{
    public class GlossaryPermissionsBiz
    {
        public GlossaryPermissionsBiz() { }

        /// <summary>
        /// Inserts a record into the tb_Permissions table.
        /// </summary>
        /// <returns></returns>
        public void PermissionsInsert(string GlossaryID, string ToUserID, string ToUserName)
        {
            GlossaryPermissionsDac dac = new GlossaryPermissionsDac();
            PermissionsType permissionsType = new PermissionsType();
            DataSet ds = new DataSet();

            string[] ToUser = ToUserID.Split('/');
            string[] ToName = ToUserName.Split('&');
            for (int i = 0; i < ToUser.Length; i++)
            {
                // User 인지 부서인지 Check
                GlossaryProfileBiz biz_ = new GlossaryProfileBiz();
                ImpersonUserinfo u = biz_.UserSelect(ToUser[i]);

                if (ToUser[i] != "" && !String.IsNullOrEmpty(u.UserID))
                {
                    permissionsType.GlossaryID = Convert.ToInt32(GlossaryID);
                    permissionsType.ToUserID = ToUser[i];
                    permissionsType.ToUserName = ToName[i];
                    dac.PermissionsInsert(permissionsType);
                }
                else
                {
                    PermissionsInsert_Dept(GlossaryID, ToUser[i], ToName[i]);
                }
            }
        }

        /// <summary>
        /// 끌.지식 권한추가(모임정보추가)
        /// </summary>
        /// <param name="GlossaryID"></param>
        /// <param name="ToUserID"></param>
        /// <param name="ToUserName"></param>
        /// <param name="AuthCL"></param>
        public void PermissionsInsert(string GlossaryID, string ToUserID, string ToUserName, string AuthCL, string CommonID)
        {
            GlossaryPermissionsDac dac = new GlossaryPermissionsDac();
            PermissionsType permissionsType = new PermissionsType();
            DataSet ds = new DataSet();

            string[] ToUser = ToUserID.Split('/');
            string[] ToName = ToUserName.Split('&');
            string[] ItemType = AuthCL.Split('/');

            for (int i = 0; i < ToUser.Length; i++)
            {
                if (ToUser[i].Trim().Length > 0)
                {
                    // 끌.모임 - 지식
                    if (ItemType[i] == "M")
                    {
                        GlossaryGatheringBiz gatheringBiz = new GlossaryGatheringBiz();

                        gatheringBiz.GatheringMenuAuth_Insert(ToUser[i], "Knowledge", CommonID);

                        /*
                        Author : 개발자-김성환D, 리뷰자-이정선G
                        Create Date : 2016.02.17 
                        Desc : 공유시 모임멤버도 추가되도록 권한 로직 추가
                        */
                        ds = gatheringBiz.GlossaryGatheringAuth_Select(ToUser[i]);
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            permissionsType.GlossaryID = Convert.ToInt32(GlossaryID);
                            permissionsType.ToUserID = ds.Tables[0].Rows[j]["AuthID"].ToString();
                            permissionsType.ToUserName = ds.Tables[0].Rows[j]["DISPLAYNAME"].ToString();
                            dac.PermissionsInsert(permissionsType);
                        }
                    }
                    else
                    {
                        //// User 인지 부서인지 Check
                        // GlossaryProfileBiz biz_ = new GlossaryProfileBiz();
                        //ImpersonUserinfo u = biz_.UserSelect(ToUser[i]);

                        //if (ToUser[i] != "" && !String.IsNullOrEmpty(u.UserID))
                        //{
                            permissionsType.GlossaryID = Convert.ToInt32(GlossaryID);
                            permissionsType.ToUserID = ToUser[i];
                            permissionsType.ToUserName = ToName[i];
                            dac.PermissionsInsert(permissionsType);
                        //}
                        //else
                        //{
                        //    PermissionsInsert_Dept(GlossaryID, ToUser[i], ToName[i]);
                        //}
                    }
                }
            }
        }

        

        /// <summary>
        /// 부서는 재귀 호출로 처리
        /// </summary>
        /// <param name="GlossaryID"></param>
        /// <param name="ToUserID"></param>
        /// <param name="ToUserName"></param>
        public void PermissionsInsert_Dept(string GlossaryID, string ToUserID, string ToUserName)
        {
            //string connectionStringName = "ConnOrgChart";
            string connectionStringName = "ConnGlossary";
            Database db = DatabaseFactory.CreateDatabase(connectionStringName);
            DbCommand dbCommand = db.GetStoredProcCommand("up_list_department_person");

            db.AddInParameter(dbCommand, "departmentNumber", DbType.String, ToUserID.ToString());

            using (DataSet ds = db.ExecuteDataSet(dbCommand))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        string employeeID = (dr["employeeID"] == DBNull.Value) ? null : dr.Field<string>("employeeID");
                        string koreanName = (dr["koreanName"] == DBNull.Value) ? null : dr.Field<string>("koreanName");
                        if (!String.IsNullOrEmpty(employeeID)) { PermissionsInsert(GlossaryID, employeeID, (koreanName + "/" + ToUserName).ToString()); }
                    }
                }
            }

        }

        /// <summary>
        /// GlossaryID 값을 주면 그에 해당하는 Name과 사번을 반환하도록 변경함
        /// </summary>
        /// <returns>DataSet</returns>
        public List<PermissionsType> PermissionsSelect(Int64 iD)
        {
            GlossaryPermissionsDac dac = new GlossaryPermissionsDac();
            DataSet ds = new DataSet();
            List<PermissionsType> listPermissionsType = dac.PermissionsSelect(iD);

            return listPermissionsType;
        }

        /// <summary>
        /// Selects all records from the tb_Permissions table.
        /// </summary>
        public List<PermissionsType> PermissionsSelectAll()
        {
            GlossaryPermissionsDac dac = new GlossaryPermissionsDac();
            DataSet ds = new DataSet();
            List<PermissionsType> listPermissionsType = dac.PermissionsSelectAll();

            return listPermissionsType;
        }

        /// <summary>
        /// Updates a record in the tb_Permissions table.
        /// </summary>
        public void PermissionsUpdate(PermissionsType permissionsType)
        {
            GlossaryPermissionsDac dac = new GlossaryPermissionsDac();
            DataSet ds = new DataSet();
            dac.PermissionsUpdate(permissionsType);

            return;
        }

        /// <summary>
        /// Deletes a record from the tb_Permissions table by a composite primary key.
        /// </summary>
        public void PermissionsDelete(Int64 iD)
        {
            GlossaryPermissionsDac dac = new GlossaryPermissionsDac();
            DataSet ds = new DataSet();
            dac.PermissionsDelete(iD);

            return;
        }

        /// <summary>
        /// ViewPage Permisstion Check
        /// </summary>
        public int Permissions_Check(string ItemID, string UserID)
        {
            GlossaryPermissionsDac dac = new GlossaryPermissionsDac();
            DataSet ds = new DataSet();

            int returnValue = dac.Permissions_Check(ItemID, UserID);

            return returnValue;
        }

        /// <summary>
        /// Creates a new instance of the PermissionsType class and populates it with data from the specified DataRow.
        /// </summary>
        private PermissionsType GetPermissionsTypeMapData(DataRow dr)
        {
            PermissionsType permissionsType = new PermissionsType();
            permissionsType.ID = (dr["ID"] == DBNull.Value) ? 0 : dr.Field<long>("ID");
            permissionsType.GlossaryID = (dr["GlossaryID"] == DBNull.Value) ? 0 : dr.Field<int>("GlossaryID");
            permissionsType.ToUserID = (dr["ToUserID"] == DBNull.Value) ? null : dr.Field<string>("ToUserID");

            return permissionsType;
        }
    }
}
