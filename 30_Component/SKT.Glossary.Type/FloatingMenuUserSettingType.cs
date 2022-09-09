using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Glossary.Type
{
    /// <summary>
    /// 설명: Data entity class for tb_FloatingMenuUserSetting table.
    /// 작성일 : 2013-09-11
    /// 작성자 : miksystem.com
    /// </summary>
    public class FloatingMenuUserSettingType
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FloatingMenuUserSettingType class.
        /// </summary>
        public FloatingMenuUserSettingType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the FloatingMenuUserSettingType class.
        /// </summary>
        public FloatingMenuUserSettingType(string userID, string displayFloatingIconYN, string dragSearchActivateYN, string doubleClickSearchActivateYN, int positionX, int positionY, DateTime createdDT, DateTime updatedDT)
        {
            this.UserID = userID;
            this.DisplayFloatingIconYN = displayFloatingIconYN;
            this.DragSearchActivateYN = dragSearchActivateYN;
            this.DoubleClickSearchActivateYN = doubleClickSearchActivateYN;
            this.PositionX = positionX;
            this.PositionY = positionY;
            this.CreatedDT = createdDT;
            this.UpdatedDT = updatedDT;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the UserID value.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Gets or sets the DisplayFloatingIconYN value.
        /// </summary>
        public string DisplayFloatingIconYN { get; set; }

        /// <summary>
        /// Gets or sets the DragSearchActivateYN value.
        /// </summary>
        public string DragSearchActivateYN { get; set; }

        /// <summary>
        /// Gets or sets the DoubleClickSearchActivateYN value.
        /// </summary>
        public string DoubleClickSearchActivateYN { get; set; }


        public string TikleNoteYN { get; set; }
        /// <summary>
        /// Gets or sets the PositionX value.
        /// </summary>
        public int PositionX { get; set; }

        /// <summary>
        /// Gets or sets the PositionY value.
        /// </summary>
        public int PositionY { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDT value.
        /// </summary>
        public DateTime CreatedDT { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedDT value.
        /// </summary>
        public DateTime UpdatedDT { get; set; }

        #endregion
    }
}
