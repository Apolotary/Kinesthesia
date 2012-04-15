using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kinesthesia.Model.MIDI
{
    /// <summary>
    /// class that plays MIDI files
    /// </summary>
    class MidiPlayer
    {
        /// <summary>
        /// midi manager instance
        /// </summary>
        private MidiManager midMan = MidiManager.Instance;
        
        /// <summary>
        /// retrieve and parse MIDI CSV file at the following path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<Track> ParseMIDIFileInCSV(string path)
        {
            midMan.Clock.Reset();
            List<Track> trList = MidiTextParser.RetrieveTracksAndNotesList(path);
            
            return trList;
        }

        /// <summary>
        /// play scheduled file
        /// </summary>
        public void PlayParsedFile()
        {
            midMan.Clock.Start();
            Thread.Sleep(50);
        }

    }
}
