using UnityEngine;
using UnityEngine.UI;

public class ChangeColorBasedOnBool : MonoBehaviour
{
    public bool isActive;  // Le booléen qui va déterminer la couleur
   [SerializeField] public Image image;    // L'élément UI Image à changer de couleur

    // Couleurs pour le booléen true et false
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.gray;
    public bool start = true;
   

    void Update()
    {
      
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
    public void SwapState()
    {
        if (TestBlackSmith.Instance.canAddNewCase|| start|| isActive)
        {
            start = false;
            isActive = !isActive;
            UpdateColor();
        }
        
    }

}
