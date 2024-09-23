using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ControllerModule.Controllers
{
    /// <summary>
    /// Class that manages the movement between multiple controllers
    /// </summary>
    public class ControllerManager : UtilsModule.Singleton<ControllerManager>
    {
        #region Singleton

        /// <inheritdoc/>
        protected override bool DestroyOnLoad => true;

        #endregion

        /// <summary>
        /// List of the controllers used
        /// </summary>
        private static readonly Stack<Controller> stack = new();

        /// <summary>
        /// Current controller being used
        /// </summary>
        private static Controller ActiveController => stack.Count > 0 ? stack.Peek() : null;
        

        /// <summary>
        /// Switches to the given controller
        /// </summary>
        /// <param name="controller">Controller to switch to</param>
        public static void SwitchTo(Controller controller)
        {
            
            // If exists, switch out current
            if (ActiveController != null)
            {
                Debug.Log(ActiveController.tag);
                
                //character model dissapears when taking control
                if (ActiveController.tag == "Player")
                {
                    if(ActiveController.gameObject.activeSelf)
                    {
                        ActiveController.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        ActiveController.gameObject.SetActive(false);
                    }
                    
                }
                
                if (ActiveController.tag == "Boat")
                {
                    Debug.Log("ShutdownSpeed");
                    ActiveController.gameObject.GetComponent<BoatController>().ShutdownBoatAcceleration();
                }
                
                ActiveController.SwitchOut();
            }
                

            // Cancel if given not found
            if (controller == null)
                return;

            // Switch in the given
            controller.SwitchIn();
            stack.Push(controller);
            Debug.Log(ActiveController.tag);
            //character model reappears when relinquishing control
            if (ActiveController.tag == "Player")
            {
                if (!ActiveController.gameObject.activeSelf)
                {

                    ActiveController.gameObject.SetActive(true);
                    
                    ActiveController.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    ActiveController.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                }

            }

            // Set the controller to the given
            CameraMovement.Instance.SetController(controller);
        }

        /// <summary>
        /// Replaces the current controller with the given controller
        /// </summary>
        private static void ReplaceCurrent(Controller controller)
        {
            // Skip if current not found
            if (ActiveController == null)
                return;

            // Remove current
            //Controller current = stack.Pop();
            //current.SwitchOut();
            //controller.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;

            // Switch to given
            SwitchTo(controller);
        }

        /// <summary>
        /// Switches to the previous controller
        /// </summary>
        public static void BackTo()
        {
            if (stack.Count <= 1)
                return;

            Controller previous = stack.Skip(1).First();
            ReplaceCurrent(previous);
        }
    }
}