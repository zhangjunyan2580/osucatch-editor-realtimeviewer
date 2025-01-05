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
        public ViewerManager? viewerManager;

        public float fontScale = 1;

        public float CatcherAreaHeight { get; set; }

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
                viewerManager.BuildNearby();

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
            if (w * 4 > h)
            {
                w = h / 4;
                x = (this.Size.Width - w) / 2;
            }
            if (h / 4 > w)
            {
                h = w * 4;
                y = (this.Size.Height - h) / 2;
            }
            GL.Viewport(x, y, w, h);
        }

        public void Init()
        {
            // 盘子区间
            this.CatcherAreaHeight = 384f - 350f;

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
            Vector2 border = new Vector2(1.0f, 4.0f) * 32.0f;
            GL.Ortho(-border.X, 512.0 + border.X, 2048 + border.Y, -border.Y, 0.0, 1.0);
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
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
            double MinStartTime = -1;

            for (int b = viewerManager.NearbyHitObjects.Count - 1; b >= 0; b--)
            {
                WithDistancePalpableCatchHitObject hitObject = viewerManager.NearbyHitObjects[b];

                if (MaxStartTime < 0 || hitObject.currentObject.StartTime > MaxStartTime) MaxStartTime = hitObject.currentObject.StartTime;
                if (MinStartTime < 0 || hitObject.currentObject.StartTime < MinStartTime) MinStartTime = hitObject.currentObject.StartTime;
                float diff = (float)(hitObject.currentObject.StartTime - viewerManager.currentTime);
                // 0=在顶端 1=在判定线上 >1=超过判定线
                float alpha = 1.0f;
                if (diff < viewerManager.ApproachTime * viewerManager.State_ARMul && diff > -(viewerManager.ApproachTime * (viewerManager.State_ARMul + 1)))
                {
                    alpha = 1 - (diff / (float)viewerManager.ApproachTime);
                    this.DrawHitcircle(hitObject, alpha, circleDiameter, viewerManager.DistanceType);
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
            if (app.Default.TimingLine_ShowRed)
            {
                timingControlPoints = timingControlPoints.Distinct().ToList();
                DrawTimingPoints(timingControlPoints);
            }
            if (app.Default.TimingLine_ShowGreen)
            {
                difficultyControlPoints = difficultyControlPoints.Distinct().ToList();
                DrawDifficultyControPoints(difficultyControlPoints);
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

        private void DrawHitcircle(WithDistancePalpableCatchHitObject wdpch, float alpha, int circleDiameter, DistanceType distanceType)
        {
            PalpableCatchHitObject hitObject = wdpch.currentObject;
            Vector2 pos = new Vector2(hitObject.EffectiveX, 384 * alpha - this.CatcherAreaHeight + 640);
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
            if (textureRightX > 512) labelPosStart.X -= diameter + texture.Width;
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
            Vector2 rp0 = new Vector2(0, 384 - CatcherAreaHeight + 640);
            Vector2 rp1 = new Vector2(512, 384 - CatcherAreaHeight + 640);
            DrawLine(rp0, rp1, Color.White);
        }

        public void DrawTimingPoints(List<TimingControlPoint> timingControlPoints)
        {
            if (viewerManager == null) return;
            timingControlPoints.ForEach(timingControlPoint =>
            {
                if (timingControlPoint.Time < 0 || timingControlPoint.BPM <= 0) return;
                float diff = (float)(timingControlPoint.Time - viewerManager.currentTime);
                // 0=在顶端 1=在判定线上 >1=超过判定线
                float alpha = 1.0f;
                if (diff < viewerManager.ApproachTime * viewerManager.State_ARMul && diff > -(viewerManager.ApproachTime * (viewerManager.State_ARMul + 1)))
                {
                    alpha = 1 - (diff / (float)viewerManager.ApproachTime);
                    Vector2 rp0 = new Vector2(0, 384 * alpha - this.CatcherAreaHeight + 640);
                    Vector2 rp1 = new Vector2(512, 384 * alpha - this.CatcherAreaHeight + 640);
                    DrawLine(rp0, rp1, Color.Red);
                    Texture2D? BPMTexture = TextureFromString(timingControlPoint.BPM.ToString("F0"), fontScale);
                    if (BPMTexture == null) return;
                    this.DrawLabel(BPMTexture, rp0, true, Color.Red);
                    BPMTexture.Dispose();
                }

            });
        }

        public void DrawDifficultyControPoints(List<DifficultyControlPoint> difficultyControlPoints)
        {
            if (viewerManager == null) return;
            difficultyControlPoints.ForEach(difficultyControlPoint =>
            {
                if (difficultyControlPoint.Time < 0 || difficultyControlPoint.SliderVelocity <= 0) return;
                float diff = (float)(difficultyControlPoint.Time - viewerManager.currentTime);
                // 0=在顶端 1=在判定线上 >1=超过判定线
                float alpha = 1.0f;
                if (diff < viewerManager.ApproachTime * viewerManager.State_ARMul && diff > -(viewerManager.ApproachTime * (viewerManager.State_ARMul + 1)))
                {
                    alpha = 1 - (diff / (float)viewerManager.ApproachTime);
                    Vector2 rp0 = new Vector2(0, 384 * alpha - this.CatcherAreaHeight + 640);
                    Vector2 rp1 = new Vector2(512, 384 * alpha - this.CatcherAreaHeight + 640);
                    DrawLine(rp0, rp1, Color.LightGreen);
                    Texture2D? BPMTexture = TextureFromString(difficultyControlPoint.SliderVelocity.ToString("F2"), fontScale);
                    if (BPMTexture == null) return;
                    this.DrawLabel(BPMTexture, rp1, false, Color.LightGreen);
                    BPMTexture.Dispose();
                }

            });
        }

        public void DrawBarLines(List<BarLine> barLines)
        {
            if (viewerManager == null) return;
            barLines.ForEach(barLine =>
            {
                if (barLine.StartTime < 0) return;
                float diff = (float)(barLine.StartTime - viewerManager.currentTime);
                // 0=在顶端 1=在判定线上 >1=超过判定线
                float alpha = 1.0f;
                if (diff < viewerManager.ApproachTime * viewerManager.State_ARMul && diff > -(viewerManager.ApproachTime * (viewerManager.State_ARMul + 1)))
                {
                    alpha = 1 - (diff / (float)viewerManager.ApproachTime);
                    Vector2 rp0 = new Vector2(0, 384 * alpha - this.CatcherAreaHeight + 640);
                    Vector2 rp1 = new Vector2(512, 384 * alpha - this.CatcherAreaHeight + 640);
                    if (barLine.Major) DrawLine(rp0, rp1, Color.LightGray);
                    else DrawLine(rp0, rp1, Color.Gray);
                }

            });
        }

    }
}
