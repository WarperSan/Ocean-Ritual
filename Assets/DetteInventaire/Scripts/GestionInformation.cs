
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine.UI;
using UnityEngine;

public class GestionInformation : MonoBehaviour
{
    public static GestionInformation Instance { get; private set; }

    [SerializeField] Image Photo;
    [SerializeField] ShowForm Form;
    [SerializeField] GameObject stat;
    [SerializeField] GameObject informationStat;

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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetNewInformationGem(GemData Gem)
    {
        Form.CreateUi(Gem.Shape.GetForme());
        Photo.sprite = Gem.sprite;
    }
    

    
}
