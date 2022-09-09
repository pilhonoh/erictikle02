using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Mobile.EwsWrapper
{
    /// <summary>
    /// Author : 이충렬
    /// CreateDate : 2013.08.28
    /// Desc : Mobile Web 메일의 시그니쳐 구조 Class
    /// </summary>
    [Serializable]
    public class SignatureItem
    {
        public SignatureItem()
        {
            AutoAddSignature = false;
            SignatureHtml = string.Empty;
            SignatureText = string.Empty;
        }

        public bool AutoAddSignature { set; get; }
        public string SignatureHtml { set; get; }
        public string SignatureText { set; get; }
    }
}
