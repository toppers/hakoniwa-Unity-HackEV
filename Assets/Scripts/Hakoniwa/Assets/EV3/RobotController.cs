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
        public int armMotorConst = 10;
        public int armMotorPower = 100;
        private GameObject root;
        private GameObject myObject;
        private Hakoniwa.Assets.IRobotMotor motor_a;
        private Hakoniwa.Assets.IRobotMotor motor_b;
        private Hakoniwa.Assets.IRobotMotor motor_arm;
        private Hakoniwa.Assets.IRobotMotorSensor motor_a_sensor;
        private Hakoniwa.Assets.IRobotMotorSensor motor_b_sensor;
        private Hakoniwa.Assets.IRobotMotorSensor motor_arm_sensor;
        private Hakoniwa.Assets.IRobotColorSensor colorSensor;
        private Hakoniwa.Assets.IRobotUltraSonicSensor ultrasonicSensor;
        private Hakoniwa.Assets.IRobotTouchSensor touchSensor0;
        private Hakoniwa.Assets.IRobotTouchSensor touchSensor1;
        private Hakoniwa.Assets.IRobotGyroSensor gyroSensor;
        private bool isConnected;
        private ulong micon_simtime;
        private IEV3Parts parts;
        private HakoniwaRobotConfigInfo robotConfig;

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
        public void Initialize()
        {
            this.root = GameObject.Find("Robot");
            this.myObject = GameObject.Find("Robot/" + this.transform.name);
            this.parts = myObject.GetComponentInChildren<Hakoniwa.Assets.EV3.IEV3Parts>();

            this.isConnected = false;
            this.InitActuator();
            this.InitSensor();

            GameObject hakoniwa = GameObject.Find("Hakoniwa");
            HakoniwaConfig hakoniwa_cfg = hakoniwa.GetComponentInChildren<Hakoniwa.Core.HakoniwaConfig>();
            if (hakoniwa_cfg == null)
            {
                UnityEngine.Debug.LogError("Not found hakoniwa_cfg : " + hakoniwa_cfg);
                return;
            }
            this.writer = this.myObject.GetComponentInChildren<IoWriter>();
            this.reader = this.myObject.GetComponentInChildren<IoReader>();
            this.robotConfig = hakoniwa_cfg.GetRobotConfig(this.transform.name);
            if (robotConfig == null)
            {
                UnityEngine.Debug.LogError("Not found hakoniwa_robot_cfg : " + robotConfig);
                return;
            }
            if (hakoniwa_cfg.IsMmap(robotConfig))
            {
                this.writer.filepath = hakoniwa_cfg.GetRobotMmapWriterFilePath(this.transform.name);
                this.reader.filepath = hakoniwa_cfg.GetRobotMmapReaderFilePath(this.transform.name);
                this.writer.InitializeMmap();
                this.reader.InitializeMmap();
            }
            else
            {
                this.writer.host = hakoniwa_cfg.GetRobotConfig(this.transform.name).Udp.AthrillIpAddr;
                this.writer.port = hakoniwa_cfg.GetRobotConfig(this.transform.name).Udp.AthrillPort;
                this.reader.port = hakoniwa_cfg.GetRobotConfig(this.transform.name).Udp.UnityPort;
                this.writer.Initialize();
                this.reader.Initialize();
            }
            this.reader.SetCallback(UdpServerCallback);
        }
        public void DoPublish(long hakoniwa_time)
        {
            //Debug.Log("hakoniwa_time=" + hakoniwa_time);
            this.writer.SetSimTime((ulong)hakoniwa_time);
            this.reader.DoRun();

            if (this.IsConnected())
            {
                this.writer.Publish();
            }
        }

        private void InitActuator()
        {
            GameObject obj;
            string subParts = this.parts.GetMotorA();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + this.parts.GetMotorA()).gameObject;
                this.motor_a = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotMotor>();
                motor_a.Initialize(obj);
                this.motor_a_sensor = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotMotorSensor>();
            }
            subParts = this.parts.GetMotorB();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + this.parts.GetMotorB()).gameObject;
                this.motor_b = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotMotor>();
                motor_b.Initialize(obj);
                this.motor_b_sensor = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotMotorSensor>();
            }
            subParts = this.parts.GetMotorC();
            if (subParts != null)
            {
                //Debug.Log("parts=" + this.parts.GetMotorC());
                if (root.transform.Find(this.transform.name + "/" + this.parts.GetMotorC()) != null)
                {
                    obj = root.transform.Find(this.transform.name + "/" + this.parts.GetMotorC()).gameObject;
                    this.motor_arm = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotMotor>();
                    motor_arm.Initialize(obj);
                    this.motor_arm_sensor = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotMotorSensor>();
                    motor_arm.SetForce(this.motorPower);
                }
            }

            if (this.motor_a != null)
            {
                motor_a.SetForce(this.motorPower);
            }
            if (this.motor_b != null)
            {
                motor_b.SetForce(this.motorPower);
            }
        }
        private void InitSensor()
        {
            GameObject obj;
            string subParts = this.parts.GetColorSensor();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + this.parts.GetColorSensor()).gameObject;
                colorSensor = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotColorSensor>();
                colorSensor.Initialize(obj);
            }
            subParts = this.parts.getUltraSonicSensor();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + this.parts.getUltraSonicSensor()).gameObject;
                ultrasonicSensor = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotUltraSonicSensor>();
                ultrasonicSensor.Initialize(obj);
            }
            subParts = this.parts.getTouchSensor0();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + this.parts.getTouchSensor0()).gameObject;
                touchSensor0 = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotTouchSensor>();
                touchSensor0.Initialize(obj);
            }
            subParts = this.parts.getTouchSensor1();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + this.parts.getTouchSensor1()).gameObject;
                touchSensor1 = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotTouchSensor>();
                touchSensor1.Initialize(obj);
            }
            subParts = this.parts.getGyroSensor();
            if (subParts != null)
            {
                if (root.transform.Find(this.transform.name + "/" + this.parts.getGyroSensor()) != null)
                {
                    obj = root.transform.Find(this.transform.name + "/" + this.parts.getGyroSensor()).gameObject;
                    gyroSensor = obj.GetComponentInChildren<Hakoniwa.Assets.IRobotGyroSensor>();
                    gyroSensor.Initialize(obj);
                }
            }
        }
        private void UpdateSensor()
        {
            if (this.motor_a_sensor != null)
            {
                motor_a_sensor.UpdateSensorValues();
                this.writer.Set("motor_angle_a", (int)motor_a_sensor.GetDegree());
            }
            if (this.motor_b_sensor != null)
            {
                motor_b_sensor.UpdateSensorValues();
                this.writer.Set("motor_angle_b", (int)motor_b_sensor.GetDegree());
            }
            if (this.motor_arm_sensor != null)
            {
                motor_arm_sensor.UpdateSensorValues();
                this.writer.Set("motor_angle_c", (int)motor_arm_sensor.GetDegree());
            }
            if (this.colorSensor != null)
            {
                colorSensor.UpdateSensorValues();
                this.writer.Set("sensor_reflect", (int)(this.colorSensor.GetLightValue() * 100f));
                ColorRGB color_sensor_rgb;
                this.colorSensor.GetRgb(out color_sensor_rgb);
                this.writer.Set("sensor_rgb_r", color_sensor_rgb.r);
                this.writer.Set("sensor_rgb_g", color_sensor_rgb.g);
                this.writer.Set("sensor_rgb_b", color_sensor_rgb.b);
                this.writer.Set("sensor_color", (int)this.colorSensor.GetColorId());
            }
            if (this.ultrasonicSensor != null)
            {
                ultrasonicSensor.UpdateSensorValues();
                this.writer.Set("sensor_ultrasonic", (int)(this.ultrasonicSensor.GetDistanceValue() * 10));
            }
            if (touchSensor0 != null)
            {
                touchSensor0.UpdateSensorValues();
                if (this.touchSensor0.IsPressed())
                {
                    //Debug.Log("Touched0:");
                    this.writer.Set("touch_sensor0", 4095);
                }
                else
                {
                    this.writer.Set("touch_sensor0", 0);
                }
            }
            if (touchSensor1 != null)
            {
                touchSensor1.UpdateSensorValues();
                if (this.touchSensor1.IsPressed())
                {
                    //Debug.Log("Touched1:");
                    this.writer.Set("touch_sensor1", 4095);
                }
                else
                {
                    this.writer.Set("touch_sensor1", 0);
                }
            }
            if (gyroSensor != null)
            {
                gyroSensor.UpdateSensorValues();
                this.writer.Set("gyro_degree", (int)gyroSensor.GetDegree());
                this.writer.Set("gyro_degree_rate", (int)gyroSensor.GetDegreeRate());
            }

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
            if ((this.motor_a_sensor != null) && (reset != 0))
            {
                this.motor_a_sensor.ClearDegree();
                Debug.Log("reset tire1");
            }
            this.reader.RefData("motor_reset_angle_b", out reset);
            if ((this.motor_b_sensor != null) && (reset != 0))
            {
                this.motor_b_sensor.ClearDegree();
                Debug.Log("reset tire2");
            }
            this.reader.RefData("motor_reset_angle_c", out reset);
            if ((this.motor_arm_sensor != null) && (reset != 0))
            {
                this.motor_arm_sensor.ClearDegree();
                Debug.Log("reset arm");
            }
            this.reader.RefData("gyro_reset", out reset);
            if ((this.gyroSensor != null) && (reset != 0))
            {
                this.gyroSensor.ClearDegree();
                //Debug.Log("reset gyro");
            }
        }

        private void UpdateActuator()
        {
            int power_a = 0;
            int power_b = 0;
            int power_c = 0;
            int isStop_a = 0;
            int isStop_b = 0;
            int isStop_c = 0;

            this.reader.RefData("motor_power_a", out power_a);
            this.reader.RefData("motor_power_b", out power_b);
            this.reader.RefData("motor_power_c", out power_c);

            this.reader.RefData("motor_stop_a", out isStop_a);
            this.reader.RefData("motor_stop_b", out isStop_b);
            this.reader.RefData("motor_stop_c", out isStop_c);

            if (this.motor_a != null)
            {
                this.motor_a.SetTargetVelicty(power_a * powerConst);
            }
            if (this.motor_b != null)
            {
                this.motor_b.SetTargetVelicty(power_b * powerConst);
            }
            if (this.motor_arm != null)
            {
                //Debug.Log("moter_c:" + power_c);
                this.motor_arm.SetTargetVelicty(power_c * this.armMotorConst);
            }
        }
    }

}