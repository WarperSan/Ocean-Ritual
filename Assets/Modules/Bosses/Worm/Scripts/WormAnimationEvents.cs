using UnityEngine;

namespace BossesModule.Worm
{
    public class WormAnimationEvents : MonoBehaviour
    {
        public WormTree tree;

        #region Spawn

        public void SetSpawning()
        {
            if (!TryGetComponent(out Animator animator))
                return;

            animator.SetBool("isSpawning", false);
            tree.GetRoot().SetData(WormTree.CURRENT_TARGET, TargetGeneral.Instance.BoatTarget);
        }

        #endregion

        #region Underwater

        public void EscapeDiveEnded()   => tree.OnEscapeStartEnded();
        public void EscapeEmergeEnded() => tree.OnEscapeEndEnded();

        public void RepositionDiveEnded()   => tree.OnRepositionStartEnded();
        public void RepositionEmergeEnded() => tree.OnRepositionEndEnded();

        #endregion

        #region Attack

        public void IceStormAnimationEnded() => tree.OnAttackEnded();

        public void IceWaveStart() => tree.IceWaveStart();

        public void IceWaveEnd() => tree.OnAttackEnded();

        #endregion
    }
}