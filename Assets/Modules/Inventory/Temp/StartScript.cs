using UnityEngine;

public class StartScript : MonoBehaviour
{
    // public FishingModule.Fish[] fishes;
    // public bool save = true;
    // public bool extraData = true;

    // private void Start()
    // {
    //     // Load all items (Should do when the game first starts)
    //     InventoryModule.Registry.Load();
        
    //     // Tries to load the save #1, skip if failed
    //     if (!SaveModule.SaveManager.Load(1))
    //         return;

    //     // Fetch the cached data
    //     SaveModule.SaveData data = SaveModule.SaveManager.LoadFromCache();

    //     data.Fishes ??= new();

    //     // Scroll through every fish in save
    //     foreach (FishingModule.FishData fishData in data.Fishes)
    //     {
    //         FishingModule.Fish fishAsset = fishData.GetAsset<FishingModule.Fish>();
    //         Debug.Log(fishAsset.DisplayName + ": " + fishData.Amount);
    //     }

    //     data.Fishes.RemoveAll(fishes[0].Namespace);

    //     // Add fish to the inventory
    //     data.Fishes.Add(this.fishes);

    //     // Save
    //     SaveModule.SaveManager.SaveToCache(data);
    //     SaveModule.SaveManager.Save(1, true);
    // }
}