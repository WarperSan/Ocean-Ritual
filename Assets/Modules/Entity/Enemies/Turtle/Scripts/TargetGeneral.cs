using UnityEngine;
using UtilsModule;

public class TargetGeneral : Singleton<TargetGeneral>
{
    public Transform Target;

    public Transform BoatTarget;

    #region Singleton

    /// <inheritdoc/>
    protected override bool DestroyOnLoad => true;

    #endregion
}