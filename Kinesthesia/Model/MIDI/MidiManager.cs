using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace Kinesthesia.Model.MIDI
{
    /// <summary>
    /// This class is managing everything that is related to MIDI-messages
    /// It is implemented as a singleton to avoid multiple message sending
    /// </summary>
    class MidiManager
    {
        private static MidiManager instance;

        // ToDO: write configuration method for chosen device
        /// <summary>
        /// usually the default midi-device under 0 is microsoft's wavetable synth
        /// </summary>
        private OutputDevice _outputDevice = OutputDevice.InstalledDevices[1];
        
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// returning an instance of MidiManager
        /// </summary>
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

        public List<string> GetTheListOfMidiDevices()
        {
            List<string> devices = new List<string>();

            foreach (OutputDevice device in OutputDevice.InstalledDevices)
            {
                devices.Add(device.Name);
            }

            return devices;
        }

        public void ChangeMidiDevice(string deviceName)
        {
            if (_outputDevice.IsOpen) _outputDevice.Close();
            for (int i = 0; i < OutputDevice.InstalledDevices.Count(); ++i)
            {
                OutputDevice device = OutputDevice.InstalledDevices[i];
                if (deviceName == device.Name)
                {
                    _outputDevice = device;
                }
            }
            _outputDevice.Open();
        }

        public string CurrentOutputDeviceName()
        {
            return _outputDevice.Name;
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
        
        /// <summary>
        /// send raw note on message
        /// </summary>
        /// <param name="pitch"></param>
        /// <param name="velocity"></param>
        /// <param name="time"></param>
        public void SendNoteOnMessage(int pitch, int velocity, float time)
        {
            NoteOnMessage noteOnMsg = new NoteOnMessage(_outputDevice, Channel.Channel1, ConvertToPitch(pitch), velocity, time);
            noteOnMsg.SendNow();
        }

        /// <summary>
        /// schedule raw note on message
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

        /// <summary>
        /// send raw note off message
        /// </summary>
        /// <param name="pitch"></param>
        /// <param name="time"></param>
        public void SendNoteOffMessage(int pitch, float time)
        {
            NoteOffMessage noteOffMessage = new NoteOffMessage(_outputDevice, Channel.Channel1, ConvertToPitch(pitch), 0, time);
            noteOffMessage.SendNow();
        }

        /// <summary>
        /// schedule raw note off message
        /// </summary>
        /// <param name="pitch"></param>
        /// <param name="time"></param>
        public void ScheduleNoteOffMessage(int pitch, float time)
        {
            NoteOffMessage noteOffMessage = new NoteOffMessage(_outputDevice, Channel.Channel1, ConvertToPitch(pitch), 0, time);
            _clock.Schedule(noteOffMessage);
        }

        /// <summary>
        /// schedule any callback message 
        /// </summary>
        /// <param name="message">message which inherits from the Message class</param>
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

        /// <summary>
        /// Retrieving parameter from enum structure by number
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="number"></param>
        /// <returns></returns>
        public T NumToEnum<T>(int number)
        {
            return (T)Enum.ToObject(typeof(T), number);
        }
    }
}
