using ExtensionsModule;
using UnityEngine;

public class StarsManager : MonoBehaviour
{
    [SerializeField] GameObject Bronze;
    [SerializeField] GameObject Argent;
    [SerializeField] GameObject Or;
    [SerializeField] GameObject Empty;

    [SerializeField]
    private Transform[] starContainers;

    [SerializeField] int totalStars = 16;

    public void CreateStars(GemData gemData)
    {
        int lvl = gemData.LVL;

        // On vide d'abord le conteneur pour �viter les doublons
        foreach (Transform item in this.starContainers)
            item.RemoveAll();

        // Calcul du nombre de chaque �toile
        int numOr = lvl / 10;            // Chaque �toile Or repr�sente 10 niveaux
        int remainingAfterOr = lvl % 10;

        int numArgent = remainingAfterOr / 5; // Chaque �toile Argent repr�sente 5 niveaux
        int remainingAfterArgent = remainingAfterOr % 5;

        int numBronze = remainingAfterArgent; // Chaque �toile Bronze repr�sente 1 niveau

        int count = 0;

        // Cr�ation des �toiles Or
        for (int i = 0; i < numOr; i++)
        {
            this.CreateStar(this.Or, count);
            count++;
        }

        // Cr�ation des �toiles Argent
        for (int i = 0; i < numArgent; i++)
        {
            this.CreateStar(this.Argent, count);
            count++;
        }

        // Cr�ation des �toiles Bronze
        for (int i = 0; i < numBronze; i++)
        {
            this.CreateStar(this.Bronze, count);
            count++;
        }

        for (int i = numBronze + numArgent + numOr; i < totalStars; i++)
        {
            this.CreateStar(this.Empty, count);
            count++;
        }
    }

    private void CreateStar(GameObject prefab, int starIndex)
    {
        Transform parent = this.starContainers[starIndex / 4];
        Instantiate(prefab, parent);
    }
}
