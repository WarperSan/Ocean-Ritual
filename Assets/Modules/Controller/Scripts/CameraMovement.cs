using ExtensionsModule;
using UnityEngine;
using UtilsModule;

namespace ControllerModule.Controllers
{
    /// <summary>
    /// Moves the camera to the given object
    /// </summary>
    public class CameraMovement : Singleton<CameraMovement>
    {
        private const float LERP_THRESHOLD = 0.5f;

        [SerializeField]
        [Tooltip("Determines how fast the camera moves")]
        private float camSpeed = 1.0f;

        private Transform trackedObject;
        private bool isLerping = true;

        /// <summary>
        /// Updates the movement of the camera
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateMovement(float elapsed)
        {
            if (trackedObject == null)
                return;

            float duration = isLerping ? camSpeed * elapsed : 1;

            if (isLerping && Vector3.Distance(transform.position, trackedObject.position) < LERP_THRESHOLD)
                isLerping = false;

            transform.LerpToTarget(trackedObject, duration);
        }

        /// <summary>
        /// Updates the controller to follow
        /// </summary>
        public void SetController(Controller controller, bool teleportToTarget = true)
        {
            trackedObject = controller.Eyes;
            isLerping = !teleportToTarget;
            transform.SetParent(trackedObject);

            UpdateMovement(0);
        }

        #region MonoBehaviour

        /// <inheritdoc cref="Update" />
        private void Update() => UpdateMovement(Time.deltaTime);

        #endregion

        #region Singleton

        /// <inheritdoc/>
        protected override bool DestroyOnLoad => true;

        #endregion
    }
}