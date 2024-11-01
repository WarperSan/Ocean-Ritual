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
    public void UpGrade()
    {
        Debug.Log("passeUpgrade");
        if (CanUpgrade()) // Vérifie d'abord si une amélioration est possible
        {
            width += 1;  // Augmente la largeur
            height += 1; // Augmente la hauteur
        }
        else
        {
            Debug.Log("Impossible d'améliorer : la largeur ou la hauteur dépasse déjà ou sont déja à 6.");
        }
    }
    public bool CanUpgrade()
    {
        // Vérifie si les dimensions actuelles dépassent 6 après une amélioration
        return (width < 6 && height < 6);
    }
    #endregion

    #region Object Placement

    //Checks if an object can be placed at the given coordinates
    public bool CanPlaceObject(int x, int y, bool[,] form)
    {
        int width = form.GetLength(0);
        int height = form.GetLength(1);

        // Calcul du centre en supposant que largeur et hauteur sont toujours impairs
        int centerX = width / 2;
        int centerY = height / 2;


        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int gridX = x + (i - centerX);
                int gridY = y + (j - centerY);

                // Vérification des limites de la grille et des conflits de placement
                if (form[i, j] && (gridX < 0 || gridY < 0 || gridX >= this.width || gridY >= this.height || this.Grid[gridX, gridY]))
                {
                    // Appel de la fonction de débogage en cas de problème de placement
                   // DebugPlacementIssue(gridX, gridY, form, x, y);
                  
                    return false;
                }
            }
        }
      
        return true;
    }

    // Fonction de débogage pour afficher les informations et les tableaux form et Grid
    private void DebugPlacementIssue(int gridX, int gridY, bool[,] form, int startX, int startY)
    {
        Debug.LogError($"Problème de placement détecté à (x: {gridX}, y: {gridY})");

        // Affichage du tableau form
        string formString = "Tableau form :\n" + BoolArrayToString(form);
        Debug.Log(formString);

        // Affichage de la sous-partie de Grid correspondant à la taille de form
        string gridString = "Tableau Grid (zone de vérification) :\n" + ExtractGridSectionToString(startX, startY, form.GetLength(0), form.GetLength(1));
        Debug.Log(gridString);
    }

    // Convertit un tableau booléen en chaîne de caractères pour affichage
    private string BoolArrayToString(bool[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        string result = "";

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result += array[i, j] ? "1 " : "0 ";
            }
            result += "\n";
        }

        return result;
    }

    // Extrait une section de Grid autour de (startX, startY) de la taille de form
    private string ExtractGridSectionToString(int startX, int startY, int width, int height)
    {
        string result = "";

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int gridX = startX + i - (width / 2);
                int gridY = startY + j - (height / 2);

                // Affichage de la valeur de Grid ou d'un espace vide si en dehors des limites
                if (gridX >= 0 && gridY >= 0 && gridX < this.width && gridY < this.height)
                {
                    result += this.Grid[gridX, gridY] ? "1 " : "0 ";
                }
                else
                {
                    result += ". "; // Symbole pour indiquer une zone hors de la grille
                }
            }
            result += "\n";
        }

        return result;
    }

    // Places an object at the given coordinates if possible
    public bool PlaceObject(int x, int y, bool[,] form)
    {
        //Debug.Log("passe  PlaceObject");
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
        //Debug.Log(x);
        //Debug.Log(y);
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
                    //Debug.Log(gridX);
                    //Debug.Log(gridY);
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
        // Find the previous position of the gems and remove it
        RemoveObject(gemmeToPlace.PositionX, gemmeToPlace.PositionZ, formBoolPrincipal);

        if (CanPlaceObject(x, y, form))
        {
            UpdateGrid(x, y, form);

            // Update the gems's position
            gemmeToPlace.PositionX = x;
            gemmeToPlace.PositionZ = y;

            return true;
        }
        else
        {
            // Re-add the old gems if placement fails
            UpdateGrid(gemmeToPlace.PositionX, gemmeToPlace.PositionZ, formBoolPrincipal);
            return false;
        }
    }
    //appeler si on tente de placer une gemme qui n'est pas sur la grid (de l'inventaire)
    public bool TryPlaceObjectOnGrid(int x, int y, Gem gemmeToPlace)
    {

       bool[,] form = gemmeToPlace.form.GetForme();
        if (CanPlaceObject(x, y, form))
        {
            UpdateGrid(x, y, form);
           // Debug.Log(x);
         //   Debug.Log(y);
            // Update the gems's position
            gemmeToPlace.PositionX = x;
            gemmeToPlace.PositionZ = y;
           // Debug.Log("true TryPlaceObjectOnGrid");
            return true;
        }
        //Debug.Log("false TryPlaceObjectOnGrid");
            
            return false;
        
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