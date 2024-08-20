using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] GameObject boutonManager;

    Button[] ListeBouton;
   
    void Start()
    {

        int compteur = 1;
        ListeBouton = boutonManager.GetComponentsInChildren<Button>();
        foreach (Button b in ListeBouton)
        {
            int temporaire = compteur;          
       
                b.onClick.AddListener(() => ChangerScene(temporaire));

            Debug.Log(b);
            compteur++;
        }
    }

    
    
    public void ChangerScene(int i)
    {
        Debug.Log("click");
        SceneManager.LoadScene(i);
    }
    
}
