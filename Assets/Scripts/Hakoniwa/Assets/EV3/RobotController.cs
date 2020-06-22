using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Hakoniwa.Core;

namespace Hakoniwa.Assets.EV3
{

    public class RobotController : MonoBehaviour, IAssetController
    {
        public int powerConst = 10;
        public int motorPower = 100;
        private GameObject root;
        private GameObject myObject;
        private Hakoniwa.Assets.IRobotMotor motor_a;
        private Hakoniwa.Assets.IRobotMotor motor_b;
        private Hakoniwa.Assets.IRobotMotorSensor motor_a_sensor;
        private Hakoniwa.Assets.IRobotMotorSensor motor_b_sensor;
        private Hakoniwa.Assets.IRobotColorSensor colorSensor;
        private Hakoniwa.Assets.IRobotUltraSonicSensor ultrasonicSensor;
        private bool isConnected;
        private ulong micon_simtime;
        private Hakoniwa.Assets.HackEV.HackEVParts parts;

        IoWriter writer;
        IoReader reader;

        public bool IsConnected()
        {
            return isConnected;
        }
        public long GetControllerTime()
        {
            return (long)this.micon_simtime;
        }
        public void DoUpdate()
        {
            UpdateActuator();
            UpdateSensor();
        }
        // Start is called before the first frame update
        void Start()
        {
            this.root = GameObject.Find("Robot");
            this.myObject = GameObject.Find("Robot/" + this.transform.name);
            GameObject hakoniwa = GameObject.Find("Hakoniwa");
            this.parts = hakoniwa.GetComponentInChildren<Hakoniwa.Assets.HackEV.HackEVParts>();

            this.isConnected = false;
            this.InitActuator();
            this.InitSensor();
            this.writer = this.myObject.GetComponentInChildren<IoWriter>();
            this.reader = this.myObject.GetComponentInChildren<IoReader>();
            this.reader.Initialize();
            this.reader.SetCallback(UdpServerCallback);
        }
        public void DoPublish(long hakoniwa_time)
        {
            //Debug.Log("hakoniwa_time=" + hakoniwa_time);
            this.writer.SetSimTime((ulong)hakoniwa_time);
            this.reader.DoRun();
            this.writer.Publish();
        }

        private void InitActuator()
        {
            GameObject obj = root.transform.Find(this.transform.name + "/" + this.parts.motor_a).gameObject;
            this.motor_a = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotMotor>();
            motor_a.Initialize(obj);
            this.motor_a_sensor = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotMotorSensor>();

            obj = root.transform.Find(this.transform.name + "/" + this.parts.motor_b).gameObject;
            this.motor_b = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotMotor>();
            motor_b.Initialize(obj);
            this.motor_b_sensor = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotMotorSensor>();

            motor_a.SetForce(this.motorPower);
            motor_b.SetForce(this.motorPower);
        }
        private void InitSensor()
        {
            GameObject obj;
            obj = root.transform.Find(this.transform.name + "/" + this.parts.color_sensor).gameObject;
            colorSensor = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotColorSensor>();
            colorSensor.Initialize(obj);

            obj = root.transform.Find(this.transform.name + "/" + this.parts.ultra_sonic_sensor).gameObject;
            ultrasonicSensor = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotUltraSonicSensor>();
            ultrasonicSensor.Initialize(obj);
        }
        private void UpdateSensor()
        {
            motor_a_sensor.UpdateSensorValues();
            motor_b_sensor.UpdateSensorValues();

            this.writer.Set("motor_angle_a", (int)motor_a_sensor.GetDegree());
            this.writer.Set("motor_angle_b", (int)motor_b_sensor.GetDegree());

            colorSensor.UpdateSensorValues();
            ultrasonicSensor.UpdateSensorValues();

            this.writer.Set("sensor_ultrasonic", (int)(this.ultrasonicSensor.GetDistanceValue() * 10));
            this.writer.Set("sensor_reflect", (int)(this.colorSensor.GetLightValue() * 100f));
            ColorRGB color_sensor_rgb;
            this.colorSensor.GetRgb(out color_sensor_rgb);
            this.writer.Set("sensor_rgb_r", color_sensor_rgb.r);
            this.writer.Set("sensor_rgb_g", color_sensor_rgb.g);
            this.writer.Set("sensor_rgb_b", color_sensor_rgb.b);
            this.writer.Set("sensor_color", (int)this.colorSensor.GetColorId());
        }

        private void UdpServerCallback()
        {
            this.reader.RefSimTime(0, out this.micon_simtime);
            if (this.micon_simtime == 0)
            {
                return;
            }
            //Debug.Log("micon_stime=" + this.micon_simtime);
            if (this.isConnected == false)
            {
                this.isConnected = true;
            }
            int reset = 0;
            this.reader.RefData("motor_reset_angle_a", out reset);
            if (reset != 0)
            {
                this.motor_a_sensor.ClearDegree();
                Debug.Log("reset tire1");
            }
            this.reader.RefData("motor_reset_angle_b", out reset);
            if (reset != 0)
            {
                this.motor_b_sensor.ClearDegree();
                Debug.Log("reset tire2");
            }
        }

        private void UpdateActuator()
        {
            int power_a = 0;
            int power_b = 0;
            int isStop_a = 0;
            int isStop_b = 0;

            this.reader.RefData("motor_power_a", out power_a);
            this.reader.RefData("motor_power_b", out power_b);

            this.reader.RefData("motor_stop_a", out isStop_a);
            this.reader.RefData("motor_stop_b", out isStop_b);

            this.motor_a.SetTargetVelicty(power_a * powerConst);
            this.motor_b.SetTargetVelicty(power_b * powerConst);
        }
    }

}