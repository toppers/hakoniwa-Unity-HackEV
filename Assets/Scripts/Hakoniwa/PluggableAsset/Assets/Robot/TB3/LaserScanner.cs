using Hakoniwa.Core.Utils;
using Hakoniwa.PluggableAsset.Assets.Robot;
using Hakoniwa.PluggableAsset.Communication.Pdu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakoniwa.PluggableAsset.Assets.Robot.TB3
{
    public class LaserScanner : MonoBehaviour, ILaserScan
    {
        public static bool is_debug = false;
        public static int view_interval = 36;
        private const int max_count = 360;
        private float contact_distance = 350f; /* cm */
        private float[] distances = new float[max_count];
        private GameObject sensor;
        private float angle_min = 0.0f;
        private float angle_max = 6.26573181152f; //← 6.265 rad = 359°
        private float range_min = 0.119999997318f; //← 12cm
        private float range_max = 3.5f; //← 3.5m
        private float angle_increment = 0.0174532923847f; //← 0.01745 rad = 1°
        private float time_increment = 2.98800005112e-05f;
        private float scan_time = 0.0f;
        private Quaternion init_angle;

        public void Initialize(object root)
        {
            this.sensor = (GameObject)root;
            this.init_angle = this.transform.localRotation;
        }
        public void UpdateSensorData(Pdu pdu)
        {
            long t = UtilTime.GetUnixTime();
            uint t_sec = (uint)((long)(t / 1000000));
            uint t_nsec = (uint)((long)(t % 1000000)) * 1000;
            pdu.Ref("header").Ref("stamp").SetData("secs", t_sec);
            pdu.Ref("header").Ref("stamp").SetData("nsecs", t_nsec);
            pdu.Ref("header").SetData("frame_id", "laser");

            pdu.SetData("angle_min", angle_min);
            pdu.SetData("angle_max", angle_max);
            pdu.SetData("range_min", range_min);
            pdu.SetData("range_max", range_max);
            pdu.SetData("ranges", distances);
            pdu.SetData("angle_increment", angle_increment);
            pdu.SetData("time_increment", time_increment);
            pdu.SetData("scan_time", scan_time);
        }

        public void UpdateSensorValues()
        {
            this.sensor.transform.localRotation = this.init_angle;
            for (int i = 0; i < max_count; i++)
            {
                distances[i] = GetSensorValue(i) / 100.0f;
                this.sensor.transform.Rotate(0, 1, 0);
            }
        }

        private float GetSensorValue(int degree)
        {
            //Vector3 fwd = sensor.transform.TransformDirection(Vector3.forward);
            Vector3 fwd = sensor.transform.forward;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(this.sensor.transform.position);
            if (Physics.Raycast(transform.position, fwd, out hit, contact_distance))
            {
                if (is_debug && (degree % view_interval) == 0)
                {
                    Debug.DrawRay(this.sensor.transform.position, fwd * hit.distance, Color.red, 0.05f, false);
                }
                //Debug.Log(hit.collider.gameObject.name);
                return hit.distance;
            }
            else
            {
                return contact_distance;
                //Debug.DrawRay(this.sensor.transform.position, fwd, Color.green, 10, false);
            }
        }
    }
}

