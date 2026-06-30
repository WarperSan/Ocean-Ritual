using ExtensionsModule;
using UnityEngine;

namespace ControllerModule.Controllers
{
    public class LookController : MonoBehaviour
    {
        #region Parameters

        [Header("Parameters")]
        [SerializeField]
        [Min(0)]
        [Tooltip("Determines how fast the camera rotates")]
        private float sensitivity = 5.0f;

        #endregion

        #region Angles Clamp

        [Header("Angles Clamp")]
        [SerializeField]
        [Tooltip("Max angles that this controller can rotate")]
        private Vector3 maxAngles;

        [SerializeField]
        [Tooltip("Axis on which the angles are clamped (0 or 1)")]
        private Vector3Int clampAxis;

        #endregion

        #region Object Rotations

        [Header("Object Rotations")]
        [SerializeField]
        [Tooltip("Determines with which offset the anchor rotates")]
        private Transform parentController;

        [SerializeField]
        [Tooltip("Root of the object to turn horizontally")]
        private Rigidbody self;

        [Tooltip("Object that will turn the camera")]
        public Transform cameraAnchor;

        #endregion

        #region Update

        /// <summary>
        /// Current rotation
        /// </summary>
        private Vector3 camRotation = Vector3.zero;

        /// <summary>
        /// Updates the rotation of the target
        /// </summary>
        /// <param name="direction">Direction of the rotation</param>
        /// <param name="elapsed">Time since the last frame</param>
        public void UpdateRotation(Vector2 direction, float elapsed)
        {
            // Update rotation
            camRotation = camRotation.ClampRotation(
                elapsed * sensitivity * new Vector3(-direction.y, direction.x, 0),
                maxAngles,
                clampAxis
            );

            Vector3 copy = camRotation;

            // Add offset
            if (parentController != null)
                copy += parentController.eulerAngles;

            // Rotate correct Transform
            if (self != null)
            {
                self.rotation = Quaternion.Euler(0, copy.y, 0);

                copy.y = 0;
            }

            if (cameraAnchor != null)
                cameraAnchor.localRotation = Quaternion.Euler(copy);
        }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc cref="Start" />
        private void Start() => camRotation = cameraAnchor.localEulerAngles;

        #if UNITY_EDITOR
        /// <inheritdoc cref="OnValidate" />
        private void OnValidate()
        {
            // Limit angles between [0; 180]
            maxAngles.Set(
                Mathf.Clamp(maxAngles.x, 0, 180),
                Mathf.Clamp(maxAngles.y, 0, 180),
                Mathf.Clamp(maxAngles.z, 0, 180)
            );

            // Make sure the axis are only 0 or 1
            clampAxis.Set(
                clampAxis.x <= 0 ? 0 : 1,
                clampAxis.y <= 0 ? 0 : 1,
                clampAxis.z <= 0 ? 0 : 1
            );
        }

        /// <inheritdoc cref="OnDrawGizmosSelected" />
        private void OnDrawGizmosSelected()
        {
            // Only in inspector
            if (Application.isPlaying)
                return;

            Vector3 center = cameraAnchor?.position ?? Vector3.zero;
            Vector3 angles = maxAngles * Mathf.Deg2Rad;

            // X
            if (clampAxis.x == 1)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawLine(
                    center,
                    center + new Vector3(0, Mathf.Sin(angles.x), Mathf.Cos(angles.x))
                );

                Gizmos.DrawLine(
                    center,
                    center + new Vector3(0, Mathf.Sin(-angles.x), Mathf.Cos(-angles.x))
                );
            }

            // Y
            if (clampAxis.y == 1)
            {
                Gizmos.color = Color.green;

                Gizmos.DrawLine(
                    center,
                    center + new Vector3(Mathf.Sin(angles.y), 0, Mathf.Cos(angles.y))
                );

                Gizmos.DrawLine(
                    center,
                    center + new Vector3(Mathf.Sin(-angles.y), 0, Mathf.Cos(-angles.y))
                );
            }

            // Z
            if (clampAxis.z == 1)
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawLine(
                    center,
                    center + new Vector3(Mathf.Cos(angles.z), Mathf.Sin(angles.z), 0)
                );

                Gizmos.DrawLine(
                    center,
                    center + new Vector3(Mathf.Cos(-angles.z), Mathf.Sin(-angles.z), 0)
                );
            }
        }

        #endif

        #endregion
    }
}