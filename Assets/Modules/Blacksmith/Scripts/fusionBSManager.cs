using System.Linq;
using UnityEngine;

public class fusionBSManager : MonoBehaviour
{
    [SerializeField]
    private Fill fill;

    [SerializeField]
    private GameObject button;

    private FusionCase[] inputs;
    private FusionCase output;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private AudioClip coolSound;

    private void Awake()
    {
        FusionCase[] cases = GetComponentsInChildren<FusionCase>();

        output = cases.FirstOrDefault(c => c.role == CaseRole.OUTPUT);
        inputs = cases.Where(c => c.role == CaseRole.INPUT).ToArray();

        if (output == null)
        {
            Debug.LogError($"'{nameof(fusionBSManager)}' expected an output.");
            enabled = false;
            return;
        }

        if (inputs.Length < 2)
        {
            Debug.LogError($"'{nameof(fusionBSManager)}' expected at least two inputs.");
            enabled = false;
            return;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        bool buttonActive = true;

        foreach (FusionCase item in inputs)
        {
            if (item.gem == null)
            {
                buttonActive = false;
                break;
            }

            if (item.gem.LVL <= 0)
            {
                buttonActive = false;
                break;
            }
        }

        button.SetActive(buttonActive);

        if (fill.CanFuse)
        {
            FusionCase left = inputs[0];
            FusionCase right = inputs[1];

            int lvl = GemHelper.fusionGemTab(left.gem.LVL, right.gem.LVL);
            GemData TheGemme = GeneratorGem.GenerateRandomGemme(lvl, left.gem);
            TheGemme.sprite = sprite;
            output.ReceiveGem(TheGemme);
            left.ClearGem();
            right.ClearGem();

            SoundManager.Instance.PlaySound(coolSound, SoundType.UI);

            // Clear fill
            fill.EmptyFill();
        }
    }
}