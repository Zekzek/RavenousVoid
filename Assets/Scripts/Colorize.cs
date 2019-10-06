using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtiliZek
{
    public class Colorize : MonoBehaviour
    {
        public Material colorMaterial;
        public Material dimMaterial;
        public Renderer[] renderers;

        private bool lightUp;
        public bool LightUp
        {
            get { return lightUp; }
            set
            {
                lightUp = value;
                if (lightUp)
                {
                    foreach (Renderer renderer in renderers)
                        renderer.material = colorMaterial;
                }
                else
                {
                    foreach (Renderer renderer in renderers)
                        renderer.material = dimMaterial;
                }
            }
        }

        private void Start()
        {
            LightUp = false;
        }
    }
}