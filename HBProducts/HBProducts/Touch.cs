using System;
using System.Collections.Generic;
using System.Text;
using Urho;

namespace HBProducts
{
    public class Touch : Component
    {
        readonly float touchSensitivity;
        readonly Input input;
        bool zoom;


        public float CameraDistance { get; set; }

        public Touch(IntPtr handle) : base(handle) { }

        public Touch(float touchSensitivity, Input input)
        {
            this.touchSensitivity = touchSensitivity;
            this.input = input;
            CameraDistance = ThreeDModelViewer.CameraInitialDist;
            zoom = false;
        }

        public void UpdateTouches(float timeStep)
        {
            zoom = false; // reset bool

            // Zoom in/out
            if (input.NumTouches == 2)
            {
                TouchState touch1, touch2;
                touch1 = input.GetTouch(0);
                touch2 = input.GetTouch(1);

                // Check for zoom pattern (touches moving in opposite directions and on empty space)
                if (touch1.TouchedElement != null && touch2.TouchedElement != null && ((touch1.Delta.Y > 0 && touch2.Delta.Y < 0) || (touch1.Delta.Y < 0 && touch2.Delta.Y > 0)))
                    zoom = true;
                else
                    zoom = false;

                if (zoom)
                {
                    int sens = 0;
                    // Check for zoom direction (in/out)
                    if (Math.Abs(touch1.Position.Y - touch2.Position.Y) > Math.Abs(touch1.LastPosition.Y - touch2.LastPosition.Y))
                        sens = -1;
                    else
                        sens = 1;
                    CameraDistance += Math.Abs(touch1.Delta.Y - touch2.Delta.Y) * sens * touchSensitivity / 50.0f;
                    CameraDistance = MathHelper.Clamp(CameraDistance, ThreeDModelViewer.CameraMinDist, ThreeDModelViewer.CameraMaxDist); // Restrict zoom range to [1;20]
                }
            }


        }
    }
}
