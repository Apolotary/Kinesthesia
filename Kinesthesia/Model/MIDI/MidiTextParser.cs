using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kinesthesia.Model.ConfigManager;

namespace Kinesthesia.Model.MIDI
{
    class MidiTextParser
    {
        /// <summary>
        /// parsing MIDI file into tracklist with notes
        /// </summary>
        /// <param name="path">midi text path</param>
        /// <returns></returns>
        public static List<Track> RetrieveTracksAndNotesList(string path)
        {
            List<string[]> parsedData = CSVParser.parseCSV(path);

            int lastTrackNum = Convert.ToInt32(parsedData[parsedData.Count() - 2][0]);
            Track[] tracklist = new Track[lastTrackNum];

            for (int i = 0; i < lastTrackNum; i ++)
            {
                tracklist[i] = new Track();
                tracklist[i].TrackNumber = i+1;
            }

            bool hasPickedTrack = false;
            int pickedTrack = 0;

            foreach (var stringse in parsedData)
            {
                int currTrack = Convert.ToInt32(stringse[0]);

                if (stringse.Contains("Start_track"))
                {
                    pickedTrack = currTrack;
                    hasPickedTrack = true;
                }

                if (hasPickedTrack && currTrack == pickedTrack)
                {
                    RawNote nt;
                    RawTempo tp;
                    if (stringse.Contains("Note_on_c"))
                    {
                        nt = new RawNote(Convert.ToInt32(stringse[4]), Convert.ToInt32(stringse[5]), (float)Convert.ToDouble(stringse[1]), true);
                        tracklist[currTrack-1].AddNote(nt);
                    }
                    else if (stringse.Contains("Note_off_c"))
                    {
                        nt = new RawNote(Convert.ToInt32(stringse[4]), Convert.ToInt32(stringse[5]), (float)Convert.ToDouble(stringse[1]), false);
                        tracklist[currTrack-1].AddNote(nt);
                    }
                    else if (stringse.Contains("Tempo"))
                    {
                        tp = new RawTempo((float)Convert.ToDouble(stringse[1]), Convert.ToInt32(stringse[3]));
                        tracklist[currTrack - 1].AddTempo(tp);
                    }
                    else if (stringse.Contains("End_track"))
                    {
                        tracklist[currTrack - 1].OverallLength = Convert.ToInt32(stringse[1]);
                    }
                    else if (stringse.Contains("Title_t"))
                    {
                        tracklist[currTrack - 1].TrackName = stringse[3];
                    }
                }
            }

            List<Track> resList = new List<Track>();

            foreach (var track in tracklist)
            {
                resList.Add(track);
            }
            
            return resList;
        }
    }
}
