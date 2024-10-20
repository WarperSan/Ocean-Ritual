using UnityEngine;

using UnityEngine;

public class Fill : MonoBehaviour
{
    [SerializeField] public RectTransform leftArmFill;       // Remplissage du bras gauche
    [SerializeField] public RectTransform rightArmFill;      // Remplissage du bras droit
    [SerializeField] public RectTransform centerFill;        // Remplissage central

    [SerializeField] public float removingAmout = 10f;       // Quantité à retirer

    [SerializeField] public float growthPer10Percent = 60f;  // Croissance des bras gauche et droit par tranche de 10%
    [SerializeField] public float centerGrowthPer10Percent = 40f; // Croissance du centre par tranche de 10%

    [Range(0, 1)] public float fillAmountCote = 0f;          // Remplissage pour les côtés gauche et droit
    [Range(0, 1)] public float fillAmountMilieu = 0f;        // Remplissage pour la partie centrale

    [SerializeField] ForgeButton button;
    [SerializeField] public bool CanFuse = false;
    void Update()
    {
        if (!button.isButtonHeld)
        {
            RemovingFill();
        }

        updateFillCase();
    }

    public void AddingFill(float amount)
    {
        // Si les côtés ne sont pas à 100%, on ajoute d'abord à fillAmountCote
        if (fillAmountCote < 1f)
        {
            float spaceInCote = 1f - fillAmountCote;
            if (amount <= spaceInCote)
            {
                // Si la quantité à ajouter rentre dans le côté
                fillAmountCote += amount;
            }
            else
            {
                // Si on dépasse, on ajoute ce qu'il reste à fillAmountMilieu
                fillAmountCote = 1f; // Les côtés sont remplis à 100%
                float remainingAmount = amount - spaceInCote;
                fillAmountMilieu = Mathf.Min(fillAmountMilieu + remainingAmount, 1f); // Limite à 100%
            }
        }
        else
        {
            // Si les côtés sont déjà remplis, tout va au centre
            fillAmountMilieu = Mathf.Min(fillAmountMilieu + amount, 1f); // Limite à 100%
        }
    }

    public void RemovingFill()
    {
        // Quantité à retirer en fonction du temps écoulé entre les frames
        float amountToRemove = removingAmout * Time.deltaTime;

        // On commence par retirer du milieu si nécessaire
        if (fillAmountCote < 1f || fillAmountMilieu < 1f)
        {
            if (fillAmountMilieu > 0f)
            {
                fillAmountMilieu = Mathf.Max(fillAmountMilieu - amountToRemove, 0f); // Limite à 0%
            }
            else if (fillAmountCote > 0f)
            {
                // Ensuite, on retire des côtés si le milieu est vide
                fillAmountCote = Mathf.Max(fillAmountCote - amountToRemove, 0f); // Limite à 0%
            }
        }
    }


    private void updateFillCase()
    {
        CanFuse = fillAmountCote >= 1f && fillAmountMilieu >= 1f;
        // Remplissage des côtés gauche et droit
        float armGrowthFactor = fillAmountCote * 10f * growthPer10Percent;

        // Ajuster la taille des bras gauche et droit
        leftArmFill.sizeDelta = new Vector2(armGrowthFactor, leftArmFill.sizeDelta.y);
        rightArmFill.sizeDelta = new Vector2(armGrowthFactor, rightArmFill.sizeDelta.y);

        // Remplissage du centre
        float centerGrowthFactor = fillAmountMilieu * 10f * centerGrowthPer10Percent;

        // Ajuster la taille du centre (en hauteur)
        centerFill.sizeDelta = new Vector2(centerFill.sizeDelta.x, centerGrowthFactor);
    }
}
