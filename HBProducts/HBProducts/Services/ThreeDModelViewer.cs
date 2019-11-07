using System;
using System.Diagnostics;
using Urho;
using Urho.Actions;
using Urho.Gui;
using Urho.Shapes;
using System.Threading.Tasks;


namespace HBProducts
{
    public class ThreeDModelViewer : Urho.Application
    {

        public const float MinScale = 0.008f;
        public const float CameraInitialDist = 0.01f;
        public const float MaxScale = 0.03f;
        public const float ZoomSpeed = 0.001f;

        private float scale = 0.01f;
        private float x, y;
        private Node cameraNode, sensorNode;
        private Scene scene;

        private TouchState touch1, touch2;

        private string threeDModelName;


        [Preserve]
        public ThreeDModelViewer(ApplicationOptions options) : base(options) { }

        protected override async void Start()
        {
            base.Start();
        }

        public void setThreeDModelName(string threeDModelName)
        {
            this.threeDModelName = threeDModelName;
        }

        /**
        * Method for starting the scene. The method should be called on the Main thread of UrhoSharp,
        * so if it used outside of this class the main thread should be specified.
        * Example call : Urho.Application.InvokeOnMain(()=>modelViewer.startDisplaying());
        */
        public void startDisplaying()
        {
            Debug.WriteLine("The 3d model name is :" + threeDModelName);

            if (threeDModelName != null)
                Create3DObject();
        }

        //Creates a 3D object using the threeDModelName as 3D model directory.
        private async Task Create3DObject()
        {
            // 3D scene with Octree
            scene = new Scene(Context);
            scene.CreateComponent<Octree>();

            // Create a node for the Sensor
            sensorNode = scene.CreateChild();
            sensorNode.Position = new Vector3(0, 0, 5);
            sensorNode.Rotation = new Quaternion(5, 0, 10);
            sensorNode.SetScale(0.01f);

            // Create a static model component - Sphere:
            StaticModel sensor = sensorNode.CreateComponent<StaticModel>();
            sensor.Model = ResourceCache.GetModel(threeDModelName); //Load the model

            // Light
            Node lightNode = scene.CreateChild();
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Directional;
            light.Range = 20;
            light.Brightness = 1f;
            lightNode.SetDirection(new Vector3(0.4f, -0.5f, 0.3f));

            // Camera
            cameraNode = scene.CreateChild();
            var camera = cameraNode.CreateComponent<Camera>();

            // Viewport
            var viewport = new Viewport(Context, scene, camera, null);
            Renderer.SetViewport(0, viewport);
        }
       
        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);
            Input input = Input; // TO BE CHECKED

            MoveCameraByTouches(timeStep);
        }

        protected void MoveCameraByTouches(float timeStep)
        {
            const float touchSensitivity = 200f;

            var input = Input; //TO BE CHECKED

            if (input.NumTouches > 0)
            {
                touch1 = input.GetTouch(0);

                if (input.NumTouches == 2)
                {
                    touch2 = input.GetTouch(1);

                    // Check for zoom pattern (touches moving in opposite directions and on empty space)
                    if ((((touch1.Delta.Y > 0 && touch2.Delta.Y < 0) || (touch1.Delta.Y < 0 && touch2.Delta.Y > 0))))
                    {

                        // Check for zoom direction (in/out)
                        if (Math.Abs(touch1.Position.Y - touch2.Position.Y) < Math.Abs(touch1.LastPosition.Y - touch2.LastPosition.Y))
                        {
                            if (!(scale <= MinScale))
                            {
                                //Unzoom
                                scale -= ZoomSpeed;
                                sensorNode.SetScale(scale);
                            }
                        }
                        else
                        {
                            if (!(scale >= MaxScale))
                            {
                                //Zoom
                                scale += ZoomSpeed;
                                sensorNode.SetScale(scale);
                            }
                        }

                        Debug.WriteLine("Sensor scale changed to: " + scale);
                    }
                }
                else if (input.NumTouches == 1)
                {
                    //TouchState state = input.GetTouch(0);
                    if (touch1.Delta.X != 0 || touch1.Delta.Y != 0)
                    {


                        var camera = sensorNode.GetComponent<StaticModel>();
                        if (camera == null)
                            return;

                        x += touchSensitivity / Graphics.Height * touch1.Delta.X;
                        y += touchSensitivity / Graphics.Height * touch1.Delta.Y;


                        sensorNode.Rotation = new Quaternion(y, x, 0);
                    }
                }

            }

        }
    }
}
