using System;
using System.Collections.Generic;
using UnityEngine;

public static class EnumGeneral
{
    // D�finir un dictionnaire pour associer TypeOfSocle � une �num�ration sp�cifique
    public static Dictionary<TypeOfSocle, Type> TypeDeSocleToEnum = new()
    {
        {
            TypeOfSocle.Weapon, typeof(TypeWeapon)
        },
        {
            TypeOfSocle.Bateau, typeof(TypeBoat)
        },
        {
            TypeOfSocle.Net, typeof(TypeNet)
        },
    };

    // M�thode pour obtenir l'�num�ration associ�e � un TypeOfSocle
    public static Type GetEnumType(TypeOfSocle typeDeSocle)
    {
        if (TypeDeSocleToEnum.ContainsKey(typeDeSocle))
            return TypeDeSocleToEnum[typeDeSocle];
        else
        {
            Debug.LogError($"TypeOfSocle {typeDeSocle} non g�r�.");
            return null;
        }
    }

    public enum TypeOfSocle
    {
        Weapon,
        Bateau,
        Net,
    }

    // �num�ration pour les types d'armes
    public enum TypeWeapon
    {
        VitesseRechargement,
        Attaque,
        VitesseBalle,
        TailleDeBalle,
        VitesseDeTire,
        CapaciterDeBall,

        Porter,
    }

    // �num�ration pour les types de bateaux
    public enum TypeBoat
    {
        Vie,
        VitesseDeNavigation,
        Maniment,
    }

    // �num�ration pour les types de filets
    public enum TypeNet
    {
        Poid,
        PlusDePoissonAttraper,
        CercleDePeche,
        TempsEntreLesVague,
    }

    public enum EffectType
    {
        Nothing,
        Glace,
        Feu,
    }

    public enum TypeOfSort
    {
        Nom,
        Type,
        Quantite,
        Fusion,
        Niveau,
    }

    public enum ColorsName
    {
        Rouge,
        Blue,
        orange,
        Vert,
        Mauve,
    }
}

//
[Serializable]
public class TypeQuantity<TEnum>
{
    public TEnum Type;
    public float Quantite;

    public TypeQuantity(TEnum type, float quantite)
    {
        Type = type;
        Quantite = quantite;
    }
}

[Serializable]
public class UpgradeNameData
{
    public int quantity;
    public string name;

    // Constructeur par d�faut
    public UpgradeNameData()
    {
        // Valeurs par d�faut si n�cessaire
        quantity = 0;
        name = string.Empty;
    }

    public UpgradeNameData(int quantity, string name)
    {
        this.quantity = quantity;
        this.name = name;
    }
}

[Serializable]
public class UpgradeStats
{
    public List<UpgradeNameData> baseStats = new();
    public List<UpgradeNameData> previewStats = new();
    public int upgradeCost;

    public string name;

    // Constructeur par d�faut
    public UpgradeStats()
    {
        // Valeurs par d�faut si n�cessaire
        upgradeCost = 0;
    }

    public UpgradeStats(
        List<UpgradeNameData> baseStats,
        List<UpgradeNameData> previewStats,
        int                   upgradeCost,
        string                name
    )
    {
        this.baseStats = baseStats;
        this.previewStats = previewStats;
        this.upgradeCost = upgradeCost;
        this.name = name;
    }
}

[Serializable]
public class FormBool
{
    public int width;
    public int height;
    public List<bool> flatForme;

    public FormBool(bool[,] form, int width, int height)
    {
        this.width = width;
        this.height = height;
        flatForme = new List<bool>(width * height);

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
                flatForme.Add(form[i, j]);
        }
    }

    public FormBool(List<bool> form, int width, int height)
    {
        this.width = width;
        this.height = height;
        flatForme = new List<bool>(form);
    }

    public bool[,] GetForme()
    {
        bool[,] form = new bool[width, height];

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                int flatIndex = (height - 1 - j) * width + i; // Reverse rows, but keep columns order
                form[i, j] = flatForme[flatIndex];
            }
        }
        return form;
    }

    public bool[,] Rotate(bool[,] form, int angle)
    {
        bool[,] rotatedForme = form;

        // Normalize the angle to one of the expected values (90, 180, -90)
        angle = (angle % 360 + 360) % 360;

        if (angle == 90)
            RotateForm90(); // Rotate 90� clockwise
        else if (angle == 180)
        {
            RotateForm90();
            RotateForm90(); // Rotate twice for 180�
        }
        else if (angle == -90 || angle == 270)
        {
            RotateForm90();
            RotateForm90();
            RotateForm90(); // Rotate three times for -90� (270� clockwise)
        }

        return rotatedForme;
    }

    private void RotateForm90()
    {
        int size = (int)Mathf.Round(Mathf.Sqrt(flatForme.Count));
        var rotated = new List<bool>(new bool[size * size]);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // Calcul des index � partir de flatForme pour une rotation de 90�
                int originalIndex = y * size + x;
                int rotatedIndex = x * size + (size - 1 - y);
                rotated[rotatedIndex] = flatForme[originalIndex];
            }
        }

        // Mise � jour de flatForme avec les nouvelles valeurs
        flatForme = rotated;
    }
}

public static class DictionaryGenerator
{
    #region GameObject Dictionary Generation

    // Function to generate a dictionary of GameObjects from a given path
    public static Dictionary<string, GameObject> DictionaryGameObjectGenerator(string Path)
    {
        // Create a new dictionary to store GameObjects
        var Dictionary = new Dictionary<string, GameObject>();

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