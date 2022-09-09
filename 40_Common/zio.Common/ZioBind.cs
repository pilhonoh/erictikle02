using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Data;

namespace Zio.Common
{
    public class ZioBind 
    {
      
        [WebMethod]
        public void DropdownlistDatatableSimple(DropDownList ddlcontrol, DataTable datasource, string textfield, string valuefield)
        {            
            ddlcontrol.DataSource = datasource;
            ddlcontrol.DataTextField = textfield;
            ddlcontrol.DataValueField = valuefield;
            ddlcontrol.DataBind();
        }

    }
}
