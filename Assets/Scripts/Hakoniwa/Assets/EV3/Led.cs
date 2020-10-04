using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets;

namespace Hakoniwa.Assets.EV3
{
    public class Led : MonoBehaviour, IRobotLed
    {
        public Material[] materials = new Material[4];
        private Renderer rend;

        public void Initialize(GameObject root)
        {
            rend = GetComponent<Renderer>();
            rend.material.color = materials[0].color;
        }

        public void SetLedColor(LedColor color)
        {
            //Debug.Log("color=" +color);
            rend.material.color = materials[(int)color].color;
        }
    }
}

