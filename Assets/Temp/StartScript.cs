using UnityEngine;

public class StartScript : MonoBehaviour
{
    public Inventory.Item item;
    public bool save = true;
    public bool extraData = true;

    private void Start()
    {
        Save.SaveManager.Load(1);
        Inventory.Registry.FetchAll();

        Save.SaveData data = Save.SaveManager.LoadFromCache();

        if (this.save)
        {
            data.item = item.Save();
            Save.SaveManager.SaveToCache(data);
            Save.SaveManager.Save(1, true);
            return;
        }

        Inventory.ItemData itemData = data.item;

        if (this.extraData)
        {
            if (Inventory.Registry.GetExtraData(itemData, out Fishing.FishSOData fishData))
            {
                Debug.Log(fishData.Amount);
            }
            
            return;
        }

        if (Inventory.Registry.GetLoadedItem(itemData, out Fishing.Fish fish))
        {
            Debug.Log(fish.DisplayName);
        }
    }
}