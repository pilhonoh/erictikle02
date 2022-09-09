using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    /// <summary>
    /// 설명: Data entity class for tb_Permissions table.
    /// 작성일 : 2014-05-13
    /// 작성자 : miksystem.com
    /// </summary>
    public class PermissionsType
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PermissionsType class.
        /// </summary>
        public PermissionsType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PermissionsType class.
        /// </summary>
        public PermissionsType(int glossaryID, string toUserID)
        {
            this.GlossaryID = glossaryID;
            this.ToUserID = toUserID;
        }

        /// <summary>
        /// Initializes a new instance of the PermissionsType class.
        /// </summary>
        public PermissionsType(long iD, int glossaryID, string toUserID)
        {
            this.ID = iD;
            this.GlossaryID = glossaryID;
            this.ToUserID = toUserID;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the ID value.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Gets or sets the GlossaryID value.
        /// </summary>
        public int GlossaryID { get; set; }

        /// <summary>
        /// Gets or sets the ToUserID value.
        /// </summary>
        public string ToUserID { get; set; }

        /// <summary>
        /// 2014-05-15 Mr.No
        /// </summary>
        public string ToUserName { get; set; }

        public string ToUserType { get; set; }

        #endregion
    }
}
