using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;

namespace SKT.Common
{
    public class MultiLangImgHelper : Image
    {
        public MultiLangImgHelper()
        {
            this.Page = new Page();
        }

        public MultiLangImgHelper(System.Web.UI.Page Page)
        {
            this.Page = Page;
        }

        public string GetImgURL(string ImgPath)
        {
            string DirectoryName = Path.GetDirectoryName(ImgPath);
            string FileName = Path.GetFileNameWithoutExtension(ImgPath);
            string Extension = Path.GetExtension(ImgPath);
            CultureInfo CurrentCultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            string MultilangImgPath = DirectoryName + "\\" + FileName + "." + CurrentCultureInfo.Name + Extension;
            string ServerMultilangImgPath = Page.Server.MapPath(DirectoryName +"\\"+ FileName + "." + CurrentCultureInfo.Name + Extension);
            if (File.Exists(ServerMultilangImgPath))
                return MultilangImgPath;

            return ImgPath;
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.ImageUrl = GetImgURL(this.ImageUrl);
        }
    }
}
