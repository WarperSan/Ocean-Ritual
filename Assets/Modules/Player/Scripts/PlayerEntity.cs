using System.Collections.Generic;
using UnityEngine;

namespace EntityModule
{
    public class PlayerEntity : Entity
    {
        #region Entity

        /// <inheritdoc/>
        public override bool TakeDamage => false;

        #endregion
    }
}

