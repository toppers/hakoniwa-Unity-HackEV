using Hakoniwa.PluggableAsset.Assets.Robot.EV3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakoniwa.PluggableAsset.Assets.Robot.EV3.TurtleBot3
{
    public class TurtleBot3Parts : MonoBehaviour, IEV3Parts
    {
        private string motor_a = "Phys/RightTire";
        private string motor_b = "Phys/LeftTire";
        private string color_sensor0 = null;
        private string color_sensor1 = null;
        private string ultra_sonic_sensor = null;
        private string gyro_sensor = null;
        private string motor_arm = null;
        private string touch_sensor0 = null;
        private string touch_sensor1 =null;
        private string led = null;
        private string gps = "Phys/Axis";

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
        public string getTouchSensor0()
        {
            return touch_sensor0;
        }

        public string getGyroSensor()
        {
            return gyro_sensor;
        }

        public string GetMotorC()
        {
            return motor_arm;
        }

        string IEV3Parts.getTouchSensor1()
        {
            return touch_sensor1;
        }
        public string GetLed()
        {
            return led;
        }
        public string getGpsSensor()
        {
            return gps;
        }

        public string GetColorSensor0()
        {
            return color_sensor0;
        }

        public string GetColorSensor1()
        {
            return color_sensor1;
        }
    }
}
