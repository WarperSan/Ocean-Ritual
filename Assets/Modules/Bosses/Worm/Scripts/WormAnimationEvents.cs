using BossesModule.Golem;
using EntityModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossesModule.Worm
{
    public class WormAnimationEvents : MonoBehaviour
    {
        public WormTree tree;

        #region Spawn

        public void SetSpawning()
        {
            if (!this.TryGetComponent(out Animator animator))
                return;

            animator.SetBool("isSpawning", false);
            tree.GetRoot().SetData(WormTree.CURRENT_TARGET, TargetGeneral.Instance.BoatTarget);
        }

        #endregion
    }
}
