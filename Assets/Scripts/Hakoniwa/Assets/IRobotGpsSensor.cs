using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets;

namespace Hakoniwa.Assets
{
    public interface IRobotGpsSensor : IRobotSensor
    {
        double GeLatitude();
        double GetLongitude();
        int GetStatus();
    }
}
