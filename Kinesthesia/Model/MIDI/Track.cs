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
        private List<RawTempo> _tempos;
        private int _trackNumber;
        private int _overallLength;
        private string _trackName;

        public List<RawNote> Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }
        public List<RawTempo> Tempos
        {
            get { return _tempos; }
            set { _tempos = value; }
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
        public string TrackName
        {
            get { return _trackName; }
            set { _trackName = value; }
        }

        public Track()
        {
            _notes = new List<RawNote>();
            _tempos = new List<RawTempo>();
            _trackName = " \"No Title\"";
            _trackNumber = 0;
        }

        public Track(List<RawNote> ns, List<RawTempo> tps, int cn, string name)
        {
            _notes = ns;
            _tempos = tps;
            _trackNumber = cn;
            _trackName = name;
        }

        public void AddNote(RawNote nt)
        {
            _notes.Add(nt);
        }

        public void AddTempo(RawTempo tp)
        {
            _tempos.Add(tp);
        }
    }
}
