using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kinesthesia.Model.MIDI
{
    class TrackPlayer
    {
        private MidiManager midMan = MidiManager.Instance;

        private Track _track;
        private int _velocity = 10;

        public Track Track
        {
            get { return _track; }
            set { _track = value; }
        }

        public int Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public TrackPlayer (Track tr)
        {
            _track = tr;
            ScheduleTrack(tr);
        }
        
        private void ScheduleTrack(Track tr)
        {
            foreach (var rawNote in tr.Notes)
            {
                if (rawNote.IsNoteOnType)
                {
                    midMan.ScheduleCallbackMessage(new RawNoteCallbackMessage(new RawNoteCallbackMessage.CallbackType(NoteOnCallbackHandler), rawNote.Time / 1000, rawNote));
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
            // note velocity
            //midMan.SendNoteOnMessage(note.Note, note.Velocity, time);
            
            // custom velocity
            midMan.SendNoteOnMessage(note.Note, _velocity, time);
        }

        private void NoteOffCallbackHandler(float time, RawNote note)
        {
            midMan.SendNoteOffMessage(note.Note, time);
        }

        private void BPMChangeCallBackHandler(float time, RawTempo tempo)
        {
            midMan.Clock.BeatsPerMinute = CalculateBPM(tempo.Value);
        }

        private float CalculateBPM(int tempo)
        {
            return 60000000 / tempo;
        }
    }
}
