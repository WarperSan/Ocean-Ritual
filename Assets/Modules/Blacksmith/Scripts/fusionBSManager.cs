using System.Linq;
using UnityEngine;

public class fusionBSManager : MonoBehaviour
{
    [SerializeField] Fill fill;
    [SerializeField] GameObject button;

    private FusionCase[] inputs;
    private FusionCase output;

    [SerializeField] Sprite sprite;

    [SerializeField] AudioClip coolSound;

    private void Awake()
    {
        FusionCase[] cases = this.GetComponentsInChildren<FusionCase>();

        this.output = cases.FirstOrDefault(c => c.role == CaseRole.OUTPUT);
        this.inputs = cases.Where(c => c.role == CaseRole.INPUT).ToArray();

        if (this.output == null)
        {
            Debug.LogError($"'{nameof(fusionBSManager)}' expected an output.");
            this.enabled = false;
            return;
        }

        if (this.inputs.Length < 2)
        {
            Debug.LogError($"'{nameof(fusionBSManager)}' expected at least two inputs.");
            this.enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool buttonActive = true;

        foreach (FusionCase item in this.inputs)
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
            FusionCase left = this.inputs[0];
            FusionCase right = this.inputs[1];

            int lvl = GemHelper.fusionGemTab(left.gem.LVL, right.gem.LVL);
            GemData TheGemme = GeneratorGem.GenerateRandomGemme(lvl, left.gem);
            TheGemme.sprite = sprite;
            this.output.ReceiveGem(TheGemme);
            left.ClearGem();
            right.ClearGem();

            SoundManager.Instance.PlaySound(coolSound, SoundType.UI);

            // Clear fill
            fill.EmptyFill();
        }
    }
}
