using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsModule;

public class TargetGeneral : Singleton<TargetGeneral>
{
    public Transform Target;

    #region Singleton

    /// <inheritdoc/>
    protected override bool DestroyOnLoad => true;

    #endregion
}
