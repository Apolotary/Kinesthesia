using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using Microsoft.Kinect;

namespace Kinesthesia.Model.GestureRecognition
{
    class GestureEventArgs: EventArgs
    {
        public GestureEventArgs (Joint j, SkeletonPoint p)
        {
            joint = j;
            point = p;
        }

        public Joint joint;
        public SkeletonPoint point;
    }
}
