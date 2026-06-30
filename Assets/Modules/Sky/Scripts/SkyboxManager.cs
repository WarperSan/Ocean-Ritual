using System.Collections.Generic;
using UnityEngine;

namespace SkyModule
{
    public class SkyboxManager : MonoBehaviour
    {
        public List<Transform> parallaxObjects;
        public MeshRenderer dome;
        private float offset;
        public float factor;

        public float timePerCycle = 10;

        // Update is called once per frame
        private void Update()
        {
            float elapsed = Time.deltaTime;
            offset += elapsed;

            float scaledOffset = offset / timePerCycle;

            foreach (Transform item in parallaxObjects)
            {
                Vector3 rotation = item.rotation.eulerAngles;
                rotation.y = scaledOffset * 360f;

                item.rotation = Quaternion.Euler(rotation);
            }

            dome.material.mainTextureOffset = new Vector2(scaledOffset, 0);
        }
    }
}