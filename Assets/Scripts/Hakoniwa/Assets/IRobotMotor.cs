using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakoniwa.Assets
{
    public interface IRobotMotor : IRobotActuator
    {
        void SetForce(int force);
        void SetStop(bool stop);
        void SetTargetVelicty(int targetVelocity);
    }
}
