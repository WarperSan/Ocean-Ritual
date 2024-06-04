using UnityEngine;

public class StartScript : MonoBehaviour
{
    public Inventory.Item item;
    public bool save = true;

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
        }
        else
        {
            Inventory.ItemData itemData = data.item;

            if (Inventory.Registry.GetLoadedItem(itemData, out Fishing.Fish fish))
            {
                fish.Load(itemData);
                Debug.Log(fish.DisplayName);
            }
        }
    }
}