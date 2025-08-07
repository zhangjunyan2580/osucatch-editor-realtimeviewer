using System.Runtime.InteropServices;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Catch.Objects;
using osu.Game.Utils;

namespace osucatch_editor_realtimeviewer
{
    public partial class BeatmapConverterOsuStable
    {
        private class HitObjectManagerCatch
        {
            private LegacyRandom random = new(1337);

            [DllImport("StableCompatLib.dll", EntryPoint = "randomNextCalc")]
            private static extern int RandomNextStableCompat0(int value, int lowerBound, int upperBound);

            private int RandomNextStableCompat(int lowerBound, int upperBound) => RandomNextStableCompat0(random.Next(), lowerBound, upperBound);

            public List<PalpableCatchHitObject> ConvertSlider(IBeatmap beatmap, JuiceStream juiceStream)
            {

                List<PalpableCatchHitObject> palpableHitObjects = new();

                palpableHitObjects.Add(new Fruit
                {
                    StartTime = juiceStream.StartTime,
                    X = juiceStream.OriginalX,
                    ComboIndex = juiceStream.ComboIndex,
                    IsSelected = juiceStream.IsSelected
                });

                LegacySliderAdditionalData sliderData = new(beatmap, juiceStream);

                int lastTime = (int)juiceStream.StartTime;

                int fruitIndex = 1;
                for (int i = 0; i < sliderData.SliderScoreTimingPoints.Count; i++)
                {
                    int time = sliderData.SliderScoreTimingPoints[i];

                    if (time - lastTime > 80)
                    {
                        float var = (time - lastTime);
                        while (var > 100)
                            var /= 2;

                        for (float j = lastTime + var; j < time; j += var)
                        {
                            TinyDroplet tinyDroplet = new TinyDroplet
                            {
                                StartTime = (int)j,
                                X = sliderData.GetPositionByTime((int)j).X + RandomNextStableCompat(-20, 20),
                                ComboIndex = juiceStream.ComboIndex,
                                IsSelected = juiceStream.IsSelected
                            };
                            palpableHitObjects.Add(tinyDroplet);
                        }
                    }

                    lastTime = time;

                    if (i < sliderData.SliderScoreTimingPoints.Count - 1)
                    {
                        int repeatLocation = sliderData.SliderRepeatPoints.BinarySearch(time);

                        if (repeatLocation >= 0)
                        {
                            palpableHitObjects.Add(new Fruit
                            {
                                StartTime = time,
                                X = repeatLocation % 2 == 1 ? juiceStream.OriginalX : sliderData.Position2.X,
                                ComboIndex = juiceStream.ComboIndex,
                                IsSelected = juiceStream.IsSelected
                            });
                            fruitIndex++;
                        }
                        else
                        {
                            random.Next();
                            palpableHitObjects.Add(new Droplet
                            {
                                StartTime = time,
                                X = sliderData.GetPositionByTime(time).X,
                                ComboIndex = juiceStream.ComboIndex,
                                IsSelected = juiceStream.IsSelected
                            });
                        }
                    }
                }

                palpableHitObjects.Add(new Fruit
                {
                    StartTime = sliderData.EndTime,
                    X = sliderData.EndPosition.X,
                    ComboIndex = juiceStream.ComboIndex,
                    IsSelected = juiceStream.IsSelected
                });

                return palpableHitObjects;
            }

            public List<PalpableCatchHitObject> ConvertSpinner(IBeatmap beatmap, BananaShower bananaShower)
            {
                List<PalpableCatchHitObject> palpableHitObjects = new List<PalpableCatchHitObject>();

                float interval = (int)bananaShower.EndTime - (int)bananaShower.StartTime;
                while (interval > 100)
                    interval /= 2;

                if (interval <= 0)
                {
                    return palpableHitObjects;
                }

                int count = 0;
                for (float currentTime = (int)bananaShower.StartTime; currentTime <= (int)bananaShower.EndTime; currentTime += interval)
                {
                    palpableHitObjects.Add(new Banana
                    {
                        StartTime = (int)currentTime,
                        OriginalX = RandomNextStableCompat(0, 512),
                        BananaIndex = count,
                        IsSelected = bananaShower.IsSelected
                    });
                    random.Next();
                    random.Next();
                    random.Next();
                    count++;
                }
                return palpableHitObjects;
            }

        }

    }
}
