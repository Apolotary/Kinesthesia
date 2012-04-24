using System;
using System.Windows;
using Microsoft.Kinect;

namespace Kinect.Toolbox
{
    public class SwipeGestureDetector : GestureDetector
    {
        // private fields
        private double _swipeMinimalLength;
        private double _swipeMaximalLength;
        private double _swipeMinimalHeight;
        private double _swipeMaximalHeight;
        private int _swipeMinimalDuration;
        private int _swipeMaximalDuration;
        private Vector3 vectorToReturn;

        // properties
        public double SwipeMinimalLength
        {
            get { return _swipeMinimalLength; }
            set { _swipeMinimalLength = value; }
        }
        public double SwipeMaximalLength
        {
            get { return _swipeMaximalLength; }
            set { _swipeMaximalLength = value; }
        }
        public double SwipeMinimalHeight
        {
            get { return _swipeMinimalHeight; }
            set { _swipeMinimalHeight = value; }
        }
        public double SwipeMaximalHeight
        {
            get { return _swipeMaximalHeight; }
            set { _swipeMaximalHeight = value; }
        }
        public int SwipeMinimalDuration
        {
            get { return _swipeMinimalDuration; }
            set { _swipeMinimalDuration = value; }
        }
        public int SwipeMaximalDuration
        {
            get { return _swipeMaximalDuration; }
            set { _swipeMaximalDuration = value; }
        }

        public SwipeGestureDetector(int windowSize = 20)
            : base(windowSize)
        {
            
        }

        bool ScanPositions(Func<Vector3, Vector3, bool> heightFunction, Func<Vector3, Vector3, bool> directionFunction, Func<Vector3, Vector3, bool> lengthFunction, int minTime, int maxTime)
        {
            int start = 0;

            for (int index = 1; index < Entries.Count - 1; index++)
            {
                if (!heightFunction(Entries[0].Position, Entries[index].Position) || !directionFunction(Entries[index].Position, Entries[index + 1].Position))
                {
                    start = index;
                }

                if (lengthFunction(Entries[index].Position, Entries[start].Position))
                {
                    double totalMilliseconds = (Entries[index].Time - Entries[start].Time).TotalMilliseconds;
                    if (totalMilliseconds >= minTime && totalMilliseconds <= maxTime)
                    {
                        return true;
                    }
                }
                vectorToReturn = Entries[index].Position;
            }

            return false;
        }

        protected override void LookForGesture()
        {
            Point pointToReturn = new Point(vectorToReturn.X, vectorToReturn.Y);
            if (ScanPositions((p1, p2) => Math.Abs(p2.Y - p1.Y) < _swipeMaximalHeight, // Height
                (p1, p2) => p2.X - p1.X > -0.01f, // Progression to right
                (p1, p2) => Math.Abs(p2.X - p1.X) > _swipeMinimalLength, // Length
                _swipeMinimalDuration, _swipeMaximalDuration)) // Duration
            {
                RaiseGestureDetected("SwipeToRight", pointToReturn);
                return;
            }

            // Swipe to left
            if (ScanPositions((p1, p2) => Math.Abs(p2.Y - p1.Y) < _swipeMaximalHeight,  // Height
                (p1, p2) => p2.X - p1.X < 0.01f, // Progression to right
                (p1, p2) => Math.Abs(p2.X - p1.X) > _swipeMinimalLength, // Length
                _swipeMinimalDuration, _swipeMaximalDuration))// Duration
            {
                RaiseGestureDetected("SwipeToLeft", pointToReturn);
                return;
            }

            // Swipe up
            if (ScanPositions((p1, p2) => Math.Abs(p2.Y - p1.Y) > _swipeMinimalHeight,  // Height
                (p1, p2) => p2.Y - p1.Y < 0.01f, // Downwards progression
                (p1, p2) => Math.Abs(p2.X - p1.X) < _swipeMaximalLength, // Length
                _swipeMinimalDuration, _swipeMaximalDuration))// Duration
            {
                RaiseGestureDetected("SwipeDown", pointToReturn);
                return;
            }

            // Swipe down
            if (ScanPositions((p1, p2) => Math.Abs(p2.Y - p1.Y) > _swipeMinimalHeight,  // Height
                (p1, p2) => p2.Y - p1.Y > -0.01f, // Upwards progression
                (p1, p2) => Math.Abs(p2.X - p1.X) < _swipeMaximalLength, // Length
                _swipeMinimalDuration, _swipeMaximalDuration))// Duration
            {
                RaiseGestureDetected("SwipeUp", pointToReturn);
                return;
            }
        }
    }
}