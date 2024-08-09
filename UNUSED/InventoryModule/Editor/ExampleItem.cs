public class ExampleItem : Inventory.Item<ExampleData1>
{
    public int TestPublic = 100;

    protected override bool IsCorrupted(ExampleData1 data) => data.Test < 0;
    protected override void SetData(ExampleData1 data) => data.Test = this.TestPublic;
    protected override ExampleData1 GetData() => new()
    {
        Test = this.TestPublic,
    };

}

[System.Serializable]
public class ExampleData1 : Inventory.ItemData
{
    public int Test;
}