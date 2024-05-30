using System.IO;
using UnityEngine;

namespace Save
{
    /// <summary>
    /// Class that manages the saving and the loading of the game state
    /// </summary>
    public class SaveManager
    {
        #region Getters

        /// <returns>Path to the save files</returns>
        private static string GetDirectory() => Application.persistentDataPath + "/Saves/";

        /// <returns>Name of the save file at the given index</returns>
        private static string GetFileName(int saveIndex) => $"save{saveIndex}.ocrt";

        /// <returns>Full path of the save fle at the given index</returns>
        private static string GetPath(int saveIndex)
        {
            // Create directory if missing
            string directory = GetDirectory();

            if (!Directory.Exists(directory))
                _ = Directory.CreateDirectory(directory);

            return directory + GetFileName(saveIndex);
        }

        /// <returns>Current version of the save system</returns>
        private static string GetVersion() => Application.version;

        #endregion

        #region File Actions

        /// <summary>
        /// Writes the given data to a save file
        /// </summary>
        /// <param name="data">Data to wriet</param>
        /// <param name="saveIndex">Index of the save file</param>
        /// <param name="overwrite">Can overwrite an existing file</param>
        /// <returns>Succeed to write the file</returns>
        private static bool WriteDataToFile(SaveData data, int saveIndex, bool overwrite = false)
        {
            string path = GetPath(saveIndex);

            // Check for overwrite
            if (!overwrite && File.Exists(path))
            {
                Debug.LogWarning($"A file already exists at '{path}'.");
                return false;
            }

            // Write data to file
#if UNITY_EDITOR
            string json = JsonUtility.ToJson(data, true);
#else
            string json = JsonUtility.ToJson(data);
#endif
            File.WriteAllText(path, json);
            Debug.Log($"File saved at: '{path}'.");

            return true;
        }

        /// <summary>
        /// Loads the data from the given save file
        /// </summary>
        /// <param name="data">Loaded data</param>
        /// <returns>Succeed to load</returns>
        private static bool ReadDataFromFile(string path, out SaveData? data)
        {
            data = null;

            // Check if file exists
            if (!File.Exists(path))
            {
                Debug.LogWarning($"No save file is stored at '{path}'.");
                return false;
            }

            try
            {
                string content = File.ReadAllText(path);
                data = JsonUtility.FromJson<SaveData>(content);

                Debug.Log($"File loaded from: '{path}'.");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Error while loading '{path}': " + e.Message);
            }

            return false;
        }

        #endregion

        #region Cache

        private static SaveData? cachedData = null;
        private static int currentSaveIndex = 0;

        /// <summary>
        /// Loads the save file from the cache
        /// </summary>
        /// <returns>Loaded data</returns>        
        public static SaveData LoadFromCache()
        {
            // If cached loaded, return cache
            if (cachedData.HasValue)
                return cachedData.Value;

            Debug.LogWarning("It is recommended to cache the save file before trying to load it.");

            // If load succeed, return cache
            if (Load(currentSaveIndex))
                return cachedData.Value;

            Debug.LogError($"Loading the save file '{currentSaveIndex}' failed.");
            return default;
        }

        /// <summary>
        /// Replaces the cached data with the given data
        /// </summary>
        /// <param name="data">New data to cache</param>
        public static void SaveToCache(SaveData data) => cachedData = data;

        #endregion

        #region Actions

        /// <summary>
        /// Tries to save the current state of the game at the given save index
        /// </summary>
        /// <param name="saveIndex">Index of the save file</param>
        /// <param name="overwrite">Can overwrite an existing file</param>
        /// <returns>Succeed to save</returns>
        public static bool Save(int saveIndex, bool overwrite = false)
        {
            // Modify data from cached data
            SaveData data = cachedData ?? default;
            OnSave?.Invoke(ref data);

            // Store the version of the save file
            data.VERSION = GetVersion();

            // Write to file
            return WriteDataToFile(data, saveIndex, overwrite);
        }

        /// <summary>
        /// Tries to load the given save and updates the cached data
        /// </summary>
        /// <param name="saveIndex">Index of the save file</param>
        /// <returns>Succeed to load</returns>
        public static bool Load(int saveIndex)
        {
            string path = GetPath(saveIndex);

            // Fetch content and cache
            bool succeed = ReadDataFromFile(path, out SaveData? data);
            cachedData = data;

            // If the load succeed
            if (succeed)
                OnLoad?.Invoke(cachedData.Value);

            // Update selected save
            SaveManager.currentSaveIndex = saveIndex;

            return succeed;
        }

        #endregion

        #region Events

        public delegate void SaveEvent(ref SaveData data);
        public static event SaveEvent OnSave;

        public delegate void LoadEvent(SaveData data);
        public static event LoadEvent OnLoad;

        #endregion
    }
}