using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKT.Mobile.EwsWrapper
{
    //재조합 될 Appointment Class 
    [Serializable]
    public class MonthAppointment
    {
        public DateTime date = DateTime.Now;
        public bool grayday = false;
        public string week = string.Empty;
        public List<AppointmentItem> appointmentMonthList = new List<AppointmentItem>();
    }
}
