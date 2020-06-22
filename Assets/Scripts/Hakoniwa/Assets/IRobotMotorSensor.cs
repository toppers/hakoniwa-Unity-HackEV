using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets;

namespace Hakoniwa.Assets
{
    public interface IRobotMotorSensor : IRobotSensor
    {
        float GetDegree();
        void ClearDegree();
    }
}
