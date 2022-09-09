using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Exchange.WebServices.Data;

namespace SKT.Mobile.EwsWrapper
{
    public class TaskItem
    {
        public TaskItem()
        {
            Subject = string.Empty;
            Importance = 0;
            StartDate = null;
            DueDate = null;
            Nodeadline = true;
            Status = TaskStatus.InProgress;
            UniqueId = string.Empty;
            ChangeKey = string.Empty;
        }
        public string Subject { set; get; }
        public MessageBody Body { set; get; }
        public int Importance { set; get; }
        public DateTime? StartDate { set; get; }
        public DateTime? DueDate { set; get; }
        public bool Nodeadline { set; get; }
        public TaskStatus Status { set; get; }
        public string UniqueId { set; get; }
        public string ChangeKey { set; get; }
        //public DateTime AssignedTime { set; get; }
        //public List<TaskRepeatItem> Recurrence { set; get; }
        //public DateTime ReminderDueBy { set; get; }
    }

    /*
    public class TaskRepeatItem
    {
        public TaskRepeatItem()
        {
            StartRepeat = DateTime.Now;
            //EndRepeat = DateTime.Now;
            Patten = string.Empty;
        }

        public DateTime StartRepeat { set; get; }
        //public DateTime EndRepeat { set; get; }
        public string Patten { set; get; }
    }
     */
}
