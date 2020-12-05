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
        private string gyro_sensor = "RoboModel_Axis/HackEV_L8_Sidewinder_Head/EV3_GyroSensor/EV3_GyroSensor_Ash01";
        private string motor_arm = "RoboModel_Axis/HackEV_L8_MiddleMotor/EV3_InteractiveServoMotor_L/EV3_InteractiveServoMotor_L_MotorRoot";
        private string touch_sensor0 = "RoboModel_Axis/HackEV_L9_Sidewinder_ShoulderSensor/HackEV_L8_TouchSensor2/L8_DoubleBevelGear20_Bk 1";
        private string touch_sensor1 = "RoboModel_Axis/HackEV_L6_RC1_LightSensor/EV3_ColorSensor/ColorSensorCollider";
        private string led = "RoboModel_Axis/EV3_IntelligentBlock/EV3_IntelligentBlock_Root/EV3_IntelligentBlock_Ash01";
        private string gps = "RoboModel_Axis";

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
        public string getGpsSensor()
        {
            return gps;
        }
    }
}
