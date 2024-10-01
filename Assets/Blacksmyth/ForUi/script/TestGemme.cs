using UnityEngine;

public class TestGemme : MonoBehaviour
{
    // Instance statique du singleton
    private static TestGemme instance;

    [SerializeField] Gemcomponent Gem;
    [SerializeField] GemData TemporaryGemData;
    [SerializeField] bool show;
    [SerializeField] GemData GemData;

    // Propriété pour accéder à l'instance
    public static TestGemme Instance
    {
        get
        {
            if (instance == null)
            {
                // Cherche l'instance dans la scène
                instance = FindObjectOfType<TestGemme>();

                // Si aucune instance n'existe, créer un nouvel objet
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(TestGemme).Name);
                    instance = singletonObject.AddComponent<TestGemme>();
                }
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Assurez-vous que le singleton ne soit pas détruit lors du chargement d'une nouvelle scène
        DontDestroyOnLoad(gameObject);
    }

    public void ConvertGemme()
    {
        GemData = GemHelper.ConvertGemToGemData(Gem.GemScript);
        TemporaryGemData = GemHelper.ConvertGemToGemData(Gem.GemScript);
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
    }
}
