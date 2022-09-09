using System;
using System.Collections.Generic; 
using System.Linq; 
using System.Text;
using System.Data; 
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SKT.Tnet.Framework.Utilities
{

	/// <summary>
	/// JSON 객체 Helper
	/// </summary>
	public static class JsonHelper
	{


		/// <summary>
		/// DataTable 객체를 JSON 객체 String로 반환하여 준다. 
		/// </summary>
		/// <param name="dt">DataTable</param>
		/// <returns>JSON string</returns>
		public static string JsonHelper_Serialization(this DataRow tr)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();
			Dictionary<string, object> row = new Dictionary<string, object>();

			foreach (DataColumn col in tr.Table.Columns)
			{
				row.Add(col.ColumnName, tr[col.ColumnName]);
			}
			return jss.Serialize(row);
		}

        /// <summary>
        /// DataRow[] 객체를 JSON 객체 String로 반환하여 준다. 
        /// </summary>
        /// <param name="dRows">DataRow[]</param>
        /// <returns>JSON string</returns>
        public static string JsonHelper_Serialization(this DataRow[] dRows)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;

            foreach (DataRow dr in dRows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dr.Table.Columns)
                {
                    row.Add(col.ColumnName, dr[col.ColumnName]);
                }
                rows.Add(row);
            }
            return jss.Serialize(rows);
        }


		/// <summary>
		/// DataTable 객체를 JSON 객체 String로 반환하여 준다. 
		/// </summary>
		/// <param name="dt">DataTable</param>
		/// <returns>JSON string</returns>
		public static string JsonHelper_Serialization(this DataTable dt)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();

			List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
			Dictionary<string, object> row;

			foreach (DataRow dr in dt.Rows)
			{
				row = new Dictionary<string, object>();
				foreach (DataColumn col in dt.Columns)
				{
					row.Add(col.ColumnName, dr[col.ColumnName]); 
				}
				rows.Add(row); 
			}

			return jss.Serialize(rows); 
		}



        /// <summary>
        /// DataTable 객체를 Select 하여 JSON 객체 String로 반환하여 준다. 
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="select">DataTable select 문</param>
        /// <returns>JSON string</returns>
		public static string JsonHelper_Serialization(this DataTable dt, string select)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();

			List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
			Dictionary<string, object> row;


			DataRow[] dRows = dt.Select(select);

			foreach (DataRow dr in dRows)
			{
				row = new Dictionary<string, object>();
				foreach (DataColumn col in dt.Columns)
				{
					row.Add(col.ColumnName, dr[col.ColumnName]);
				}
				rows.Add(row);
			}

			return jss.Serialize(rows);
		}


		/// <summary>
        /// DataTable 객체를 JSON 객체 String로 반환하여 준다. 
		/// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="select">DataTable select 문</param>
        /// <param name="order">DataTable sorting order</param>
        /// <returns>JSON string</returns>
		public static string JsonHelper_Serialization(this DataTable dt, string select, string order)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();

			List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
			Dictionary<string, object> row;


			DataView dv = dt.DefaultView;
			dv.RowFilter = select;
			dv.Sort = order;

			DataTable newTable = dv.ToTable();

			foreach (DataRowView dr in dv)
			{
				row = new Dictionary<string, object>();
				foreach (DataColumn col in newTable.Columns)
				{
					row.Add(col.ColumnName, dr[col.ColumnName]);
				}
				rows.Add(row);
			}

			return jss.Serialize(rows);
		}

        /// <summary>
        /// Dictionary&lt;string, object&gt; 객체를 JSON 객체 String로 반환하여 준다. 
        /// </summary>
        /// <param name="row">Dictionary</param>
        /// <returns>JSON string</returns>
        public static string JsonHelper_Serialization(this Dictionary<string, object> row)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            rows.Add(row);            

            return jss.Serialize(rows);
        }


		/// <summary>
		/// Dictionary&lt;string, object&gt; 객체를 JSON 객체 String로 반환하여 준다. 
		/// </summary>
		/// <param name="row">Dictionary</param>
		/// <returns>JSON string</returns>
		public static string JsonHelper_SerializationDictionary(this Dictionary<string, object> row)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();
			return jss.Serialize(row);
		}





		/// <summary>
        /// List&lt;Dictionary&lt;string, object&gt;&gt; 객체를 JSON 객체 String로 반환하여 준다. 
		/// </summary>
        /// <param name="rows">Dictionary</param>
		/// <returns>JSON string</returns>
		public static string JsonHelper_Serialization(this List<Dictionary<string, object>> rows)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();
			return jss.Serialize(rows);
		}



		/// <summary>
		/// JSON 객체 string 을 List&lt;Dictionary&lt;string, object&gt;&gt; 형태로 반환하여 준다.
		/// </summary>
		/// <param name="JsonText">JSON string</param>
		/// <returns>List&lt;Dictionary&lt;string, object&gt;&gt;</returns>
		public static Dictionary<string, object> JsonHelper_DeserializeDictionary(this string JsonText)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();
			return jss.Deserialize<Dictionary<string, object>>(JsonText);
		}




        /// <summary>
        /// JSON 객체 string 을 List&lt;Dictionary&lt;string, object&gt;&gt; 형태로 반환하여 준다.
        /// </summary>
        /// <param name="JsonText">JSON string</param>
        /// <returns>List&lt;Dictionary&lt;string, object&gt;&gt;</returns>
		public static List<Dictionary<string, object>> JsonHelper_Deserialize(this string JsonText)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();
			return jss.Deserialize<List<Dictionary<string, object>>>(JsonText);
		}

        /// <summary>
        /// JSON 객체 string 을 DataTable 형태로 반환하여 준다.
        /// </summary>
        /// <param name="JsonText">JSON string</param>
        /// <returns>DataTable</returns>
        public static DataTable JsonHelper_DeserializeToDataTable(this string JsonText)
        {
            DataTable rtnDt = new DataTable();

            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<Dictionary<string, object>> listJSON = jss.Deserialize<List<Dictionary<string, object>>>(JsonText);
            
            //Create Column
            foreach(string key in listJSON[0].Keys)
            {
                rtnDt.Columns.Add(new DataColumn(key,typeof(String)));
            }
            
            for (int i = 0; i < listJSON.Count; i++)
            {  
                DataRow newRow = rtnDt.NewRow();

                foreach (KeyValuePair<string, object> pairJSON in listJSON[i])
                {
                    newRow[pairJSON.Key] = pairJSON.Value;                    
                }
                rtnDt.Rows.Add(newRow);
            }
            return rtnDt;
        }

	}
}
