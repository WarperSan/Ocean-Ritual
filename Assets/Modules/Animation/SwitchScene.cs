using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    [SerializeField] private int numberOfTheScene; // Index de la scène à charger

    // Méthode appelée par le bouton pour changer de scène
    public void ChangeScene()
    {
        SceneManager.LoadScene(numberOfTheScene);
    }
}
