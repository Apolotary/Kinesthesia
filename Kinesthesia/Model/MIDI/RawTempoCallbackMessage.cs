using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace Kinesthesia.Model.MIDI
{
    class RawTempoCallbackMessage : Message
    {
        private RawTempo _tempo;

        public RawTempo Tempo
        {
            get { return _tempo; }
            set { _tempo = value; }
        }

        /// <summary>
        /// Delegate called when a CallbackMessage is sent.
        /// </summary>
        /// <param name="time">The time at which this event was scheduled.</param>
        /// <returns>Additional messages which should be scheduled as a result of this callback,
        /// or null.</returns>
        public delegate void CallbackType(float time, RawTempo tempo);

        /// <summary>
        /// The callback to invoke when this message is "sent".
        /// </summary>
        public CallbackType Callback { get { return _callback; } }
        private CallbackType _callback;

        public RawTempoCallbackMessage(CallbackType callbackType, float t, RawTempo tempo)
            : base(t)
        {
            _tempo = tempo;
            _callback = callbackType;
        }
       

        /// <summary>
        /// Sends this message immediately, ignoring the beatTime.
        /// </summary>
        public override void SendNow()
        {
            _callback(Time, _tempo);
        }

        /// <summary>
        /// Returns a copy of this message, shifted in time by the specified amount.
        /// </summary>
        public override Message MakeTimeShiftedCopy(float delta)
        {
            return new RawTempoCallbackMessage(_callback, Time + delta, _tempo);
        }
    }
}
