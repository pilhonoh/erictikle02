using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    /// <summary>
    /// 설명: Data entity class for tb_PermissionsTemp table.
    /// 작성일 : 2014-06-13
    /// 작성자 : miksystem.com
    /// </summary>
    public class PermissionsTempType
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PermissionsTempType class.
        /// </summary>
        public PermissionsTempType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PermissionsTempType class.
        /// </summary>
        public PermissionsTempType(long iD, int glossaryTempID, string toUserID, string toUserName)
        {
            this.ID = iD;
            this.GlossaryTempID = glossaryTempID;
            this.ToUserID = toUserID;
            this.ToUserName = toUserName;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the ID value.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Gets or sets the GlossaryTempID value.
        /// </summary>
        public int GlossaryTempID { get; set; }

        /// <summary>
        /// Gets or sets the ToUserID value.
        /// </summary>
        public string ToUserID { get; set; }

        /// <summary>
        /// Gets or sets the ToUserName value.
        /// </summary>
        public string ToUserName { get; set; }

        #endregion
    }
}
