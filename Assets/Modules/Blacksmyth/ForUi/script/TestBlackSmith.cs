using TMPro;
using UnityEngine;

public class TestBlackSmith : MonoBehaviour
{
    // Instance statique du singleton
    private static TestBlackSmith instance;

    [SerializeField] Gemcomponent Gem;
    [SerializeField] GemData TemporaryGemData;
    [SerializeField] bool show;
    [SerializeField] GemData GemData;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] TextMeshProUGUI MissingCase;
    [SerializeField] TextMeshProUGUI ModifiedCase;
    [SerializeField] public bool canAddNewCase;
   public  bool CasseOnlytrue;
    // Propri�t� pour acc�der � l'instance
    public static TestBlackSmith Instance
    {
        get
        {
            if (instance == null)
            {
                // Cherche l'instance dans la sc�ne
                instance = FindObjectOfType<TestBlackSmith>();

                // Si aucune instance n'existe, cr�er un nouvel objet
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(TestBlackSmith).Name);
                    instance = singletonObject.AddComponent<TestBlackSmith>();
                }
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Assurez-vous que le singleton ne soit pas d�truit lors du chargement d'une nouvelle sc�ne
        DontDestroyOnLoad(gameObject);
    }

    public void ConvertGemme()
    {
        GemData = GemHelper.ConvertGemToGemData(Gem.GemScript);
        TemporaryGemData = GemHelper.ConvertGemToGemData(Gem.GemScript);
    }

    public void SetData(GemData gem)
    {
        GemData = gem;
        TemporaryGemData = new(gem);
    }

    // Update is called once per frame
    void Update()
    {
        if (show)
        {
            show = !show;
            ConvertGemme();
            Show();
        }
    }

    public void Show()
    {
       CreationCase.Instance.CreateUi(TemporaryGemData.Shape.GetForme());
        UpdateUI();
    }
    public void ChangeValueGemme(int x ,int y,bool boolean)
    {
        GemHelper.ModifiedList(ref TemporaryGemData.Shape.flatForme, x,y,boolean, TemporaryGemData.Shape.height);
        UpdateUI();
    }
    // Tableaux pour stocker les textes originaux
    private string[] originalTexts;  // 0 = cost, 1 = MissingCase, 2 = ModifiedCase
    public void UpdateUI()
    {
        Debug.Log("passeUpdateUI");
        // Initialisation des textes originaux seulement si ce n'est pas encore fait
        if (originalTexts == null)
        {
            originalTexts = new string[3]; // Allocation du tableau
                                           // Sauvegarde des textes originaux avant modification
            originalTexts[0] = cost.text;            // Texte original du co�t
            originalTexts[1] = MissingCase.text;     // Texte original des cases manquantes
            originalTexts[2] = ModifiedCase.text;    // Texte original des cases modifi�es
        }

        // Obtenir les informations � partir des listes de formes
        (int totalCost, int modifiedSpot, int missingSpot,bool CasseOnlytrue) = GemHelper.GetInformationAboutForm(
            GemData.Shape.flatForme,
            TemporaryGemData.Shape.flatForme,
            GemData.LVL
        );
        this.CasseOnlytrue = CasseOnlytrue;
        // Mise � jour des textes avec les nouvelles valeurs, tout en sauvegardant les textes originaux
        cost.text = originalTexts[0] + " " + totalCost.ToString();             // Ajoute la valeur originale avec la nouvelle valeur du co�t
        MissingCase.text = originalTexts[1] + " " + missingSpot.ToString();    // Ajoute la valeur originale avec le nombre de cases manquantes
        ModifiedCase.text = originalTexts[2] + " " + modifiedSpot.ToString();  // Ajoute        nale avec le nombre de cases modifi�es

        // Si aucune case n'est manquante, on d�sactive la possibilit� d'ajouter de nouvelles cases
        canAddNewCase = missingSpot > 0;
    }


    public void RestoreOriginalTexts()
    {
        // Restaurer les textes originaux si n�cessaire
        cost.text = originalTexts[0];
        MissingCase.text = originalTexts[1];
        ModifiedCase.text = originalTexts[2];
    }
}
