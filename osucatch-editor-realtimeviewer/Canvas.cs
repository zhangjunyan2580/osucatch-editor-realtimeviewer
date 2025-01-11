using Editor_Reader;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Catch.Objects;
using osu.Game.Rulesets.Objects;
using Color = OpenTK.Graphics.Color4;

namespace osucatch_editor_realtimeviewer
{

    public class Canvas : OpenTK.GLControl
    {
        /*
         * screen size: 640x480
         * playfield size: 512x384
         * 
         * TimePerPixels = ApproachTime / 384
         * 
         * |                  |
         * |==================|
         * |<---width: 640--->|
         * |                  |
         * |                  |
         * |------------------| N screen catcher | ΔTime = N * ApproachTime * 1.25
         * |                  |
         * |==================| (screen top) | ΔTime = ApproachTime
         * |                  |
         * |                  |
         * |                  |
         * |------------------| catcher height: 384 (current time) | ΔTime = 0
         * |                  | 
         * |==================| screen height: 480 (screen bottom) | ΔTime = -TimePerPixels * (480 - 384) = -ApproachTime / 4
         * |                  |
         * |                  |
         * |                  |
         * |------------------| -N screen catcher | ΔTime = -N * ApproachTime * 1.25
         * |                  |
         * |==================|
         * |                  |
         */



        public int screensContain = 4;

        public ViewerManager? viewerManager;

        public float fontScale = 1;

        private Texture2D? hitCircleTexture;
        private Texture2D? DropTexture;
        private Texture2D? BananaTexture;

        public static Color[] Default_Colors = new Color[8] {
            // Rainbow! >_<
            new Color(255, 191, 191, 255),
            new Color(255, 210, 128, 255),
            new Color(255, 255, 128, 255),
            new Color(128, 255, 128, 255),
            new Color(128, 255, 255, 255),
            new Color(128, 191, 255, 255),
            new Color(191, 128, 255, 255),
            new Color(255, 128, 255, 255),
        };

        public Color[] Combo_Colors = new Color[8] { Default_Colors[1], Default_Colors[3], Default_Colors[5], Default_Colors[7], Default_Colors[6], Default_Colors[4], Default_Colors[0], Default_Colors[2] };

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

            if (viewerManager != null)
            {
                viewerManager.BuildNearby(screensContain);

                this.DrawJudgementLine();
                this.Draw();

            }
            this.SwapBuffers();
        }

        private void Canvas_Resize(object? sender, EventArgs? e)
        {
            int w = this.Size.Width;
            int h = this.Size.Height;
            int x = 0;
            int y = 0;
            double width_height = (640.0 + 2 * Border_Width) / (480.0 * this.screensContain + 2 * Border_Height);
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
            this.hitCircleTexture = this.TextureFromFile(Form1.Path_Img_Hitcircle);
            this.DropTexture = this.TextureFromFile(Form1.Path_Img_Drop);
            this.BananaTexture = this.TextureFromFile(Form1.Path_Img_Banana);

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

        private Texture2D? TextureFromFile(string path)
        {
            try
            {
                return new Texture2D(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));
            }
            catch (Exception ex)
            {
                Form1.ConsoleLog("Read texture file failed: " + path + "\r\n" + ex, Form1.LogType.Drawing, Form1.LogLevel.Error);
                return null;
            }
        }

        private Texture2D? TextureFromString(string s, float fontscale)
        {
            try
            {
                return new Texture2D(s, fontscale);
            }
            catch (Exception ex)
            {
                Form1.ConsoleLog("Build text texture failed: " + s + "\r\n" + ex, Form1.LogType.Drawing, Form1.LogLevel.Error);
                return null;
            }
        }


        public void Draw()
        {
            if (viewerManager == null || viewerManager.Beatmap == null) { return; }
            if (viewerManager.CustomComboColours.Count > 0)
            {
                Combo_Colors = viewerManager.CustomComboColours.ToArray();
            }
            int circleDiameter = (int)(108.848 - viewerManager.Beatmap.Difficulty.CircleSize * 8.9646);
            float fruitSpeed = 384 / viewerManager.ApproachTime;
            List<TimingControlPoint> timingControlPoints = new List<TimingControlPoint>();
            List<DifficultyControlPoint> difficultyControlPoints = new List<DifficultyControlPoint>();

            double MaxStartTime = -1;

            for (int b = viewerManager.NearbyHitObjects.Count - 1; b >= 0; b--)
            {
                WithDistancePalpableCatchHitObject hitObject = viewerManager.NearbyHitObjects[b];

                if (MaxStartTime < 0 || hitObject.currentObject.StartTime > MaxStartTime) MaxStartTime = hitObject.currentObject.StartTime;

                double deltaTime = hitObject.currentObject.StartTime - viewerManager.currentTime;
                if (screensContain > 1)
                {
                    double timeSpan = screensContain * viewerManager.ApproachTime * 1.25;
                    if (deltaTime <= timeSpan && deltaTime >= -timeSpan)
                    {
                        this.DrawHitcircle(hitObject, deltaTime, circleDiameter, viewerManager.DistanceType);
                    }
                }
                else
                {
                    double upTime = this.viewerManager.ApproachTime + circleDiameter * this.viewerManager.TimePerPixels;
                    double bottomTime = this.viewerManager.ApproachTime / 4 + circleDiameter * this.viewerManager.TimePerPixels;
                    if (deltaTime <= upTime && deltaTime >= -bottomTime)
                    {
                        this.DrawHitcircle(hitObject, deltaTime, circleDiameter, viewerManager.DistanceType);
                    }
                }


                if (app.Default.TimingLine_ShowRed)
                {
                    var timingControlPoint = hitObject.GetTimingPoint(viewerManager.Beatmap);
                    timingControlPoints.Add(timingControlPoint);
                }
                if (app.Default.TimingLine_ShowGreen)
                {
                    var difficultyControlPoint = hitObject.GetDifficultyControlPoint(viewerManager.Beatmap);
                    difficultyControlPoints.Add(difficultyControlPoint);
                }
            }


            if (app.Default.BarLine_Show)
            {
                List<BarLine> barLines = viewerManager.Beatmap.BarLines.Where((barLine) => barLine.StartTime >= 0 && barLine.StartTime <= MaxStartTime + 1).ToList();
                DrawBarLines(barLines);
            }

            if (app.Default.TimingLine_ShowGreen)
            {
                difficultyControlPoints = difficultyControlPoints.Distinct().ToList();
                DrawDifficultyControPoints(difficultyControlPoints);
            }

            if (app.Default.TimingLine_ShowRed)
            {
                timingControlPoints = timingControlPoints.Distinct().ToList();
                DrawTimingPoints(timingControlPoints);
            }
        }



        private void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Color4(color);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(start.X, start.Y);
            GL.Vertex2(end.X, end.Y);
            GL.End();
            GL.Enable(EnableCap.Texture2D);
        }

        private void DrawHitcircle(WithDistancePalpableCatchHitObject wdpch, double deltaTime, int circleDiameter, DistanceType distanceType)
        {
            PalpableCatchHitObject hitObject = wdpch.currentObject;
            double baseY = (screensContain <= 1) ? 384 : 240.0 * this.screensContain;
            Vector2 pos = new Vector2(64 + hitObject.EffectiveX, (float)(baseY - deltaTime / this.viewerManager.TimePerPixels));
            if (Form1.Combo_Colour)
            {
                int comboColorIndex = (hitObject.ComboIndex) % Combo_Colors.Length;
                Color color = Combo_Colors[comboColorIndex];
                if (hitObject is TinyDroplet)
                {
                    if (hitObject.HyperDash) this.DrawHyperDashCircle(DropTexture, pos, (int)(circleDiameter * hitObject.Scale / 2));
                    this.DrawCircle(DropTexture, pos, (int)(circleDiameter * hitObject.Scale / 2), color);
                }
                else if (hitObject is Droplet)
                {
                    if (hitObject.HyperDash) this.DrawHyperDashCircle(DropTexture, pos, (int)(circleDiameter * hitObject.Scale));
                    this.DrawCircle(DropTexture, pos, (int)(circleDiameter * hitObject.Scale), color);
                }
                else if (hitObject is Fruit)
                {
                    if (hitObject.HyperDash) this.DrawHyperDashCircle(hitCircleTexture, pos, circleDiameter);
                    this.DrawCircle(hitCircleTexture, pos, circleDiameter, color);
                }
                else if (hitObject is Banana)
                {
                    this.DrawCircle(BananaTexture, pos, circleDiameter, Color.Yellow);
                }
            }
            else
            {
                if (hitObject is TinyDroplet)
                {
                    if (hitObject.HyperDash)
                        this.DrawCircle(DropTexture, pos, (int)(circleDiameter * hitObject.Scale / 2), new Color(1.0f, 0f, 0f, 1.0f));
                    else
                        this.DrawCircle(DropTexture, pos, (int)(circleDiameter * hitObject.Scale / 2), new Color(1.0f, 1.0f, 1.0f, 1.0f));
                }
                else if (hitObject is Droplet)
                {
                    if (hitObject.HyperDash)
                        this.DrawCircle(DropTexture, pos, (int)(circleDiameter * hitObject.Scale), new Color(1.0f, 0f, 0f, 1.0f));
                    else
                        this.DrawCircle(DropTexture, pos, (int)(circleDiameter * hitObject.Scale), new Color(1.0f, 1.0f, 1.0f, 1.0f));
                }
                else if (hitObject is Fruit)
                {
                    if (hitObject.HyperDash)
                        this.DrawCircle(hitCircleTexture, pos, circleDiameter, new Color(1.0f, 0f, 0f, 1.0f));
                    else
                        this.DrawCircle(hitCircleTexture, pos, circleDiameter, new Color(1.0f, 1.0f, 1.0f, 1.0f));
                }
                else if (hitObject is Banana)
                {
                    this.DrawCircle(BananaTexture, pos, circleDiameter, Color.Yellow);
                }
            }
            if (distanceType != DistanceType.None && (hitObject is Fruit || (hitObject is Droplet && hitObject is not TinyDroplet)))
            {
                string distanceString = wdpch.GetDistanceString(distanceType);
                if (distanceString == "") return;
                Texture2D? distanceTexture = TextureFromString(distanceString, fontScale);
                if (distanceTexture == null) return;
                if (distanceString.Length > 0) this.DrawDistance(distanceTexture, pos, circleDiameter, Color.LightBlue);
                distanceTexture.Dispose();
            }
        }

        private void DrawDistance(Texture2D? texture, Vector2 notePos, int diameter, Color color)
        {
            if (texture == null) return;
            Vector2 labelPosStart = notePos;
            labelPosStart.X += diameter / 2;
            labelPosStart.Y -= diameter / 2;

            float textureRightX = labelPosStart.X + texture.Width;
            if (textureRightX > 640) labelPosStart.X -= diameter + texture.Width;
            labelPosStart.Y += ((float)diameter - texture.Height) / 2;
            texture.Draw(labelPosStart, new Vector2(0, 0), color);
        }

        private void DrawLabel(Texture2D? texture, Vector2 pos, bool isLeft, Color color)
        {
            if (texture == null) return;
            if (isLeft) texture.Draw(pos, new Vector2(30, 0), color);
            else texture.Draw(pos, new Vector2(texture.Width - 30, 0), color);
        }

        private void DrawCircle(Texture2D? texture, Vector2 pos, int diameter, Color color)
        {
            if (texture == null) return;
            texture.Draw(pos, diameter, diameter, new Vector2(diameter * 0.5f), color);
        }

        private void DrawHyperDashCircle(Texture2D? texture, Vector2 pos, int diameter)
        {
            if (texture == null) return;
            texture.Draw(pos, diameter * 1.4f, diameter * 1.4f, new Vector2(diameter * 1.4f * 0.5f), Color.Red);
        }

        private void DrawJudgementLine()
        {
            if (screensContain > 1)
            {
                Vector2 rp0 = new Vector2(64, (float)(240.0 * this.screensContain));
                Vector2 rp1 = new Vector2(576, (float)(240.0 * this.screensContain));
                DrawLine(rp0, rp1, Color.White);
            }
            else
            {
                Vector2 rp0 = new Vector2(64, 384);
                Vector2 rp1 = new Vector2(576, 384);
                DrawLine(rp0, rp1, Color.White);
            }
        }

        public void DrawTimingPoints(List<TimingControlPoint> timingControlPoints)
        {
            if (viewerManager == null) return;
            timingControlPoints.ForEach(timingControlPoint =>
            {
                if (timingControlPoint.Time < 0 || timingControlPoint.BPM <= 0) return;
                double deltaTime = timingControlPoint.Time - viewerManager.currentTime;
                if (screensContain > 1)
                {
                    double timeSpan = screensContain * viewerManager.ApproachTime * 1.25;
                    if (deltaTime <= timeSpan && deltaTime >= -timeSpan)
                    {
                        int posY = (int)(240.0 * this.screensContain - deltaTime / this.viewerManager.TimePerPixels);
                        Vector2 rp0 = new Vector2(64, posY);
                        Vector2 rp1 = new Vector2(576, posY);
                        DrawLine(rp0, rp1, Color.Red);
                        Texture2D? BPMTexture = TextureFromString(timingControlPoint.BPM.ToString("F0"), fontScale);
                        if (BPMTexture == null) return;
                        this.DrawLabel(BPMTexture, rp0, true, Color.Red);
                        BPMTexture.Dispose();
                    }
                }
                else
                {
                    double upTime = this.viewerManager.ApproachTime;
                    double bottomTime = this.viewerManager.ApproachTime / 4;
                    if (deltaTime <= upTime && deltaTime >= -bottomTime)
                    {
                        int posY = (int)(384 - deltaTime / this.viewerManager.TimePerPixels);
                        Vector2 rp0 = new Vector2(64, posY);
                        Vector2 rp1 = new Vector2(576, posY);
                        DrawLine(rp0, rp1, Color.Red);
                        Texture2D? BPMTexture = TextureFromString(timingControlPoint.BPM.ToString("F0"), fontScale);
                        if (BPMTexture == null) return;
                        this.DrawLabel(BPMTexture, rp0, true, Color.Red);
                        BPMTexture.Dispose();
                    }
                }
            });
        }

        public void DrawDifficultyControPoints(List<DifficultyControlPoint> difficultyControlPoints)
        {
            if (viewerManager == null) return;
            difficultyControlPoints.ForEach(difficultyControlPoint =>
            {
                if (difficultyControlPoint.Time < 0 || difficultyControlPoint.SliderVelocity <= 0) return;
                double deltaTime = difficultyControlPoint.Time - viewerManager.currentTime;
                if (screensContain > 1)
                {
                    double timeSpan = screensContain * viewerManager.ApproachTime * 1.25;
                    if (deltaTime <= timeSpan && deltaTime >= -timeSpan)
                    {
                        int posY = (int)(240.0 * this.screensContain - deltaTime / this.viewerManager.TimePerPixels);
                        Vector2 rp0 = new Vector2(64, posY);
                        Vector2 rp1 = new Vector2(576, posY);
                        DrawLine(rp0, rp1, Color.LightGreen);
                        Texture2D? BPMTexture = TextureFromString(difficultyControlPoint.SliderVelocity.ToString("F2"), fontScale);
                        if (BPMTexture == null) return;
                        this.DrawLabel(BPMTexture, rp1, false, Color.LightGreen);
                        BPMTexture.Dispose();
                    }
                }
                else
                {
                    double upTime = this.viewerManager.ApproachTime;
                    double bottomTime = this.viewerManager.ApproachTime / 4;
                    if (deltaTime <= upTime && deltaTime >= -bottomTime)
                    {
                        int posY = (int)(384 - deltaTime / this.viewerManager.TimePerPixels);
                        Vector2 rp0 = new Vector2(64, posY);
                        Vector2 rp1 = new Vector2(576, posY);
                        DrawLine(rp0, rp1, Color.LightGreen);
                        Texture2D? BPMTexture = TextureFromString(difficultyControlPoint.SliderVelocity.ToString("F2"), fontScale);
                        if (BPMTexture == null) return;
                        this.DrawLabel(BPMTexture, rp1, false, Color.LightGreen);
                        BPMTexture.Dispose();
                    }
                }
            });
        }

        public void DrawBarLines(List<BarLine> barLines)
        {
            if (viewerManager == null) return;
            barLines.ForEach(barLine =>
            {
                if (barLine.StartTime < 0) return;
                double deltaTime = barLine.StartTime - viewerManager.currentTime;
                if (screensContain > 1)
                {
                    double timeSpan = screensContain * viewerManager.ApproachTime * 1.25;
                    if (deltaTime <= timeSpan && deltaTime >= -timeSpan)
                    {
                        int posY = (int)(240.0 * this.screensContain - deltaTime / this.viewerManager.TimePerPixels);
                        Vector2 rp0 = new Vector2(64, posY);
                        Vector2 rp1 = new Vector2(576, posY);
                        if (barLine.Major) DrawLine(rp0, rp1, Color.LightGray);
                        else DrawLine(rp0, rp1, Color.Gray);
                    }
                }
                else
                {
                    double upTime = this.viewerManager.ApproachTime;
                    double bottomTime = this.viewerManager.ApproachTime / 4;
                    if (deltaTime <= upTime && deltaTime >= -bottomTime)
                    {
                        int posY = (int)(384 - deltaTime / this.viewerManager.TimePerPixels);
                        Vector2 rp0 = new Vector2(64, posY);
                        Vector2 rp1 = new Vector2(576, posY);
                        if (barLine.Major) DrawLine(rp0, rp1, Color.LightGray);
                        else DrawLine(rp0, rp1, Color.Gray);
                    }
                }
            });
        }

    }
}
