using Hakoniwa.PluggableAsset.Assets.Robot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakoniwa.PluggableAsset.Assets.Robot.TB3
{
    public class Motor : MonoBehaviour
    {
        private float power_const = 300;
        private int motor_velocity_av_count = 0;
        private int motor_velocity_av_max = 10;
        private float motor_sum_degree = 0.0f;
        private float rotation_angle_rate = 0.0f;
        private float motor_radius = 0.033f; //3.3cm

        private GameObject obj;
        private HingeJoint joint;
        private JointMotor motor;
        private float targetVelocity;
        private int force;
        private bool isStop;
        private Quaternion prevRotation;
        private float deg;
        private Rigidbody rigid_body;
        private float deltaTime;

        public float GetRadius()
        {
            return motor_radius;
        }

        public void Initialize(System.Object root)
        {
            this.obj = (GameObject)root;
            this.rigid_body = this.obj.GetComponent<Rigidbody>();
            this.deltaTime = Time.fixedDeltaTime;
            this.isStop = false;
            this.joint = this.obj.GetComponent<HingeJoint>();
            this.prevRotation = this.obj.transform.localRotation;
        }
        public float GetVelocity()
        {
            return (this.motor_radius * this.rotation_angle_rate);
        }

        private void CalcVelocity(float diff_degree)
        {
            this.motor_sum_degree += diff_degree;
            if (this.motor_velocity_av_count >= this.motor_velocity_av_max)
            {
                this.rotation_angle_rate = this.motor_sum_degree / (this.deltaTime * this.motor_velocity_av_max);
                this.motor_velocity_av_count = 0;
            }
            else
            {
                this.motor_velocity_av_count++;
            }

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
            float diff;
            var diff_rot = this.obj.transform.localRotation * Quaternion.Inverse(this.prevRotation);
            diff = Map360To180(diff_rot.eulerAngles.x);
            this.prevRotation = this.obj.transform.localRotation;
            this.deg += diff;
            this.CalcVelocity(diff);
        }
    }
}
