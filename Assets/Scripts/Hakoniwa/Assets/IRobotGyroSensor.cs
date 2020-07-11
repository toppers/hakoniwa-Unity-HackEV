using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets;

namespace Hakoniwa.Assets
{
    public interface IRobotGyroSensor : IRobotSensor
    {
        float GetDegree();
        float GetDegreeRate();
        void ClearDegree();
    }
}
