using UnityEngine;

public class StartScript : MonoBehaviour
{
    public Fishing.Fish fish;
    public bool save = true;
    public bool extraData = true;

    private void Start()
    {
        // Load all items (Should do when the game first starts)
        Inventory.Registry.Load();
        
        // Tries to load the save #1, skip if failed
        if (!Save.SaveManager.Load(1))
            return;

        // Fetch the cached data
        Save.SaveData data = Save.SaveManager.LoadFromCache();

        // Scroll through every fish in save
        foreach ((Fishing.Fish asset, Fishing.FishSOData data) item in data.Fishes.Data)
        {
            Debug.Log(item.asset.DisplayName + ": " + item.data.Amount);
        }

        // Add fish to the inventory
        data.Fishes.Add(fish);

        // Save
        Save.SaveManager.SaveToCache(data);
        Save.SaveManager.Save(1, true);
    }
}