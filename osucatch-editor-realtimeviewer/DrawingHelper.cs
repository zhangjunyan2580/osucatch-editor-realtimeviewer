using OpenTK.Graphics;
using OpenTK;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Catch.Objects;
using osu.Game.Rulesets.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osucatch_editor_realtimeviewer
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

    public class DrawingHelper
    {
        public float CurrentTime { get; set; }
        public ControlPointInfo? ControlPointInfo { get; set; }
        List<BarLine> BarLines { get; set; }
        public List<WithDistancePalpableCatchHitObject> CatchHitObjects { get; set; }
        public List<WithDistancePalpableCatchHitObject> NearbyHitObjects { get; set; }
        public int ApproachTime { get; set; }
        public float FruitSpeed { get; set; }
        public float TimePerPixels { get; set; }
        private int CircleDiameter { get; set; }
        public DistanceType DistanceType { get; set; }
        public List<Color4> CustomComboColours { get; set; }

        public List<Color4> DefaultCustomComboColours = new() {
            new (255, 191, 191, 255),
            new (128, 191, 255, 255),
            new (128, 255, 128, 255),
            new (191, 128, 255, 255),
            new (128, 255, 255, 255),
            new (255, 128, 255, 255),
        };

        public int ScreensContain { get; set; }

        public DrawingHelper()
        {
            ScreensContain = 4;
            CurrentTime = 0;
            DistanceType = DistanceType.None;
            CatchHitObjects = new List<WithDistancePalpableCatchHitObject> { };
            NearbyHitObjects = new List<WithDistancePalpableCatchHitObject> { };
            BarLines = new List<BarLine> { };
            CustomComboColours = DefaultCustomComboColours;
        }

        public void LoadBeatmap(IBeatmap convertedBeatmap, int mods = 0)
        {
            ControlPointInfo = convertedBeatmap.ControlPointInfo;
            BarLines = convertedBeatmap.BarLines;
            CatchHitObjects = BeatmapConverter.GetPalpableObjects(convertedBeatmap, (DistanceType != DistanceType.None));

            float moddedAR = convertedBeatmap.Difficulty.ApproachRate;
            ApproachTime = (int)((moddedAR < 5) ? 1800 - moddedAR * 120 : 1200 - (moddedAR - 5) * 150);
            FruitSpeed = 384 / ApproachTime;
            TimePerPixels = ApproachTime / 384.0f;
            float moddedCS = convertedBeatmap.Difficulty.CircleSize;
            CircleDiameter = (int)(108.848 - moddedCS * 8.9646);
            CustomComboColours = convertedBeatmap.CustomComboColours;
            if (CustomComboColours.Count <= 0) CustomComboColours = DefaultCustomComboColours;
        }

        public void Draw()
        {
            BuildNearby();

            List<TimingControlPoint> timingControlPoints = new List<TimingControlPoint>();
            List<DifficultyControlPoint> difficultyControlPoints = new List<DifficultyControlPoint>();

            double MaxStartTime = -1;

            for (int b = NearbyHitObjects.Count - 1; b >= 0; b--)
            {
                WithDistancePalpableCatchHitObject hitObject = NearbyHitObjects[b];

                if (MaxStartTime < 0 || hitObject.currentObject.StartTime > MaxStartTime) MaxStartTime = hitObject.currentObject.StartTime;

                double deltaTime = hitObject.currentObject.StartTime - CurrentTime;
                if (ScreensContain > 1)
                {
                    double timeSpan = ScreensContain * ApproachTime * 1.25;
                    if (deltaTime <= timeSpan && deltaTime >= -timeSpan)
                    {
                        this.DrawHitcircle(hitObject, deltaTime);
                    }
                }
                else
                {
                    double upTime = ApproachTime + CircleDiameter * TimePerPixels;
                    double bottomTime = ApproachTime / 4 + CircleDiameter * TimePerPixels;
                    if (deltaTime <= upTime && deltaTime >= -bottomTime)
                    {
                        this.DrawHitcircle(hitObject, deltaTime);
                    }
                }


                if (app.Default.TimingLine_ShowRed && ControlPointInfo != null)
                {
                    var timingControlPoint = hitObject.GetTimingPoint(ControlPointInfo);
                    timingControlPoints.Add(timingControlPoint);
                }
                if (app.Default.TimingLine_ShowGreen && ControlPointInfo != null)
                {
                    var difficultyControlPoint = hitObject.GetDifficultyControlPoint(ControlPointInfo);
                    difficultyControlPoints.Add(difficultyControlPoint);
                }
            }


            if (app.Default.BarLine_Show)
            {
                List<BarLine> barLines = BarLines.Where((barLine) => barLine.StartTime >= 0 && barLine.StartTime <= MaxStartTime + 1).ToList();
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

        public void DrawBarLines(List<BarLine> barLines)
        {
            barLines.ForEach(barLine =>
            {
                if (barLine.StartTime < 0) return;
                double deltaTime = barLine.StartTime - CurrentTime;
                if (ScreensContain > 1)
                {
                    double timeSpan = ScreensContain * ApproachTime * 1.25;
                    if (deltaTime <= timeSpan && deltaTime >= -timeSpan)
                    {
                        int posY = (int)(240.0 * ScreensContain - deltaTime / TimePerPixels);
                        Vector2 rp0 = new Vector2(64, posY);
                        Vector2 rp1 = new Vector2(576, posY);
                        if (barLine.Major) Canvas.DrawLine(rp0, rp1, Color.LightGray);
                        else Canvas.DrawLine(rp0, rp1, Color.Gray);
                    }
                }
                else
                {
                    double upTime = ApproachTime;
                    double bottomTime = ApproachTime / 4;
                    if (deltaTime <= upTime && deltaTime >= -bottomTime)
                    {
                        int posY = (int)(384 - deltaTime / TimePerPixels);
                        Vector2 rp0 = new Vector2(64, posY);
                        Vector2 rp1 = new Vector2(576, posY);
                        if (barLine.Major) Canvas.DrawLine(rp0, rp1, Color.LightGray);
                        else Canvas.DrawLine(rp0, rp1, Color.Gray);
                    }
                }
            });
        }

        public void DrawTimingPoints(List<TimingControlPoint> timingControlPoints)
        {
            timingControlPoints.ForEach(timingControlPoint =>
            {
                if (timingControlPoint.Time < 0 || timingControlPoint.BPM <= 0) return;
                double deltaTime = timingControlPoint.Time - CurrentTime;
                if (ScreensContain > 1)
                {
                    double timeSpan = ScreensContain * ApproachTime * 1.25;
                    if (deltaTime <= timeSpan && deltaTime >= -timeSpan)
                    {
                        int posY = (int)(240.0 * ScreensContain - deltaTime / TimePerPixels);
                        Canvas.DrawBPMLabel(timingControlPoint.BPM, posY);
                    }
                }
                else
                {
                    double upTime = ApproachTime;
                    double bottomTime = ApproachTime / 4;
                    if (deltaTime <= upTime && deltaTime >= -bottomTime)
                    {
                        int posY = (int)(384 - deltaTime / TimePerPixels);
                        Canvas.DrawBPMLabel(timingControlPoint.BPM, posY);
                    }
                }
            });
        }

        public void DrawDifficultyControPoints(List<DifficultyControlPoint> difficultyControlPoints)
        {
            difficultyControlPoints.ForEach(difficultyControlPoint =>
            {
                if (difficultyControlPoint.Time < 0 || difficultyControlPoint.SliderVelocity <= 0) return;
                double deltaTime = difficultyControlPoint.Time - CurrentTime;
                if (ScreensContain > 1)
                {
                    double timeSpan = ScreensContain * ApproachTime * 1.25;
                    if (deltaTime <= timeSpan && deltaTime >= -timeSpan)
                    {
                        int posY = (int)(240.0 * ScreensContain - deltaTime / TimePerPixels);
                        Canvas.DrawSVLabel(difficultyControlPoint.SliderVelocity, posY);
                    }
                }
                else
                {
                    double upTime = ApproachTime;
                    double bottomTime = ApproachTime / 4;
                    if (deltaTime <= upTime && deltaTime >= -bottomTime)
                    {
                        int posY = (int)(384 - deltaTime / TimePerPixels);
                        Canvas.DrawSVLabel(difficultyControlPoint.SliderVelocity, posY);
                    }
                }
            });
        }

        private void DrawHitcircle(WithDistancePalpableCatchHitObject wdpch, double deltaTime)
        {
            PalpableCatchHitObject hitObject = wdpch.currentObject;
            double baseY = (ScreensContain <= 1) ? 384 : 240.0 * this.ScreensContain;
            Vector2 pos = new Vector2(64 + hitObject.EffectiveX, (float)(baseY - deltaTime / TimePerPixels));
            bool withColor = app.Default.Combo_Colour;
            int comboColorIndex = (hitObject.ComboIndex) % CustomComboColours.Count;
            Color4 color = CustomComboColours[comboColorIndex];

            if (hitObject is TinyDroplet) Canvas.DrawTinyDroplet(pos, CircleDiameter, hitObject.Scale, color, withColor, hitObject.HyperDash);
            else if (hitObject is Droplet) Canvas.DrawDroplet(pos, CircleDiameter, hitObject.Scale, color, withColor, hitObject.HyperDash);
            else if (hitObject is Fruit) Canvas.DrawFruit(pos, CircleDiameter, color, withColor, hitObject.HyperDash);
            else if (hitObject is Banana) Canvas.DrawBanana(pos, CircleDiameter);

            if (DistanceType != DistanceType.None && (hitObject is Fruit || (hitObject is Droplet && hitObject is not TinyDroplet)))
            {
                string distanceString = wdpch.GetDistanceString(DistanceType);
                Canvas.DrawDistanceLabel(distanceString, pos, CircleDiameter);
            }
        }

        public void BuildNearby()
        {
            NearbyHitObjects.Clear();
            if (this.CatchHitObjects == null)
            {
                throw new Exception("Please LoadBeatmap before Drawing.");
            }
            double timeSpan = ScreensContain * ApproachTime * 1.25 + CircleDiameter * TimePerPixels * 2;
            int startIndex = (ScreensContain <= 1) ? this.HitObjectsLowerBound(CurrentTime - ApproachTime / 4 - CircleDiameter * TimePerPixels) : this.HitObjectsLowerBound(CurrentTime - timeSpan / 2);
            int endIndex = (ScreensContain <= 1) ? this.HitObjectsUpperBound(CurrentTime + ApproachTime + CircleDiameter * TimePerPixels) : this.HitObjectsUpperBound(CurrentTime + timeSpan / 2);
            // Console.WriteLine(startIndex + "->" + endIndex);
            for (int k = startIndex; k <= endIndex; k++)
            {
                if (k < 0)
                {
                    continue;
                }
                else if (k >= this.CatchHitObjects.Count)
                {
                    break;
                }
                this.NearbyHitObjects.Add(this.CatchHitObjects[k]);
            }
        }

        private int HitObjectsLowerBound(double target)
        {
            if (this.CatchHitObjects == null) return 0;
            int first = 0;
            int last = this.CatchHitObjects.Count - 1;
            int count = last - first;
            while (count > 0)
            {
                int step = count / 2;
                int it = first + step;
                var hitObject = this.CatchHitObjects[it];
                float endTime = (float)hitObject.currentObject.StartTime;
                if (endTime < target)
                {
                    first = ++it;
                    count -= step + 1;
                }
                else
                {
                    count = step;
                }
            }
            return first;
        }

        private int HitObjectsUpperBound(double target)
        {
            if (this.CatchHitObjects == null) return 0;
            int first = 0;
            int last = this.CatchHitObjects.Count - 1;
            int count = last - first;
            while (count > 0)
            {
                int step = count / 2;
                int it = first + step;
                float startTime = (float)(this.CatchHitObjects[it].currentObject.StartTime);
                if (!(target < startTime))
                {
                    first = ++it;
                    count -= step + 1;
                }
                else
                {
                    count = step;
                }
            }
            return first;
        }
    }


}
