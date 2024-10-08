using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GestionInformation : MonoBehaviour
{
    public static GestionInformation Instance { get; private set; }
    public string path = "Inventory";
    [SerializeField] Image Photo;
    [SerializeField] ShowForm Form;
    [SerializeField] GameObject Gun;
    [SerializeField] GameObject Boat;
    [SerializeField] GameObject Net;
    Dictionary<string, GameObject> data;

    // Appelé avant le premier frame
    void Awake()
    {
        // Assurer que seul un singleton existe
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Garde cet objet lors du changement de scène
        }
        else
        {
            Destroy(gameObject); // Si une instance existe déjà, on détruit ce nouvel objet
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void GetResource()
    {
        // Récupérer les objets GameObject à partir du dictionnaire
        data = DictionaryGenerator.DictionaryGameObjectGenerator(path);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetNewInformationGem(GemData Gem)
    {
        if (data == null)
        {
            GetResource();
        }

        Form.CreateUi(Gem.Shape.GetForme());
        Photo.sprite = Gem.sprite;
        ChangeInfoStat(Gem);
    }

    // Affiche les informations de la gemme (stats) sur le UI
    public void ChangeInfoStat(GemData Gem)
    {
        // Ajouter les stats pour les armes
        if (Gem.typeWeapon != null && Gem.typeWeapon.Count > 0)
        {
            PopulateStatUI(Gem.typeWeapon, Gun);
        }

        // Ajouter les stats pour les bateaux
        if (Gem.typeBoat != null && Gem.typeBoat.Count > 0)
        {
            PopulateStatUI(Gem.typeBoat, Boat);
        }

        // Ajouter les stats pour les filets
        if (Gem.typeNet != null && Gem.typeNet.Count > 0)
        {
            PopulateStatUI(Gem.typeNet, Net);
        }
    }

    // Méthode générique pour remplir les UI en fonction des types (arme, bateau, filet)
    private void PopulateStatUI<TEnum>(List<TypeQuantity<TEnum>> list, GameObject parent)
    {
        // Efface les enfants précédents s'il y en a
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }

        // Instancie et remplit les éléments horizontaux
        for (int i = 0; i < list.Count; i += 2) // Pour chaque paire d'éléments
        {
            // Récupère le prefab de "horizontale" à partir du dictionnaire
            if (!data.TryGetValue("horizontale", out GameObject prefab))
            {
                Debug.LogWarning("Prefab 'horizontale' introuvable dans le dictionnaire.");
                continue;
            }

            // Instancie le prefab de "horizontale" sous le parent correspondant (Gun, Boat, Net)
            GameObject horizontalInstance = Instantiate(prefab, parent.transform);

            // Remplit les deux enfants avec les types et quantités, s'ils existent
            for (int j = 0; j < 2; j++)
            {
                int index = i + j;
                if (index < list.Count)
                {
                    Transform child = horizontalInstance.transform.GetChild(j); // Récupère l'enfant (0 ou 1)
                    TextMeshProUGUI txt = child.GetComponentInChildren<TextMeshProUGUI>(); // Récupère le composant Text

                    if (txt != null)
                    {
                        // Définit le texte avec le nom de l'enum et la quantité
                        txt.text = $"{list[index].Type.ToString()} : {list[index].Quantite}";
                    }
                }
            }
        }
    }
}
