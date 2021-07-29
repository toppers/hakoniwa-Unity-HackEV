using Hakoniwa.PluggableAsset.Communication.Pdu;
using Hakoniwa.PluggableAsset.Communication.Pdu.Accessor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakoniwa.PluggableAsset.Assets.Robot.TB3
{
    public class MotorController
    {
        private Motor[] motors = new Motor[2];      // 0: R, 1: L
        private float[] prev_angle = new float[2];  // 0: R, 1: L
        private float[] delta_angle = new float[2];  // 0: R, 1: L
        private float[] moving_distance = new float[2];  // 0: R, 1: L
        private int motor_power = 500;
        private float motor_interval_distance = 16.0f; // 16cm
        private TwistAccessor pdu_motor_control_accessor;

        internal Motor GetRightMotor()
        {
            return motors[0];
        }
        internal Motor GetLeftMotor()
        {
            return motors[1];
        }

        public void Initialize(GameObject root, Transform transform, ITB3Parts parts, IPduReader pdu_reader)
        {
            GameObject obj;
            this.pdu_motor_control_accessor = new TwistAccessor(pdu_reader.GetReadOps().Ref(null));

            for (int i = 0; i < 2; i++)
            {
                string subParts = parts.GetMotor(i);
                if (subParts != null)
                {
                    obj = root.transform.Find(transform.name + "/" + subParts).gameObject;
                    Debug.Log("path=" + transform.name + "/" + subParts);
                    motors[i] = obj.GetComponentInChildren<Motor>();
                    motors[i].Initialize(obj);
                    motors[i].SetForce(this.motor_power);
                }
            }
        }

        public void CopySensingDataToPdu()
        {
            for (int i = 0; i < 2; i++)
            {
                this.motors[i].UpdateSensorValues();
                var angle = motors[i].GetDegree();
                this.delta_angle[i] = angle - this.prev_angle[i];
                this.prev_angle[i] = angle;

                this.moving_distance[i] = ((Mathf.Deg2Rad * this.delta_angle[i]) / Mathf.PI) * motors[i].GetRadius();
                //Debug.Log("d[" + i + "]=" + this.moving_distance[i]);
                //Debug.Log("delta_angle[" + i + "]=" + this.delta_angle[i]);
            }
        }
        public void DoActuation()
        {
            double target_velocity;
            double target_rotation_angle_rate;

            //target_velocity = this.pdu_reader.GetReadOps().Ref("linear").GetDataFloat64("x");
            //target_rotation_angle_rate = this.pdu_reader.GetReadOps().Ref("angular").GetDataFloat64("z");
            target_velocity = this.pdu_motor_control_accessor.linear.x;
            target_rotation_angle_rate = this.pdu_motor_control_accessor.angular.z;

            //Debug.Log("target_velocity=" + target_velocity);
            //Debug.Log("target_rotation_angle_rate=" + target_rotation_angle_rate);
            // V_R(右車輪の目標速度) = V(目標速度) + d × ω(目標角速度)
            // V_L(左車輪の目標速度) = V(目標速度) - d × ω(目標角速度)
            motors[0].SetTargetVelicty((float)(target_velocity + motor_interval_distance * target_rotation_angle_rate));
            motors[1].SetTargetVelicty((float)(target_velocity - motor_interval_distance * target_rotation_angle_rate));
        }

        internal float GetDeltaMovingDistance()
        {
            return ((this.moving_distance[0] + this.moving_distance[1]) / 2.0f);
        }
    }
}

