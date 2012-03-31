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
        public List<Track> trList;

        public void ParseMIDIFileInCSV(string path)
        {
            trList = MidiTextParser.RetrieveTracksAndNotesList(path);
            
            CalculateOverallLength(trList);

            for (int i = 0; i < trList.Count(); ++i)
            {
                ScheduleTrack(trList[i]);
            }

        }

        public void PlayParsedFile()
        {
            midMan.Clock.Start();
            Thread.Sleep(50);
        }

        private void ScheduleTrack(Track tr)
        {
            foreach (var rawNote in tr.Notes)
            {
                if (rawNote.IsNoteOnType)
                {
                    midMan.ScheduleCallbackMessage(new RawNoteCallbackMessage(new RawNoteCallbackMessage.CallbackType(NoteOnCallbackHandler), rawNote.Time/1000, rawNote));
                }
                else
                {
                    midMan.ScheduleCallbackMessage(new RawNoteCallbackMessage(new RawNoteCallbackMessage.CallbackType(NoteOffCallbackHandler), rawNote.Time / 1000, rawNote));
                }
            }

            foreach (var rawTempo in tr.Tempos)
            {
                midMan.ScheduleCallbackMessage(new RawTempoCallbackMessage(new RawTempoCallbackMessage.CallbackType(BPMChangeCallBackHandler), rawTempo.Time, rawTempo));
            }
        }

        private void NoteOnCallbackHandler(float time, RawNote note)
        {
            midMan.SendNoteOnMessage(note.Note, note.Velocity, time);
        }

        private void NoteOffCallbackHandler(float time, RawNote note)
        {
            midMan.SendNoteOffMessage(note.Note, time);
        }

        private void BPMChangeCallBackHandler(float time, RawTempo tempo)
        {
            midMan.Clock.BeatsPerMinute = CalculateBPM(tempo.Value);
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

        private float CalculateBPM(int tempo)
        {
            return 60000000/tempo;
        }
    }
}
