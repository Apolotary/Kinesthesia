using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kinesthesia.UI_Controllers
{
    /// <summary>
    /// Interaction logic for NoteRectangleControl.xaml
    /// </summary>
    public partial class NoteRectangleControl : UserControl
    {
        public event EventHandler RectangleTriggered;
        public event EventHandler RectangleUntriggered;

        private double alignFactorTop = 1.0;
        private double alignFactorLeft = 1.0;
        private string note = "C#";
        private int octave = 7;
        private int velocity = 83;
        private bool isSendingSignal = false;
        private Rect innerRect = new Rect(0, 0, 200, 200);
        private Brush fillBrush = Brushes.DarkRed;
        private Brush borderBrush = Brushes.Green;
       
        public double AlignFactorTop { get; set; }
        public double AlignFactorLeft { get; set; }
        public Rectangle NoteRectangle { get; set; }
        public string Note { get; set; }
        public int Octave { get; set; }
        public int Velocity { get; set; }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public NoteRectangleControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// since this element is added via interface builder
        /// it's better to use a separate method for passing the note data
        /// </summary>
        /// <param name="nt">note string</param>
        /// <param name="oct">octave</param>
        /// <param name="vel">velocity</param>
        /// <param name="alt">alignment factor top</param>
        /// <param name="all">alignment factor left</param>
        /// <param name="fill">fill color for rect</param>
        /// <param name="border">border color for rect</param>
        public void InsertNoteData(string nt, int oct, int vel, double alt, double all, Brush fill, Brush border)
        {
            note = nt;
            octave = oct;
            velocity = vel;
            alignFactorTop = alt;
            alignFactorLeft = all;
            fillBrush = fill;
            borderBrush = border;
            UpdateNote();
            UpdateRect();
            UpdateNoteRectangle();
        }

        /// <summary>
        /// update the color for note rectangle
        /// </summary>
        public void UpdateNoteRectangle()
        {
            noteRectangle.Stroke = borderBrush;
        }

        /// <summary>
        /// filling the rectangle
        /// </summary>
        public void FillRectangle()
        {
            noteRectangle.Fill = fillBrush;
        }

        ///
        /// 
        public void UnFillRectangle()
        {
            noteRectangle.Fill = Brushes.Transparent;
        }

        /// <summary>
        /// update data for intersection rect
        /// </summary>
        public void UpdateRect()
        {
            innerRect = new Rect(this.Margin.Left*alignFactorLeft, this.Margin.Top*alignFactorTop, noteRectangle.Width, noteRectangle.Height);
        }

        /// <summary>
        /// update note label
        /// </summary>
        public void UpdateNote()
        {
            noteLabel.Content = note + octave;
        }

        /// <summary>
        /// checking if the current point intersects with rectangle
        /// if the signal hasn't been triggered yet, we call the event handler
        /// if the signal was already triggered and there's no intersection
        /// then we call RectangleUntriggered event handler
        /// </summary>
        /// <param name="givenPoint">point with which we should check the intersection</param>
        public void CheckIntersectionWithPoints (Point givenPoint)
        {
            if (innerRect.Contains(givenPoint))
            {
                if (!isSendingSignal)
                {
                    NoteEventArgs ntargs = new NoteEventArgs(note, octave, velocity);
                    RectangleTriggered(this, ntargs);
                }
            }
            else
            {
                if (isSendingSignal)
                {
                    NoteEventArgs ntargs = new NoteEventArgs(note, octave, velocity);
                    RectangleUntriggered(this, ntargs);
                }
            }
        }

        /// <summary>
        /// simply switch the signal state
        /// </summary>
        public void SwitchSignal()
        {
            if (isSendingSignal)
            {
                isSendingSignal = false;
            }
            else
            {
                isSendingSignal = true;
            }
        }

        /// <summary>
        /// returning current signal state
        /// </summary>
        /// <returns></returns>
        public bool IsSendingSignal()
        {
            return isSendingSignal;
        }
    }
}
