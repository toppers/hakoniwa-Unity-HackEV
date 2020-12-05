using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets.EV3;

namespace Hakoniwa.Assets.HackEV
{
    public class HackEVSkeltonParts : MonoBehaviour, IEV3Parts
    {
        private string motor_a = "LeftTire";
        private string motor_b = "RightTire";
        private string color_sensor = "ColorSensor/Camera";
        private string ultra_sonic_sensor = "Axis/Body/UltraSonicSensor";
        private string gyro_sensor = null;
        private string motor_arm = "ArmMotor";
        private string touch_sensor0 = null;
        private string touch_sensor1 = "ColorSensor/FrontTouchSensor";
        private string led = "Axis/Body/LED";

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
    }
}
