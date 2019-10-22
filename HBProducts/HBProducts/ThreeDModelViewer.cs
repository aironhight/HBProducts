using System;
using System.Diagnostics;
using Urho;
using System.Threading.Tasks;

namespace HBProducts
{
    class ThreeDModelViewer : Urho.Application
    {
        public const float CameraMinDist = 0.001f;
        public const float CameraInitialDist = 0.01f;
        public const float CameraMaxDist = 0.02f;

        Node cameraNode;
        Node earthNode;
        //Node rootNode;
        Scene scene;
        float x, y;
        Node asd;
        Touch touch;
        bool zoom;
        public float ModelScale { get; set; }
        float scale = 0.01f;
        TouchState touch1, touch2;


        [Preserve]
        public ThreeDModelViewer(ApplicationOptions options) : base(options) { }

        protected override async void Start()
        {
            base.Start();
            //await Create3DObject();
            ModelScale = CameraInitialDist;
            Create3DObject();
        }

        private async Task Create3DObject()
        {
            // 3D scene with Octree
            scene = new Scene(Context);
            scene.CreateComponent<Octree>();

            //    // Create a node for the Earth
            //    rootNode = scene.CreateChild();
            //    rootNode.Position = new Vector3(0, 0, 20);
            asd = scene.CreateChild();
            asd.Position = new Vector3(0, 0, 5);
            asd.Rotation = new Quaternion(5, 0, 10);
            asd.SetScale(0.01f);

            // Create a static model component - Sphere:
            StaticModel earth = asd.CreateComponent<StaticModel>();
            earth.Model = ResourceCache.GetModel("Materials/Square.mdl"); // or simply Material.FromImage("Textures/Earth.jpg")

            //    // Light
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
            Input input = Input;
            //touch.UpdateTouches(timeStep);

            MoveCameraByTouches(timeStep);
            //SimpleMoveCamera3D(timeStep);
            base.OnUpdate(timeStep);
        }

        protected void MoveCameraByTouches(float timeStep)
        {
            const float touchSensitivity = 200f;

            var input = Input;

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
                            if (!(scale <= 0.008f))
                            {
                                //Unzoom
                                scale -= 0.0001f;
                                asd.SetScale(scale);
                            }
                        }
                        else
                        {
                            if (!(scale >= 0.03f))
                            {
                                //Zoom
                                scale += 0.0001f;
                                asd.SetScale(scale);
                            }
                        }

                        Debug.WriteLine(scale);


                    }



                }
                else if (input.NumTouches == 1)
                {
                    //TouchState state = input.GetTouch(0);
                    if (touch1.Delta.X != 0 || touch1.Delta.Y != 0)
                    {


                        var camera = asd.GetComponent<StaticModel>();
                        if (camera == null)
                            return;

                        x += touchSensitivity / Graphics.Height * touch1.Delta.X;
                        y += touchSensitivity / Graphics.Height * touch1.Delta.Y;


                        asd.Rotation = new Quaternion(y, x, 0);
                    }
                }

            }

        }
    }
}
