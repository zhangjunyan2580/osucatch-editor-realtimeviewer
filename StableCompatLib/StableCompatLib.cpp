#include <windows.h>

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
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
    return TRUE;
}