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
        Enum NextMode();

        /// <summary>
        /// Sets the current mode to the given mode
        /// </summary>
        void SetMode(Enum mode);
    }

    /// <inheritdoc cref="IMultiMode"/>
    /// <remarks>
    /// This is a generic wrapper for <see cref="IMultiMode"/>
    /// </remarks>
    public interface IMultiMode<T> : IMultiMode where T : Enum
    {
        /// <inheritdoc cref="IMultiMode.NextMode"/>
        new T NextMode();

        /// <inheritdoc/>
        Enum IMultiMode.NextMode() => NextMode();

        /// <inheritdoc cref="IMultiMode.SetMode(Enum)"/>
        void SetMode(T mode);

        /// <inheritdoc/>
        void IMultiMode.SetMode(Enum mode)
        {
            if (mode is T modeT)
                SetMode(modeT);
        }
    }
}