using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;

public static class GemHelperBlacksmith
{
    public static int modifiedSpotCost = 5;
    public static int missingSpotCost = 10;

    public static (int cost, int modifiedSpot, int missingSpot) GetInformationAboutForm(List<bool> formOriginal, List<bool> modifiedForm,int lvlGem)
    {
        // Vérification si la liste modifiée est plus grande que l'originale
        if (modifiedForm.Count > formOriginal.Count)
        {
            throw new ArgumentException("La liste modifiée ne peut pas être plus grande que la liste originale.");
        }

        int modifiedSpot = 0;
        int missingSpot = 0;

        int originalTrueCount = 0;
        int modifiedTrueCount = 0;

        // Comparaison des booléens dans chaque position des deux listes
        for (int i = 0; i < formOriginal.Count; i++)
        {
            if (formOriginal[i]) originalTrueCount++;  // Compte les "true" dans l'original
            if (i < modifiedForm.Count && modifiedForm[i]) modifiedTrueCount++;  // Compte les "true" dans le modifié

            // Si la valeur est différente entre l'original et le modifié
            if (i < modifiedForm.Count && formOriginal[i] != modifiedForm[i])
            {
                modifiedSpot++;
            }
        }

        // Vérification si la liste modifiée contient plus de "true" que l'originale
        if (modifiedTrueCount > originalTrueCount)
        {
            throw new ArgumentException("Le nombre de 'true' dans la liste modifiée ne peut pas être supérieur à celui de la liste originale.");
        }

        // Calcul du nombre de spots manquants
        missingSpot = originalTrueCount - modifiedTrueCount;

        // Calcul du coût total  cout augment plus le niveau de la Gem est haut
        int totalCost = Mathf.CeilToInt((modifiedSpot * modifiedSpotCost) + (missingSpot * (1 + (lvlGem / 100f))));


        return (totalCost, modifiedSpot, missingSpot);
    }
    //ex
    //1-1 =2  
    //2-2 = 3 
    //2-1 = 2 
    //1-3= 2
    public static int fusionGemTab(int lvlLeftGem, int lvlRightGem)
    {
        return (lvlLeftGem >= lvlRightGem) ? lvlLeftGem + (lvlLeftGem == lvlRightGem ? 1 : 0) : lvlLeftGem + 1;
    }
    public static void ModifiedList(ref List<bool> ListBool, int Xposition, int YPosition, bool value, int height, int width)
    {
        // Calculer l'index dans la liste
        int index = YPosition * width + Xposition;

        // Vérifier que l'index est valide
        if (index >= 0 && index < ListBool.Count)
        {
            ListBool[index] = value; // Modifier la valeur à la position spécifiée
        }
        else
        {
            Debug.LogWarning("Index hors limites. Impossible de modifier la liste.");
        }
    }


}
