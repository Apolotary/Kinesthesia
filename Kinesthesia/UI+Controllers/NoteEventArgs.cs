using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinesthesia.UI_Controllers
{
    class NoteEventArgs : EventArgs
    {
        public NoteEventArgs(string not, int octav, int velocit)
        {
            note = not;
            octave = octav;
            velocity = velocit;
        }

        public string note;
        public int octave;
        public int velocity;
    }
}
