using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets;

namespace Hakoniwa.Assets
{
    public enum LedColor
    {
        LED_COLOR_NONE = 0,
        LED_COLOR_RED = 1,
        LED_COLOR_GREEN = 2,
        LED_COLOR_ORANGE = 3,
    }

    public interface IRobotLed : IRobotActuator
    {
        void SetLedColor(LedColor color);
    }
}

