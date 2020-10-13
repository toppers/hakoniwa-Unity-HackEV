using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets;

namespace Hakoniwa.Assets.EV3
{
    public class Motor : MonoBehaviour, IRobotMotor, IRobotMotorSensor
    {
        private GameObject obj;
        private HingeJoint joint;
        private JointMotor motor;
        private int targetVelocity;
        private int force;
        private bool isStop;
        private Quaternion prevRotation;
        private float deg;
        private Rigidbody rigid_body;

        public void Initialize(GameObject root)
        {
            this.obj = root;
            this.rigid_body = this.obj.GetComponent<Rigidbody>();
            this.isStop = false;
            this.joint = this.obj.GetComponent<HingeJoint>();
            this.prevRotation = this.obj.transform.localRotation;
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

        public void SetTargetVelicty(int targetVelocity)
        {
            this.targetVelocity = targetVelocity;
            this.motor.targetVelocity = targetVelocity;
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
        public void UpdateSensorValues()
        {
            float diff;
            var diff_rot = this.obj.transform.localRotation * Quaternion.Inverse(this.prevRotation);
            diff = Map360To180(diff_rot.eulerAngles.x);
            this.prevRotation = this.obj.transform.localRotation;
            this.deg += diff;
        }
    }
}

