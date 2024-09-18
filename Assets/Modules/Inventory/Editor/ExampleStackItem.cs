public class ExampleStackItem : InventoryModule.Item<ExampleData2>
{
    public int TestPublic = 100;

    protected override bool IsCorrupted(ExampleData2 data) => data.Test < 0;
    protected override void SetData(ExampleData2 data) => data.Test = this.TestPublic;
    protected override ExampleData2 GetData() => new()
    {
        Test = this.TestPublic,
    };
}

[System.Serializable]
public class ExampleData2 : InventoryModule.ItemStackData<ExampleData2>
{
    public int Test;

    public override bool IsSame(ExampleData2 other) => this.Test == other.Test;
}