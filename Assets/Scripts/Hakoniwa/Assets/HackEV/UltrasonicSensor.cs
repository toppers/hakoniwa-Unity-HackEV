using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets;

namespace Hakoniwa.Assets.HackEV
{
    public class UltrasonicSensor : MonoBehaviour, IRobotUltraSonicSensor
    {
        private float contact_distance = 120;
        public float distanceValue;
        private GameObject frontSensor;

        public void Initialize(GameObject root)
        {
            frontSensor = root;
            this.distanceValue = this.contact_distance;
        }

        public float GetDistanceValue()
        {
            return distanceValue;
        }
        public void UpdateSensorValues()
        {
            //nothing to do
        }
        private void OnTriggerStay(Collider other)
        {


            Vector3 Apoint = frontSensor.transform.position;
            Vector3 Bpoint = other.gameObject.transform.position;

            // Distance between Sensor and Object
            this.distanceValue = Vector3.Distance(Apoint, Bpoint);

        }
        private void OnTriggerExit(Collider other)
        {
            this.distanceValue = contact_distance;
        }

    }
}

