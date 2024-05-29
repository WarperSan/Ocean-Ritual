using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

static public class EnumGeneral 
{
     // Définir un dictionnaire pour associer TypeDeSocle à une énumération spécifique
    public static Dictionary<TypeDeSocle, Type> TypeDeSocleToEnum = new()
    {
        { TypeDeSocle.Arme, typeof(TypeArme) },
        { TypeDeSocle.Bateau, typeof(TypeBateau) },
        { TypeDeSocle.Filet, typeof(TypeFilet) }
    };

    // Méthode pour obtenir l'énumération associée à un TypeDeSocle
    public static Type GetEnumType(TypeDeSocle typeDeSocle)
    {
        if (TypeDeSocleToEnum.ContainsKey(typeDeSocle))
        {
            return TypeDeSocleToEnum[typeDeSocle];
        }
        else
        {
            Debug.LogError($"TypeDeSocle {typeDeSocle} non géré.");
            return null;
        }
    }
    public enum TypeDeSocle
    {
        Arme,
        Bateau,
        Filet
    }

    // Énumération pour les types d'armes
    public enum TypeArme
    {
        ReloadSpeed,
        attack,
        bulletspeed,
        bulletSize,
        fireRate
    }

    // Énumération pour les types de bateaux
    public enum TypeBateau
    {
        Life,
        Shield,
        Armor,
        Resistance,
        navigateSpeed
    }

    // Énumération pour les types de filets
    public enum TypeFilet
    {
        MoreWeight,
        NumberofFishCatch,
        DistanceOfEnnemieSpawn,
        TimeBetwenneEnnemieSpawn,
        

    }



}
[System.Serializable]
public class TypeQuantite<TEnum>
{
    public TEnum Type;
    public float Quantite;

    public TypeQuantite(TEnum type, float quantite)
    {
        Type = type;
        Quantite = quantite;
    }
}


[Serializable]
public struct FormeBool
{
    public bool[,] forme;

    public FormeBool(bool[,] forme)
    {
        this.forme = forme;
    }

    // Editeur personnalisé pour afficher et éditer la forme booléenne
#if UNITY_EDITOR
    public void InitializeIfNeeded(int width, int height)
    {
        if (forme == null || forme.GetLength(0) != width || forme.GetLength(1) != height)
        {
            forme = new bool[width, height];
        }
    }
#endif
}
