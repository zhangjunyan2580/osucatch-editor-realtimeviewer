using osuTK;

namespace osucatch_editor_realtimeviewer
{
    public partial class BeatmapConverterOsuStable
    {
        private class Segment
        {
            internal Vector2 Start;

            internal Vector2 End;

            internal float Length
            {
                get { return (End - Start).LengthStableCompat; }
            }

            internal Segment(Vector2 Start, Vector2 End)
            {
                this.Start = Start;
                this.End = End;
            }
        };

    }
}
