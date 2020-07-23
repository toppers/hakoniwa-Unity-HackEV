﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets.EV3;

namespace Hakoniwa.Assets.EduRobot
{
    public class EduRobotParts : MonoBehaviour, IEV3Parts
    {
        private string motor_a = "RoboModel_Axis/EV3_LeftMotor/HackEV_L8_Wheel2/L8_Tire_Bk";
        private string motor_b = "RoboModel_Axis/EV3_RightMotor/HackEV_L8_Wheel/L8_Tire_Bk 1";
        private string color_sensor = "RoboModel_Axis/EV3_ColorSensor/EV3_ColorSensor_Wh06/ColorSensor";
        private string ultra_sonic_sensor = "RoboModel_Axis/EV3_Sidewinder_Head/EV3_UltrasonicSensor/EV3_UltrasonicSensor_Bk10";
        private string touch_sensor0 = "RoboModel_Axis/EV3_TouchSensor/TouchSensor";
        private string touch_sensor1 = "RoboModel_Axis/EV3_Bumper/Bumper_Front";

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
            return null;
        }

        public string GetMotorC()
        {
            return null;
        }

        string IEV3Parts.getTouchSensor1()
        {
            return touch_sensor1;
        }
    }
}
