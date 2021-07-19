using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Hakoniwa.Core;
using Hakoniwa.PluggableAsset.Assets;
using Hakoniwa.PluggableAsset.Communication.Connector;
using Hakoniwa.PluggableAsset.Communication.Pdu;
using Assets.Scripts.Hakoniwa.PluggableAsset.Assets.Robot.TB3;
using Assets.Scripts.Hakoniwa.PluggableAsset.Assets.Robot;

namespace Hakoniwa.PluggableAsset.Assets.Robot.TB3
{
    public class RobotController : MonoBehaviour, IInsideAssetController
    {
        private GameObject root;
        private GameObject myObject;
        private ITB3Parts parts;
        private string my_name;
        private PduIoConnector pdu_io;
        private IPduWriter pdu_laser_scan;
        private IPduWriter pdu_imu;
        private IPduWriter pdu_odometry;
        private IPduReader pdu_motor_control;
        private ILaserScan laser_scan;
        private IMUSensor imu;
        private MotorController motor_controller;

        public void CopySensingDataToPdu()
        {
            //LaserSensor
            this.laser_scan.UpdateSensorValues();
            this.laser_scan.UpdateSensorData(pdu_laser_scan.GetWriteOps().Ref(null));

            //IMUSensor
            this.imu.UpdateSensorValues();
            this.imu.UpdateSensorData(pdu_imu.GetWriteOps().Ref(null));

            //Odometry
            this.CalcOdometry();
        }

        private Vector3 current_pos = Vector3.zero; //ROS 
        private void CalcOdometry()
        {
            double delta_time = Time.fixedDeltaTime;
            float delta_s = this.motor_controller.GetDeltaMovingDistance();
            Vector3 unity_current_angle = this.imu.GetCurrentEulerAngle();//degree, Unity
            Vector3 delta_pos = Vector3.zero; //ROS

            delta_pos.x = delta_s * Mathf.Cos(Mathf.Deg2Rad * unity_current_angle.y);
            delta_pos.y = delta_s * Mathf.Sin(Mathf.Deg2Rad * unity_current_angle.y);

            current_pos += delta_pos;

            Vector3 delta_angle = Vector3.zero;
            var unity_delta_angle = this.imu.GetDeltaEulerAngle();//degree, Unity
            delta_angle.x = 0f;
            delta_angle.y = 0f;
            delta_angle.z = unity_delta_angle.y;

            /*
             * PDU
             */
            //header
            TimeStamp.Set(this.pdu_odometry.GetWriteOps().Ref(null));
            this.pdu_odometry.GetWriteOps().Ref("header").SetData("frame_id", "/odom");

            //child_frame_id
            this.pdu_odometry.GetWriteOps().SetData("child_frame_id", "/base_footprint");
            //pose.pose.position
            this.pdu_odometry.GetWriteOps().Ref("pose").Ref("pose").Ref("position").SetData("x", (double)current_pos.x);
            this.pdu_odometry.GetWriteOps().Ref("pose").Ref("pose").Ref("position").SetData("y", (double)current_pos.y);
            this.pdu_odometry.GetWriteOps().Ref("pose").Ref("pose").Ref("position").SetData("z", (double)current_pos.z);

            //pose.pose.orientation
            this.pdu_odometry.GetWriteOps().Ref("pose").Ref("pose").SetData("orientation", 
                this.pdu_imu.GetReadOps().Ref("orientation"));

            //twist.twist.linear
            this.pdu_odometry.GetWriteOps().Ref("twist").Ref("twist").Ref("linear").SetData("x", (double)delta_pos.x / delta_time);
            this.pdu_odometry.GetWriteOps().Ref("twist").Ref("twist").Ref("linear").SetData("y", (double)delta_pos.y / delta_time);
            this.pdu_odometry.GetWriteOps().Ref("twist").Ref("twist").Ref("linear").SetData("z", (double)delta_pos.z / delta_time);
            //twist.twist.angular
            this.pdu_odometry.GetWriteOps().Ref("twist").Ref("twist").Ref("angular").SetData("x", (double)delta_angle.x / delta_time);
            this.pdu_odometry.GetWriteOps().Ref("twist").Ref("twist").Ref("angular").SetData("y", (double)delta_angle.y / delta_time);
            this.pdu_odometry.GetWriteOps().Ref("twist").Ref("twist").Ref("angular").SetData("z", (double)delta_angle.z / delta_time);
        }

        public void DoActuation()
        {
            this.motor_controller.DoActuation();
        }

        public string GetName()
        {
            return this.my_name;
        }

        public void Initialize()
        {
            Debug.Log("TurtleBot3");
            this.root = GameObject.Find("Robot");
            this.myObject = GameObject.Find("Robot/" + this.transform.name);
            this.parts = myObject.GetComponentInChildren<ITB3Parts>();
            this.my_name = string.Copy(this.transform.name);
            this.pdu_io = PduIoConnector.Get(this.GetName());
            this.InitActuator();
            this.InitSensor();
        }

        private void InitSensor()
        {
            GameObject obj;
            string subParts = this.parts.GetLaserScan();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + subParts).gameObject;
                Debug.Log("path=" + this.transform.name + "/" + subParts);
                laser_scan = obj.GetComponentInChildren<ILaserScan>();
                laser_scan.Initialize(obj);
            }
            subParts = this.parts.GetIMU();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + subParts).gameObject;
                Debug.Log("path=" + this.transform.name + "/" + subParts);
                imu = obj.GetComponentInChildren<IMUSensor>();
                imu.Initialize(obj);
            }
            this.pdu_laser_scan = this.pdu_io.GetWriter(this.GetName() + "_Tb3LaserSensorPdu");
            if (this.pdu_laser_scan == null)
            {
                throw new ArgumentException("can not found LaserScan pdu:" + this.GetName() + "_Tb3LaserSensorPdu");
            }
            this.pdu_imu = this.pdu_io.GetWriter(this.GetName() + "_Tb3ImuSensorPdu");
            if (this.pdu_imu == null)
            {
                throw new ArgumentException("can not found Imu pdu:" + this.GetName() + "_Tb3ImuSensorPdu");
            }
            this.pdu_odometry = this.pdu_io.GetWriter(this.GetName() + "_Tb3OdometryPdu");
            if (this.pdu_odometry == null)
            {
                throw new ArgumentException("can not found Imu pdu:" + this.GetName() + "_Tb3OdometryPdu");
            }
        }

        private void InitActuator()
        {
            motor_controller = new MotorController();
            this.pdu_motor_control = this.pdu_io.GetReader(this.GetName() + "_Tb3CmdVelPdu");
            if (this.pdu_motor_control == null)
            {
                throw new ArgumentException("can not found CmdVel pdu:" + this.GetName() + "_Tb3CmdVelPdu");
            }
            motor_controller.Initialize(this.root, this.transform, this.parts, this.pdu_motor_control);
        }
    }
}