using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkyModule
{
    public class SkyboxManager : MonoBehaviour
    {
        public List<ParallaxObject> parallaxObjects;
        public MeshRenderer dome;
        private float offset;
        public float factor;

        // Update is called once per frame
        void Update()
        {
            float elapsed = Time.deltaTime;

            foreach (ParallaxObject item in this.parallaxObjects)
            {
                item.origin.Rotate(Vector3.up, item.factor * elapsed);
            }

            offset += elapsed * this.factor / 100f;// * this.factor;

            dome.material.mainTextureOffset = new Vector2(offset, 0);

            //offset += elapsed * this.factor * 0.25f / 90f;
            //dome.material.mainTextureOffset = new Vector2(offset, 0);
        }

        [Serializable]
        public struct ParallaxObject
        {
            public float factor;
            public Transform origin;
        }
    }
}