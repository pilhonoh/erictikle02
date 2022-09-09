using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

public partial class common_module_proc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string part = Request.QueryString.Get("part");
        string uuid = Request.QueryString.Get("uuid");

        string result = "";

        //숫자형 String 값을 리턴 받는다.
        result = getProcedure(part, uuid);

        //Response.Write(part + " :: "+ result + "\n");

        if (int.Parse(result) > 0)
            result = "true";
        else
            result = "false";


        Response.Write(result);
    }

    /*
     *  프로시져 호출
     *    
     *  1. dbo.up_MasterMsgReceiveUnReadCount, param : @ToUUID    [수신함]
     *  2. dbo.up_MasterSupexMyFeedCountSelect, param: @UUID      [My Feed]         
     */
    public static String getProcedure(string part, string uuid)
    {
        string db_ip = ConfigurationManager.AppSettings["MSSQL_DB_IP"];
        string db_name = ConfigurationManager.AppSettings["MSSQL_DB_NAME"];
        string db_id = ConfigurationManager.AppSettings["MSSQL_DB_ID"];
        string db_pw = ConfigurationManager.AppSettings["MSSQL_DB_PW"];

        //DB 접속 정보 설정
        string conString = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" + db_id + ";Password=" + db_pw + ";Initial Catalog=" + db_name + ";Data Source=" + db_ip;
        string temp = "";
        string procedure = "";
        string param = "";

        if ("msg".Equals(part))
        {
            procedure = "dbo.up_MasterMsgReceiveUnReadCount";
            param = "@ToUUID";
        }
        else
        {
            procedure = "dbo.up_MasterSupexMyFeedCountSelect";
            param = "@UUID";
        }

        try
        {
            DataSet ds = new DataSet();

            //DB Connection
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = conString;

            OleDbDataAdapter Adpt = new OleDbDataAdapter(procedure, conn);
            Adpt.SelectCommand.CommandType = CommandType.StoredProcedure;

            //파라미터 설정
            Adpt.SelectCommand.Parameters.Add(param, SqlDbType.VarChar);
            Adpt.SelectCommand.Parameters[param].Direction = ParameterDirection.Input;
            Adpt.SelectCommand.Parameters[param].Value = uuid;

            Adpt.Fill(ds);

            DataTable dt = ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                temp += dr[0];
            }

            conn.Close();
        }
        catch (Exception E)
        {
            System.Console.WriteLine(E.Message);
            temp = E.Message;
        }

        return temp;
    }
}