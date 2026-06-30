using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    [SerializeField]
    private int numberOfTheScene; // Index de la sc�ne � charger

    // M�thode appel�e par le bouton pour changer de sc�ne
    public void ChangeScene() => SceneManager.LoadScene(numberOfTheScene);
}