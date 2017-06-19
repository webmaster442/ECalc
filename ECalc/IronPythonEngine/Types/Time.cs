using System;

namespace ECalc.IronPythonEngine.Types
{
    public class Time: IComparable<Time>, IEquatable<Time>
    {
        private TimeSpan _time;

        public Time()
        {
            _time = TimeSpan.FromSeconds(0);
        }

        public Time(double seconds)
        {
            _time = TimeSpan.FromSeconds(seconds);
        }

        public Time(double minutes, double seconds) :
            this(minutes * 60 + seconds)
        { }

        public Time(double hours, double minutes, double seconds) :
            this(hours * 60 + minutes, seconds)
        { }

        public Time(double days, double hours, double minutes, double seconds) :
            this(days * 24 + hours, minutes, seconds)

        { }

        public override int GetHashCode()
        {
            return _time.GetHashCode();
        }

        public override string ToString()
        {
            if (_time.Days > 0)
                return string.Format("{0} days, {1:00}:{2:00}:{3:00}", _time.Days, _time.Hours, _time.Minutes, _time.Seconds);
            else
                return string.Format("{0:00}:{1:00}:{2:00}", _time.Hours, _time.Minutes, _time.Seconds);
        }

        public override bool Equals(object obj)
        {
            Time other = obj as Time;
            if (other == null) return false;
            return Equals(other);
        }

        public int CompareTo(Time other)
        {
            return _time.CompareTo(other._time);
        }

        public bool Equals(Time other)
        {
            return (other._time == _time);
        }

        public static Time operator + (Time a, Time b)
        {
            var tmp = a._time.TotalSeconds + b._time.TotalSeconds;
            return new Time(tmp);
        }

        public static Time operator + (Time a, double b)
        {
            var tmp = a._time.TotalSeconds + b;
            return new Time(tmp);
        }

        public static Time operator - (Time a, Time b)
        {
            var tmp = a._time.TotalSeconds - b._time.TotalSeconds;
            return new Time(tmp);
        }

        public static Time operator -(Time a, double b)
        {
            var tmp = a._time.TotalSeconds - b;
            return new Time(tmp);
        }

        public static Time operator * (Time a, Time b)
        {
            var tmp = a._time.TotalSeconds * b._time.TotalSeconds;
            return new Time(tmp);
        }

        public static Time operator *(Time a, double b)
        {
            var tmp = a._time.TotalSeconds * b;
            return new Time(tmp);
        }

        public static Time operator / (Time a, Time b)
        {
            var tmp = a._time.TotalSeconds / b._time.TotalSeconds;
            return new Time(tmp);
        }

        public static Time operator /(Time a, double b)
        {
            var tmp = a._time.TotalSeconds / b;
            return new Time(tmp);
        }

        public static implicit operator double (Time t)
        {
            return t._time.TotalSeconds;
        }
    }
}
