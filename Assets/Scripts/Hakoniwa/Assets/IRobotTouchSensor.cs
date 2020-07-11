using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakoniwa.Assets
{
    public interface IRobotTouchSensor: IRobotSensor
    {
        bool IsPressed();
    }
}
