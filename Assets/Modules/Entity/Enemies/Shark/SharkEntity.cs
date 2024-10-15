using EntityModule.Entities;
using System;
using System.Diagnostics;
using UnityEngine;

namespace EntityModule.Enemies
{
    public class SharkEntity : EntityBehaviour
    {
        #region EntityBehaviour

        protected override void OnStart()
        {
            base.OnStart();
        }

        /// <inheritdoc/>
        private void Update()
        {
            this.UpdateTree();
            
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    UnityEngine.Debug.Log("touched");
        //    if (other.tag == "Player")
        //    {
        //        UnityEngine.Debug.Log("Hit Player");
        //    }
        //}
        #endregion
    }
}

