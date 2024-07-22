using UnityEngine;

namespace Buoyancy
{
    /// <summary>
    /// Class that defines all the scripts that represent a buoyancy volume
    /// </summary>
    public abstract class BuoyancyVolume : MonoBehaviour
    {
        private void UpdateVolume(float elapsed)
        {
            bool wasInWater = this.IsInWater;

            // Update buoyancy
            this.ApplyBuoyancy();

            bool isInWater = this.IsInWater;

            // If did not change state, call stay
            if (wasInWater == isInWater)
            {
                if (isInWater)
                    this.OnStayWater(elapsed);
                else
                    this.OnStayAir(elapsed);
            }
            else
            {
                if (isInWater)
                    this.OnEnterWater();
                else
                    this.OnExitWater();
            }
        }

        #region MonoBehaviour

        /// <inheritdoc/>
        private void FixedUpdate() => this.UpdateVolume(Time.fixedDeltaTime);

        #endregion

        #region Abstract

        /// <summary>
        /// Determines if this volume is in the water or not
        /// </summary>
        public bool IsInWater => this.isInWater;

        /// <summary>
        /// Stores if this volume is considered to be in the water or not
        /// </summary>
        protected bool isInWater = false;

        /// <summary>
        /// Applies the buoyancy for this volume
        /// </summary>
        protected abstract void ApplyBuoyancy();

        #endregion

        #region Events

        /// <summary>
        /// Called when this volume enters the water
        /// </summary>
        protected virtual void OnEnterWater() { }

        /// <summary>
        /// Called when this volume exits the water
        /// </summary>
        protected virtual void OnExitWater() { }

        /// <summary>
        /// Called each frame this volume stays in the air
        /// </summary>
        protected virtual void OnStayAir(float elapsed) { }

        /// <summary>
        /// Called each frame this volume stays in the water
        /// </summary>
        protected virtual void OnStayWater(float elapsed) { }

        #endregion
    }
}
