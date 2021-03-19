using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Hakoniwa.Core.Simulation.Environment
{
    class UnityEnvironmentOperation : IEnvironmentOperation
    {
        private GameObject root;
        public UnityEnvironmentOperation()
        {
            this.root = GameObject.Find("Robot");
        }
        private Rigidbody[] rigidbodies;
        private Vector3[] initial_pos;
        private Quaternion[] initial_angle;
        private bool[] initial_kinematics;
        private bool[] initial_collitions;

        public void Save()
        {
            this.rigidbodies = GameObject.Find("Hakoniwa").GetComponentsInChildren<Rigidbody>();
            this.initial_pos = new Vector3[this.rigidbodies.Length];
            this.initial_angle = new Quaternion[this.rigidbodies.Length];
            this.initial_kinematics = new bool[this.rigidbodies.Length];
            this.initial_collitions = new bool[this.rigidbodies.Length];
            int i = 0;
            foreach (var rigidbody in rigidbodies)
            {
                Vector3 tmp = rigidbody.transform.position;
                Quaternion angle = rigidbody.transform.rotation;
                this.initial_pos[i] = new Vector3(tmp.x, tmp.y, tmp.z);
                this.initial_angle[i] = new Quaternion(angle.x, angle.y, angle.z, angle.w);
                this.initial_collitions[i] = rigidbody.detectCollisions;
                this.initial_kinematics[i] = rigidbody.isKinematic;
                i++;
            }
        }
        public void Restore()
        {
#if false
            Physics.autoSimulation = false;
            foreach (Transform child in this.root.transform)
            {
                Debug.Log("child=" + child.name);
                GameObject obj = root.transform.Find(child.name).gameObject;
                IAssetController ctrl = obj.GetComponentInChildren<Hakoniwa.Core.IAssetController>();
                //ctrl.Stop();
            }
#endif

            //set iskinematic true
            foreach (var rigidbody in this.rigidbodies)
            {
                //Debug.Log("rigidbody=" + rigidbody + "isKinematic=" + rigidbody.isKinematic);
                rigidbody.isKinematic = true;
                rigidbody.detectCollisions = false;
            }
            int i = 0;
            foreach (var rigidbody in rigidbodies)
            {
                rigidbody.transform.position = this.initial_pos[i];
                rigidbody.transform.rotation = this.initial_angle[i];
                i++;
            }
            i = 0;
            foreach (var rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = this.initial_kinematics[i];
                rigidbody.detectCollisions = this.initial_collitions[i];
                i++;
            }
#if false
            foreach (Transform child in this.root.transform)
            {
                //Debug.Log("child=" + child.name);
                GameObject obj = root.transform.Find(child.name).gameObject;
                IAssetController ctrl = obj.GetComponentInChildren<Hakoniwa.Core.IAssetController>();
                //ctrl.Restart();
            }
#endif
        }

    }
}
