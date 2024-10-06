using BehaviourModule.Interfaces;
using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using UnityEngine;

namespace EntityModule.Enemies
{
    public class SharkBehaviourTree : MonoBehaviour, IVisualizable
    {
        /// <inheritdoc/>
        private void Start() => this.RebuildRoot();

        #region IVisualizable

        protected Node root;

        /// <inheritdoc/>
        public Node GetRoot() => this.root;

        /// <inheritdoc/>
        public void RebuildRoot() => this.root = this.SetUpTree();

        private Node SetUpTree()
        {
            // Attack when in range
            // Delay between strikes
            // Rushes towards the player and attacks it

            Sequence _root = new();
            _root += this.AttackSequence();
            _root += new Parallel(
                this.RotateSequence(),
                this.MovementSequence()
            );

            return null;
        }

        #endregion

        #region Attack

        private Node AttackSequence()
        {
            Sequence attackSequence = new();

            return attackSequence.Alias("Attack Sequence");
        }
        #endregion

        #region Movement

        private Node MovementSequence()
        {
            Sequence movementSequence = new();

            return movementSequence.Alias("Movement Sequence");
        }
        #endregion

        #region Rotation
        private Node RotateSequence()
        {
            Sequence rotateSequence = new();
            

            return rotateSequence.Alias("Rotate Sequence");
        }
        #endregion


    }
}