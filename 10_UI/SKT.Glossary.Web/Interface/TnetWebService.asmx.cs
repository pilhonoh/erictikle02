using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SKT.Glossary.Biz;
using SKT.Glossary.Type;
using SKT.Glossary.Dac;
using SKT.Common;
using System.Data.Common;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SKT.Glossary.Web.Interface
{
    /// <summary>
    /// TnetWebService의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    // [System.Web.Script.Services.ScriptService]
    public class TnetWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(Description = "TikleMainTopList")]
        public XmlDocument GlossaryMainTopList()
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

            // Create the root element
            XmlElement rootNode = xmlDoc.CreateElement("ReturnValue");
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode);
            

            // Create a new <Category> element and add it to the root node
            XmlElement rowsNode = xmlDoc.CreateElement("ROWS");
            xmlDoc.DocumentElement.PrependChild(rowsNode);

            GlossaryInterfaceBiz biz = new GlossaryInterfaceBiz();
            DataSet ds = biz.TnetInterface_GlossaryMainTopList();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string Tikle_LinkUrl = System.Configuration.ConfigurationManager.AppSettings["BaseURL"].ToString() + "Glossary/GlossaryView.aspx?ItemID=" + dr["CommonID"].ToString();
                string Tikle_Title = dr["Title"].ToString();
                string Tikle_CreateDate = dr["REGDATE"].ToString();
                string Tikle_RegAuthor = dr["UserName"].ToString();

                XmlElement rowNode = xmlDoc.CreateElement("ROW");
                XmlElement rowTitleNode = xmlDoc.CreateElement("Title");
                rowTitleNode.InnerText = Tikle_Title;
                XmlElement rowLinkUrlNode = xmlDoc.CreateElement("LinkUrl");
                rowLinkUrlNode.InnerText = Tikle_LinkUrl;
                XmlElement rowRegDateNode = xmlDoc.CreateElement("RegDate");
                rowRegDateNode.InnerText = Tikle_CreateDate;
                XmlElement rowRegAuthorNode = xmlDoc.CreateElement("RegAuthor");
                rowRegAuthorNode.InnerText = Tikle_RegAuthor;
                rowNode.AppendChild(rowTitleNode);
                rowNode.AppendChild(rowLinkUrlNode);
                rowNode.AppendChild(rowRegDateNode);
                rowNode.AppendChild(rowRegAuthorNode);
                rowsNode.AppendChild(rowNode);
            }





            /*
            productAttribute = doc.CreateAttribute("id");
            productAttribute.Value = "02";
            productNode.Attributes.Append(productAttribute);
            productsNode.AppendChild(productNode);
            */




            /*
            newNode = xmlDoc.SelectSingleNode("ROWS");
            xmlEle = xmlDoc.CreateElement("ROW");
            xmlAtb = xmlDoc.CreateAttribute("Title");
            xmlAtb.Value = "";
            xmlEle.SetAttributeNode(xmlAtb);
            xmlAtb = xmlDoc.CreateAttribute("LinkUrl");
            xmlAtb.Value = "";
            xmlEle.SetAttributeNode(xmlAtb);
            xmlAtb = xmlDoc.CreateAttribute("RegDate");
            xmlAtb.Value = "";
            xmlEle.SetAttributeNode(xmlAtb);
 
            newNode.AppendChild(xmlEle); // 위에서 찾은 부모 Node에 자식 노드로 추가..
            */

            return xmlDoc;

            /*
            // Write down the XML declaration
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

            // Create the root element
            XmlElement rootNode = xmlDoc.CreateElement("CategoryList");
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode);

            // Create a new <Category> element and add it to the root node
            XmlElement parentNode = xmlDoc.CreateElement("Category");

            // Set attribute name and value!
            parentNode.SetAttribute("ID", "01");

            xmlDoc.DocumentElement.PrependChild(parentNode);

            // Create the required nodes
            XmlElement mainNode = xmlDoc.CreateElement("MainCategory");
            XmlElement descNode = xmlDoc.CreateElement("Description");
            XmlElement activeNode = xmlDoc.CreateElement("Active");

            // retrieve the text 
            XmlText categoryText = xmlDoc.CreateTextNode("XML");
            XmlText descText = xmlDoc.CreateTextNode("This is a list my XML articles.");
            XmlText activeText = xmlDoc.CreateTextNode("true");

            // append the nodes to the parentNode without the value
            parentNode.AppendChild(mainNode);
            parentNode.AppendChild(descNode);
            parentNode.AppendChild(activeNode);

            // save the value of the fields into the nodes
            mainNode.AppendChild(categoryText);
            descNode.AppendChild(descText);
            activeNode.AppendChild(activeText);

            // Save to the XML file
            xmlDoc.Save(Server.MapPath("categories.xml"));

            Response.Write("XML file created");
            */
        }

        [WebMethod(Description = "TikleMainTopList")]
        public string GlossaryMainTopList2()
        {
            /*
            <div>
              <ul class='board bd01'>
                <li class='clfix'>
                  <span class='subject'>
                    <a href='http://inputsupex.sktelecom.com/Supex/Contents/Supex/SupexList.aspx?SupexType=I&SupexMode=V&SupexID=2852' target='inpspx'>
                      <span class='trim_width'>
                        <nobr>[Idea][F&U신용정보] 일반해지 채권관리 프로세스 변경</nobr>
                      </span>
                    </a>
                  </span>
                  <p class='list_right'>
                    <span class='date'>08-22</span>
                  </p>
                </li>
              </ul>
            </div>
            */

            GlossaryInterfaceBiz biz = new GlossaryInterfaceBiz();

            DataSet ds = biz.TnetInterface_GlossaryMainTopList();
            string sContent = string.Empty;
            string sLi = string.Empty;
            string sContentTemp = @"
<div>
  <ul class='board bd01'>
{0}
  </ul>
</div>";
            string sLiTemp = @"
<li class='clfix'>
    <span class='subject'>
    <a href='{0}' target='inpspx'>
        <span class='trim_width'>
        <nobr>{1}</nobr>
        </span>
    </a>
    </span>
    <p class='list_right'>
    <span class='date'>{2}</span>
    </p>
</li>";

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string Tikle_LinkUrl = System.Configuration.ConfigurationManager.AppSettings["BaseURL"].ToString() + "Glossary/GlossaryView.aspx?ItemID=" + dr["CommonID"].ToString();
                string Tikle_Title = dr["Title"].ToString();
                string Tikle_CreateDate = dr["REGDATE"].ToString();

                sLi += string.Format(sLiTemp, Tikle_LinkUrl, Tikle_Title, Tikle_CreateDate);
            }

            sContent = string.Format(sContentTemp, sLi);

            return sContent;
        }


        [WebMethod(Description = "TikleMainTopList")]
        public XmlDocument GlossaryMainTopList3()
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

            // Create the root element
            XmlElement rootNode = xmlDoc.CreateElement("ReturnValue");
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode);


            // Create a new <Category> element and add it to the root node
            XmlElement rowsNode = xmlDoc.CreateElement("ROWS");
            xmlDoc.DocumentElement.PrependChild(rowsNode);

            GlossaryInterfaceBiz biz = new GlossaryInterfaceBiz();
            DataSet ds = biz.TnetInterface_GlossaryMainTopList3();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string Tikle_LinkUrl = System.Configuration.ConfigurationManager.AppSettings["BaseURL"].ToString() + "Glossary/GlossaryView.aspx?ItemID=" + dr["CommonID"].ToString();
                if (dr["Gubun"].ToString() == "QA") {
                    Tikle_LinkUrl = System.Configuration.ConfigurationManager.AppSettings["BaseURL"].ToString() + "QnA/QnAView.aspx?ItemID=" + dr["CommonID"].ToString();
                }
                
                string Tikle_Title = dr["Title"].ToString();
                string Tikle_CreateDate = dr["REGDATE"].ToString();
                string Tikle_RegAuthor = dr["UserName"].ToString();

                XmlElement rowNode = xmlDoc.CreateElement("ROW");
                XmlElement rowTitleNode = xmlDoc.CreateElement("Title");
                rowTitleNode.InnerText = Tikle_Title;
                XmlElement rowLinkUrlNode = xmlDoc.CreateElement("LinkUrl");
                rowLinkUrlNode.InnerText = Tikle_LinkUrl;
                XmlElement rowRegDateNode = xmlDoc.CreateElement("RegDate");
                rowRegDateNode.InnerText = Tikle_CreateDate;
                XmlElement rowRegAuthorNode = xmlDoc.CreateElement("RegAuthor");
                rowRegAuthorNode.InnerText = Tikle_RegAuthor;
                rowNode.AppendChild(rowTitleNode);
                rowNode.AppendChild(rowLinkUrlNode);
                rowNode.AppendChild(rowRegDateNode);
                rowNode.AppendChild(rowRegAuthorNode);
                rowsNode.AppendChild(rowNode);
            }

            return xmlDoc;
        }

        /// <summary>
        /// Mr.No
        /// 2015-07-16
        /// [PHOTOURL] 추가
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "TikleMainTopList")]
        public XmlDocument GlossaryMainTopList4()
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

            // Create the root element
            XmlElement rootNode = xmlDoc.CreateElement("ReturnValue");
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode);


            // Create a new <Category> element and add it to the root node
            XmlElement rowsNode = xmlDoc.CreateElement("ROWS");
            xmlDoc.DocumentElement.PrependChild(rowsNode);

            GlossaryInterfaceBiz biz = new GlossaryInterfaceBiz();
            DataSet ds = biz.TnetInterface_GlossaryMainTopList4();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string Tikle_LinkUrl = System.Configuration.ConfigurationManager.AppSettings["BaseURL"].ToString() + "Glossary/GlossaryView.aspx?ItemID=" + dr["CommonID"].ToString();
                if (dr["Gubun"].ToString() == "QA")
                {
                    Tikle_LinkUrl = System.Configuration.ConfigurationManager.AppSettings["BaseURL"].ToString() + "QnA/QnAView.aspx?ItemID=" + dr["CommonID"].ToString();
                }

                string Tikle_Title = dr["Title"].ToString();
                string Tikle_CreateDate = dr["REGDATE"].ToString();
                string Tikle_RegAuthor = dr["UserName"].ToString();
                // Mr.No 2015-07-16
                string Tikle_PHOTOURL = (dr["PHOTOURL"] == DBNull.Value) ? string.Empty : dr.Field<string>("PHOTOURL"); 
                string Tikle_Contents = (dr["Contents"] == DBNull.Value) ? string.Empty : dr.Field<string>("Contents");
                string Tikle_Domain = (dr["domain"] == DBNull.Value) ? string.Empty : dr.Field<string>("domain");

                XmlElement rowNode = xmlDoc.CreateElement("ROW");
                XmlElement rowTitleNode = xmlDoc.CreateElement("Title");
                rowTitleNode.InnerText = Tikle_Title;
                XmlElement rowLinkUrlNode = xmlDoc.CreateElement("LinkUrl");
                rowLinkUrlNode.InnerText = Tikle_LinkUrl;
                XmlElement rowRegDateNode = xmlDoc.CreateElement("RegDate");
                rowRegDateNode.InnerText = Tikle_CreateDate;
                XmlElement rowRegAuthorNode = xmlDoc.CreateElement("RegAuthor");
                rowRegAuthorNode.InnerText = Tikle_RegAuthor;

                // Mr.No 2015-07-16
                XmlElement rowPHOTOURLAuthorNode = xmlDoc.CreateElement("PHOTOURL");
                rowPHOTOURLAuthorNode.InnerText = Tikle_PHOTOURL;
                XmlElement rowContentsAuthorNode = xmlDoc.CreateElement("CONTENTS");
                //rowContentsAuthorNode.InnerText = Tikle_Contents;
                XmlElement rowDomainAuthorNode = xmlDoc.CreateElement("DOMAIN");
                rowDomainAuthorNode.InnerText = Tikle_Domain;

                // Mr.Kim 2015-07-29
                //XmlCDataSection CData;
                //XmlNode CData = xmlDoc.CreateCDataSection(Tikle_Contents.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").Replace("&apos;", "'").Replace("&quot;", "\"").Replace("&nbsp;", " "));
                //Tikle_Contents = System.Text.RegularExpressions.Regex.Replace(Tikle_Contents, @"<[^>]+>|&nbsp;", "");
                Tikle_Contents = Tikle_Contents.Replace("&amp;nbsp;", " ").Replace("&amp;gt;", ">");
                
                                                         
                XmlNode CData = xmlDoc.CreateCDataSection(HttpContext.Current.Server.HtmlDecode(Tikle_Contents));
                rowContentsAuthorNode.AppendChild(CData);
                
                // Mr.No 2015-07-16 
                // 기존 로직에서  순서 변경 T끌의 최신글 목록 (제목, 작성자, 시간, 내용, 도메인(그림url))
                rowNode.AppendChild(rowTitleNode);      // 제목
                rowNode.AppendChild(rowRegAuthorNode);  // 작성자
                rowNode.AppendChild(rowRegDateNode);    // 시간
                // Mr.No 2015-07-16 (New Add)
                rowNode.AppendChild(rowContentsAuthorNode); // 내용
                //rowNode.AppendChild(CData); // 내용
                rowNode.AppendChild(rowPHOTOURLAuthorNode); // 그림 url

                rowNode.AppendChild(rowLinkUrlNode);    // 도메인

                rowNode.AppendChild(rowDomainAuthorNode);    // 도메인

                /*
                Author : 개발자-김성환D, 리뷰자-이정선G
                Create Date : 2016.03.02
                Desc : 
                */
                //if (!(dr["Gubun"].ToString() == "QA" && dr["CommonID"].ToString() == "0"))
                //{
                    rowsNode.AppendChild(rowNode);
                //}
            }
            return xmlDoc;
        }

        /// <summary>
        /// 티넷 포틀릿 - DT블로그
        /// </summary>
        /// <returns>string</returns>
        [WebMethod]
        public GlossaryRow DTBlogList()
        {
            GlossaryRow rt = new GlossaryRow();

            DataSet ds = new DataSet();

            GlossaryMainBiz biz = new GlossaryMainBiz();
            ds = biz.GlossaryInterfaceTnet();

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Row row = new Row();
                    row.CommonID = dr["CommonID"].ToString();
                    row.Title = dr["Title"].ToString();
                    row.Title = HttpUtility.HtmlDecode(row.Title).Replace("&#39;", "'");

                    string dateTime = Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
                    row.CreateDate = dateTime;

                    row.Summary = StripHTMLTags(dr["Summary"].ToString(), false).Replace("&nbsp;", " ");
                    row.LastUserName = dr["UserName"].ToString();

                    DataSet attachList = AttachmentHelper.SelectAttach(Convert.ToInt32(dr["CommonID"].ToString()), 0, 314);
                    if (attachList != null)
                    {
                        if(attachList.Tables[0].Rows.Count > 0)
                            row.AttachYN = "Y";
                        else
                            row.AttachYN = "N";
                    }
                    else
                    {
                        row.AttachYN = "N";
                    }
                        
                    string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
                    string strLink = BaseURL + "Glossary/GlossaryView.aspx?ItemID=" + row.CommonID + "&Wtype=" + dr["WType"].ToString();
                    row.LinkUrl = strLink;

                    rt.Rows.Add(row);
                }
            }
            return rt;
        }

        public string StripHTMLTags(string str)
        {
            str = str.Replace("</p>", "\r\n").Replace("</ul>", "\r\n").ToString();
            return System.Text.RegularExpressions.Regex.Replace(str, @"<(.|)*?>", String.Empty).Replace("&#63;", ""); ;
        }

        public string StripHTMLTags(string source, bool IsLineUse = true)
        {
            try
            {
                string result = source;

                result = Regex.Replace(result, "(<( )*head([^>])*>).*(<( )*(/)( )*head( )*>)", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                result = Regex.Replace(result, @"(<( )*script([^>])*>).*(<( )*(/)( )*script( )*>)", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                result = Regex.Replace(result, "(<( )*style([^>])*>).*(<( )*(/)( )*style( )*>)", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                //result = Regex.Replace(result, @"<( )*table([^>])*>.*</table>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                result = Regex.Replace(result, @"<( )*br( )*>", "#Line", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                result = Regex.Replace(result, @"(</p>)", "#Line", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                result = Regex.Replace(result, @"<[^>]*>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                result = Regex.Replace(result, @"</[^>]*>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                result = Regex.Replace(result, "(\r)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                result = Regex.Replace(result, "(\n)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                result = Regex.Replace(result, "(\t)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                if (IsLineUse == true)
                {
                    result = Regex.Replace(result, @"(#Line(&nbsp;)*)+", "<br/>");
                }
                else
                {
                    result = result.Replace("#Line", "");
                }

                // 아래 둘다 필요 Ascii 코드 변경
                result = Regex.Replace(result, @"( )+", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                result = Regex.Replace(result, @"( )+", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                //result = Regex.Replace(result, @"&nbsp;", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                return result;
            }
            catch
            {
                return source;
            }
        }
    }
}



[Serializable]
public class GlossaryRow
{
    public GlossaryRow()
    {
        Rows = new List<Row>();
    }
    public List<Row> Rows;
}

public class Row
{
    public string CommonID { get; set; }
    public string Title { get; set; }
    public string CreateDate { get; set; }
    public string LastUserName { get; set; }
    public string WType { get; set; }

    [XmlIgnore]
    public string Summary { get; set; }
    [XmlElement("Summary")]
    public System.Xml.XmlCDataSection SummaryCDATA
    {
        get
        {
            return new System.Xml.XmlDocument().CreateCDataSection(Summary);
        }
        set
        {
            Summary = value.Value;
        }
    }


    public string AttachYN { get; set; }
    public string LinkUrl { get; set; }
}

