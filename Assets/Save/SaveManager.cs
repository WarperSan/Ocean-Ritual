using Extensions;
using Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var directory = GetDirectory();

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
            var path = GetPath(saveIndex);

            // Check for overwrite
            if (!overwrite && File.Exists(path))
            {
                Debug.LogWarning($"A file already exists at '{path}'.");
                return false;
            }

            // Write data to file
            File.WriteAllText(path, JsonUtility.ToJson(data));
            Debug.Log($"File saved at: '{path}'.");

            return true;
        }

        /// <summary>
        /// Loads the data from the given save file
        /// </summary>
        /// <returns>Loaded data or null if an error occured</returns>
        private static SaveData? ReadDataFromFile(string path)
        {
            // Check if file exists
            if (!File.Exists(path))
            {
                Debug.LogWarning($"No save file is stored at '{path}'.");
                return null;
            }

            try
            {
                var content = File.ReadAllText(path);
                SaveData data = JsonUtility.FromJson<SaveData>(content);

                Debug.Log($"File loaded from: '{path}'.");

                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Error while loading '{path}': " + e.Message);
            }

            return null;
        }

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
            // Fetch all objects to save
            IEnumerable<ISaveable> objects = FetchObjects();

            // Create the save data
            SaveData data = new();
            objects.ForEach(i => i.OnSaving(ref data));

            return Save(data, saveIndex, overwrite);
        }

        /// <summary>
        /// Tries to save the given state of the game at the given save index
        /// </summary>
        /// <param name="data">State of the game</param>
        /// <param name="saveIndex">Index of the save file</param>
        /// <param name="overwrite">Can overwrite an existing file</param>
        /// <returns>Succeed to save</returns>
        public static bool Save(SaveData data, int saveIndex, bool overwrite = false)
        {
            // Store the version of the save file
            data.VERSION = GetVersion();

            // Write to file
            var success = WriteDataToFile(data, saveIndex, overwrite);

            // Notify of the success
            if (success)
            {
                // Fetch all objects to notify
                IEnumerable<ISaveable> objects = FetchObjects();
                objects.ForEach(i => i.OnSaved());
            }

            return success;
        }

        /// <summary>
        /// Tries to load the given save 
        /// </summary>
        /// <param name="saveIndex">Index of the save file</param>
        /// <returns>Succeed to load</returns>
        public static bool Load(int saveIndex)
        {
            var path = GetPath(saveIndex);

            // Fetch content
            SaveData? d = ReadDataFromFile(path);

            // If save file valid, load
            return d.HasValue && Load(d.Value);
        }

        /// <summary>
        /// Tries to load the given game state
        /// </summary>
        /// <param name="data">Game state to load</param>
        /// <returns>Succeed to load</returns>
        public static bool Load(SaveData data)
        {
            // Fetch all objects to load
            IEnumerable<ISaveable> objects = FetchObjects();

            // Convert to version
            var version = data.VERSION;
            foreach (ISaveable item in objects)
            {
                // If save file invalid, cancel load
                if (!item.ConvertToVersion(version, ref data))
                {
                    var currentVersion = GetVersion();
                    Debug.LogWarning($"The conversion from '{version}' to '{currentVersion}' is impossible.");
                    return false;
                }
            }

            // Load the objects
            objects.ForEach(i => i.OnLoading(data));

            return true;
        }

        #endregion

        #region ISaveable

        /// <returns>Objects to save</returns>
        private static IEnumerable<ISaveable> FetchObjects() => Object.FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();

        #endregion
    }
}