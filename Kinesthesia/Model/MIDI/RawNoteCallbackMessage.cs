using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace Kinesthesia.Model.MIDI
{
    class RawNoteCallbackMessage : Message
    {
        private RawNote _note;

        public RawNote Note
        {
            get { return _note; }
            set { _note = value; }
        }

        /// <summary>
        /// Delegate called when a CallbackMessage is sent.
        /// </summary>
        /// <param name="time">The time at which this event was scheduled.</param>
        /// <returns>Additional messages which should be scheduled as a result of this callback,
        /// or null.</returns>
        public delegate void CallbackType(float time, RawNote note);

        /// <summary>
        /// The callback to invoke when this message is "sent".
        /// </summary>
        public CallbackType Callback { get { return _callback; } }
        private CallbackType _callback;

        public RawNoteCallbackMessage(CallbackType callbackType, float t, RawNote note) : base(t)
        {
            _note = note;
            _callback = callbackType;
        }
       

        /// <summary>
        /// Sends this message immediately, ignoring the beatTime.
        /// </summary>
        public override void SendNow()
        {
            _callback(Time, _note);
        }

        /// <summary>
        /// Returns a copy of this message, shifted in time by the specified amount.
        /// </summary>
        public override Message MakeTimeShiftedCopy(float delta)
        {
            return new RawNoteCallbackMessage(_callback, Time + delta, _note);
        }
    }
}
