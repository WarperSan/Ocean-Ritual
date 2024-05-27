using Save;
using UnityEngine;

public class Test1 : SaveBehaviour
{
    public int A;

    protected override void OnSave(ref SaveData data) 
    {
        data.monsterA = new Test1Data()
        {
            A = this.A
        };
    }

    protected override void OnLoad(SaveData data) 
    {
        this.A = data.monsterA.A;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        this.transform.Rotate(new Vector3(0, this.A * Time.deltaTime, 0));
    }
}