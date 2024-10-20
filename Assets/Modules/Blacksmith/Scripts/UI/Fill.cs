using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fill : MonoBehaviour
{
  
 [SerializeField]   public RectTransform leftArmFill;   // Remplissage du bras gauche
    [SerializeField] public RectTransform rightArmFill;  // Remplissage du bras droit
    [SerializeField] public RectTransform centerFill;    // Remplissage central

    [SerializeField] public float armMaxWidth = 100f;    // La largeur maximale des bras (de gauche et de droite)
    [SerializeField] public float centerMaxHeight = 200f; // La hauteur maximale de la partie centrale

    [Range(0, 1)] public float fillAmount = 0f;  // La valeur de remplissage entre 0 et 1

    void Update()
    {
        // Remplir les bras gauche et droit
        if (fillAmount <= 0.5f)
        {
            float armFill = (fillAmount / 0.5f) * armMaxWidth;
            leftArmFill.sizeDelta = new Vector2(armFill, leftArmFill.sizeDelta.y);
            rightArmFill.sizeDelta = new Vector2(armFill, rightArmFill.sizeDelta.y);
        }
        else
        {
            // Remplir la partie centrale après que les bras soient remplis
            float centerFillAmount = ((fillAmount - 0.5f) / 0.5f) * centerMaxHeight;
            centerFill.sizeDelta = new Vector2(centerFill.sizeDelta.x, centerFillAmount);
        }
    }
}