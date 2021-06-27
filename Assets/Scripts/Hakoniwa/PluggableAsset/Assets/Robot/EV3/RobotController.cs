using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Hakoniwa.Core;
using Hakoniwa.PluggableAsset.Assets;
using Hakoniwa.PluggableAsset.Communication.Connector;
using Hakoniwa.PluggableAsset.Communication.Pdu;

namespace Hakoniwa.PluggableAsset.Assets.Robot.EV3
{
    public class RobotController : MonoBehaviour, IInsideAssetController
    {
        public int powerConst = 10;
        public int motorPower = 100;
        public int armMotorConst = 10;
        public int armMotorPower = 100;
        private GameObject root;
        private GameObject myObject;
        private IRobotMotor motor_a;
        private IRobotMotor motor_b;
        private IRobotMotor motor_c;
        private IRobotMotorSensor motor_a_sensor;
        private IRobotMotorSensor motor_b_sensor;
        private IRobotMotorSensor motor_arm_sensor;
        private IRobotColorSensor colorSensor0;
        private IRobotColorSensor colorSensor1;
        private IRobotUltraSonicSensor ultrasonicSensor;
        private IRobotTouchSensor touchSensor0;
        private IRobotTouchSensor touchSensor1;
        private IRobotGyroSensor gyroSensor;
        private IRobotGpsSensor gpsSensor;
        private IRobotLed led;
        private IEV3Parts parts;
        private PduIoConnector pdu_io;
        private GameObject sample_obj;

        private IPduReader pdu_reader;
        private IPduWriter pdu_writer;
        private string my_name = null;
        public void Initialize()
        {
            Debug.Log("Enter Robomodel");
            this.root = GameObject.Find("Robot");
            this.myObject = GameObject.Find("Robot/" + this.transform.name);
            this.parts = myObject.GetComponentInChildren<IEV3Parts>();
            this.my_name = string.Copy(this.transform.name);
            this.InitActuator();
            this.InitSensor();
            this.pdu_io = PduIoConnector.Get(this.GetName());
            this.pdu_reader = this.pdu_io.GetReader(this.GetName() + "_Ev3ActuatorPdu");
            this.pdu_writer = this.pdu_io.GetWriter(this.GetName() + "_Ev3SensorPdu");
            this.sample_obj = GameObject.Find("RoboModel_Axis");
        }
        public string GetName()
        {
            return this.my_name;
        }

        public void DoActuation()
        {
            int power_a = 0;
            int power_b = 0;
            int power_c = 0;
            int led_color = 0;
            led_color = this.pdu_reader.GetReadOps().GetDataInt32("led");
            power_a = this.pdu_reader.GetReadOps().GetDataInt32("motor_power_a");
            power_b = this.pdu_reader.GetReadOps().GetDataInt32("motor_power_b");
            power_c = this.pdu_reader.GetReadOps().GetDataInt32("motor_power_c");


            if (this.led != null)
            {
                this.led.SetLedColor((LedColor)(((led_color) & 0x3)));
            }
            if (this.motor_a != null)
            {
                this.motor_a.SetTargetVelicty(power_a * powerConst);
            }
            if (this.motor_b != null)
            {
                this.motor_b.SetTargetVelicty(power_b * powerConst);
            }
            if (this.motor_c != null)
            {
                this.motor_c.SetTargetVelicty(power_c * this.armMotorConst);
            }

            int reset = this.pdu_reader.GetReadOps().GetDataInt32("motor_reset_angle_a");
            if ((this.motor_a_sensor != null) && (reset != 0))
            {
                this.motor_a_sensor.ClearDegree();
                Debug.Log("reset tire1");
            }
            reset = this.pdu_reader.GetReadOps().GetDataInt32("motor_reset_angle_b");
            if ((this.motor_b_sensor != null) && (reset != 0))
            {
                this.motor_b_sensor.ClearDegree();
                Debug.Log("reset tire2");
            }
            reset = this.pdu_reader.GetReadOps().GetDataInt32("motor_reset_angle_c");
            if ((this.motor_arm_sensor != null) && (reset != 0))
            {
                this.motor_arm_sensor.ClearDegree();
                Debug.Log("reset arm");
            }
            reset = this.pdu_reader.GetReadOps().GetDataInt32("gyro_reset");
            if ((this.gyroSensor != null) && (reset != 0))
            {
                this.gyroSensor.ClearDegree();
                //Debug.Log("reset gyro");
            }
        }

        public void CopySensingDataToPdu()
        {
            if (this.motor_a_sensor != null)
            {
                motor_a_sensor.UpdateSensorValues();
                this.pdu_writer.GetWriteOps().SetData("motor_angle_a", (int)motor_a_sensor.GetDegree());
            }
            if (this.motor_b_sensor != null)
            {
                motor_b_sensor.UpdateSensorValues();
                this.pdu_writer.GetWriteOps().SetData("motor_angle_b", (int)motor_b_sensor.GetDegree());
            }
            if (this.motor_arm_sensor != null)
            {
                motor_arm_sensor.UpdateSensorValues();
                this.pdu_writer.GetWriteOps().SetData("motor_angle_c", (int)motor_arm_sensor.GetDegree());
            }
            if (this.colorSensor0 != null)
            {
                colorSensor0.UpdateSensorValues();
                this.pdu_writer.GetWriteOps().SetData("sensor_reflect0", (int)(this.colorSensor0.GetLightValue() * 100f));
                ColorRGB color_sensor_rgb;
                this.colorSensor0.GetRgb(out color_sensor_rgb);
                this.pdu_writer.GetWriteOps().SetData("sensor_rgb_r0", color_sensor_rgb.r);
                this.pdu_writer.GetWriteOps().SetData("sensor_rgb_g0", color_sensor_rgb.g);
                this.pdu_writer.GetWriteOps().SetData("sensor_rgb_b0", color_sensor_rgb.b);
                this.pdu_writer.GetWriteOps().SetData("sensor_color0", (int)this.colorSensor0.GetColorId());
            }
            if (this.colorSensor1 != null)
            {
                colorSensor1.UpdateSensorValues();
                this.pdu_writer.GetWriteOps().SetData("sensor_reflect1", (int)(this.colorSensor1.GetLightValue() * 100f));
                ColorRGB color_sensor_rgb;
                this.colorSensor1.GetRgb(out color_sensor_rgb);
                this.pdu_writer.GetWriteOps().SetData("sensor_rgb_r1", color_sensor_rgb.r);
                this.pdu_writer.GetWriteOps().SetData("sensor_rgb_g1", color_sensor_rgb.g);
                this.pdu_writer.GetWriteOps().SetData("sensor_rgb_b1", color_sensor_rgb.b);
                this.pdu_writer.GetWriteOps().SetData("sensor_color1", (int)this.colorSensor1.GetColorId());
            }
            if (this.ultrasonicSensor != null)
            {
                ultrasonicSensor.UpdateSensorValues();
                //Debug.Log("ultrasonic=" + this.ultrasonicSensor.GetDistanceValue());
                this.pdu_writer.GetWriteOps().SetData("sensor_ultrasonic", (int)(this.ultrasonicSensor.GetDistanceValue() * 10));
                //Debug.Log("PDU:ultrasonic=" + this.pdu_writer.GetReadOps().GetDataInt32("sensor_ultrasonic"));
            }
            if (touchSensor0 != null)
            {
                touchSensor0.UpdateSensorValues();
                if (this.touchSensor0.IsPressed())
                {
                    //Debug.Log("Touched0:");
                    this.pdu_writer.GetWriteOps().SetData("touch_sensor0", 4095);
                }
                else
                {
                    this.pdu_writer.GetWriteOps().SetData("touch_sensor0", 0);
                }
            }
            if (touchSensor1 != null)
            {
                touchSensor1.UpdateSensorValues();
                if (this.touchSensor1.IsPressed())
                {
                    //Debug.Log("Touched1:");
                    this.pdu_writer.GetWriteOps().SetData("touch_sensor1", 4095);
                }
                else
                {
                    this.pdu_writer.GetWriteOps().SetData("touch_sensor1", 0);
                }
            }
            if (gyroSensor != null)
            {
                gyroSensor.UpdateSensorValues();
                this.pdu_writer.GetWriteOps().SetData("gyro_degree", (int)gyroSensor.GetDegree());
                this.pdu_writer.GetWriteOps().SetData("gyro_degree_rate", (int)gyroSensor.GetDegreeRate());
            }
            if (gpsSensor != null)
            {
                gpsSensor.UpdateSensorValues();
                this.pdu_writer.GetWriteOps().SetData("gps_lon", gpsSensor.GetLongitude());
                this.pdu_writer.GetWriteOps().SetData("gps_lat", gpsSensor.GeLatitude());
            }

        }

        private void InitActuator()
        {
            GameObject obj;
            string subParts = this.parts.GetMotorA();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + this.parts.GetMotorA()).gameObject;
                this.motor_a = obj.GetComponentInChildren<IRobotMotor>();
                motor_a.Initialize(obj);
                motor_a.SetForce(this.motorPower);
                this.motor_a_sensor = obj.GetComponentInChildren<IRobotMotorSensor>();
            }
            subParts = this.parts.GetMotorB();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + this.parts.GetMotorB()).gameObject;
                this.motor_b = obj.GetComponentInChildren<IRobotMotor>();
                motor_b.Initialize(obj);
                motor_b.SetForce(this.motorPower);
                this.motor_b_sensor = obj.GetComponentInChildren<IRobotMotorSensor>();
            }
            subParts = this.parts.GetMotorC();
            if (subParts != null)
            {
                //Debug.Log("parts=" + this.parts.GetMotorC());
                if (root.transform.Find(this.transform.name + "/" + this.parts.GetMotorC()) != null)
                {
                    obj = root.transform.Find(this.transform.name + "/" + this.parts.GetMotorC()).gameObject;
                    this.motor_c = obj.GetComponentInChildren<IRobotMotor>();
                    motor_c.Initialize(obj);
                    this.motor_arm_sensor = obj.GetComponentInChildren<IRobotMotorSensor>();
                    motor_c.SetForce(this.motorPower);
                }
            }
            subParts = this.parts.GetLed();
            if (subParts != null)
            {
                if (root.transform.Find(this.transform.name + "/" + this.parts.GetLed()) != null)
                {
                    obj = root.transform.Find(this.transform.name + "/" + this.parts.GetLed()).gameObject;
                    this.led = obj.GetComponentInChildren<IRobotLed>();
                    led.Initialize(obj);
                }
            }

        }
        private void InitSensor()
        {
            GameObject obj;
            string subParts = this.parts.GetColorSensor0();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + this.parts.GetColorSensor0()).gameObject;
                colorSensor0 = obj.GetComponentInChildren<IRobotColorSensor>();
                colorSensor0.Initialize(obj);
            }
            subParts = this.parts.GetColorSensor1();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + this.parts.GetColorSensor1()).gameObject;
                colorSensor1 = obj.GetComponentInChildren<IRobotColorSensor>();
                colorSensor1.Initialize(obj);
            }
            subParts = this.parts.getUltraSonicSensor();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + this.parts.getUltraSonicSensor()).gameObject;
                ultrasonicSensor = obj.GetComponentInChildren<IRobotUltraSonicSensor>();
                ultrasonicSensor.Initialize(obj);
            }
            subParts = this.parts.getTouchSensor0();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + this.parts.getTouchSensor0()).gameObject;
                touchSensor0 = obj.GetComponentInChildren<IRobotTouchSensor>();
                touchSensor0.Initialize(obj);
            }
            subParts = this.parts.getTouchSensor1();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + this.parts.getTouchSensor1()).gameObject;
                touchSensor1 = obj.GetComponentInChildren<IRobotTouchSensor>();
                touchSensor1.Initialize(obj);
            }
            subParts = this.parts.getGyroSensor();
            if (subParts != null)
            {
                if (root.transform.Find(this.transform.name + "/" + this.parts.getGyroSensor()) != null)
                {
                    obj = root.transform.Find(this.transform.name + "/" + this.parts.getGyroSensor()).gameObject;
                    gyroSensor = obj.GetComponentInChildren<IRobotGyroSensor>();
                    gyroSensor.Initialize(obj);
                }
            }
            subParts = this.parts.getGpsSensor();
            if (subParts != null)
            {
                if (root.transform.Find(this.transform.name + "/" + this.parts.getGpsSensor()) != null)
                {
                    obj = root.transform.Find(this.transform.name + "/" + this.parts.getGpsSensor()).gameObject;
                    gpsSensor = obj.GetComponentInChildren<IRobotGpsSensor>();
                    gpsSensor.Initialize(obj);
                }
            }
        }


    }

}