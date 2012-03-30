using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kinesthesia.Model.MIDI
{
    class MidiPlayer
    {

        private MidiManager midMan = MidiManager.Instance;
        private int overallLength;

        public void PlayMidiFileInTXT(string path)
        {
            List<Track> trList =
                MidiTextParser.RetrieveTracksAndNotesList(path);
            
            CalculateOverallLength(trList);

            for (int i = 1; i < trList.Count(); ++i)
            {
                ScheduleTrack(trList[i]);
            }

            midMan.Clock.Start();
            Thread.Sleep(50);
            //midMan.Clock.Stop();
        }

        private void ScheduleTrack(Track tr)
        {
            foreach (var rawNote in tr.Notes)
            {
                if (rawNote.IsNoteOnType)
                {
                    midMan.ScheduleNoteOnMessage(rawNote.Note, rawNote.Velocity, rawNote.Time/1000);
                }
                else
                {
                    midMan.ScheduleNoteOffMessage(rawNote.Note, rawNote.Time/1000);
                }
            }
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
