using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

static public class EnumGeneral 
{
     // Définir un dictionnaire pour associer TypeOfSocle à une énumération spécifique
    public static Dictionary<TypeOfSocle, Type> TypeDeSocleToEnum = new()
    {
        { TypeOfSocle.Arme, typeof(TypeWeapon) },
        { TypeOfSocle.Bateau, typeof(TypeBoat) },
        { TypeOfSocle.Filet, typeof(TypeNet) }
    };

    // Méthode pour obtenir l'énumération associée à un TypeOfSocle
    public static Type GetEnumType(TypeOfSocle typeDeSocle)
    {
        if (TypeDeSocleToEnum.ContainsKey(typeDeSocle))
        {
            return TypeDeSocleToEnum[typeDeSocle];
        }
        else
        {
            Debug.LogError($"TypeOfSocle {typeDeSocle} non géré.");
            return null;
        }
    }
    public enum TypeOfSocle
    {
        Arme,
        Bateau,
        Filet
    }

    // Énumération pour les types d'armes
    public enum TypeWeapon
    {
        ReloadSpeed,
        attack,
        bulletspeed,
        bulletSize,
        fireRate,
        AmmoCapacity,
   
        Range
    }

    // Énumération pour les types de bateaux
    public enum TypeBoat
    {
        Life,
        Shield,
        Armor,
        Resistance,
        navigateSpeed
    }

    // Énumération pour les types de filets
    public enum TypeNet
    {
        MoreWeight,
        NumberofFishCatch,
        DistanceOfEnnemieSpawn,
        TimeBetwenneEnnemieSpawn,
        

    }


    public enum EffectType
    {
        Nothing,
        Glace,
        Feu
    }
    public enum TypeOfSort
    {
        Nom,
        Type,
        Quantite,
        Fusion,
        Niveau
    }
}
//
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
public class FormeBool
{
    public int width;
    public int height;
    public List<bool> flatForme;

    public FormeBool(bool[,] forme, int width, int height)
    {
        this.width = width;
        this.height = height;
        this.flatForme = new List<bool>(width * height);
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                flatForme.Add(forme[i, j]);
            }
        }
    }

    public bool[,] GetForme()
    {
        bool[,] forme = new bool[width, height];
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                int flatIndex = (height - 1 - j) * width + i; // Reverse rows, but keep columns order
                forme[i, j] = flatForme[flatIndex];
            }
        }
        return forme;
    }
    public bool[,] Rotate(bool[,] forme, int angle)
    {
        bool[,] rotatedForme = forme;

        // Normalize the angle to one of the expected values (90, 180, -90)
        angle = (angle % 360 + 360) % 360;

        if (angle == 90)
        {
            RotateForme90(); // Rotate 90° clockwise
        }
        else if (angle == 180)
        {
            RotateForme90();
             RotateForme90(); // Rotate twice for 180°
        }
        else if (angle == -90 || angle == 270)
        {
            RotateForme90();
            RotateForme90();
            RotateForme90(); // Rotate three times for -90° (270° clockwise)
        }

        return rotatedForme;
    }

    private void RotateMinus90(bool[,] forme)
    {
        
    }
    private void RotateForme90()
    {
        int size = (int)Mathf.Round(Mathf.Sqrt(flatForme.Count));
        List<bool> rotated = new List<bool>(new bool[size * size]);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // Calcul des index à partir de flatForme pour une rotation de 90°
                int originalIndex = y * size + x;
                int rotatedIndex = x * size + (size - 1 - y);
                rotated[rotatedIndex] = flatForme[originalIndex];
            }
        }

        // Mise à jour de flatForme avec les nouvelles valeurs
        flatForme = rotated;
    }

    private void Rotate180(bool[,] forme)
    {
       
    }
}


public static class DictionaryGenerator
{
    #region GameObject Dictionary Generation

    // Function to generate a dictionary of GameObjects from a given path
    static public Dictionary<string, GameObject> DictionaryGameObjectGenerator(string Path)
    {
        // Create a new dictionary to store GameObjects
        Dictionary<string, GameObject> Dictionary = new Dictionary<string, GameObject>();

        // Load all GameObjects from the specified path
        GameObject[] TabRessourceObject = Resources.LoadAll<GameObject>(Path);

        // Iterate through each loaded GameObject
        foreach (GameObject obj in TabRessourceObject)
        {
            // Check if the dictionary does not already contain this name
            if (!Dictionary.ContainsKey(obj.name))
            {
                // Add the GameObject to the dictionary
                Dictionary.Add(obj.name, obj);
            }
        }

        // Return the populated dictionary
        return Dictionary;
    }

    #endregion
}

