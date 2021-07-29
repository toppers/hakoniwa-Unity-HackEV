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
using Hakoniwa.PluggableAsset.Communication.Pdu.Accessor;

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
        private ImuAccessor pdu_imu_accessor;
        private IPduWriter pdu_odometry;
        private OdometryAccessor pdu_odometry_accessor;
        private IPduWriter pdu_tf;
        private IPduWriter pdu_joint_state;
        private IPduReader pdu_motor_control;
        private ILaserScan laser_scan;
        private IMUSensor imu;
        private MotorController motor_controller;
        private int tf_num = 1;
        private long current_timestamp;

        public void CopySensingDataToPdu()
        {
            this.current_timestamp = UtilTime.GetUnixTime();

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

            //joint states
            this.PublishJointStates();
        }

        private void PublishJointStates()
        {
            //ROS 0: left,  1: right
            TimeStamp.Set(this.current_timestamp, this.pdu_joint_state.GetWriteOps().Ref(null));
            //position
            double[] position = new double[2];
            position[0] = this.motor_controller.GetLeftMotor().GetCurrentAngle() * Mathf.Deg2Rad;
            position[1] = this.motor_controller.GetRightMotor().GetCurrentAngle() * Mathf.Deg2Rad;
            //position[0] = this.motor_controller.GetLeftMotor().GetDegree() * Mathf.Deg2Rad;
            //position[1] = this.motor_controller.GetRightMotor().GetDegree() * Mathf.Deg2Rad;

#if false
            Debug.Log("IMU=" + imu.GetCurrentEulerAngle() 
                + " R:d_rot=" + this.motor_controller.GetRightMotor().GetDeltaEulerAngle()
                + " R:rot=" + this.motor_controller.GetRightMotor().GetDegree());
            Debug.Log("IMU=" + motor_controller.GetRightMotor().GetCurrentAngle().eulerAngles + " Wheel=" + imu.GetCurrentAngle().eulerAngles);
            Debug.Log("IMU-Wheel=" + (motor_controller.GetRightMotor().GetCurrentAngle() * Quaternion.Inverse(imu.GetCurrentAngle()).eulerAngles));
#endif

            //velocity
            double[] velocity = new double[2];
            velocity[0] = this.motor_controller.GetLeftMotor().GetCurrentAngleVelocity() * Mathf.Deg2Rad;
            velocity[1] = this.motor_controller.GetRightMotor().GetCurrentAngleVelocity() * Mathf.Deg2Rad;

            //effort
            double[] effort = new double[2];
            effort[0] = 0.0f;
            effort[1] = 0.0f;

            //Set PDU
            this.pdu_joint_state.GetWriteOps().SetData("position", position);
            this.pdu_joint_state.GetWriteOps().SetData("velocity", velocity);
            this.pdu_joint_state.GetWriteOps().SetData("effort", effort);
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
            long t = this.current_timestamp;

            Pdu[] tf_pdus = this.pdu_tf.GetWriteOps().Refs("transforms");
            TimeStamp.Set(t, tf_pdus[0]);
            tf_pdus[0].GetPduWriteOps().Ref("header").SetData("frame_id", "odom");
            tf_pdus[0].GetPduWriteOps().SetData("child_frame_id", "base_footprint");
            SetTfPos(tf_pdus[0].Ref("transform"), this.pdu_odometry);
            tf_pdus[0].Ref("transform").SetData("rotation",
                this.pdu_imu.GetReadOps().Ref("orientation"));

        }

        private Vector3 init_pos_unity = Vector3.zero;
        private Vector3 current_pos = Vector3.zero; //ROS 
        private Vector3 prev_pos_unity = Vector3.zero;
        private Vector3 current_pos_unity = Vector3.zero;
        private void CalcOdometry()
        {

            this.current_pos_unity = this.imu.transform.position;
            Vector3 delta_pos_unity = this.current_pos_unity - this.prev_pos_unity;
            Vector3 delta_pos = Vector3.zero;
            
            delta_pos.x = delta_pos_unity.z/100.0f;
            delta_pos.y = -delta_pos_unity.x/100.0f;

            this.prev_pos_unity = this.current_pos_unity;

            //Debug.Log("delta_s=" + delta_s + " dx=" + delta_pos.x + " dy=" + delta_pos.y + " angle.y=" + unity_current_angle.y);

            current_pos.x = (this.current_pos_unity.z - this.init_pos_unity.z)/100.0f;
            current_pos.y = -(this.current_pos_unity.x - this.init_pos_unity.x)/100.0f;
            //Debug.Log("curr=" + current_pos_unity + " init=" + init_pos_unity + " imu_angle=" + this.imu.transform.rotation.eulerAngles.y);
            //Debug.Log("curr_pos_ros=" + current_pos);
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
            //this.pdu_odometry.GetWriteOps().Ref("header").SetData("frame_id", "/odom");
            this.pdu_odometry_accessor.header.frame_id = "/odom";

            //child_frame_id
            //this.pdu_odometry.GetWriteOps().SetData("child_frame_id", "/base_footprint");
            this.pdu_odometry_accessor.child_frame_id = "/base_footprint";

            //pose.pose.position
            //this.pdu_odometry.GetWriteOps().Ref("pose").Ref("pose").Ref("position").SetData("x", (double)current_pos.x);
            //this.pdu_odometry.GetWriteOps().Ref("pose").Ref("pose").Ref("position").SetData("y", (double)current_pos.y);
            //this.pdu_odometry.GetWriteOps().Ref("pose").Ref("pose").Ref("position").SetData("z", (double)current_pos.z);
            this.pdu_odometry_accessor.pose.pose.position.x = current_pos.x;
            this.pdu_odometry_accessor.pose.pose.position.y = current_pos.y;
            this.pdu_odometry_accessor.pose.pose.position.z = current_pos.z;

            //pose.pose.orientation
            //this.pdu_odometry.GetWriteOps().Ref("pose").Ref("pose").SetData("orientation", 
            //    this.pdu_imu.GetReadOps().Ref("orientation"));
            this.pdu_odometry_accessor.pose.pose.orientation = this.pdu_imu_accessor.orientation;

            //twist.twist.linear
            //this.pdu_odometry.GetWriteOps().Ref("twist").Ref("twist").Ref("linear").SetData("x", (double)delta_pos.x / Time.fixedDeltaTime);
            //this.pdu_odometry.GetWriteOps().Ref("twist").Ref("twist").Ref("linear").SetData("y", (double)delta_pos.y / Time.fixedDeltaTime);
            //this.pdu_odometry.GetWriteOps().Ref("twist").Ref("twist").Ref("linear").SetData("z", (double)delta_pos.z / Time.fixedDeltaTime);
            this.pdu_odometry_accessor.twist.twist.linear.x = delta_pos.x / Time.fixedDeltaTime;
            this.pdu_odometry_accessor.twist.twist.linear.y = delta_pos.y / Time.fixedDeltaTime;
            this.pdu_odometry_accessor.twist.twist.linear.z = delta_pos.z / Time.fixedDeltaTime;


            //twist.twist.angular
            //this.pdu_odometry.GetWriteOps().Ref("twist").Ref("twist").Ref("angular").SetData("x", (double)delta_angle.x / Time.fixedDeltaTime);
            //this.pdu_odometry.GetWriteOps().Ref("twist").Ref("twist").Ref("angular").SetData("y", (double)delta_angle.y / Time.fixedDeltaTime);
            //this.pdu_odometry.GetWriteOps().Ref("twist").Ref("twist").Ref("angular").SetData("z", (double)delta_angle.z / Time.fixedDeltaTime);
            this.pdu_odometry_accessor.twist.twist.angular.x = delta_angle.x / Time.fixedDeltaTime;
            this.pdu_odometry_accessor.twist.twist.angular.y = delta_angle.y / Time.fixedDeltaTime;
            this.pdu_odometry_accessor.twist.twist.angular.z = delta_angle.z / Time.fixedDeltaTime;
        }

        public void DoActuation()
        {
            this.motor_controller.DoActuation();
            //Physics.SyncTransforms();
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
            this.init_pos_unity = this.imu.transform.position;
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
            this.pdu_imu_accessor = new ImuAccessor(this.pdu_imu.GetReadOps().Ref(null));
            this.pdu_odometry = this.pdu_io.GetWriter(this.GetName() + "_odomPdu");
            if (this.pdu_odometry == null)
            {
                throw new ArgumentException("can not found Imu pdu:" + this.GetName() + "_odomPdu");
            }
            this.pdu_odometry_accessor = new OdometryAccessor(this.pdu_odometry.GetReadOps().Ref(null));

            this.pdu_tf = this.pdu_io.GetWriter(this.GetName() + "_tfPdu");
            if (this.pdu_tf == null)
            {
                throw new ArgumentException("can not found Tf pdu:" + this.GetName() + "_tfPdu");
            }
            this.pdu_tf.GetWriteOps().InitializePduArray("transforms", tf_num);

            this.pdu_joint_state = this.pdu_io.GetWriter(this.GetName() + "_joint_statesPdu");
            if (this.pdu_joint_state == null)
            {
                throw new ArgumentException("can not found joint_states pdu:" + this.GetName() + "_joint_statesPdu");
            }
            //this.pdu_joint_state.GetWriteOps().Ref("header").SetData("frame_id", "/base_link");
            this.pdu_joint_state.GetWriteOps().Ref("header").SetData("frame_id", "");
            string[] joint_names = new string[2];
            joint_names[0] = "wheel_left_joint";
            joint_names[1] = "wheel_right_joint";
            this.pdu_joint_state.GetWriteOps().SetData("name", joint_names);
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