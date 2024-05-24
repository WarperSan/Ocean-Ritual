using Interfaces;
using Save;
using UnityEngine;

public class Test2 : MonoBehaviour, ISaveable
{
    public int B;

    public int C;

    public void OnLoading(SaveData data)
    {
        this.B = data.monsterB.A;
    }

    public void OnSaving(ref SaveData data)
    {
        data.monsterB = new Test1Data()
        {
            A = B,
        };
    }
}