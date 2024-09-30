using UnityEngine;
using UnityEngine.UI;

public class ChangeColorBasedOnBool : MonoBehaviour
{
    public bool isActive;  // Le booléen qui va déterminer la couleur
   [SerializeField] public Image image;    // L'élément UI Image à changer de couleur

    // Couleurs pour le booléen true et false
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.gray;

   

    void Update()
    {
        // Vous pouvez appeler UpdateColor() à chaque changement de "isActive" selon vos besoins
        if (Input.GetKeyDown(KeyCode.Space)) // Exemple : Change le booléen avec la barre d'espace
        {
            isActive = !isActive;
            UpdateColor();
        }
    }

    public void UpdateColor()
    {
        if (isActive)
        {
            image.color = activeColor; // Si le booléen est true, couleur verte
        }
        else
        {
            image.color = inactiveColor; // Sinon, couleur grise
        }
    }
}
