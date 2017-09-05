using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics.Joints;
using eae_coolkatz.Images;
using FarseerPhysics.Collision;
using FarseerPhysics;
using FarseerPhysics.DebugView;
using eae_koolcatz;
using FarseerPhysics.Factories;
using System.Xml.Serialization;

namespace eae_coolkatz.Screens
{
    public class GameplayScreen : GameScreen
    {
        private World world;

        Camera2D camera;
        DebugViewXNA debug;

        private Body floor;
        private Body wallLeft;
        private Body wallRight;

        public Image background;

        //Angel Truck Stuff
        private Body truckAngelCollisionBox;

        public Image truckAngel;
        public Image tireAngel;

        Body _wheelBackAngel;
        Body _wheelFrontAngel;
        WheelJoint _springBackAngel;
        WheelJoint _springFrontAngel;

        bool rearInTheAirAngel = false;
        bool frontInTheAirAngel = false;
        bool angelCrash = false;
        bool angelFlipped = false;

        bool angelSpawned = true;
        bool demonSpawned = true;

        //Demon Truck Stuff
        private Body truckDemonCollisionBox;

        public Image truckDemon;
        public Image tireDemon;

        Body _wheelBackDemon;
        Body _wheelFrontDemon;
        WheelJoint _springBackDemon;
        WheelJoint _springFrontDemon;

        bool rearInTheAirDemon = false;
        bool frontInTheAirDemon = false;
        bool demonCrash = false;
        bool demonFlipped = false;

        const float MaxSpeed = 30.0f;
        private float _accelerationAngel;
        private float _accelerationDemon;

        const float _respawnDelayDemon = 2; // seconds
        float _remainingDelayDemon = _respawnDelayDemon;

        const float _respawnDelayAngel = 2; // seconds
        float _remainingDelayAngel = _respawnDelayAngel;

        const float _flipTimeDemon = 50;
        float _demonFlipTimer = _flipTimeDemon;
        bool demonFlick = false;

        const float _flipTimeAngel = 50;
        float _angelFlipTimer = _flipTimeAngel;
        bool angelFlick = false;

        public override void LoadContent()
        {
            base.LoadContent();
            background.LoadContent();

            truckAngel.LoadContent();
            tireAngel.LoadContent();

            truckDemon.LoadContent();
            tireDemon.LoadContent();

            camera = new Camera2D(ScreenManager.Instance.GraphicsDevice);

            if(world == null)
            {
                world = new World(Vector2.UnitY*10);
            }
            else
            {
                world.Clear();
            }

            if(debug == null)
            {
                debug = new DebugViewXNA(world);
                debug.AppendFlags(DebugViewFlags.Shape);
                debug.AppendFlags(DebugViewFlags.PolygonPoints);
                debug.LoadContent(ScreenManager.Instance.GraphicsDevice, ScreenManager.Instance.Content);
            }


            floor = new Body(world);

            {
                Vertices terrain = new Vertices();
                terrain.Add(ConvertUnits.ToSimUnits(0, 880));
                //terrain.Add(ConvertUnits.ToSimUnits(200, 880));
                //terrain.Add(ConvertUnits.ToSimUnits(400, 870));
                //terrain.Add(ConvertUnits.ToSimUnits(600, 890));
                //terrain.Add(ConvertUnits.ToSimUnits(800, 875));
                //terrain.Add(ConvertUnits.ToSimUnits(1000, 880));
                //terrain.Add(ConvertUnits.ToSimUnits(1200, 880));
                //terrain.Add(ConvertUnits.ToSimUnits(1400, 880));
                //terrain.Add(ConvertUnits.ToSimUnits(1600, 885));
                //terrain.Add(ConvertUnits.ToSimUnits(1800, 875));
                ////terrain.Add(ConvertUnits.ToSimUnits(20, 890));
                ////terrain.Add(ConvertUnits.ToSimUnits(20, 890));
                terrain.Add(ConvertUnits.ToSimUnits(1920, 885));

                for (int i = 0; i < terrain.Count - 1; ++i)
                {
                    FixtureFactory.AttachEdge(terrain[i], terrain[i + 1], floor);
                }

                floor.Friction = 0.6f;
            }

            world.Gravity = new Vector2(0,13.5f);

            wallLeft = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(2f), ConvertUnits.ToSimUnits(1080f), 10f);
            wallLeft.Position = ConvertUnits.ToSimUnits(0, 540);
            wallLeft.IsStatic = true;
            wallLeft.Restitution = 0.2f;
            wallLeft.Friction = 0.2f;

            wallRight = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(2f), ConvertUnits.ToSimUnits(1080f), 10f);
            wallRight.Position = ConvertUnits.ToSimUnits(1920, 540);
            wallRight.IsStatic = true;
            wallRight.Restitution = 0.2f;
            wallRight.Friction = 0.2f;

            CircleShape wheelShape = new CircleShape(0.2f, 0.8f);

            Vertices chassisShapeDemon = new Vertices(4);

            chassisShapeDemon.Add(ConvertUnits.ToSimUnits(-80, -15));
            chassisShapeDemon.Add(ConvertUnits.ToSimUnits(-70, 20));
            chassisShapeDemon.Add(ConvertUnits.ToSimUnits(85, 5));
            chassisShapeDemon.Add(ConvertUnits.ToSimUnits(85, -5));

            PolygonShape chassisDemon = new PolygonShape(chassisShapeDemon, 2);

            truckDemonCollisionBox = new Body(world);
            truckDemonCollisionBox.BodyType = BodyType.Dynamic;
            truckDemonCollisionBox.Position = new Vector2(1.1f, -1.0f);
            truckDemonCollisionBox.CreateFixture(chassisDemon);

            _wheelBackDemon = new Body(world);
            _wheelBackDemon.BodyType = BodyType.Dynamic;
            _wheelBackDemon.Position = new Vector2(0.45f, -0.65f);
            _wheelBackDemon.CreateFixture(wheelShape);
            _wheelBackDemon.Friction = 0.8f;

            wheelShape.Density = 1;
            _wheelFrontDemon = new Body(world);
            _wheelFrontDemon.BodyType = BodyType.Dynamic;
            _wheelFrontDemon.Position = new Vector2(1.05f, -0.6f);
            _wheelFrontDemon.CreateFixture(wheelShape);
            _wheelFrontDemon.Friction = 0.8f;

            Vector2 axisDemon = new Vector2(0.0f, -1.2f);
            _springBackDemon = new WheelJoint(truckDemonCollisionBox, _wheelBackDemon, _wheelBackDemon.Position, axisDemon, true);
            _springBackDemon.MotorSpeed = 0.0f;
            _springBackDemon.MaxMotorTorque = 6.0f;
            _springBackDemon.MotorEnabled = true;
            _springBackDemon.Frequency = 4.0f;
            _springBackDemon.DampingRatio = 0.7f;
            world.AddJoint(_springBackDemon);

            _springFrontDemon = new WheelJoint(truckDemonCollisionBox, _wheelFrontDemon, _wheelFrontDemon.Position, axisDemon, true);
            _springFrontDemon.MotorSpeed = 0.0f;
            _springFrontDemon.MaxMotorTorque = 6.0f;
            _springFrontDemon.MotorEnabled = true;
            _springFrontDemon.Frequency = 4.0f;
            _springFrontDemon.DampingRatio = 0.7f;
            world.AddJoint(_springFrontDemon);

            Vertices chassisShapeAngel = new Vertices(4);

            chassisShapeAngel.Add(ConvertUnits.ToSimUnits(-85, 0));
            chassisShapeAngel.Add(ConvertUnits.ToSimUnits(-85, 10));
            chassisShapeAngel.Add(ConvertUnits.ToSimUnits(80, 15));
            chassisShapeAngel.Add(ConvertUnits.ToSimUnits(80, -15));

            PolygonShape chassisAngel = new PolygonShape(chassisShapeAngel, 2);

            truckAngelCollisionBox = new Body(world);
            truckAngelCollisionBox.BodyType = BodyType.Dynamic;
            truckAngelCollisionBox.Position = new Vector2(17.7f, -1.0f);
            truckAngelCollisionBox.CreateFixture(chassisAngel);

            _wheelBackAngel = new Body(world);
            _wheelBackAngel.BodyType = BodyType.Dynamic;
            _wheelBackAngel.Position = new Vector2(17.7f, -0.65f);
            _wheelBackAngel.CreateFixture(wheelShape);
            _wheelBackAngel.Friction = 0.8f;

            wheelShape.Density = 1;
            _wheelFrontAngel = new Body(world);
            _wheelFrontAngel.BodyType = BodyType.Dynamic;
            _wheelFrontAngel.Position = new Vector2(18.4f, -0.65f);
            _wheelFrontAngel.CreateFixture(wheelShape);
            _wheelFrontAngel.Friction = 0.8f;

            Vector2 axisAngel = new Vector2(0.0f, -1.2f);
            _springBackAngel = new WheelJoint(truckAngelCollisionBox, _wheelBackAngel, _wheelBackAngel.Position, axisAngel, true);
            _springBackAngel.MotorSpeed = 0.0f;
            _springBackAngel.MaxMotorTorque = 6.0f;
            _springBackAngel.MotorEnabled = true;
            _springBackAngel.Frequency = 4.0f;
            _springBackAngel.DampingRatio = 0.7f;
            world.AddJoint(_springBackAngel);

            _springFrontAngel = new WheelJoint(truckAngelCollisionBox, _wheelFrontAngel, _wheelFrontAngel.Position, axisAngel, true);
            _springFrontAngel.MotorSpeed = 0.0f;
            _springFrontAngel.MaxMotorTorque = 6.0f;
            _springFrontAngel.MotorEnabled = true;
            _springFrontAngel.Frequency = 4.0f;
            _springFrontAngel.DampingRatio = 0.7f;
            world.AddJoint(_springFrontAngel);

            //camera.TrackingBody = body;
            //camera.EnableTracking = true;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            background.UnloadContent();

            truckAngel.UnloadContent();
            tireAngel.UnloadContent();

            truckDemon.UnloadContent();
            tireDemon.UnloadContent();
        }

        void Rear_OnSeperationAngel(Fixture a, Fixture b)
        {
            rearInTheAirAngel = true;
        }

        void Front_OnSeperationAngel(Fixture a, Fixture b)
        {
            frontInTheAirAngel = true;
        }

        bool Rear_OnCollisionAngel(Fixture a, Fixture b, Contact contact)
        {
            angelSpawned = false;
            rearInTheAirAngel = false;

            return true;
        }

        bool Front_OnCollisionAngel(Fixture a, Fixture b, Contact contact)
        {
            angelSpawned = false;
            frontInTheAirAngel = false;

            return true;
        }

        void Rear_OnSeperationDemon(Fixture a, Fixture b)
        {
            rearInTheAirDemon = true;
        }

        void Front_OnSeperationDemon(Fixture a, Fixture b)
        {
            frontInTheAirDemon = true;
        }

        bool Rear_OnCollisionDemon(Fixture a, Fixture b, Contact contact)
        {
            demonSpawned = false;
            rearInTheAirDemon = false;
            return true;
        }

        bool Front_OnCollisionDemon(Fixture a, Fixture b, Contact contact)
        {
            demonSpawned = false;
            frontInTheAirDemon = false;
            return true;
        }

        bool FlipCheck_Demon(Fixture a, Fixture b, Contact contact)
        {
            if (b.Body == floor)
            {
                //Console.WriteLine("\nDEBUG2");
                demonCrash = true;
                demonFlipped = true;
            }
            return true;
        }

        void StillFlipped_Demon(Fixture a, Fixture b)
        {
            //Console.WriteLine("\nDEBUG3");
            if(b.Body == floor)
            {
                _remainingDelayDemon = _respawnDelayDemon;
                demonFlipped = false;
                demonCrash = false;
            }
        }

        void Reset_Demon()
        {
            //Console.WriteLine("\nDEBUG4");
            truckDemonCollisionBox.Position = new Vector2(1.1f, -1.0f);
            truckDemonCollisionBox.Rotation = 0.0f;
            truckDemonCollisionBox.LinearVelocity = new Vector2(0, 0);
            truckDemonCollisionBox.AngularVelocity = 0f;

            _wheelBackDemon.Position = new Vector2(0.6f, -0.65f);
            _wheelFrontDemon.Position = new Vector2(1.2f, -0.65f);

            demonFlipped = false;
            demonCrash = false;
            demonSpawned = true;
            _remainingDelayDemon = _respawnDelayDemon;
        }

        bool FlipCheck_Angel(Fixture a, Fixture b, Contact contact)
        {
            if (b.Body == floor)
            {
                angelCrash = true;
                angelFlipped = true;
            }
            return true;
        }

        void StillFlipped_Angel(Fixture a, Fixture b)
        {
            if(b.Body == floor)
            {
                _remainingDelayAngel = _respawnDelayAngel;
                angelFlipped = false;
                angelCrash = false;
            }
        }

        void Reset_Angel()
        {
            truckAngelCollisionBox.Position = new Vector2(18.0f, -1.0f);
            truckAngelCollisionBox.Rotation = 0.0f;
            truckAngelCollisionBox.LinearVelocity = new Vector2(0, 0);
            truckAngelCollisionBox.AngularVelocity = 0f;

            _wheelBackAngel.Position = new Vector2(17.90f, -0.65f);

            _wheelFrontAngel.Position = new Vector2(18.50f, -0.65f);

            angelFlipped = false;
            angelCrash = false;
            angelSpawned = true;
            _remainingDelayAngel = _respawnDelayAngel;
        }

        public override void Update(GameTime gameTime)
        {
            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));

            _wheelBackAngel.OnCollision += new OnCollisionEventHandler(Rear_OnCollisionAngel);
            _wheelFrontAngel.OnCollision += new OnCollisionEventHandler(Front_OnCollisionAngel);
            _wheelBackAngel.OnSeparation += new OnSeparationEventHandler(Rear_OnSeperationAngel);
            _wheelFrontAngel.OnSeparation += new OnSeparationEventHandler(Front_OnSeperationAngel);

            truckAngelCollisionBox.OnCollision += new OnCollisionEventHandler(FlipCheck_Angel);
            truckAngelCollisionBox.OnSeparation += new OnSeparationEventHandler(StillFlipped_Angel);

            _springBackAngel.MotorSpeed = Math.Sign(_accelerationAngel) * MathHelper.SmoothStep(0f, MaxSpeed, Math.Abs(_accelerationAngel));
            _springFrontAngel.MotorSpeed = Math.Sign(_accelerationAngel) * MathHelper.SmoothStep(0f, MaxSpeed, Math.Abs(_accelerationAngel));
            if (Math.Abs(_springBackAngel.MotorSpeed) < MaxSpeed * 0.06f || (Math.Abs(_springFrontAngel.MotorSpeed) < MaxSpeed * 0.06f))
            {
                _springBackAngel.MotorEnabled = false;
                _springFrontAngel.MotorEnabled = false;
            }
            else
            {
                _springBackAngel.MotorEnabled = true;
                _springFrontAngel.MotorEnabled = true;
            }

            // Move our sprite based on arrow keys being pressed:
            if (InputManager.Instance.KeyDown(Keys.Right))
            {
                if (frontInTheAirAngel && rearInTheAirAngel)
                {
                    truckAngelCollisionBox.ApplyAngularImpulse(0.03f);
                }
                else
                {
                    _accelerationAngel = Math.Min(_accelerationAngel + (float)(5.0 * gameTime.ElapsedGameTime.TotalSeconds), 1f);
                    _springFrontAngel.MotorEnabled = false;
                    _springBackAngel.MotorEnabled = true;
                }
            }
            else if (InputManager.Instance.KeyDown(Keys.Left))
            {
                if (frontInTheAirAngel && rearInTheAirAngel && !angelSpawned)
                {
                    truckAngelCollisionBox.ApplyAngularImpulse(-0.03f);
                }
                else
                {
                    _accelerationAngel = Math.Max(_accelerationAngel - (float)(5.0 * gameTime.ElapsedGameTime.TotalSeconds), -1f);
                    _springBackAngel.MotorEnabled = false;
                    _springFrontAngel.MotorEnabled = true;
                }
            }
            else if (InputManager.Instance.KeyDown(Keys.Down))
            {
                truckAngelCollisionBox.ApplyAngularImpulse(-0.03f);
            }
            else
                _accelerationAngel = 0f;

            if (InputManager.Instance.KeyPressed(Keys.Up))
            {
                if (!rearInTheAirAngel || !frontInTheAirAngel)
                    truckAngelCollisionBox.ApplyForce(new Vector2(0, -500), truckAngelCollisionBox.Position + new Vector2(0.15f, 0f));
            }

            else if (InputManager.Instance.KeyPressed(Keys.RightShift))
            {
                if (!rearInTheAirAngel)
                {
                    angelFlick = true;
                    truckAngelCollisionBox.AngularDamping = 40f;
                    truckAngelCollisionBox.AngularVelocity = 110f;
                }
            }

            if(angelFlick)
            {
                var timer = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                _angelFlipTimer -= timer;

                //Console.WriteLine(_angelFlipTimer);

                if(_angelFlipTimer <= 0)
                {
                    Console.WriteLine("Debug");
                    truckAngelCollisionBox.AngularDamping = 0f;
                    truckAngelCollisionBox.AngularVelocity = 0f;
                    _angelFlipTimer = _flipTimeAngel;
                    angelFlick = false;
                }
            }

            if (angelCrash)
            {
                var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _remainingDelayAngel -= timer;

                if (_remainingDelayAngel <= 0 && angelFlipped)
                {
                    Reset_Angel();
                }
            }

            _wheelBackDemon.OnCollision += new OnCollisionEventHandler(Rear_OnCollisionDemon);
            _wheelFrontDemon.OnCollision += new OnCollisionEventHandler(Front_OnCollisionDemon);
            _wheelBackDemon.OnSeparation += new OnSeparationEventHandler(Rear_OnSeperationDemon);
            _wheelFrontDemon.OnSeparation += new OnSeparationEventHandler(Front_OnSeperationDemon);

            truckDemonCollisionBox.OnCollision += new OnCollisionEventHandler(FlipCheck_Demon);
            truckDemonCollisionBox.OnSeparation += new OnSeparationEventHandler(StillFlipped_Demon);

            _springBackDemon.MotorSpeed = Math.Sign(_accelerationDemon) * MathHelper.SmoothStep(0f, MaxSpeed, Math.Abs(_accelerationDemon));
            _springFrontDemon.MotorSpeed = Math.Sign(_accelerationDemon) * MathHelper.SmoothStep(0f, MaxSpeed, Math.Abs(_accelerationDemon));

            if (Math.Abs(_springBackDemon.MotorSpeed) < MaxSpeed * 0.06f || (Math.Abs(_springFrontDemon.MotorSpeed) < MaxSpeed * 0.06f))
            {
                _springBackDemon.MotorEnabled = false;
                _springFrontDemon.MotorEnabled = false;
            }
            else
            {
                _springBackDemon.MotorEnabled = true;
                _springFrontDemon.MotorEnabled = true;
            }

            // Move our sprite based on arrow keys being pressed:
            if (InputManager.Instance.KeyDown(Keys.D))
            {
                if (frontInTheAirDemon && rearInTheAirDemon && !demonSpawned)
                {
                    truckDemonCollisionBox.ApplyAngularImpulse(0.03f);
                }
                else
                {
                    _accelerationDemon = Math.Min(_accelerationDemon + (float)(5.0 * gameTime.ElapsedGameTime.TotalSeconds), 1f);
                    _springFrontDemon.MotorEnabled = false;
                    _springBackDemon.MotorEnabled = true;
                }
            }
            else if (InputManager.Instance.KeyDown(Keys.A))
            {
                if (frontInTheAirDemon && rearInTheAirDemon)
                {
                    truckDemonCollisionBox.ApplyAngularImpulse(-0.03f);
                }
                else
                {
                    _accelerationDemon = Math.Max(_accelerationDemon - (float)(5.0 * gameTime.ElapsedGameTime.TotalSeconds), -1f);
                    _springBackDemon.MotorEnabled = false;
                    _springFrontDemon.MotorEnabled = true;
                }
            }
            else if (InputManager.Instance.KeyDown(Keys.S))
            {
                truckDemonCollisionBox.ApplyAngularImpulse(0.02f);
            }
            else
                _accelerationDemon = 0f;

            if (InputManager.Instance.KeyPressed(Keys.LeftShift))
            {
                if (!frontInTheAirDemon)
                {
                    demonFlick = true;
                    truckDemonCollisionBox.AngularDamping = 40f;
                    truckDemonCollisionBox.AngularVelocity = -120f;
                }
            }

            else if (InputManager.Instance.KeyPressed(Keys.W))
            {
                if (!rearInTheAirDemon || !frontInTheAirDemon)
                     truckDemonCollisionBox.ApplyForce(new Vector2(0, -500), truckDemonCollisionBox.Position + new Vector2(-0.125f, 0f));
            }

            if (demonFlick)
            {
                var timer = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                _demonFlipTimer -= timer;

                if (_demonFlipTimer <= 0)
                {
                    truckDemonCollisionBox.AngularDamping = 0;
                    truckDemonCollisionBox.AngularVelocity = 0;
                    _demonFlipTimer = _flipTimeDemon;
                    demonFlick = false;
                }
            }

            if (demonCrash)
            {
                //Console.WriteLine("\nDEBUG1");

                var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _remainingDelayDemon -= timer;

                if (_remainingDelayDemon <= 0 && demonFlipped)
                {
                    Reset_Demon();
                }
            }

            background.Update(gameTime);
            truckAngel.Update(gameTime);
            tireAngel.Update(gameTime);
            truckDemon.Update(gameTime);
            tireDemon.Update(gameTime);
            camera.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);

            truckAngel.Draw(spriteBatch, ConvertUnits.ToDisplayUnits(truckAngelCollisionBox.Position), truckAngelCollisionBox.Rotation, true);
            tireAngel.Draw(spriteBatch, ConvertUnits.ToDisplayUnits(_wheelBackAngel.Position), _wheelBackAngel.Rotation, false);
            tireAngel.Draw(spriteBatch, ConvertUnits.ToDisplayUnits(_wheelFrontAngel.Position), _wheelFrontAngel.Rotation, false);

            truckDemon.Draw(spriteBatch, ConvertUnits.ToDisplayUnits(truckDemonCollisionBox.Position), truckDemonCollisionBox.Rotation, false);
            tireDemon.Draw(spriteBatch, ConvertUnits.ToDisplayUnits(_wheelBackDemon.Position), _wheelBackDemon.Rotation, false);
            tireDemon.Draw(spriteBatch, ConvertUnits.ToDisplayUnits(_wheelFrontDemon.Position), _wheelFrontDemon.Rotation, false);

            spriteBatch.End();

            spriteBatch.Begin();
            //debug.RenderDebugData(ref camera.SimProjection, ref camera.SimView);
            base.Draw(spriteBatch);
        }

        public static Vector2 CalculateOrigin(Body b)
        {
            Vector2 lBound = new Vector2(float.MaxValue);
            Transform trans;
            b.GetTransform(out trans);

            for (int i = 0; i < b.FixtureList.Count; ++i)
            {
                for (int j = 0; j < b.FixtureList[i].Shape.ChildCount; ++j)
                {
                    AABB bounds;
                    b.FixtureList[i].Shape.ComputeAABB(out bounds, ref trans, j);
                    Vector2.Min(ref lBound, ref bounds.LowerBound, out lBound);
                }
            }

            // calculate body offset from its center and add a 1 pixel border
            // because we generate the textures a little bigger than the actual body's fixtures
            return ConvertUnits.ToDisplayUnits(b.Position - lBound) + new Vector2(1f);
        }
    }
}
