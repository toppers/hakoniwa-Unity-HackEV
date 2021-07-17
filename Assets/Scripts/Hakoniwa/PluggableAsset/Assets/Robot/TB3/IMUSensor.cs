using Hakoniwa.Core.Utils;
using Hakoniwa.PluggableAsset.Assets.Robot;
using Hakoniwa.PluggableAsset.Communication.Pdu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Hakoniwa.PluggableAsset.Assets.Robot.TB3
{
    class IMUSensor : MonoBehaviour, IIMUSensor
    {
        private GameObject sensor;
        private float deltaTime;
        private Vector3 prev_velocity = Vector3.zero;
        private Rigidbody my_rigidbody;

        public void Initialize(object root)
        {
            this.sensor = (GameObject)root;
            this.my_rigidbody = this.sensor.GetComponentInChildren<Rigidbody>();
            this.deltaTime = Time.fixedDeltaTime;
        }

        private void UpdateOrientation(Pdu pdu)
        {
            pdu.Ref("orientation").SetData("x", (double)-this.sensor.transform.rotation.z);
            pdu.Ref("orientation").SetData("y", (double)this.sensor.transform.rotation.x);
            pdu.Ref("orientation").SetData("z", (double)-this.sensor.transform.rotation.y);
            pdu.Ref("orientation").SetData("w", (double)this.sensor.transform.rotation.w);
        }
        private void UpdateAngularVelocity(Pdu pdu)
        {
            pdu.Ref("angular_velocity").SetData("x", (double)my_rigidbody.angularVelocity.z);
            pdu.Ref("angular_velocity").SetData("y", (double)-my_rigidbody.angularVelocity.x);
            pdu.Ref("angular_velocity").SetData("z", (double)my_rigidbody.angularVelocity.y);
        }
        private void UpdateLinearAcceleration(Pdu pdu)
        {
            Vector3 current_velocity = this.sensor.transform.InverseTransformDirection(my_rigidbody.velocity);
            Vector3 acceleration = (current_velocity - prev_velocity) / deltaTime;
            prev_velocity = current_velocity;
            //gravity element
            acceleration += transform.InverseTransformDirection(Physics.gravity);

            pdu.Ref("linear_acceleration").SetData("x", (double)acceleration.z);
            pdu.Ref("linear_acceleration").SetData("y", (double)-acceleration.x);
            pdu.Ref("linear_acceleration").SetData("z", (double)acceleration.y);
        }

        public void UpdateSensorData(Pdu pdu)
        {
            long t = UtilTime.GetUnixTime();
            uint t_sec = (uint)((long)(t / 1000000));
            uint t_nsec = (uint)((long)(t % 1000000)) * 1000;
            pdu.Ref("header").Ref("stamp").SetData("sec", t_sec);
            pdu.Ref("header").Ref("stamp").SetData("nanosec", t_nsec);
            pdu.Ref("header").SetData("frame_id", "imu");

            //orientation
            UpdateOrientation(pdu);

            //angular_velocity
            UpdateAngularVelocity(pdu);

            //linear_acceleration
            UpdateLinearAcceleration(pdu);
        }


        public void UpdateSensorValues()
        {
            //TODO
        }
    }
}
