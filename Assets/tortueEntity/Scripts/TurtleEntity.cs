using EntityModule.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleEntity :  EntityBehaviour
{
    #region EntityBehaviour

    protected override void OnStart()
    {
        base.OnStart();
    }

    /// <inheritdoc/>
    private void Update()
    {
        this.UpdateTree();
    }

    #endregion
}

