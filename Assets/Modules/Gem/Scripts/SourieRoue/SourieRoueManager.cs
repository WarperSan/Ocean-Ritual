using UnityEngine;

public class SourieRoueManager : MonoBehaviour
{
    public LayerMask layerSocle;
    public LayerMask layerVoid;
    private GameObject objetMemoire; // Pour garder en mémoire le CG détecté
    private int layerInitial = 26;
    private GameObject objetTouche; // Pour garder l'objet CV détecté
    private GameObject ObjetSurbrillance = null;
    private GameObject Socle;
    private bool[,] formBoolPrincipal;
    [SerializeField] string layerInitialName = "Socle";
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (objetMemoire == null)
            {
                DetecterEtSelectionnerObjet();
            }
            else
            {
                PosseGemme();
            }


        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (objetMemoire != null)
            {
                rotate();
            }
        }
        else
        {
            if (objetMemoire != null)
            {
                GameObject temp = DeteterCV();
                if (temp != null)
                {


                    objetTouche = temp;

                }

                if (objetTouche != null)
                {
                    DeplacerObjetSurCV(objetMemoire.transform, objetTouche.transform);
                }
            }
            else
            {
                
                surbrillance();
            }



        }


    }
    void surbrillance()
    {
        GameObject temp = DetecterGemmeAvecRaycast();
        if (temp != null)
        {
         
            temp = RemonterDeDeuxParents(temp.transform).gameObject;

            if (temp != ObjetSurbrillance )
            {
                if (ObjetSurbrillance != null)
                {
                    enleverSurbrillance();
                }

                ObjetSurbrillance = temp;
                foreach (Transform child in ObjetSurbrillance.transform)
                {
                    child.GetComponent<Outline>().enabled = true;
                }
            }
          
               


        }
        else
        {
            if (ObjetSurbrillance != null)
            {
                enleverSurbrillance();
                ObjetSurbrillance = temp;
            }
        }
       
        
    }

    void enleverSurbrillance()
    {
        if (ObjetSurbrillance != null)
        {
            foreach (Transform child in ObjetSurbrillance.transform)
            {
                child.GetComponent<Outline>().enabled = false;
            }
        }
    }
    void rotate()
    {
        GemmeComponant gemme = objetMemoire.GetComponent<GemmeComponant>();


        gemme.GemmeScript.forme.Rotate(gemme.GemmeScript.forme.GetForme(), 90);
        objetMemoire.transform.Rotate(Vector3.up, 90f);
    }
    void PosseGemme()
    {
        GemmeComponant gemme = objetMemoire.GetComponent<GemmeComponant>();

        GemmeGrid grid = Socle.GetComponent<GemmeGrid>();

        EmplacementSocle coordone = objetTouche.GetComponentInParent<EmplacementSocle>();
        if (grid.TryPlaceObjectOnGrid(coordone.x, coordone.z, gemme.GemmeScript.forme.GetForme(), gemme.GemmeScript, formBoolPrincipal))
        {
            DeplacerObjet(objetMemoire.transform, true);
            DeselectionnerObjet();
        }


    }

    void DetecterEtSelectionnerObjet()
    {
        GameObject temp = DetecterGemmeAvecRaycast();
        if (temp != null)
        {
            objetTouche = temp;
        }


        if (objetTouche != null)
        {
            Transform parent = RemonterDeDeuxParents(objetTouche.transform);

            // Vérifier si le parent existe
            if (parent != null)
            {
                Debug.Log(parent.gameObject);
                GemmeComponant scriptGemme = parent.gameObject.GetComponent<GemmeComponant>();
                // Vérifier si l'objet détecté est un CubeGemme (CG) ou un CubeVide (CV)
                if (scriptGemme != null) // C'est un CG
                {
                    // Si un CG est déjà en mémoire, on ne fait rien (ne pas interagir avec un autre CG)
                    if (objetMemoire != null)
                    {
                        Debug.Log("Un CubeGemme est déjà sélectionné, rien à faire.");
                        return; // Ne fait rien si un CG est déjà en mémoire
                    }

                    // Si aucun CG n'est en mémoire, on mémorise le CG
                    Debug.Log("CubeGemme détecté.");
                    DeplacerObjet(parent);
                    objetMemoire = parent.gameObject; // On garde le CG en mémoire

                    layerInitial = objetMemoire.layer; // Stocker le layer initial
                    Transform SocleParent = RemonterjusquaSocleParents(objetMemoire.transform);
                    Socle = SocleParent.gameObject;
                    ChangerLayer(objetMemoire, 0); // Changer temporairement le layer
                    if (formBoolPrincipal == null)
                    {
                        formBoolPrincipal = scriptGemme.GemmeScript.forme.GetForme();
                    }
                }
                else // C'est un CV
                {
                    Debug.Log("CubeVide détecté.");

                    if (objetMemoire != null) // Si on a déjà détecté un CG avant
                    {
                        // Mémoriser le CV détecté
                        this.objetTouche = objetTouche;
                    }
                }
            }
        }
    }
    GameObject DeteterCV()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 9, layerVoid))
        {
            //  Debug.Log("Objet détecté : " + hit.collider.gameObject.name);
            return hit.collider.gameObject;
        }

        return null;
    }

    void DeplacerObjet(Transform parent, bool inverse = false)
    {
        // Déplacer l'objet de 2 unités sur l'axe Y en coordonnées locales
        if (!inverse)
        {
            parent.localPosition += new Vector3(0, 2, 0);
        }
        else
        {
            parent.localPosition += new Vector3(0, -2, 0);
        }

    }

    GameObject DetecterGemmeAvecRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 9, layerSocle))
        {
            //  Debug.Log("Objet détecté : " + hit.collider.gameObject.name);
            return hit.collider.gameObject;
        }

        return null;
    }

    Transform RemonterDeDeuxParents(Transform objet)
    {
        Transform parent = objet.parent;

        if (parent != null)
        {
            return parent.parent; // Remonte directement au deuxième parent
        }

        return null;
    }
    Transform RemonterjusquaSocleParents(Transform objet)
    {
        Transform parent = objet.parent;

        if (parent != null)
        {
            parent = parent.parent; // Remonte au deuxième parent

            if (parent != null)
            {

                Debug.Log(parent.parent.gameObject.name);
                return parent.parent; // Remonte au 3em parent

            }
        }

        return null;
    }

    void DeplacerObjetSurCV(Transform objetCG, Transform objetCV)
    {
        Transform VoidParent = objetCV.parent;
        // Calculer la nouvelle position en coordonnées locales du CV
        Vector3 nouvellePositionLocale = new Vector3(
            VoidParent.localPosition.x,
            objetCG.localPosition.y,
            VoidParent.localPosition.z
        );

        // Déplacer l'objet CG en coordonnées locales
        objetCG.localPosition = nouvellePositionLocale;
    }

    void ChangerLayer(GameObject objet, int nouveauLayer)
    {
        // Changer le layer de l'objet lui-même
        objet.layer = nouveauLayer;

        // Changer le layer de tous ses enfants récursivement
        foreach (Transform child in objet.transform)
        {
            ChangerLayer(child.gameObject, nouveauLayer);
        }
    }

    void RestaurerLayerInitial(GameObject objet)
    {

        objet.layer = LayerMask.NameToLayer(layerInitialName);

        foreach (Transform child in objet.transform)
        {
            RestaurerLayerInitial(child.gameObject);
        }
    }

    void DeselectionnerObjet()
    {
        if (objetMemoire != null)
        {
            RestaurerLayerInitial(objetMemoire); // Rétablir le layer initial
            objetMemoire = null; // Réinitialiser l'objet en mémoire
            objetTouche = null;  // Réinitialiser l'objet CV en mémoire
            formBoolPrincipal = null;
        }
    }
}
