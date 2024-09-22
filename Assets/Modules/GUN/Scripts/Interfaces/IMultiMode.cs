using System;

namespace WeaponModule
{
    /// <summary>
    /// Defines a weapon that has multiple modes
    /// </summary>
    public interface IMultiMode
    {
        /// <summary>
        /// Fetches the next mode
        /// </summary>
        public Enum NextMode();

        /// <summary>
        /// Sets the current mode to the given mode
        /// </summary>
        public void SetMode(Enum mode);
    }

    /// <summary>
    /// Generic wrapper for <see cref="IMultiMode"/>
    /// </summary>
    public interface IMultiMode<T> : IMultiMode where T : Enum
    {
        /// <inheritdoc cref="IMultiMode.NextMode"/>
        public new T NextMode();

        /// <inheritdoc/>
        Enum IMultiMode.NextMode() => this.NextMode();

        /// <inheritdoc cref="IMultiMode.SetMode(Enum)"/>
        public void SetMode(T mode);

        /// <inheritdoc/>
        void IMultiMode.SetMode(Enum mode)
        {
            if (mode is T modeT)
                this.SetMode(modeT);
        }
    }
}