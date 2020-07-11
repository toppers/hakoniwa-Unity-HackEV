using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets.EV3;

namespace Hakoniwa.Assets.HackEV
{
    public class HackEVParts : MonoBehaviour, IEV3Parts
    {
        private string motor_a = "RoboModel_Axis/HackEV_L8_LeftMotor/HackEV_L8_Wheel2/L8_Tire_Bk";
        private string motor_b = "RoboModel_Axis/HackEV_L8_RightMotor/HackEV_L8_Wheel/L8_Tire_Bk 1";
        private string color_sensor = "RoboModel_Axis/HackEV_L6_RC1_LightSensor/EV3_ColorSensor/EV3_ColorSensor_Wh06/ColorSensor";
        private string ultra_sonic_sensor = "RoboModel_Axis/HackEV_L8_Sidewinder_Head/EV3_UltrasonicSensor/EV3_UltrasonicSensor_Bk10";

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
            return null;
        }

        public string getGyroSensor()
        {
            throw null;
        }
    }
}
