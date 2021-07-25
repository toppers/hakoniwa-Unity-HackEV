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
using Hakoniwa.Core.Utils;

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
        private IPduWriter pdu_tf;
        private IPduReader pdu_motor_control;
        private ILaserScan laser_scan;
        private IMUSensor imu;
        private MotorController motor_controller;
        private int tf_num = 3;

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

            //Motor
            this.motor_controller.CopySensingDataToPdu();

            //Tf
            this.PublishTf();
        }
        private void SetTfPos(Pdu pdu_tf, Vector3 pos)
        {
            pdu_tf.GetPduWriteOps().Ref("translation").SetData("x", (double)pos.x);
            pdu_tf.GetPduWriteOps().Ref("translation").SetData("y", (double)pos.y);
            pdu_tf.GetPduWriteOps().Ref("translation").SetData("z", (double)pos.z);
        }
        private void SetTfPosUnity(Pdu pdu_tf, Vector3 pos_unity)
        {
            pdu_tf.GetPduWriteOps().Ref("translation").SetData("x", (double)pos_unity.z);
            pdu_tf.GetPduWriteOps().Ref("translation").SetData("y", (double)-pos_unity.x);
            pdu_tf.GetPduWriteOps().Ref("translation").SetData("z", (double)pos_unity.y);
        }
        private void SetTfPos(Pdu pdu_tf, IPduWriter pdu_odm_pos)
        {
            pdu_tf.GetPduWriteOps().Ref("translation").SetData("x", pdu_odm_pos.GetReadOps().Ref("pose").Ref("pose").Ref("position").GetDataFloat64("x"));
            pdu_tf.GetPduWriteOps().Ref("translation").SetData("y", pdu_odm_pos.GetReadOps().Ref("pose").Ref("pose").Ref("position").GetDataFloat64("y"));
            pdu_tf.GetPduWriteOps().Ref("translation").SetData("z", pdu_odm_pos.GetReadOps().Ref("pose").Ref("pose").Ref("position").GetDataFloat64("z"));
        }
        private void SetTfOrientationUnity(Pdu pdu_tf, Quaternion orientation_unity)
        {
            pdu_tf.GetPduWriteOps().SetData("x", (double)orientation_unity.z);
            pdu_tf.GetPduWriteOps().SetData("y", (double)-orientation_unity.x);
            pdu_tf.GetPduWriteOps().SetData("z", (double)orientation_unity.y);
            pdu_tf.GetPduWriteOps().SetData("w", (double)-orientation_unity.w);
        }
        private void SetTfOrientation(Pdu pdu_tf, Quaternion orientation)
        {
            pdu_tf.GetPduWriteOps().SetData("x", (double)orientation.x);
            pdu_tf.GetPduWriteOps().SetData("y", (double)orientation.y);
            pdu_tf.GetPduWriteOps().SetData("z", (double)orientation.z);
            pdu_tf.GetPduWriteOps().SetData("w", (double)orientation.w);
        }

        private void PublishTf()
        {
            long t = UtilTime.GetUnixTime();

            Pdu[] tf_pdus = this.pdu_tf.GetWriteOps().Refs("transforms");
            TimeStamp.Set(t, tf_pdus[0]);
            tf_pdus[0].GetPduWriteOps().Ref("header").SetData("frame_id", "odom");
            tf_pdus[0].GetPduWriteOps().SetData("child_frame_id", "base_footprint");
            SetTfPos(tf_pdus[0].Ref("transform"), this.pdu_odometry);
            tf_pdus[0].Ref("transform").SetData("rotation",
                this.pdu_imu.GetReadOps().Ref("orientation"));

            TimeStamp.Set(t, tf_pdus[1]);
            tf_pdus[1].GetPduWriteOps().Ref("header").SetData("frame_id", "base_link");
            tf_pdus[1].GetPduWriteOps().SetData("child_frame_id", "wheel_left_link");
            SetTfPos(tf_pdus[1].Ref("transform"), new Vector3(0, 0.08f, 0.023f));
            Quaternion angle1 = this.motor_controller.motors[1].obj.transform.localRotation;
            angle1 = this.imu.transform.localRotation * angle1;
            SetTfOrientationUnity(tf_pdus[1].Ref("transform").Ref("rotation"), angle1);

            TimeStamp.Set(t, tf_pdus[2]);
            tf_pdus[2].GetPduWriteOps().Ref("header").SetData("frame_id", "base_link");
            tf_pdus[2].GetPduWriteOps().SetData("child_frame_id", "wheel_right_link");
            SetTfPos(tf_pdus[2].Ref("transform"), new Vector3(0, -0.08f, 0.023f));
            Quaternion angle2 = this.motor_controller.motors[0].obj.transform.localRotation;
            angle2 = this.imu.transform.localRotation * angle2;
            SetTfOrientationUnity(tf_pdus[2].Ref("transform").Ref("rotation"), angle2);
            Debug.Log("this.imu.transform.localRotation=" + this.imu.transform.localRotation);
            Debug.Log("angle2=" + angle2);
            //Debug.Log("angle=" + this.motor_controller.motors[0].obj.transform.localRotation.eulerAngles);
            //float theta = motor_controller.motors[0].obj.transform.localRotation.eulerAngles.x;
            //var q = new Quaternion(0.706825181105f*Mathf.Sign(theta * Mathf.Deg2Rad / 2.0f), 0, 0, -0.707388269167f*Mathf.Cos(theta * Mathf.Deg2Rad / 2.0f));
            //var q = Quaternion.AngleAxis(this.motor_controller.motors[0].GetDegree(), Vector3.left);
            //Debug.Log("q=" + q);
            //SetTfOrientationUnity(tf_pdus[2].Ref("transform").Ref("rotation"), q);
        }

        private Vector3 init_pos = Vector3.zero; //ROS 
        private Vector3 current_pos = Vector3.zero; //ROS 
        private void CalcOdometry()
        {
            double delta_time = Time.fixedDeltaTime;
            float delta_s = this.motor_controller.GetDeltaMovingDistance()/ 100.0f;
            Vector3 unity_current_angle = this.imu.GetCurrentEulerAngle();//degree, Unity
            Vector3 delta_pos = Vector3.zero; //ROS

            delta_pos.x = delta_s * Mathf.Cos(Mathf.Deg2Rad * unity_current_angle.y);
            delta_pos.y = delta_s * Mathf.Sin(Mathf.Deg2Rad * unity_current_angle.y);
            //Debug.Log("delta_s=" + delta_s + " dx=" + delta_pos.x + " dy=" + delta_pos.y + " angle.y=" + unity_current_angle.y);

            //current_pos += delta_pos;
            current_pos.x = (this.imu.transform.position.z - this.init_pos.z)/100.0f;
            current_pos.y = -(this.imu.transform.position.x - this.init_pos.x)/100.0f;
            //Debug.Log("odm.x=" + current_pos.x + " odm.y=" + current_pos.y);
            //Debug.Log("body.x=" + this.transform.position.x + " body.y=" + this.transform.position.y + " body.z=" + this.transform.position.z);

            Vector3 delta_angle = Vector3.zero;
            var unity_delta_angle = this.imu.GetDeltaEulerAngle();//degree, Unity
            delta_angle.x = 0f;
            delta_angle.y = 0f;
            delta_angle.z = unity_delta_angle.y * Mathf.Deg2Rad;

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
            this.init_pos = this.imu.transform.position;
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
            this.pdu_laser_scan = this.pdu_io.GetWriter(this.GetName() + "_scanPdu");
            if (this.pdu_laser_scan == null)
            {
                throw new ArgumentException("can not found LaserScan pdu:" + this.GetName() + "_scanPdu");
            }
            this.pdu_imu = this.pdu_io.GetWriter(this.GetName() + "_imuPdu");
            if (this.pdu_imu == null)
            {
                throw new ArgumentException("can not found Imu pdu:" + this.GetName() + "_imuPdu");
            }
            this.pdu_odometry = this.pdu_io.GetWriter(this.GetName() + "_odomPdu");
            if (this.pdu_odometry == null)
            {
                throw new ArgumentException("can not found Imu pdu:" + this.GetName() + "_odomPdu");
            }
            this.pdu_tf = this.pdu_io.GetWriter(this.GetName() + "_tfPdu");
            if (this.pdu_tf == null)
            {
                throw new ArgumentException("can not found Tf pdu:" + this.GetName() + "_tfPdu");
            }
            this.pdu_tf.GetWriteOps().InitializePduArray("transforms", tf_num);
        }

        private void InitActuator()
        {
            motor_controller = new MotorController();
            this.pdu_motor_control = this.pdu_io.GetReader(this.GetName() + "_cmd_velPdu");
            if (this.pdu_motor_control == null)
            {
                throw new ArgumentException("can not found CmdVel pdu:" + this.GetName() + "_cmd_velPdu");
            }
            motor_controller.Initialize(this.root, this.transform, this.parts, this.pdu_motor_control);
        }
    }
}