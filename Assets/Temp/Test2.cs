using Save;

public class Test2 : SaveBehaviour
{
    public int B;

    public int C;

    protected override void OnLoad(SaveData data) 
    {
        this.B = data.monsterB.A;
    }

    protected override void OnSave(ref SaveData data) 
    {
        data.monsterB = new Test1Data() 
        {
            A = B,
        };
    }
}