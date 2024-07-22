using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Buoyancy
{
    // To customize, you can change the values in:
    // - GetForceAtDepth()
    // - OnEnterWater()
    // - OnExitWater()

    public class VoxelBuoyancyVolume : BuoyancyVolume
    {
        #region TEMP

        // Obtains the Y position of the water at the given position
        public static float GetWaterHeight(Vector3 pos) => Mathf.Sin(Time.time + pos.x) / 5;

        #endregion

        public float offset;
        public Rigidbody rb;
        public Collider Collider;

        private Vector3 GetForceAtDepth(Vector3 singularForce, float depth) => 2 * depth * singularForce;

        #region MonoBehaviour

        private void Awake() => this.BuildVoxels(this.Collider.bounds);

        #endregion

        #region Voxels

        [Range(1, 24), SerializeField, Tooltip("Determines how many voxel each dimension has")]
        private uint density = 1;

        private Vector3[] voxels = System.Array.Empty<Vector3>();
        private Vector3 voxelSize = Vector3.zero;

        /// <summary>
        /// Builds an array of voxels that fills the given bounds
        /// </summary>
        private void BuildVoxels(Bounds bounds)
        {
            // Fetch bounds data
            Vector3 distances = bounds.size;
            Vector3 offsets = bounds.center;

            // Calculate dimensions
            Vector3Int numbers = new Vector3Int(
                Mathf.CeilToInt(distances.x * this.density),
                Mathf.CeilToInt(distances.y * this.density),
                Mathf.CeilToInt(distances.z * this.density)
            );
            Vector3 sizes = new Vector3(
                distances.x / numbers.x,
                distances.y / numbers.y,
                distances.z / numbers.z
            );

            // Precalculate used data
            Vector3 halfNumbers = numbers / 2;
            Vector3 halfSizes = sizes / 2;
            offsets += halfSizes;

            // Set values
            this.voxels = new Vector3[numbers.x * numbers.y * numbers.z];
            this.voxelSize = halfSizes;

            // Create every voxel
            Vector3 pos = Vector3.zero;
            int index = 0;

            for (int x = 0; x < numbers.x; x++)
            {
                pos.x = ((x - halfNumbers.x) * sizes.x) + offsets.x;
                for (int y = 0; y < numbers.y; y++)
                {
                    pos.y = ((y - halfNumbers.y) * sizes.y) + offsets.y;
                    for (int z = 0; z < numbers.z; z++)
                    {
                        pos.z = ((z - halfNumbers.z) * sizes.z) + offsets.z;

                        this.voxels.SetValue(pos, index);

                        ++index;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the global version of the given position and the depth of the point
        /// </summary>
        private float GetVoxelPosition(Vector3 localPosition, out Vector3 globalPosition)
        {
            globalPosition = this.transform.TransformPoint(localPosition);
            return globalPosition.y - GetWaterHeight(globalPosition) - this.offset;
        }

        #endregion

        #region BuoyancyVolume

        /// <inheritdoc/>
        protected override void ApplyBuoyancy()
        {
            Vector3 singularForce = -Physics.gravity / this.voxels.Length;

            // Apply a force for every point underwater
            foreach (var item in this.voxels)
            {
                float depth = this.GetVoxelPosition(item, out Vector3 pos);

                if (depth >= 0)
                    continue;

                this.rb.AddForceAtPosition(
                    this.GetForceAtDepth(singularForce, -depth),
                    pos
                );
            }

            this.isInWater = GetWaterHeight(this.rb.worldCenterOfMass) > this.rb.worldCenterOfMass.y;
        }

        /// <inheritdoc/>
        protected override void OnEnterWater() => this.rb.drag = 0.5f;

        /// <inheritdoc/>
        protected override void OnExitWater() => this.rb.drag = 0.1f;

        #endregion

        #region Gizmos
#if UNITY_EDITOR
        private readonly Gradient color = new()
        {
            colorKeys = new GradientColorKey[]
            {
                new(Color.blue, 0),
                new(Color.red, 1)
            }
        };

        private void OnDrawGizmos()
        {
            // Draw each voxel point
            foreach (var item in this.voxels)
            {
                float depth = this.GetVoxelPosition(item, out Vector3 pos);

                Gizmos.color = color.Evaluate(1 + depth);
                Gizmos.DrawCube(pos, voxelSize);
            }

            // Draw wave (FOR DEBUG)
            Gizmos.color = Color.grey;

            for (float x = -2; x < 2; x += 0.1f)
            {
                for (float z = -2; z < 2; z += 0.1f)
                {
                    Vector3 pos = this.transform.position + new Vector3(x, 0, z);
                    pos.y = GetWaterHeight(pos);

                    Gizmos.DrawCube(pos, Vector3.one * 0.2f);
                }
            }
        }
#endif
        #endregion
    }
}