using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets;

namespace Hakoniwa.Assets.EV3
{
    public class UltrasonicSensor : MonoBehaviour, IRobotUltraSonicSensor
    {
        private float contact_distance = 12; /* centimeters/10 */
        public float distanceValue; /* centimeters/10 */
        private GameObject frontSensor;

        public void Initialize(GameObject root)
        {
            frontSensor = root;
            this.distanceValue = this.contact_distance;
        }

        public float GetDistanceValue()
        {
            return distanceValue * 10; /* centimeters */
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

