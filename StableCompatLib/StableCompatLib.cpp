#include <windows.h>
#include <math.h>

extern "C" {
    __declspec(dllexport) double __stdcall computeVelocity(
        double timingBeatLength,
        double sliderVelocityAsBeatLength,
        double difficultySliderTickRate,
        double sliderComboPointDistance
    )
    {
        double mult = (double) ((float) (-sliderVelocityAsBeatLength)) / 100.0;
        double beatLength = timingBeatLength * mult;
        long double t1 = (long double) sliderComboPointDistance * (long double) difficultySliderTickRate;
        long double t2 = (long double) 1000 / (long double) beatLength;
        return (double) (t1 * t2);
    }

    __declspec(dllexport) void __stdcall normalize(float x, float y, float *rx, float *ry)
    {
        double lengthSquared = (double) ((long double) x * x + (long double) y * y);
        float length = (float) sqrt(lengthSquared);
        long double mult = 1 / (long double) length;
        *rx = x * mult;
        *ry = y * mult;
    }

}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
    return TRUE;
}