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

        #region Underwater

        public void EscapeDiveEnded() => this.tree.OnEscapeStartEnded();
        public void EscapeEmergeEnded() => this.tree.OnEscapeEndEnded();

        public void RepositionDiveEnded() => this.tree.OnRepositionStartEnded();
        public void RepositionEmergeEnded() => this.tree.OnRepositionEndEnded();

        #endregion

        #region Attack

        public void IceStormAnimationEnded() => this.tree.OnAttackEnded();

        #endregion
    }
}
