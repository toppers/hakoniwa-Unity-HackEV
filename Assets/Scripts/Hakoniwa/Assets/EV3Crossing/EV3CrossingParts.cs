using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets.EV3;

namespace Hakoniwa.Assets.EV3Crossing
{
    public class EV3CrossingParts : MonoBehaviour, IEV3Parts
    {
        private string motor_arm = "EV3GuardHolder";

        public string GetColorSensor()
        {
            return null;
        }

        public string getGyroSensor()
        {
            return null;
        }

        public string GetMotorA()
        {
            return null;
        }

        public string GetMotorB()
        {
            return null;
        }

        public string GetMotorC()
        {
            return motor_arm;
            //return null;
        }

        public string getTouchSensor0()
        {
            return null;
        }

        public string getTouchSensor1()
        {
            return null;
        }

        public string getUltraSonicSensor()
        {
            return null;
        }
        public string GetLed()
        {
            return null;
        }
        public string getGpsSensor()
        {
            return null;
        }
    }

}


