using Editor_Reader;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Catch.Objects;
using osu.Game.Rulesets.Objects;
using System.Drawing;
using Color = OpenTK.Graphics.Color4;

namespace osucatch_editor_realtimeviewer
{

    public class Canvas : OpenTK.GLControl
    {
        public static int screensContain = 4;

        public static float fontScale = 1;

        private static Texture2D? hitCircleTexture;
        private static Texture2D? DropTexture;
        private static Texture2D? BananaTexture;

        private readonly float Border_Height = 32;
        private readonly float Border_Width = 32;

        public Canvas()
            : base()
        {
            this.MakeCurrent();
            this.Paint += Canvas_Paint;
            this.Resize += Canvas_Resize;
        }
        public void Canvas_Paint(object? sender, PaintEventArgs? e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            DrawJudgementLine();
            Form1.drawingHelper.Draw();

            this.SwapBuffers();
        }

        private void Canvas_Resize(object? sender, EventArgs? e)
        {
            int w = this.Size.Width;
            int h = this.Size.Height;
            int x = 0;
            int y = 0;
            double width_height = (640.0 + 2 * Border_Width) / (480.0 * screensContain + 2 * Border_Height);
            if (w / width_height > h)
            {
                w = (int)(h * width_height);
                x = (this.Size.Width - w) / 2;
            }
            else if (h * width_height > w)
            {
                h = (int)(w / width_height);
                y = (this.Size.Height - h) / 2;
            }
            GL.Viewport(x, y, w, h);
        }

        public void Init()
        {
            hitCircleTexture = TextureFromFile(Form1.Path_Img_Hitcircle);
            DropTexture = TextureFromFile(Form1.Path_Img_Drop);
            BananaTexture = TextureFromFile(Form1.Path_Img_Banana);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            this.Canvas_Resize(this, null);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Vector2 border = new Vector2(Border_Width, Border_Height) * ((screensContain > 1) ? 1 : 0);
            GL.Ortho(-border.X, 640.0 + border.X, 480 * screensContain + border.Y, -border.Y, 0.0, 1.0);
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void ScreensContainChanged()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Vector2 border = new Vector2(Border_Width, Border_Height) * ((screensContain > 1) ? 1 : 0);
            GL.Ortho(-border.X, 640.0 + border.X, 480 * screensContain + border.Y, -border.Y, 0.0, 1.0);
            this.Canvas_Resize(this, null);
        }

        private static Texture2D? TextureFromFile(string path)
        {
            try
            {
                return new Texture2D(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));
            }
            catch (Exception ex)
            {
                Log.ConsoleLog("Read texture file failed: " + path + "\r\n" + ex, Log.LogType.Drawing, Log.LogLevel.Error);
                return null;
            }
        }

        private static Texture2D? TextureFromString(string s, float fontscale)
        {
            try
            {
                return new Texture2D(s, fontscale);
            }
            catch (Exception ex)
            {
                Log.ConsoleLog("Build text texture failed: " + s + "\r\n" + ex, Log.LogType.Drawing, Log.LogLevel.Error);
                return null;
            }
        }

        public static void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Color4(color);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(start.X, start.Y);
            GL.Vertex2(end.X, end.Y);
            GL.End();
            GL.Enable(EnableCap.Texture2D);
        }

        private static void DrawDistance(Texture2D? texture, Vector2 notePos, float diameter, Color color)
        {
            if (texture == null) return;
            Vector2 labelPosStart = notePos;
            labelPosStart.X += diameter / 2;
            labelPosStart.Y -= diameter / 2;

            float textureRightX = labelPosStart.X + texture.Width;
            if (textureRightX > 640) labelPosStart.X -= diameter + texture.Width;
            labelPosStart.Y += (diameter - texture.Height) / 2;
            texture.Draw(labelPosStart, new Vector2(0, 0), color);
        }

        private static void DrawLabel(Texture2D? texture, Vector2 pos, bool isLeft, Color color)
        {
            if (texture == null) return;
            if (isLeft) texture.Draw(pos, new Vector2(30, 0), color);
            else texture.Draw(pos, new Vector2(texture.Width - 30, 0), color);
        }

        public static void DrawBPMLabel(double bpm, int posY)
        {
            Vector2 rp0 = new Vector2(64, posY);
            Vector2 rp1 = new Vector2(576, posY);
            Canvas.DrawLine(rp0, rp1, Color.Red);
            Texture2D? BPMTexture = TextureFromString(bpm.ToString("F0"), fontScale);
            if (BPMTexture == null) return;
            DrawLabel(BPMTexture, rp0, true, Color.Red);
            BPMTexture.Dispose();
        }

        public static void DrawSVLabel(double sv, int posY)
        {
            Vector2 rp0 = new Vector2(64, posY);
            Vector2 rp1 = new Vector2(576, posY);
            DrawLine(rp0, rp1, Color.LightGreen);
            Texture2D? BPMTexture = TextureFromString(sv.ToString("F2"), fontScale);
            if (BPMTexture == null) return;
            DrawLabel(BPMTexture, rp1, false, Color.LightGreen);
            BPMTexture.Dispose();
        }

        public static void DrawDistanceLabel(string distanceString, Vector2 pos, float circleDiameter)
        {
            if (distanceString == "") return;
            Texture2D? distanceTexture = TextureFromString(distanceString, fontScale);
            if (distanceTexture == null) return;
            if (distanceString.Length > 0) DrawDistance(distanceTexture, pos, circleDiameter, Color.LightBlue);
            distanceTexture.Dispose();
        }

        private static void DrawHyperDashCircle(Texture2D? texture, Vector2 pos, float diameter)
        {
            if (texture == null) return;
            texture.Draw(pos, diameter * 1.4f, diameter * 1.4f, new Vector2(diameter * 1.4f * 0.5f), Color.Red);
        }

        private static void DrawCircleWithCircleColor(Texture2D? texture, Vector2 pos, float diameter, Color color, bool isHyperDash)
        {
            if (texture == null) return;
            if (isHyperDash) DrawHyperDashCircle(texture, pos, diameter);
            texture.Draw(pos, diameter, diameter, new Vector2(diameter * 0.5f), color);
        }

        private static void DrawCircle(Texture2D? texture, Vector2 pos, float diameter, bool isHyperDash)
        {
            if (texture == null) return;
            Color color = (isHyperDash) ? Color.Red : Color.White;
            texture.Draw(pos, diameter, diameter, new Vector2(diameter * 0.5f), color);
        }

        public static void DrawFruit(Vector2 pos, float circleDiameter, Color color, bool withCircleColor, bool isHyperDash = false)
        {
            if (withCircleColor) DrawCircleWithCircleColor(hitCircleTexture, pos, circleDiameter, color, isHyperDash);
            else DrawCircle(hitCircleTexture, pos, circleDiameter, isHyperDash);
        }

        public static void DrawDroplet(Vector2 pos, float circleDiameter, float hitObjectScale, Color color, bool withCircleColor, bool isHyperDash = false)
        {
            if (withCircleColor) DrawCircleWithCircleColor(DropTexture, pos, circleDiameter * hitObjectScale, color, isHyperDash);
            else DrawCircle(DropTexture, pos, circleDiameter * hitObjectScale, isHyperDash);
        }

        public static void DrawTinyDroplet(Vector2 pos, float circleDiameter, float hitObjectScale, Color color, bool withCircleColor, bool isHyperDash = false)
        {
            if (withCircleColor) DrawCircleWithCircleColor(DropTexture, pos, circleDiameter * hitObjectScale / 2, color, isHyperDash);
            else DrawCircle(DropTexture, pos, circleDiameter * hitObjectScale / 2, isHyperDash);
        }

        public static void DrawBanana(Vector2 pos, float circleDiameter)
        {
            if (BananaTexture == null) return;
            BananaTexture.Draw(pos, circleDiameter, circleDiameter, new Vector2(circleDiameter * 0.5f), Color.Yellow);
        }

        private static void DrawJudgementLine()
        {
            if (screensContain > 1)
            {
                Vector2 rp0 = new Vector2(64, (float)(240.0 * screensContain));
                Vector2 rp1 = new Vector2(576, (float)(240.0 * screensContain));
                DrawLine(rp0, rp1, Color.White);
            }
            else
            {
                Vector2 rp0 = new Vector2(64, 384);
                Vector2 rp1 = new Vector2(576, 384);
                DrawLine(rp0, rp1, Color.White);
            }
        }





    }
}
