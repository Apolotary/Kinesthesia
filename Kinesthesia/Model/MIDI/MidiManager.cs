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
        // usually the default midi-device under 0 is microsoft's wavetable synth
        // ToDO: write configuration method for chosen device
        private OutputDevice _outputDevice = OutputDevice.InstalledDevices[1];

        /// <summary>
        /// default constructor
        /// </summary>
        public MidiManager()
        {
            _outputDevice.Open();
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
        
    }
}
