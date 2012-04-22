using System;
using System.Windows;
using Microsoft.Kinect;

namespace Kinect.Toolbox
{
    public class SwipeGestureDetector : GestureDetector
    {
        const float SwipeMinimalLength = 0.4f;
        const float SwipeMaximalLength = 0.2f;
        const float SwipeMinimalHeight = 0.1f;
        const float SwipeMaximalHeight = 0.2f;
        const int SwipeMininalDuration = 250;
        const int SwipeMaximalDuration = 1500;
        private Vector3 vectorToReturn;

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
            if (ScanPositions((p1, p2) => Math.Abs(p2.Y - p1.Y) < SwipeMaximalHeight, // Height
                (p1, p2) => p2.X - p1.X > -0.01f, // Progression to right
                (p1, p2) => Math.Abs(p2.X - p1.X) > SwipeMinimalLength, // Length
                SwipeMininalDuration, SwipeMaximalDuration)) // Duration
            {
                RaiseGestureDetected("SwipeToRight", pointToReturn);
                return;
            }

            // Swipe to left
            if (ScanPositions((p1, p2) => Math.Abs(p2.Y - p1.Y) < SwipeMaximalHeight,  // Height
                (p1, p2) => p2.X - p1.X < 0.01f, // Progression to right
                (p1, p2) => Math.Abs(p2.X - p1.X) > SwipeMinimalLength, // Length
                SwipeMininalDuration, SwipeMaximalDuration))// Duration
            {
                RaiseGestureDetected("SwipeToLeft", pointToReturn);
                return;
            }

            // Swipe up
            if (ScanPositions((p1, p2) => Math.Abs(p2.Y - p1.Y) > SwipeMinimalHeight,  // Height
                (p1, p2) => p2.Y - p1.Y < 0.01f, // Downwards progression
                (p1, p2) => Math.Abs(p2.X - p1.X) < SwipeMaximalLength, // Length
                SwipeMininalDuration, SwipeMaximalDuration))// Duration
            {
                RaiseGestureDetected("SwipeDown", pointToReturn);
                return;
            }

            // Swipe down
            if (ScanPositions((p1, p2) => Math.Abs(p2.Y - p1.Y) > SwipeMinimalHeight,  // Height
                (p1, p2) => p2.Y - p1.Y > -0.01f, // Upwards progression
                (p1, p2) => Math.Abs(p2.X - p1.X) < SwipeMaximalLength, // Length
                SwipeMininalDuration, SwipeMaximalDuration))// Duration
            {
                RaiseGestureDetected("SwipeUp", pointToReturn);
                return;
            }
        }
    }
}