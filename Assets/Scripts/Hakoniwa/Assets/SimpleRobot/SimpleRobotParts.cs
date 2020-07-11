using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets.EV3;

namespace Hakoniwa.Assets.SimpleRobot

{
    public class SimpleRobotParts : MonoBehaviour, IEV3Parts
    {
        private string motor_a = "Tire1";
        private string motor_b = "Tire2";
        private string color_sensor = "Axle/SensorHolder/SensorBox/ColorSensor";
        private string ultra_sonic_sensor = "Axle/SensorHolder/SensorBox/UltrasonicSensor";
        private string touch_sensor = "Axle/TouchSensor";

        public string GetColorSensor()
        {
            return color_sensor;
        }

        public string GetMotorA()
        {
            return motor_a;
        }

        public string GetMotorB()
        {
            return motor_b;
        }

        public string getUltraSonicSensor()
        {
            return ultra_sonic_sensor;
        }
        public string getTouchSensor()
        {
            return touch_sensor;
        }
    }
}
