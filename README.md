# Osucatch Editor Realtime Viewer

A realtime beatmap viewer for beatmap editing (osu!stable editor) in osu!catch.

## Workflow

Editor --(Editor Reader)--> Beatmap --(Osu BeatmapParser)--> HitObjects --(OpenGL)--> Frames

## Used Packages

### for beatmap parser

- System.Numerics.Tensors

### for drawing

- OpenTK
- OpenTK.GLControl

## Used Source Code

### for reading editor

- [Editor Reader](https://github.com/Karoo13/EditorReader)

### for parsing beatmap

- [osu](https://github.com/ppy/osu)
- [osu-framework](https://github.com/ppy/osu-framework)
- [osuTK](https://github.com/ppy/osuTK)
