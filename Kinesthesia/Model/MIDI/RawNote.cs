using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinesthesia.Model.MIDI
{
    /// <summary>
    /// a simple class that represents raw midi note from midi file
    /// </summary>
    class RawNote
    {
        private int _note;
        private int _velocity;
        private float _time;
        private bool _isNoteOnType;

        public int Note
        {
            get { return _note; }
            set { _note = value; }
        }
        public int Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        public float Time
        {
            get { return _time; }
            set { _time = value; }
        }
        public bool IsNoteOnType
        {
            get { return _isNoteOnType; }
            set { _isNoteOnType = value; }
        }

        public RawNote()
        {
            _note = 60;
            _velocity = 0;
            _time = 0;
            _isNoteOnType = true;
        }

        public RawNote(int n, int v, float t, bool isNoteOn)
        {
            _note = n;
            _velocity = v;
            _time = t;
            _isNoteOnType = isNoteOn;
        }

    }
}
