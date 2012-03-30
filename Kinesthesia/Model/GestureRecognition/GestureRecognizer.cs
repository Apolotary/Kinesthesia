using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Windows.Media.Media3D;

namespace Kinesthesia.Model.GestureRecognition
{
    /// <summary>
    /// Simplified gesture recognition class
    /// No Hidden Markov Models, just tracking the change in 
    /// joints positions over time
    /// </summary>
    class GestureRecognizer
    {
        #region class variables

        /// <summary>
        /// threshold upon which gestures will be fired
        /// in this case this is the minimum distance 
        /// between points which can be reached
        /// </summary>
        private double _threshold;

        /// <summary>
        /// list for coordinates
        /// </summary>
        private List<SkeletonPoint> _coordinatesList;

        /// <summary>
        /// ToDo: remove later if I won't need it
        /// </summary>
        private TimeSpan _recordingTime;

        /// <summary>
        /// comparing coordinates in list based on this variable
        /// </summary>
        private int _framesToCompare;

        /// <summary>
        /// tracked joint
        /// </summary>
        private Joint _trackedJoint;

        /// <summary>
        /// Event handlers
        /// </summary>
        public event EventHandler XaxisIncreased;
        public event EventHandler XaxisDecreased;
        public event EventHandler YaxisIncreased;
        public event EventHandler YaxisDecreased;

        #endregion

        #region properties

        public double Threshold
        {
            get { return _threshold; }
            set { _threshold = value; }
        }
        public int FramesToCompare
        {
            get { return _framesToCompare; }
            set { _framesToCompare = value; }
        }
        public Joint TrackedJoint
        {
            get { return _trackedJoint; }
            set { _trackedJoint = value; }
        }

        #endregion

        #region constructors

        /// <summary>
        /// default constructor
        /// </summary>
        public GestureRecognizer()
        {
            _framesToCompare = 60;
            _threshold = 10;
            _coordinatesList = new List<SkeletonPoint>();
        }

        /// <summary>
        /// default constructor with joint
        /// </summary>
        /// <param name="jointToTrack">default joint</param>
        public GestureRecognizer(Joint jointToTrack)
        {
            _framesToCompare = 20;
            _threshold = 10;
            _trackedJoint = jointToTrack;
            _coordinatesList = new List<SkeletonPoint>();
        }

        /// <summary>
        /// constructor with frames and threshold
        /// </summary>
        /// <param name="frames">framesToCompare</param>
        /// <param name="thresh">threshold</param>
        /// <param name="jointToTrack">default joint</param>
        public GestureRecognizer(int frames, double thresh, Joint jointToTrack)
        {
            _framesToCompare = frames;
            _threshold = thresh;
            _trackedJoint = jointToTrack;
            _coordinatesList = new List<SkeletonPoint>();
        }

        #endregion

        #region methods

        /// <summary>
        /// inserting coordinates to the array
        /// </summary>
        /// <param name="point"></param>
        public void AddCoordinate (SkeletonPoint point)
        {
            if (_coordinatesList.Count() < _framesToCompare)
            {
                _coordinatesList.Add(point);
            }
            else
            {
                ComparePoints();
            }
        }

        /// <summary>
        /// comparing first and last points
        /// </summary>
        public void ComparePoints ()
        {
            SkeletonPoint firstPoint = _coordinatesList.FirstOrDefault();
            SkeletonPoint lastPoint = _coordinatesList.LastOrDefault();

            double xDiff = Math.Abs(lastPoint.X - firstPoint.X);
            double yDiff = Math.Abs(lastPoint.Y - firstPoint.Y);
            GestureEventArgs gargs = new GestureEventArgs(_trackedJoint, lastPoint);

            if (xDiff >= _threshold)
            {
                if (lastPoint.X < firstPoint.X)
                {
                    XaxisDecreased(this, gargs);
                }
                else if (lastPoint.X > firstPoint.X)
                {
                    XaxisIncreased(this, gargs);
                }
            }
            if (yDiff >= _threshold)
            {
                if (lastPoint.Y > firstPoint.Y)
                {
                    YaxisIncreased(this, gargs);
                }
                else if (lastPoint.Y < firstPoint.Y)
                {
                    YaxisDecreased(this, gargs);
                }
            }
            _coordinatesList.Clear();
        }

        /// <summary>
        /// returning current point number
        /// </summary>
        /// <returns>current point number</returns>
        public int currPointNumber()
        {
            return _coordinatesList.Count();
        }

        /// <summary>
        /// checking if the array is full
        /// </summary>
        /// <returns></returns>
        public bool canAddCoordinates()
        {
            if (_coordinatesList.Count() < _framesToCompare + 1)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
