using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hakoniwa.Assets;
using System.Globalization;

namespace Hakoniwa.Assets.EV3
{
    public class GyroSensor : MonoBehaviour, IRobotGyroSensor
    {
        private GameObject obj;
        private Vector3 baseRotation;
        private Vector3 prevRotation;
        private float deg_rate;
        private float deltaTime;
        private bool hasResetEvent;

        public void Initialize(GameObject root)
        {
            this.obj = root;
            this.baseRotation = this.obj.transform.eulerAngles;
            this.prevRotation = this.baseRotation;
            this.deg_rate = 0.0f;
            this.deltaTime = Time.deltaTime;
            this.hasResetEvent = false;
        }

        public void ClearDegree()
        {
            this.deg_rate = 0.0f;
            this.hasResetEvent = true;
        }

        public float GetDegree()
        {
            float diff = this.obj.transform.eulerAngles.x - this.baseRotation.x;
            if (diff <= 180.0f)
            {
                return (diff);
            }
            else
            {
                return (diff - 360.0f);
            }
        }

        public float GetDegreeRate()
        {
            return this.deg_rate;
        }

        public void UpdateSensorValues()
        {
            if (this.hasResetEvent)
            {
                this.baseRotation = this.obj.transform.eulerAngles;
                this.hasResetEvent = false;
            }
            float diff = this.obj.transform.eulerAngles.x - this.prevRotation.x;
            this.deg_rate = diff / this.deltaTime;
            this.prevRotation = this.obj.transform.eulerAngles;
            //Debug.Log("deg=" + (int)this.GetDegree() + ":deg_rate=" + (int)this.deg_rate);
        }
    }
}