using EntityModule.Entities;

namespace EntityModule.Enemies
{
    public class SharkEntity : EntityBehaviour
    {
        #region EntityBehaviour

        

        /// <inheritdoc/>
        private void Update()
        {
            this.UpdateTree();
        }

        #endregion
    }
}

