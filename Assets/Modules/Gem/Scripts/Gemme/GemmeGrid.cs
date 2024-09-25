using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemmeGrid : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public bool[,] tableau;

    #endregion

    #region Unity Methods

    void Start()
    {
        //InitializeTab();
        // Exemple();
    }

    #endregion

    #region Initialization

    // Initializes the tableau with the specified width and height
    public void InitializeTab()
    {
        tableau = new bool[width, height];
    }

    #endregion

    #region Object Placement

    //Checks if an object can be placed at the given coordinates
    public bool CanPlaceObject(int x, int y, bool[,] forme)
    {
        int largeurForme = forme.GetLength(0);
        int hauteurForme = forme.GetLength(1);

        // Assuming largeurForme and hauteurForme are always odd
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

    // Places an object at the given coordinates if possible
    public bool PlaceObject(int x, int y, bool[,] forme)
    {
        if (CanPlaceObject(x, y, forme))
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
    private void RemoveObject(int x, int y, bool[,] forme)
    {
        Debug.Log(forme);
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
                    tableau[gridX, gridY] = false;
                }
            }
        }
    }

    // Updates the grid with the new object placement
    private void UpdateGrid(int x, int y, bool[,] forme)
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
    }
    // Attempts to place an object on the grid and updates the grid accordingly
    public bool TryPlaceObjectOnGrid(int x, int y, bool[,] forme, Gemme gemmeToPlace, bool[,] formBoolPrincipal)
    {
        // Find the previous position of the gemme and remove it
        RemoveObject(gemmeToPlace.PositionX, gemmeToPlace.PositionZ, formBoolPrincipal);

        if (CanPlaceObject(x, y, forme))
        {
            UpdateGrid(x, y, forme);

            // Update the gemme's position
            gemmeToPlace.PositionX = x;
            gemmeToPlace.PositionZ = y;

            return true;
        }
        else
        {
            // Re-add the old gemme if placement fails
            UpdateGrid(gemmeToPlace.PositionX, gemmeToPlace.PositionZ, formBoolPrincipal);
            return false;
        }
    }

    #endregion

    #region Example Usage

    // Example method to demonstrate placing an object
    void Exemple()
    {
        bool[,] formeT = new bool[,]
        {
            { false, false, true },
            { true, true, true },
            { false, false, true }
        };

        bool placeReussi = PlaceObject(2, 1, formeT);
        if (placeReussi)
        {
            Debug.Log("Object placed successfully!");
        }
        else
        {
            Debug.Log("Failed to place the object.");
        }
    }

    #endregion
}