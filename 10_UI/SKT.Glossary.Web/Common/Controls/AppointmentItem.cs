using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Exchange.WebServices.Data;

namespace SKT.Glossary.Web.Common.Controls
{
    [Serializable]
    public class AppointmentItem
    {
        public AppointmentItem()
        {
            Subject = string.Empty;
            Pr_Search_Key = string.Empty;
            Location = string.Empty;
            Recurrence = null;
            Start = DateTime.Now;
            End = DateTime.Now;
            IsAllDayEvent = false;
            RequiredAttendees = new List<RequiredAttendeesItem>();
            IsRecurring = false;
            IsMeeting = false;
            IsTotalRecurring = true;
        }

        public string Subject { set; get; } //제목
        public MessageBody Body { set; get; }    //Body
        public string Pr_Search_Key { set; get; }   //사용안함
        public bool HasAttachments { set; get; }    //사용안함
        public string Attachments { get; set; }     //사용안함
        public List<RequiredAttendeesItem> RequiredAttendees { get; set; }   //참석자
        public DateTime Start { get; set; } //시작
        public DateTime End { get; set; }   //종료
        public DateTime DateTimeSent { set; get; }  //사용안함
        public Recurrence Recurrence { set; get; }  //되풀이항목

        //public DateTime ReminderDueBy { set; get; } //사용안함

        public bool IsTotalRecurring { set; get; }
        public bool IsMeeting { set; get; }     //참여자여부
        public bool IsRecurring { set; get; }   //되풀이여부
        public string Location { set; get; }    //위치
        public string Id { set; get; }  //EWS 내부적으로 설정을 바꿀 때 사용하는 Id - 유저는 사용안함
        public string ICalUid { set; get; } //Uid
        public int Alarm { set; get; }  //알람(사용안함)
        public string Organizer { set; get; }   //소유자
        public string MyResponseType { set; get; }  //응답
        public bool IsCancelled { set; get; }   //취소
        public int ReminderMinutesBeforeStart { set; get; } //알람시간
        public bool IsReminderSet { set; get; } //알람 set
        public bool IsAllDayEvent { set; get; } //하루종일
        public int ConferenceType { set; get; } //중요 표시
    }

    [Serializable]
    public class Recurrence
    {
        public Recurrence()
        {
            StartDate = DateTime.Now;
            EndDate = null;
            Pattern = 0;
        }

        public DateTime StartDate { set; get; }   //반복 시작
        public DateTime? EndDate { set; get; }     //반복 종료
        public int Pattern { set; get; }     //반복 패턴
        public int? NumberOfOccurrences { set; get; }
        public bool HasEnd { set; get; }
    }

    [Serializable]
    public class RequiredAttendeesItem
    {
        public RequiredAttendeesItem()
        {
            Address = string.Empty;
            Id = string.Empty;
            MailboxType = string.Empty;
            Name = string.Empty;
            PhoneNumber = string.Empty;
            SIPID = string.Empty;
        }

        public string Address { set; get; }
        public string Id { set; get; }
        public string MailboxType { set; get; }
        public string Name { set; get; }
        public string PhoneNumber { set; get; }
        public string SIPID { set; get; }


    }
}