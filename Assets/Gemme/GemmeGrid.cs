using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemmeGrid : MonoBehaviour
{
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public bool[,] tableau;

    void Start()
    {
        InitializeTableau();
        Exemple();
    }

    public void InitializeTableau()
    {
        tableau = new bool[width, height];
    }

    public bool PeutPlacerObjet(int x, int y, bool[,] forme)
    {
        int largeurForme = forme.GetLength(0);
        int hauteurForme = forme.GetLength(1);

        // Assumant que largeurForme et hauteurForme sont toujours impairs
        int centreX = largeurForme / 2;
        int centreY = hauteurForme / 2;

        for (int i = 0; i < largeurForme; i++)
        {
            for (int j = 0; j < hauteurForme; j++)
            {
                int gridX = x + (i - centreX);
                int gridY = y + (j - centreY);

                if (forme[i, j] && (gridX < 0 || gridY < 0 || gridX >= width || gridY >= height || tableau[gridX, gridY]))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool PlacerObjet(int x, int y, bool[,] forme)
    {
        if (PeutPlacerObjet(x, y, forme))
        {
            int largeurForme = forme.GetLength(0);
            int hauteurForme = forme.GetLength(1);

            int centreX = largeurForme / 2;
            int centreY = hauteurForme / 2;

            for (int i = 0; i < largeurForme; i++)
            {
                for (int j = 0; j < hauteurForme; j++)
                {
                    if (forme[i, j])
                    {
                        int gridX = x + (i - centreX);
                        int gridY = y + (j - centreY);
                        tableau[gridX, gridY] = true;
                    }
                }
            }
            return true;
        }
        return false;
    }

    void Exemple()
    {
        bool[,] formeT = new bool[,]
        {
            { false, false, true },
            { true, true, true },
            { false, false, true }
        };

        bool placeReussi = PlacerObjet(2,1, formeT);
        if (placeReussi)
        {
            Debug.Log("Objet placé avec succès !");
        }
        else
        {
            Debug.Log("Impossible de placer l'objet.");
        }
    }
}