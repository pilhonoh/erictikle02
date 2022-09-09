using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Mobile.EwsWrapper
{
    /// <summary>
    /// Author : 이충렬
    /// CreateDate : 2013.08.28
    /// Desc : Mobile Web 메일의 Folder 구조 Class
    /// </summary>
    [Serializable]
    public class FolderItem
    {

        public FolderItem()
        {
            DisplayName = string.Empty;
            UniqueId = string.Empty;
            ParentUniqueId = string.Empty;
            UnReadMailCount = 0;
            TotalMailCount = 0;
            ChildFolderCount = 0;
            FolderClass = string.Empty;
            WellKnownFolderName = string.Empty;
            IsBase = false;
        }

        public string DisplayName { set; get; }
        public string UniqueId { set; get; }
        public string ParentUniqueId { set; get; }
        public int UnReadMailCount { set; get; }
        public int TotalMailCount { set; get; }
        public int ChildFolderCount { set; get; }
        public string FolderClass { set; get; }
        public string WellKnownFolderName { set; get; }
        public bool IsBase { set; get; } 

    }
}
