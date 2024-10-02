using UnityEngine;

public class GemmeGrid : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public bool[,] Grid;

    #endregion

    #region Unity Methods

    void Start()
    {
        //InitializeTab();
        // Exemple();
    }

    #endregion

    #region Initialization

    // Initializes the Grid with the specified width and height
    public void InitializeTab()
    {
        Grid = new bool[width, height];
    }

    #endregion

    #region Object Placement

    //Checks if an object can be placed at the given coordinates
    public bool CanPlaceObject(int x, int y, bool[,] form)
    {
        int width = form.GetLength(0);
        int height = form.GetLength(1);

        // Assuming width and height are always odd
        int centerX = width / 2;
        int centerY = height / 2;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int gridX = x + (i - centerX);
                int gridY = y + (j - centerY);

                if (form[i, j] && (gridX < 0 || gridY < 0 || gridX >= this.width || gridY >= this.height || this.Grid[gridX, gridY]))
                {
                    return false;
                }
            }
        }
        return true;
    }

    // Places an object at the given coordinates if possible
    public bool PlaceObject(int x, int y, bool[,] form)
    {
        if (CanPlaceObject(x, y, form))
        {
            int largeurForme = form.GetLength(0);
            int height = form.GetLength(1);

                int centerX = largeurForme / 2;
                int centerY = height / 2;

            for (int i = 0; i < largeurForme; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (form[i, j])
                    {
                        int gridX = x + (i - centerX);
                        int gridY = y + (j - centerY);
                        Grid[gridX, gridY] = true;
                    }
                }
            }
            return true;
        }
        return false;
    }
    private void RemoveObject(int x, int y, bool[,] form)
    {
        
        int width = form.GetLength(0);
        int height = form.GetLength(1);

        int centerX = width / 2;
        int centerY = height / 2;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (form[i, j])
                {
                    int gridX = x + (i - centerX);
                    int gridY = y + (j - centerY);
                    Grid[gridX, gridY] = false;
                }
            }
        }
    }

    // Updates the grid with the new object placement
    private void UpdateGrid(int x, int y, bool[,] form)
    {
        int width = form.GetLength(0);
        int height = form.GetLength(1);

        int centerX = width / 2;
        int centerY = height / 2;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (form[i, j])
                {
                    int gridX = x + (i - centerX);
                    int gridY = y + (j - centerY);
                  
                  
                    Grid[gridX, gridY] = true;
                }
            }
        }
    }
    // Attempts to place an object on the grid and updates the grid accordingly
    public bool TryPlaceObjectOnGrid(int x, int y, bool[,] form, Gem gemmeToPlace, bool[,] formBoolPrincipal)
    {
        // Find the previous position of the gem and remove it
        RemoveObject(gemmeToPlace.PositionX, gemmeToPlace.PositionZ, formBoolPrincipal);

        if (CanPlaceObject(x, y, form))
        {
            UpdateGrid(x, y, form);

            // Update the gem's position
            gemmeToPlace.PositionX = x;
            gemmeToPlace.PositionZ = y;

            return true;
        }
        else
        {
            // Re-add the old gem if placement fails
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