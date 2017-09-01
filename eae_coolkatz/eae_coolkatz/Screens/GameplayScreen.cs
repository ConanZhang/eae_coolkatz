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
            world = new World(Vector2.Zero);
            world.Gravity = new Vector2(0f, 1f);

            debug = new DebugViewXNA(world);
            debug.LoadContent(ScreenManager.Instance.GraphicsDevice, ScreenManager.Instance.Content);
            debug.AppendFlags(DebugViewFlags.Shape);
            debug.AppendFlags(DebugViewFlags.PolygonPoints);

            Vertices vertices = new Vertices(8);
            vertices.Add(new Vector2(-2.5f, 0.08f));
            vertices.Add(new Vector2(-2.375f, -0.46f));
            vertices.Add(new Vector2(-0.58f, -0.92f));
            vertices.Add(new Vector2(0.46f, -0.92f));
            vertices.Add(new Vector2(2.5f, -0.17f));
            vertices.Add(new Vector2(2.5f, 0.205f));
            vertices.Add(new Vector2(2.3f, 0.33f));
            vertices.Add(new Vector2(-2.25f, 0.35f));

            PolygonShape chassis = new PolygonShape(vertices, 2);

            body = new Body(world);
            body.BodyType = BodyType.Dynamic;
            body.Position = new Vector2(0.0f, -1.0f);
            body.CreateFixture(chassis);

            Image.Origin = CalculateOrigin(body);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Image.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Image.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Image.Draw(spriteBatch);
            spriteBatch.Draw(Image.Texture, ConvertUnits.ToDisplayUnits(body.Position), null, Color.White, body.Rotation, Image.Origin, 1f, SpriteEffects.None, 0f);
            Matrix proj = Matrix.CreateOrthographicOffCenter(0f, ScreenManager.Instance.GraphicsDevice.Viewport.Width, ScreenManager.Instance.GraphicsDevice.Viewport.Height, 0f, 0f, 1f);
            Matrix view = camera.View;
            debug.RenderDebugData(ref proj, ref view);
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
