using UnityEngine;

public class TestGenerateGem : MonoBehaviour
{
    [SerializeField]
    private int lvlGem1;

    [SerializeField]
    private int lvlGem2;

    [SerializeField]
    private int lvlFinal;

    [SerializeField]
    private bool test;

    [SerializeField]
    private GemData GemData = new();

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (test)
        {
            test = false;
            int lvl = GemHelper.fusionGemTab(lvlGem1, lvlGem2);
            lvlGem1 = lvl;
            lvlGem2 = lvl + 1;
            GemData = GeneratorGem.GenerateRandomGemme(lvl, GemData);
        }
    }
}