<%@ Page Language="C#" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<script runat="server">

    
protected void Page_Load(object sender,EventArgs e)
{
    
    //Response.Write("hello Azure");

    try
    {
        string _connName = "ConnCmnMgt";
        Microsoft.Practices.EnterpriseLibrary.Data.Database db = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase(_connName);
        //string name = "UK";

        //1. SQL문을 사용하여 DS에 데이터를 담는 방법
        System.Data.DataSet ds = db.ExecuteDataSet(System.Data.CommandType.Text, "Select top 10 * from tb_person");
        //2. 저장프로시저를 사용하여 DS에 데이터를 담는 방법
        //DataSet ds2 = db.ExecuteDataSet(CommandType.StoredProcedure, "GetCustomers");
        //3. Parameter를 전달 하는 방법
        //DataSet ds3 = db.ExecuteDataSet("GetCustomers", name);

        //DbCommand를 사용하여 저장프로시저 지정
        //System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand("GetCustomers");
        //Parameter 전달
        //db.AddInParameter(dbCommand, "@Country", DbType.String, name);

        //쿼리 실행 및 DataSet에 Data저장
        //DataSet ds4 = db.ExecuteDataSet(dbCommand);
        //DataTable dt = ds4.Tables[0];

        string tmp = string.Empty;

        if (ds != null)
        {
            foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
            {
                string _employeeid = string.Empty;
                string _koreanName = string.Empty;
                //Row row = new Row();
                //row.CommonID = dr["CommonID"].ToString();
                //row.Title = dr["Title"].ToString();
                _employeeid = dr["employeeid"].ToString();
                _koreanName = dr["koreanName"].ToString();

                Response.Write(_employeeid + "<br>");
                Response.Write(_koreanName + "<br>");

                //tmp = tmp + _title + "<br>";
            }
        }
    }
    catch (Exception ex)
    {

        Response.Write(ex.ToString());
    }

            
}
    
</script>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title></title>
</head>
<body>
   

</body>
</html>
