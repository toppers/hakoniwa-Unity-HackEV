﻿using Hakoniwa.PluggableAsset.Assets.Robot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakoniwa.PluggableAsset.Assets.Robot.TB3
{
    public class Motor : MonoBehaviour
    {
        private float power_const = 1000;
        private int motor_velocity_av_count = 0;
        private int motor_velocity_av_max = 10;
        private float motor_sum_degree = 0.0f;
        private float rotation_angle_rate = 0.0f;
        private float motor_radius = 3.3f; //3.3cm

        internal GameObject obj;
        private HingeJoint joint;
        private JointMotor motor;
        private float targetVelocity;
        private int force;
        private bool isStop;
        private float deg;
        private Rigidbody rigid_body;
        private Quaternion current_angle;
        private Quaternion prev_angle;
        private Quaternion diff_angle;
        private float angle_velocity;

        public float GetRadius()
        {
            return motor_radius;
        }

        public void Initialize(System.Object root)
        {
            this.obj = (GameObject)root;
            this.rigid_body = this.obj.GetComponent<Rigidbody>();
            this.isStop = false;
            this.joint = this.obj.GetComponent<HingeJoint>();
        }
        public float GetVelocity()
        {
            return (this.motor_radius * this.rotation_angle_rate);
        }

        public float GetCurrentAngleVelocity()
        {
            return this.angle_velocity;
        }

        public void SetForce(int force)
        {
            this.force = force;
            this.motor.force = force;
            this.joint.motor = this.motor;
        }
        public void SetStop(bool stop)
        {
            if (stop)
            {
                if (this.isStop == false)
                {
                    this.motor.force = 0;
                    this.motor.targetVelocity = 0;
                    this.joint.motor = this.motor;
                    this.rigid_body.drag = 10F;
                    this.rigid_body.angularDrag = 10F;
                    this.isStop = true;
                    Debug.Log("set stop");
                }
            }
            else
            {
                if (isStop == true)
                {
                    this.rigid_body.drag = 0F;
                    this.rigid_body.angularDrag = 0.05F;
                    this.motor.force = this.force;
                    this.motor.targetVelocity = this.targetVelocity;
                    this.joint.motor = this.motor;
                    this.isStop = false;
                    Debug.Log("released stop");
                }
            }
        }

        public void SetTargetVelicty(float targetVelocity)
        {
            float tmp = power_const * targetVelocity;
            //Debug.Log("MOTOR:targetVelocity=" + tmp);
            this.targetVelocity = tmp;
            this.motor.targetVelocity = tmp;
            this.joint.motor = this.motor;
        }
        public void ClearDegree()
        {
            this.deg = 0.0f;
        }
        public float GetDegree()
        {
            return this.deg;
        }
        private float Map360To180(float degree)
        {
            if (degree < 180.0f)
            {
                return degree;
            }

            return degree - 360.0f;
        }

        public static float Degree2Rad(float degree)
        {
            return degree * (Mathf.PI / 180.0f);
        }

        public void UpdateSensorValues()
        {
            //float diff;
            //var diff_rot = this.obj.transform.localRotation * Quaternion.Inverse(this.prevRotation);
            //diff = Map360To180(diff_rot.eulerAngles.y);
            //this.prevRotation = this.obj.transform.localRotation;
            //this.deg += diff;

            this.current_angle = obj.transform.localRotation;
            this.diff_angle = current_angle * Quaternion.Inverse(this.prev_angle);

            //this.diff_angle = (this.current_angle - this.prev_angle);
            this.deg += Map360To180(this.diff_angle.eulerAngles.y);

            this.angle_velocity = this.diff_angle.eulerAngles.y / Time.fixedDeltaTime;
            this.prev_angle = this.current_angle;
        }
        public float GetCurrentAngle()
        {
            return -obj.transform.localRotation.eulerAngles.y;
        }

        public float GetDeltaAngle()
        {
            return Map360To180(diff_angle.eulerAngles.y);
        }
        public Vector3 GetDeltaEulerAngle()
        {
            return diff_angle.eulerAngles;
        }
    }
}
