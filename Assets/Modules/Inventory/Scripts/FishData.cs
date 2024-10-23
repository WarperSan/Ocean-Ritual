using FishingModule;

[System.Serializable]
public class FishData : ItemData
{
    public FishSO fish;

    public FishData(FishSO fish, int quantity)
    {
        this.fish = fish;
        sprite = fish.Icon;

        this.quantity = quantity;
    }
}