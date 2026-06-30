using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionWheel : MonoBehaviour
{
    [SerializeField]
    private AudioClip popSound;

    [SerializeField]
    public List<GameObject> listeForgeable = new();

    [SerializeField]
    private GameObject boutonPrefab;

    [SerializeField]
    private GameObject Container;

    [SerializeField]
    private LinkWheelEquipment linkWheelEquipment;

    // Start is called before the first frame update
    private void Start() => CreatMenu();

    // Update is called once per frame
    private void Update()
    {
    }

    public void CreatMenu()
    {
        // Efface les anciens boutons dans le container, si n�cessaire
        foreach (Transform child in Container.transform)
            Destroy(child.gameObject);

        // Boucle � travers chaque objet de la liste `listeForgeable`
        foreach (GameObject forgeable in listeForgeable)
        {
            // R�cup�re le composant `componentGBN` de l'objet
            componentGBN gbnComponent = forgeable.GetComponent<componentGBN>();

            if (gbnComponent != null)
            {
                // Instancie un nouveau bouton � partir du prefab
                GameObject boutonInstance = Instantiate(boutonPrefab, Container.transform);

                boutonInstance.GetComponent<Button>().onClick.AddListener(() => CreatSocleFromLinkWheelEquipement(gbnComponent));

                boutonInstance.GetComponent<Button>().onClick.AddListener(() => playSound());
                TextMeshProUGUI boutonText = boutonInstance.GetComponentInChildren<TextMeshProUGUI>();

                if (boutonText != null)
                    boutonText.text = gbnComponent.gameObject.name;
            }
        }
    }

    public void playSound() => SoundManager.Instance.PlaySound(popSound, SoundType.UI, 1f);

    public void CreatSocleFromLinkWheelEquipement(componentGBN GBN) => linkWheelEquipment.ResetStand(GBN);
}