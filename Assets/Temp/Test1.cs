using Interfaces;
using Save;
using UnityEngine;

public class Test1 : MonoBehaviour, ISaveable
{
    public int A;

    public void OnLoading(SaveData data)
    {
        this.A = data.monsterA.A;
    }

    public void OnSaving(ref SaveData data) 
    {
        data.monsterA = new Test1Data()
        {
            A = this.A
        };
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        this.transform.Rotate(new Vector3(0, 45 * Time.deltaTime, 0));
    }
}