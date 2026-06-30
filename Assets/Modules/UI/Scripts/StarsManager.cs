using ExtensionsModule;
using UnityEngine;

public class StarsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Bronze;

    [SerializeField]
    private GameObject Argent;

    [SerializeField]
    private GameObject Or;

    [SerializeField]
    private GameObject Empty;

    [SerializeField]
    private Transform[] starContainers;

    [SerializeField]
    private int totalStars = 16;

    public void ClearStars() => CreateStars(0);

    public void CreateStars(GemData gemData) => CreateStars(gemData.LVL);

    private void CreateStars(int lvl)
    {
        // On vide d'abord le conteneur pour �viter les doublons
        foreach (Transform item in starContainers)
            item.RemoveAll();

        // Calcul du nombre de chaque �toile
        int numOr = lvl / 10; // Chaque �toile Or repr�sente 10 niveaux
        int remainingAfterOr = lvl % 10;

        int numArgent = remainingAfterOr / 5; // Chaque �toile Argent repr�sente 5 niveaux
        int remainingAfterArgent = remainingAfterOr % 5;

        int numBronze = remainingAfterArgent; // Chaque �toile Bronze repr�sente 1 niveau

        int count = 0;

        // Cr�ation des �toiles Or
        for (int i = 0; i < numOr; i++)
        {
            CreateStar(Or, count);
            count++;
        }

        // Cr�ation des �toiles Argent
        for (int i = 0; i < numArgent; i++)
        {
            CreateStar(Argent, count);
            count++;
        }

        // Cr�ation des �toiles Bronze
        for (int i = 0; i < numBronze; i++)
        {
            CreateStar(Bronze, count);
            count++;
        }

        for (int i = numBronze + numArgent + numOr; i < totalStars; i++)
        {
            CreateStar(Empty, count);
            count++;
        }
    }

    private void CreateStar(GameObject prefab, int starIndex)
    {
        Transform parent = starContainers[starIndex / 4];
        Instantiate(prefab, parent);
    }
}