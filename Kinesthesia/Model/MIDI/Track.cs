using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinesthesia.Model.MIDI
{
    /// <summary>
    /// simple class representing a track with list of notes
    /// </summary>
    class Track
    {
        private List<RawNote> _notes;
        private int _trackNumber;
        private int _overallLength;

        public List<RawNote> Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }
        public int TrackNumber
        {
            get { return _trackNumber; }
            set { _trackNumber = value; }
        }
        public int OverallLength
        {
            get { return _overallLength; }
            set { _overallLength = value; }
        }

        public Track()
        {
            _notes = new List<RawNote>();
            _trackNumber = 0;
        }

        public Track(List<RawNote> ns, int cn)
        {
            _notes = ns;
            _trackNumber = cn;
        }

        public void AddNote(RawNote nt)
        {
            _notes.Add(nt);
        }
    }
}
