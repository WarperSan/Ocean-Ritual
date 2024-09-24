using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ControllerModule.Controllers.Interfaces
{
    public interface IJumpable
    {
        /// <summary>
        /// Called when the player jumps
        /// </summary>
        public void OnJump();
    }
}