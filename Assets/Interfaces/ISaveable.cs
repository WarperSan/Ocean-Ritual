
namespace Interfaces
{
    public interface ISaveable
    {
        #region Virtual

        /// <summary>
        /// Called when this object is starting being saved
        /// </summary>
        /// <param name="data">Data being saved</param>
        public void OnSaving(ref Save.SaveData data);

        /// <summary>
        /// Called when this object has been saved
        /// </summary>
        public virtual void OnSaved() { }

        /// <summary>
        /// Called when this object is starting being loaded
        /// </summary>
        public void OnLoading(Save.SaveData data);

        /// <summary>
        /// Called when this object has been loaded
        /// </summary>
        public virtual void OnLoaded() { }

        /// <summary>
        /// Modifies the loaded data depending on the given version
        /// </summary>
        /// <param name="version">Version of the data</param>
        /// <param name="data">Loaded version</param>
        /// <returns>Is the save file valid?</returns>
        public virtual bool ConvertToVersion(string version, ref Save.SaveData data) => true;

        #endregion
    }
}