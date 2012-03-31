using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kinesthesia.Model.MIDI
{
    class MidiPlayer
    {
        // ToDo: add support for multiple tracks
        private MidiManager midMan = MidiManager.Instance;
        private int overallLength;

        public List<Track> ParseMIDIFileInCSV(string path)
        {
            List<Track> trList = MidiTextParser.RetrieveTracksAndNotesList(path);
            
            CalculateOverallLength(trList);

            return trList;
        }

        public void PlayParsedFile()
        {
            midMan.Clock.Start();
            Thread.Sleep(50);
        }

        private void CalculateOverallLength(List<Track> trList)
        {
            overallLength = 0;
            foreach (var track in trList)
            {
                if (track.OverallLength > overallLength)
                {
                    overallLength = track.OverallLength;
                }
            }
        }
    }
}
