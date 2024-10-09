using EntityModule.Entities;

namespace EntityModule.Enemies
{
    public class SharkEntity : EntityBehaviour
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
}

