using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using FishingModule;

public class GestionInformation : MonoBehaviour
{
    public static GestionInformation Instance { get; private set; }
    public string path = "Inventory";
    [SerializeField] Image PhotoGem;
    [SerializeField] GameObject Gem;
    [SerializeField] GameObject Fish;
    [SerializeField] Image PhotoFish;
    [SerializeField] TextMeshProUGUI FishName;
    [SerializeField] TextMeshProUGUI FishRarety;
    [SerializeField] TextMeshProUGUI FishDescription;
    [SerializeField] TextMeshProUGUI lvl;
    [SerializeField] ShowForm Form;
    [SerializeField] GameObject Gun;
    [SerializeField] GameObject Boat;
    [SerializeField] GameObject Net;
    Dictionary<string, GameObject> data;
    private RectTransform rectTransform;
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
        rectTransform = GetComponent<RectTransform>();
    }

    [SerializeField] float offsetHeight = 70;
    [SerializeField] float offsetWidth = 150;
    [SerializeField] float offsetHeightInverse = 300;
    [SerializeField] float offsetWidthInverse = 720;
    [SerializeField] float offsetHeightInverseFish = 300;
    [SerializeField] float offsetWidthInverseFish = 200;
    [SerializeField] float MousseLimitHeight = 250;
    [SerializeField] float MousseLimiteWidth = 500;



    public void InterfaceMovement(ItemData Item)
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        // Récupère la position actuelle de la souris dans l'écran (en pixels)
        Vector3 mousePosition = Input.mousePosition;

        // Applique les décalages en hauteur et en largeur
        mousePosition.x += offsetWidth;
        mousePosition.y += offsetHeight;

        // Vérifie si la position de la souris avec les offsets dépasse les limites définies
        if (mousePosition.x + MousseLimiteWidth > Screen.width )
        {
            if((Item is not FishData))
            {
                // Si ça dépasse, applique l'offset inverse horizontalement
                mousePosition.x -= offsetWidthInverse;
              
            }
            else
            {
                mousePosition.x -= offsetHeightInverseFish;
            }
         
        }

        if (mousePosition.y + MousseLimitHeight > Screen.height )
        {
            if ((Item is not FishData))
            {
                // Si ça dépasse, applique l'offset inverse verticalement
                mousePosition.y -= offsetHeightInverse;
              

            }
            else
            {
                mousePosition.y -= offsetHeightInverseFish;
            }
                
        }

        // Met à jour la position de l'objet UI pour suivre la position de la souris
        rectTransform.position = mousePosition;
    }

    public void closeRessource()
    {
        Gem.SetActive(false);
        Fish.SetActive(false);
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
    public void GetNewInformationFish(FishData Fish)
    {
        Gem.SetActive(false);
        this.Fish.SetActive(true);
        if (data == null)
        {
            GetResource();
        }
        FishName.text = Fish.name;
        FishRarety.text = Fish.rarety.ToString();
        FishDescription.text = Fish.description;
        PhotoFish.sprite = Fish.sprite;

    }
    public void GetNewInformationGem(GemData Gem)
    {
        this.Fish.SetActive(false);
        this.Gem.SetActive(true);

        if (data == null)
        {
            GetResource();
        }

        Form.CreateUi(Gem.Shape.GetForme());
        PhotoGem.sprite = Gem.sprite;
        ChangeInfoStat(Gem);
        lvl.text = $"Level of the gem :  {Gem.LVL}";

    }

    // Affiche les informations de la gemme (stats) sur le UI
    public void ChangeInfoStat(GemData Gem)
    {
        // Ajouter les stats pour les armes
        if (Gem.typeWeapon != null && Gem.typeWeapon.Count > 0)
        {
            PopulateStatUI(Gem.typeWeapon, Gun, Gem.LVL);
        }

        // Ajouter les stats pour les bateaux
        if (Gem.typeBoat != null && Gem.typeBoat.Count > 0)
        {
            PopulateStatUI(Gem.typeBoat, Boat, Gem.LVL);
        }

        // Ajouter les stats pour les filets
        if (Gem.typeNet != null && Gem.typeNet.Count > 0)
        {
            PopulateStatUI(Gem.typeNet, Net, Gem.LVL);
        }
    }

    // Méthode générique pour remplir les UI en fonction des types (arme, bateau, filet)
    private void PopulateStatUI<TEnum>(List<TypeQuantity<TEnum>> list, GameObject parent, int lvl)
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

                        // Applique la couleur du texte basée sur le ratio
                        txt.color = GetTextColorForRatio((float)list[index].Quantite / lvl);
                    }
                }
            }
        }
    }

    // Sous-fonction pour récupérer la couleur en fonction du ratio
    private Color GetTextColorForRatio(float ratio)
    {
        if (ratio == 1)
        {
            return Color.white; // Blanc pour ratio = 1
        }
        else if (ratio == 2)
        {
            return Color.green; // Vert pour ratio = 2
        }
        else if (ratio == 3)
        {
            return new Color(0.7f, 0.4f, 0.7f); // Violet clair pour ratio = 3
        }
        else if (ratio == 4)
        {
            return new Color(1f, 0.647f, 0f); // Orange pour ratio = 4
        }
        else
        {
            return Color.white; // Par défaut, la couleur est blanche
        }
    }

}
