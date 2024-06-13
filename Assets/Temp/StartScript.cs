using UnityEngine;

public class StartScript : MonoBehaviour
{
    public Fishing.Fish[] fishes;
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
        foreach (Fishing.FishData fishData in data.Fishes)
        {
            Fishing.Fish fishAsset = fishData.GetAsset<Fishing.Fish>();
            Debug.Log(fishAsset.DisplayName + ": " + fishData.Amount);
        }

        data.Fishes.RemoveAll(fishes[0].Namespace);

        // Add fish to the inventory
        data.Fishes.Add(this.fishes);

        // Save
        Save.SaveManager.SaveToCache(data);
        Save.SaveManager.Save(1, true);
    }
}