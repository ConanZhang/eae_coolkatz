using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
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
        Camera2D camera;
        private Body body;
        private Body box;
        private World world;
        public Image background;
        public Image truck;
        public Rectangle truckRect;
        DebugViewXNA debug;

        [XmlIgnore]
        public Body _wheelBack;
        [XmlIgnore]
        public Body _wheelFront;
        [XmlIgnore]
        public WheelJoint _springBack;
        [XmlIgnore]
        public WheelJoint _springFront;

        //InputManager input = new InputManager();


        public override void LoadContent()
        {
            base.LoadContent();
            background.LoadContent();
            truck.LoadContent();

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

            body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(1920f), ConvertUnits.ToSimUnits(70f), 10f);
            body.Position = ConvertUnits.ToSimUnits(960, 1080-35);
            //body.Position = ConvertUnits.ToDisplayUnits(0, 1920-35);
            body.IsStatic = true;
            body.Restitution = 0.2f;
            body.Friction = 0.2f;

            /*Vertices vertices = new Vertices(4);
            vertices.Add(new Vector2(-2.5f, 0.08f));
            vertices.Add(new Vector2(-2.375f, -0.46f));
            vertices.Add(new Vector2(-0.58f, -0.92f));
            vertices.Add(new Vector2(0.46f, -0.92f));*/

            //PolygonShape chassis = new PolygonShape(, 2);
            CircleShape wheelShape = new CircleShape(0.5f, 0.8f);

            box = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(150), ConvertUnits.ToSimUnits(100), 10f, new Point(30, 30));
            box.BodyType = BodyType.Dynamic;
            box.Restitution = 0.2f;
            box.Friction = 0.2f;
            box.Position = ConvertUnits.ToSimUnits(0, 0);
            box.IsStatic = false;
            //box.CreateFixture(chassis);
            /*
            Image.Origin = CalculateOrigin(body);
            camera.TrackingBody = body;
            camera.EnableTracking = true;
            */
            Console.WriteLine(ScreenManager.Instance.GraphicsDevice.Viewport.Width);
            Console.WriteLine(ScreenManager.Instance.GraphicsDevice.Viewport.Height);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            background.UnloadContent();
            truck.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));

            //if(input.KeyDown(Keys.Right))
            //{
            //    box.ApplyForce(Vector2.UnitX * 2);
            //}

            //if (input.KeyDown(Keys.Left))
            //{
            //    box.ApplyForce(Vector2.UnitX * -2);
            //}

            //KeyboardState state = Keyboard.GetState();

            // If they hit esc, exit
            //if (state.IsKeyDown(Keys.Escape))
            //    Exit();

            // Move our sprite based on arrow keys being pressed:

            if (InputManager.Instance.KeyDown(Keys.Right))
                box.LinearVelocity += Vector2.UnitX;
            if (InputManager.Instance.KeyDown(Keys.Left))
                box.LinearVelocity -= Vector2.UnitX;
            if (InputManager.Instance.KeyPressed(Keys.Up))
                box.LinearVelocity += Vector2.UnitY * 25;

            if (Vector2.Dot(box.LinearVelocity,Vector2.UnitX) > 5 || Vector2.Dot(box.LinearVelocity, Vector2.UnitX) < -5)
            {
                var direction = Vector2.Normalize(box.LinearVelocity);
                box.LinearVelocity = direction * 5;
            }

            


            background.Update(gameTime);
            truck.Update(gameTime);
            camera.Update(gameTime);
            //input.Update();
            base.Update(gameTime);

            //truckRect.X = box.Position.X;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            //spriteBatch.Draw(truck.Texture, truckRect, null, Color.White, body.Rotation, body.Position, SpriteEffects.None, 0f);
            spriteBatch.Draw(truck.Texture, ConvertUnits.ToDisplayUnits(box.Position), Color.White);
            spriteBatch.End();
            spriteBatch.Begin();
            debug.RenderDebugData(ref camera.SimProjection, ref camera.SimView);
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
