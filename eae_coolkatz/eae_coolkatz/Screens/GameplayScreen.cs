using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using eae_coolkatz.Images;
using FarseerPhysics.Collision;
using FarseerPhysics;
using FarseerPhysics.DebugView;
using eae_koolcatz;
using FarseerPhysics.Factories;

namespace eae_coolkatz.Screens
{
    public class GameplayScreen : GameScreen
    {
        Camera2D camera;
        private Body body;
        private World world;
        public Image Image;
        DebugViewXNA debug;

        public override void LoadContent()
        {
            base.LoadContent();
            Image.LoadContent();

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

            body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(480), ConvertUnits.ToSimUnits(50), 10f);
            body.Position = ConvertUnits.ToSimUnits(240, 50);
            body.IsStatic = true;
            body.Restitution = 0.2f;
            body.Friction = 0.2f;

            var box = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(30), ConvertUnits.ToSimUnits(30), 10f, new Point(30, 30));
            box.BodyType = BodyType.Dynamic;
            box.Restitution = 0.2f;
            box.Friction = 0.2f;
            box.Position = ConvertUnits.ToSimUnits(100, 0);
            /*
            Image.Origin = CalculateOrigin(body);
            camera.TrackingBody = body;
            camera.EnableTracking = true;
            */
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Image.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));
            Image.Update(gameTime);
            camera.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
            spriteBatch.Draw(Image.Texture, ConvertUnits.ToDisplayUnits(body.Position), null, Color.White, body.Rotation, Image.Origin, 1f, SpriteEffects.None, 0f);
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
