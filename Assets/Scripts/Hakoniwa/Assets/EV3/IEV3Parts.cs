using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakoniwa.Assets.EV3
{
    public interface IEV3Parts
    {
        string GetMotorA();
        string GetMotorB();
        string GetColorSensor();
        string getUltraSonicSensor();
    }
}
