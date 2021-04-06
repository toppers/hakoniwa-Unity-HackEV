using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakoniwa.PluggableAsset.Assets.Robot.EV3
{
    public class UltrasonicSensor : MonoBehaviour, IRobotUltraSonicSensor
    {
        private float contact_distance = 250f; /* cm */
        public float distanceValue; /* cm */
        private GameObject frontSensor;

        public void Initialize(System.Object root)
        {
            frontSensor = (GameObject)root;
            this.distanceValue = this.contact_distance;
        }

        public float GetDistanceValue()
        {
            if ((this.distanceValue < 3) || (this.distanceValue > 249))
            {
                return 255;
            }
            //Debug.Log("distance=" + this.distanceValue);
            return distanceValue * 1; /* centimeters */
        }
        public void UpdateSensorValues()
        {
            Vector3 fwd = frontSensor.transform.TransformDirection(Vector3.forward);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, fwd, out hit, contact_distance))
            {
                this.distanceValue = hit.distance;
            }
            else
            {
                this.distanceValue = contact_distance;
            }
        }
    }
}

