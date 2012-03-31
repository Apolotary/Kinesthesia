using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace Kinesthesia.Model.MIDI
{
    /// <summary>
    /// this class is managing the note sending from 
    /// interpreter class to MIDI-device
    /// </summary>
    class MidiManager
    {
        private static MidiManager instance;

        // usually the default midi-device under 0 is microsoft's wavetable synth
        // ToDO: write configuration method for chosen device
        private OutputDevice _outputDevice = OutputDevice.InstalledDevices[0];
        private Clock _clock = new Clock(200);
        
        public Clock Clock
        {
            get { return _clock; }
            set { _clock = value; }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public MidiManager()
        {
            _outputDevice.Open();
        }

        public static MidiManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MidiManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// sending note on message to MIDI port
        /// </summary>
        /// <param name="note">note written in western notation such as "C#", "B", etc</param>
        /// <param name="octave">number of the octave for this note</param>
        /// <param name="velocity">velocity with which this message was sent</param>
        public void SendNoteOnMessage(string note, int octave, int velocity)
        {
            int start = 0;
            Note theNote = Note.ParseNote(note, ref start);
            Pitch pt = theNote.PitchInOctave(octave);
            _outputDevice.SendNoteOn(Channel.Channel1, pt, velocity);
        }

        public void SendNoteOnMessage(int pitch, int velocity, float time)
        {
            NoteOnMessage noteOnMsg = new NoteOnMessage(_outputDevice, Channel.Channel1, ConvertToPitch(pitch), velocity, time);
            noteOnMsg.SendNow();
        }

        /// <summary>
        /// raw note on message
        /// </summary>
        /// <param name="pitch"></param>
        /// <param name="velocity"></param>
        /// <param name="time"></param>
        public void ScheduleNoteOnMessage(int pitch, int velocity, float time)
        {
            NoteOnMessage noteOnMsg = new NoteOnMessage(_outputDevice, Channel.Channel1, ConvertToPitch(pitch), velocity, time);
            _clock.Schedule(noteOnMsg);
        }
        
        /// <summary>
        /// sending note off message to MIDI port
        /// </summary>
        /// <param name="note">note written in western notation such as "C#", "B", etc</param>
        /// <param name="octave">number of the octave for this note</param>
        /// <param name="velocity">velocity with which this message was sent</param>
        public void SendNoteOffMessage(string note, int octave, int velocity)
        {
            int start = 0;
            Note theNote = Note.ParseNote(note, ref start);
            Pitch pt = theNote.PitchInOctave(octave);
            _outputDevice.SendNoteOff(Channel.Channel1, pt, velocity);
        }

        public void SendNoteOffMessage(int pitch, float time)
        {
            NoteOffMessage noteOffMessage = new NoteOffMessage(_outputDevice, Channel.Channel1, ConvertToPitch(pitch), 0, time);
            noteOffMessage.SendNow();
        }

        /// <summary>
        /// raw note off message
        /// </summary>
        /// <param name="pitch"></param>
        /// <param name="time"></param>
        public void ScheduleNoteOffMessage(int pitch, float time)
        {
            NoteOffMessage noteOffMessage = new NoteOffMessage(_outputDevice, Channel.Channel1, ConvertToPitch(pitch), 0, time);
            _clock.Schedule(noteOffMessage);
        }

        public void ScheduleCallbackMessage(Message message)
        {
            _clock.Schedule(message);
        }

        /// <summary>
        /// sending pitch bend message to MIDI-port
        /// </summary>
        /// <param name="value">the intensity of pitch bending</param>
        public void SendPitchBend(int value)
        {
            if (value > 16384) value = 16384;
            if (value < 0) value = 0;
            _outputDevice.SendPitchBend(Channel.Channel1, value);
        }

        /// <summary>
        /// setting volume
        /// </summary>
        /// <param name="value">volume value 0-127</param>
        public void SendVolumeChange(int value)
        {
            if (value > 140) value = 127;
            if (value < 0) value = 0;
            _outputDevice.SendControlChange(Channel.Channel1, Control.Volume, value);
        }
        
        /// <summary>
        /// converts note to pitch in range 0-127
        /// </summary>
        /// <param name="noteToConvert">note in range 0-127</param>
        /// <returns></returns>
        public Pitch ConvertToPitch(int noteToConvert)
        {
            if (noteToConvert > 127) noteToConvert = 127;
            return NumToEnum<Pitch>(noteToConvert);
        }

        public T NumToEnum<T>(int number)
        {
            return (T)Enum.ToObject(typeof(T), number);
        }
    }
}
