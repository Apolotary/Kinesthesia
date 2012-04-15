using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using Microsoft.Kinect;

namespace Kinesthesia.Model.GestureRecognition
{
    /// <summary>
    /// Event arguments to pass for gesture recognition event callbacks
    /// </summary>
    class GestureEventArgs: EventArgs
    {
        public GestureEventArgs(JointType jointToTrack, SkeletonPoint pointToSend)
        {
            joint = jointToTrack;
            point = pointToSend;
        }

        public JointType joint;
        public SkeletonPoint point;
    }
}
