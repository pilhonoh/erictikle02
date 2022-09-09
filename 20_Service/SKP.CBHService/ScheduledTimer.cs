using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SKT.CBHService
{
    public delegate void stCallBackDelegate();

    public class ScheduledTimer
    {
        private Timer _timer;
        private int _IntervalHour;
        private int _IntervalSecond;

        public ScheduledTimer()
        {
            _IntervalHour = 24;
            _IntervalSecond = 0;

        }
        public ScheduledTimer(int IntervalHour, int IntervalSecond)
        {
            _IntervalHour = IntervalHour;
            _IntervalSecond = IntervalSecond;
        }

        public TimeSpan GetDueTime(TimeSpan A, TimeSpan B)
        {
            if (A < B)
            {
                return B.Subtract(A);
            }
            else
            {
                TimeSpan target = B.Add(new TimeSpan(_IntervalHour, 0, _IntervalSecond));
                return target.Subtract(A);
                //return new TimeSpan(24, 0, 0).Subtract(B.Subtract(A)); //24시간뒤에서 값을 설정.
            }
        }
        public void SetTime(TimeSpan _time, stCallBackDelegate callback)
        {
            if (this._timer != null)
            {
                // Change 매서드 사용 가능.
                this._timer = null;
            }
            TimeSpan Now = DateTime.Now.TimeOfDay;
            TimeSpan DueTime = GetDueTime(Now, _time);


            this._timer = new Timer(new TimerCallback(delegate(object _callback)
            {
                ((stCallBackDelegate)_callback)();
            }), callback, DueTime, new TimeSpan(_IntervalHour, 0, _IntervalSecond));  //최초호출후 _IntervalHour시간마다 호출한다.  24시간.. 초..

        }


        public void StopTimer()
        {
            _timer.Dispose();
            
        }
    }
}
