using UnityEngine;

public class MouseWheelManager : MonoBehaviour
{
    public LayerMask layerSocle;
    public LayerMask layerVoid;
    private GameObject objetMemory; // Pour garder en mémoire le CG détecté
    private int layerInitial = 26;
    private GameObject objectTouch; // Pour garder l'objet CV détecté
    private GameObject ObjetGlow = null;
    private GameObject Socle;
    private bool[,] ShapeBoolMain;
    [SerializeField] string layerInitialName = "Socle";
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (objetMemory == null)
            {
                DetectAndSelectObject();
            }
            else
            {
                PlaceGemme();
            }


        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (objetMemory != null)
            {
                rotate();
            }
        }
        else
        {
            if (objetMemory != null)
            {
                GameObject temp = DetectCV();
                if (temp != null)
                {


                    objectTouch = temp;

                }

                if (objectTouch != null)
                {
                    MoveObjectOnCV(objetMemory.transform, objectTouch.transform);
                }
            }
            else
            {
                
                Glow();
            }



        }


    }
    void Glow()
    {
        GameObject temp = DetectGemmeRC();
        if (temp != null)
        {
         
            temp = ClimbeUp2Parent(temp.transform).gameObject;

            if (temp != ObjetGlow )
            {
                if (ObjetGlow != null)
                {
                    RemoveGlow();
                }

                ObjetGlow = temp;
                foreach (Transform child in ObjetGlow.transform)
                {
                    child.GetComponent<Outline>().enabled = true;
                }
            }
          
               


        }
        else
        {
            if (ObjetGlow != null)
            {
                RemoveGlow();
                ObjetGlow = temp;
            }
        }
       
        
    }

    void RemoveGlow()
    {
        if (ObjetGlow != null)
        {
            foreach (Transform child in ObjetGlow.transform)
            {
                child.GetComponent<Outline>().enabled = false;
            }
        }
    }
    void rotate()
    {
        Gemcomponent gem = objetMemory.GetComponent<Gemcomponent>();


        gem.GemScript.form.Rotate(gem.GemScript.form.GetForme(), 90);
        objetMemory.transform.Rotate(Vector3.up, 90f);
    }
    void PlaceGemme()
    {
        Gemcomponent gem = objetMemory.GetComponent<Gemcomponent>();

        GemmeGrid grid = Socle.GetComponent<GemmeGrid>();

        LocationSocle coordone = objectTouch.GetComponentInParent<LocationSocle>();
        if (grid.TryPlaceObjectOnGrid(coordone.x, coordone.z, gem.GemScript.form.GetForme(), gem.GemScript, ShapeBoolMain))
        {
            MoveObject(objetMemory.transform, true);
            notSelectObject();
        }


    }

    void DetectAndSelectObject()
    {
        GameObject temp = DetectGemmeRC();
        if (temp != null)
        {
            objectTouch = temp;
        }


        if (objectTouch != null)
        {
            Transform parent = ClimbeUp2Parent(objectTouch.transform);

            // Vérifier si le parent existe
            if (parent != null)
            {
            
                Gemcomponent scriptGemme = parent.gameObject.GetComponent<Gemcomponent>();
                // Vérifier si l'objet détecté est un CubeGemme (CG) ou un CubeVide (CV)
                if (scriptGemme != null) // C'est un CG
                {
                    // Si un CG est déjà en mémoire, on ne fait rien (ne pas interagir avec un autre CG)
                    if (objetMemory != null)
                    {
                        Debug.Log("Un CubeGemme est déjà sélectionné, rien à faire.");
                        return; // Ne fait rien si un CG est déjà en mémoire
                    }

                    // Si aucun CG n'est en mémoire, on mémorise le CG
                  
                    MoveObject(parent);
                    objetMemory = parent.gameObject; // On garde le CG en mémoire

                    layerInitial = objetMemory.layer; // Stocker le layer initial
                    Transform SocleParent = ClimbeUpParent(objetMemory.transform);
                    Socle = SocleParent.gameObject;
                    LayerChange(objetMemory, 0); // Changer temporairement le layer
                    if (ShapeBoolMain == null)
                    {
                        ShapeBoolMain = scriptGemme.GemScript.form.GetForme();
                    }
                }
                else // C'est un CV
                {
                    Debug.Log("CubeVide détecté.");

                    if (objetMemory != null) // Si on a déjà détecté un CG avant
                    {
                        // Mémoriser le CV détecté
                       // this.objectTouch = objectTouch;
                    }
                }
            }
        }
    }
    GameObject DetectCV()
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

    void MoveObject(Transform parent, bool inverse = false)
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

    GameObject DetectGemmeRC()
    {
        // Vérifie si le jeu est en cours d'exécution et si la fenêtre du jeu est focalisée
        if (!Application.isPlaying || !Application.isFocused)
        {
            // Ne fait rien si le jeu n'est pas en mode "Play" ou si la fenêtre n'a pas le focus
            return null;
        }

        // Si le jeu est en cours d'exécution et la fenêtre est focalisée, exécute le raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 9, layerSocle))
        {
            // Debug.Log("Objet détecté : " + hit.collider.gameObject.name);
            return hit.collider.gameObject;
        }

        return null;
    }

    Transform ClimbeUp2Parent(Transform objet)
    {
        Transform parent = objet.parent;

        if (parent != null)
        {
            return parent.parent; // Remonte directement au deuxième parent
        }

        return null;
    }
    Transform ClimbeUpParent(Transform objet)
    {
        Transform parent = objet.parent;

        if (parent != null)
        {
            parent = parent.parent; // Remonte au deuxième parent

            if (parent != null)
            {

               
                return parent.parent; // Remonte au 3em parent

            }
        }

        return null;
    }

    void MoveObjectOnCV(Transform objetCG, Transform objetCV)
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

    void LayerChange(GameObject objet, int nouveauLayer)
    {
        // Changer le layer de l'objet lui-même
        objet.layer = nouveauLayer;

        // Changer le layer de tous ses enfants récursivement
        foreach (Transform child in objet.transform)
        {
            LayerChange(child.gameObject, nouveauLayer);
        }
    }

    void ResetInitialLayer(GameObject objet)
    {

        objet.layer = LayerMask.NameToLayer(layerInitialName);

        foreach (Transform child in objet.transform)
        {
            ResetInitialLayer(child.gameObject);
        }
    }

    void notSelectObject()
    {
        if (objetMemory != null)
        {
            ResetInitialLayer(objetMemory); // Rétablir le layer initial
            objetMemory = null; // Réinitialiser l'objet en mémoire
            objectTouch = null;  // Réinitialiser l'objet CV en mémoire
            ShapeBoolMain = null;
        }
    }
}
