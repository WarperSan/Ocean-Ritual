using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;
using static EnumGeneral;
using UnityEditor;

public static class GemHelper
{
    public static int modifiedSpotCost = 5;
    public static int missingSpotCost = 10;

    public static (int cost, int modifiedSpot, int missingSpot,bool CasseOnlytrue) GetInformationAboutForm(List<bool> formOriginal, List<bool> modifiedForm, int lvlGem)
    {
      
        // V�rification si la liste modifi�e est plus grande que l'originale
        if (modifiedForm.Count > formOriginal.Count)
        {
            throw new ArgumentException("La liste modifi�e ne peut pas �tre plus grande que la liste originale.");
        }

        int modifiedSpot = 0;
        int missingSpot = 0;

        int originalTrueCount = 0;
        int modifiedTrueCount = 0;

        // Comparaison des bool�ens dans chaque position des deux listes
        for (int i = 0; i < formOriginal.Count; i++)
        {
            // Compte les "true" dans l'original
            if (formOriginal[i]) originalTrueCount++;

            // Compte les "true" dans le modifi� uniquement si la case correspondante est aussi "true"
            if (i < modifiedForm.Count && modifiedForm[i])
            {
                modifiedTrueCount++;

                // Si la valeur est diff�rente entre l'original et le modifi�
                if (!formOriginal[i]) // Si l'original est false et le modifi� est true
                {
                    modifiedSpot++;
                }
            }
            else if (formOriginal[i]) // Si l'original est true et le modifi� est false
            {
                missingSpot++;
            }
        }

        // V�rification si la liste modifi�e contient plus de "true" que l'originale
        if (modifiedTrueCount > originalTrueCount)
        {
            throw new ArgumentException("Le nombre de 'true' dans la liste modifi�e ne peut pas �tre sup�rieur � celui de la liste originale.");
        }
        bool CasseOnlytrue = modifiedTrueCount == 1 || originalTrueCount == 1;
        missingSpot = missingSpot - modifiedSpot;
        // Calcul du co�t total
        int totalCost = Mathf.CeilToInt((modifiedSpot * modifiedSpotCost) + (missingSpot * missingSpotCost ));
        return (totalCost, modifiedSpot, missingSpot, CasseOnlytrue);
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
    public static void ModifiedList(ref List<bool> ListBool, int Xposition, int YPosition, bool value, int width)
    {
        // Calculer l'index dans la liste
        int index = Xposition * width + YPosition;//YPosition * width + Xposition;

        // V�rifier que l'index est valide
        if (index >= 0 && index < ListBool.Count)
        {
            ListBool[index] = value; // Modifier la valeur � la position sp�cifi�e
        }
        else
        {
            Debug.LogWarning("Index hors limites. Impossible de modifier la liste.");
        }
    }
    public static GemData ConvertGemToGemData(Gem gem)
    {
        List<bool> list = new List<bool>(gem.form.flatForme);
        // Cr�ation d'un nouvel objet GemData
        GemData gemData = new GemData
        {
            // Copie des propri�t�s de Gem vers GemData
           
            Shape = new FormBool(list, gem.form.width, gem.form.height), 
            GemColorsName = gem.GemColorsName,
            LVL = gem.LVL,

            // Copie des listes des types et quantit�s
            typeWeapon = new List<TypeQuantity<TypeWeapon>>(gem.typeWeapon),
            typeBoat = new List<TypeQuantity<TypeBoat>>(gem.typeBoat),
            typeNet = new List<TypeQuantity<TypeNet>>(gem.typeNet)
        };

        return gemData;
    }
    
        public static Gem ConvertGemDataToGem(GemData gemData)
        {
            // Cr�ation d'un nouvel objet Gem
            Gem gem = new Gem
            {
                // Copie des propri�t�s de GemData vers Gem
                PositionX = 0,
                PositionZ = 0,
                form = gemData.Shape,  
                GemColorsName = gemData.GemColorsName,
                LVL = gemData.LVL,

                // Copie des listes des types et quantit�s
                typeWeapon = new List<TypeQuantity<TypeWeapon>>(gemData.typeWeapon),
                typeBoat = new List<TypeQuantity<TypeBoat>>(gemData.typeBoat),
                typeNet = new List<TypeQuantity<TypeNet>>(gemData.typeNet)
            };

            return gem;
        }
    

}
