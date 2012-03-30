using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinesthesia.Model.MIDI
{
    /// <summary>
    /// class describing raw tempo values for the first track
    /// </summary>
    class RawTempo
    {
        private int _time;
        private int _value;

        public int Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public RawTempo()
        {
            _time = 0;
            _value = 0;
        }

        public RawTempo(int t, int val)
        {
            _time = t;
            _value = val;
        }
    }
}
