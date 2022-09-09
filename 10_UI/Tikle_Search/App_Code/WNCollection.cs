using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace wisenut.common
{
    public class WNCollection
    {

        public Dictionary<String, String> COLLECTION_CONFIG = new Dictionary<String, String>();
        public Dictionary<String, Dictionary<String, String>> COLLECTION = new Dictionary<String, Dictionary<String, String>>();

        public WNCollection()
        {

            //tikle
            COLLECTION_CONFIG = new Dictionary<String, String>();
            //COLLECTION_CONFIG["SEARCH_FIELD"] = "sub_name,title,name,content";
            //COLLECTION_CONFIG["DOCUMENT_FIELD"] = "idx_key,menu_id,sub_name,title,name,content,DATE,file_name,file_path,file_cont,type,meta1,meta2,meta3,meta4";
            COLLECTION_CONFIG["SEARCH_FIELD"] = "Title,Content,TagTitle,FileName,file_content";
            COLLECTION_CONFIG["DOCUMENT_FIELD"] = "DOCID,BoardType,ID,CommonID,Title,Content,Hits,CommentsHits,UserID,UserName,DeptName,CreateDate,TagTitle,ModifyDate,DATE,FileName,Folder";
            COLLECTION["SKT_GW_GLOSSARY"] = COLLECTION_CONFIG;


        }

    }


}


/*
collection이름 : SKT_GW_GLOSSARY
search field  : Title,Content,TagTitle
document field : DOCID,BoardType,ID,CommonID,Title,Content,Hits,CommentsHits,UserID,UserName,DeptName,CreateDate,TagTitle,ModifyDate,DATE 
 * */

