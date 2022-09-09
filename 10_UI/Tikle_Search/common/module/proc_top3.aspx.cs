using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;


public partial class common_module_proc_top3 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		 Response.Write(getProcedure());
    }
    
    public static string getProcedure()
    {
        string db_ip = ConfigurationManager.AppSettings["MSSQL_DB_IP"];
        string db_name = ConfigurationManager.AppSettings["MSSQL_DB_NAME"];
        string db_id = ConfigurationManager.AppSettings["MSSQL_DB_ID"];
        string db_pw = ConfigurationManager.AppSettings["MSSQL_DB_PW"];

        //DB 접속 정보 설정
        string conString = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" + db_id + ";Password=" + db_pw + ";Initial Catalog=" + db_name + ";Data Source=" + db_ip;
        string temp = "";

        string procedure = "dbo.up_MasterSupexBoardTop3List";

        try
        {
            DataSet ds = new DataSet();

            //DB Connection
            OleDbConnection conn= new OleDbConnection();
            conn.ConnectionString = conString;

            OleDbDataAdapter Adpt = new OleDbDataAdapter(procedure, conn);
            Adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
            Adpt.Fill(ds);

            DataTable dt = ds.Tables[0];
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                temp += "<li class='ellipsis'><a href='/Supex/Contents/Supex/SupexList.aspx?SupexType=" + dr["gubuncode"] + "&SupexMode=V&SupexID=' " + dr["supexid"] + ">" + getNoticeYN(Convert.ToString(dr["noticeyn"])) + " " + dr["subject"] + "</a></li>";
				
            }

            conn.Close();
        }
        catch (Exception E)
        {
            System.Console.WriteLine(E.Message);
            
            temp = E.Message + "\n";
        }

        return temp;
    }

    public static String getNoticeYN(string noticeCode) {

        string temp = "";

        if ("".Equals(noticeCode.Trim()))
            return temp;

        if ("True".Equals(noticeCode.Trim()))
            temp = "[공지]";

        return temp;
    }
    
}