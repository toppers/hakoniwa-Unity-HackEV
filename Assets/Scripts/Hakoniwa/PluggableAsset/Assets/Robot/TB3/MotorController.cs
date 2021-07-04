using Hakoniwa.PluggableAsset.Communication.Pdu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakoniwa.PluggableAsset.Assets.Robot.TB3
{
    public class MotorController
    {
        private Motor[] motors = new Motor[2]; // 0: R, 1: L
        private int motor_power = 200;
        private float motor_interval_distance = 0.16f; // 16cm
        private IPduReader pdu_reader;

        public void Initialize(GameObject root, Transform transform, ITB3Parts parts, IPduReader pdu_reader)
        {
            GameObject obj;
            this.pdu_reader = pdu_reader;

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
            //TODO
        }
        public void DoActuation()
        {
            double target_velocity;
            double target_rotation_angle_rate;

            target_velocity = this.pdu_reader.GetReadOps().Ref("linear").GetDataFloat64("x");
            target_rotation_angle_rate = this.pdu_reader.GetReadOps().Ref("angular").GetDataFloat64("z");

            Debug.Log("target_velocity=" + target_velocity);
            Debug.Log("target_rotation_angle_rate=" + target_rotation_angle_rate);
            // V_R(右車輪の目標速度) = V(目標速度) + d × ω(目標角速度)
            // V_L(左車輪の目標速度) = V(目標速度) - d × ω(目標角速度)
            motors[0].SetTargetVelicty((float)(target_velocity + motor_interval_distance * target_rotation_angle_rate));
            motors[1].SetTargetVelicty((float)(target_velocity - motor_interval_distance * target_rotation_angle_rate));
        }
    }
}

