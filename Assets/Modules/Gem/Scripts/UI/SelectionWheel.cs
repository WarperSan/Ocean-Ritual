using BlacksmithModule;
using ControllerModule.Interfaces.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIModule;
using UnityEngine;
using UnityEngine.UI;

public class SelectionWheel : MonoBehaviour
{
    [SerializeField] public List<GameObject> listeForgeable = new();
    [SerializeField] GameObject boutonPrefab;
    [SerializeField] GameObject Container;
    [SerializeField] LinkWheelEquipment linkWheelEquipment;
    // Start is called before the first frame update
    void Start()
    {
        CreatMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatMenu()
    {
        // Efface les anciens boutons dans le container, si nécessaire
        foreach (Transform child in Container.transform)
        {
            Destroy(child.gameObject);
        }

        // Boucle à travers chaque objet de la liste `listeForgeable`
        foreach (GameObject forgeable in listeForgeable)
        {
            // Récupère le composant `componentGBN` de l'objet
            componentGBN gbnComponent = forgeable.GetComponent<componentGBN>();

            if (gbnComponent != null)
            {
                // Instancie un nouveau bouton à partir du prefab
                GameObject boutonInstance = Instantiate(boutonPrefab, Container.transform);

   
                boutonInstance.GetComponent<Button>().onClick.AddListener(() => CreatSocleFromLinkWheelEquipement(gbnComponent));

               
                TextMeshProUGUI boutonText = boutonInstance.GetComponentInChildren<TextMeshProUGUI>();
                if (boutonText != null)
                {
                    boutonText.text = gbnComponent.gameObject.name; 
                }
            }
        }
    }
 


    public void CreatSocleFromLinkWheelEquipement(componentGBN GBN)
    {
        linkWheelEquipment.ResetStand(GBN);

    }
 
}
