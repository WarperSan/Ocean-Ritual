using UnityEngine;
using UnityEngine.UI;

public class MouseWheelManager : MonoBehaviour
{
    public LayerMask layerSocle;
    public LayerMask layerVoid;

    [SerializeField]
    private GameObject objetMemory; // Pour garder en m�moire le CG d�tect�

    private int layerInitial = 26;

    [SerializeField]
    private GameObject objectTouch; // Pour garder l'objet CV d�tect�

    private GameObject ObjetGlow;
    private GameObject Socle;
    private bool[,] ShapeBoolMain;

    [SerializeField]
    private string layerInitialName = "Socle";

    [SerializeField]
    private bool GemFromInventory;

    [SerializeField]
    private GameObject SrinkGameObject;

    [SerializeField]
    private float SrinkValue = 0.2f;

    [SerializeField]
    private GemData TestGemData;

    [SerializeField]
    private GemData gemData;

    [SerializeField]
    private AudioClip MusicPlaceGemme;

    [SerializeField]
    private Button ButtonUp;

    [SerializeField]
    private Button ButtonDown;

    [SerializeField]
    private LinkWheelEquipment linkWheelEquipment;

    [SerializeField]
    private GameObject MenuActif;

    [System.Obsolete]
    private void Update()
    {
        if (MenuActif.active)
            MouseWheelControleur();
    }

    public void resetSrinkObject()
    {
        SrinkGameObject.transform.localScale = new Vector3(1, 1, 1);
        SelectionSocle();
        SrinkGameObject.transform.position = Socle.transform.position;
        SrinkGameObject.transform.rotation = Quaternion.identity;
    }

    public void TestSpawnGem(GemData gemdataREceive = null)
    {
        Gem gem;

        if (Socle == null)
            SelectionSocle();
        gem = gemdataREceive == null ? new Gem(TestGemData) : new Gem(gemdataREceive);

        gemData = gemdataREceive ?? TestGemData;
        SrinkGameObject.transform.localScale = new Vector3(1, 1, 1);
        GameObject TheGem = GeneratorGem.CreatGemmeObject(gem, SrinkGameObject.transform);
        ZoneUIHandler.Instance.ReceiveGemSocleTOInventory(TheGem);
        // TheGem.transform.SetParent(SrinkGameObject.transform);
        TheGem.transform.localPosition = new Vector3(0, 0, 0);
        SrinkGameObject.transform.localScale = new Vector3(SrinkValue, SrinkValue, SrinkValue);
        SrinkGameObject.transform.position = Socle.transform.position;
        GiveRefNewGem(TheGem);
    }

    public void GiveRefNewGem(GameObject gameObj)
    {
        GemFromInventory = true;
        Vector3 newPosition = gameObj.transform.localPosition;
        newPosition.y += 2;
        gameObj.transform.localPosition = newPosition;
        objetMemory = gameObj;
    }

    public bool TryPLaceTemporaryGem(int x, int z)
    {
        if (Socle == null)
            SelectionSocle();
        componentPowerGemObject scripSocle = Socle.GetComponent<componentPowerGemObject>();
        Gem gem = scripSocle.PowerGemObjectScript.ReceiveGemData(gemData);
        objetMemory.GetComponent<Gemcomponent>().GemScript = gem;
        return scripSocle.PowerGemObjectScript.TryPlaceTemporaryGem(x, z);
    }

    [SerializeField]
    private GameObject boutonContainer;

    public void EndRotation()
    {
        SrinkGameObject.transform.position = Socle.transform.position;
        SrinkGameObject.transform.rotation = Quaternion.identity;
        ButtonUp.interactable = true;
        ButtonDown.interactable = true;
        ActivateBouton();
    }

    public void DesactivateBouton()
    {
        // R�cup�rer tous les composants Button dans le conteneur
        Button[] buttons = boutonContainer.GetComponentsInChildren<Button>();

        // D�sactiver l'interaction pour chaque bouton
        foreach (Button button in buttons)
            button.interactable = false;
    }

    public void ActivateBouton()
    {
        // R�cup�rer tous les composants Button dans le conteneur
        Button[] buttons = boutonContainer.GetComponentsInChildren<Button>();

        // Activer l'interaction pour chaque bouton
        foreach (Button button in buttons)
            button.interactable = true;
    }

    public void StartRotation()
    {
        DesactivateBouton();
        ButtonUp.interactable = false;
        ButtonDown.interactable = false;
    }

    public void SelectionSocle() => Socle = linkWheelEquipment.GetStand();
    public void GetNextSocle()   => Socle = linkWheelEquipment.GetNextSocle();
    public bool IsAlone()        => linkWheelEquipment.IsAlone();

    public void GetPreviewSocle() => Socle = linkWheelEquipment.GetPreviewSocle();

    public int getNumberforRotation() => linkWheelEquipment.GetSkippedSocle();

    public void MouseWheelControleur()
    {
        if (!GemFromInventory)
            controleurFromSocle();
        else
            controleurFromInventory();
    }

    public void controleurFromSocle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (objetMemory == null)
                DetectAndSelectObject();
            else
                PlaceGemme();
        }

        else if (Input.GetMouseButtonDown(1))
        {
            if (objetMemory != null)
                rotate();
        }
        else
        {
            if (objetMemory != null)
            {
                GameObject temp = DetectCV();

                objectTouch = temp;

                if (objectTouch != null)
                    MoveObjectOnCV(objetMemory.transform, objectTouch.transform);
                else
                    MoveObjectToMouse(objetMemory.transform);
            }
            else
                Glow();
        }
    }

    public void controleurFromInventory()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (objetMemory != null)
                PlaceGemme(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (objetMemory != null)
                PlaceGemme(true);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (objetMemory != null)
                rotate();
        }
        else
        {
            if (objetMemory != null)
            {
                GameObject temp = DetectCV();

                objectTouch = temp;

                if (objectTouch != null)
                    MoveObjectOnCV(objetMemory.transform, objectTouch.transform);
                else
                    MoveObjectToMouse(objetMemory.transform);
            }
            else
                Glow();
        }
    }

    private void Glow()
    {
        GameObject temp = DetectGemmeRC();

        if (temp != null)
        {
            temp = ClimbeUp2Parent(temp.transform).gameObject;

            if (temp != ObjetGlow)
            {
                if (ObjetGlow != null)
                    RemoveGlow();

                ObjetGlow = temp;

                foreach (Transform child in ObjetGlow.transform)
                    child.GetComponent<Outline>().enabled = true;
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

    private void RemoveGlow()
    {
        if (ObjetGlow != null)
        {
            foreach (Transform child in ObjetGlow.transform)
                child.GetComponent<Outline>().enabled = false;
        }
    }

    private void rotate()
    {
        Gemcomponent gem = objetMemory.GetComponent<Gemcomponent>();

        gem.GemScript.form.Rotate(gem.GemScript.form.GetForme(), 90);
        objetMemory.transform.Rotate(Vector3.up, 90f);
    }

    private void PlaceGemme(bool FromInventory = false)
    {
        if (objectTouch != null)
        {
            LocationSocle coordone = objectTouch.GetComponentInParent<LocationSocle>();
            Gemcomponent gem = objetMemory.GetComponent<Gemcomponent>();

            if (!FromInventory)
            {
                GemmeGrid grid = Socle.GetComponent<GemmeGrid>();

                // Debug.Log(BoolArrayToString(ShapeBoolMain));
                if (grid.TryPlaceObjectOnGrid(coordone.x,
                        coordone.z,
                        gem.GemScript.form.GetForme(),
                        gem.GemScript,
                        ShapeBoolMain))
                {
                    MoveObject(objetMemory.transform, true);
                    notSelectObject();
                    SoundManager.Instance.PlaySound(MusicPlaceGemme, SoundType.UI, 5f);
                }
            }
            else
            {
                PowerGemObject scripSocle = Socle.GetComponent<componentPowerGemObject>().PowerGemObjectScript;

                if (TryPLaceTemporaryGem(coordone.x, coordone.z))
                {
                    Socle.GetComponent<componentPowerGemObject>().PowerGemObjectScript.GivePositionRef(gem.GemScript);
                    objetMemory.transform.SetParent(scripSocle.GemContainer.transform);
                    //Debug.Log(gem.GemScript.PositionX);
                    // Debug.Log(gem.GemScript.PositionZ);
                    //  Debug.Log(coordone.z);
                    MoveObject(objetMemory.transform, true);
                    notSelectObject();
                    SoundManager.Instance.PlaySound(MusicPlaceGemme, SoundType.UI, 5f);
                }
            }
        }
    }

    private string BoolArrayToString(bool[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        string result = "";

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
                result += array[i, j] ? "1 " : "0 ";
            result += "\n";
        }

        return result;
    }

    private void DetectAndSelectObject()
    {
        GameObject temp = DetectGemmeRC();

        if (temp != null)
            objectTouch = temp;

        if (objectTouch != null)
        {
            Transform parent = ClimbeUp2Parent(objectTouch.transform);

            // V�rifier si le parent existe
            if (parent != null)
            {
                ZoneUIHandler.Instance.activeRaycast();
                ZoneUIHandler.Instance.ReceiveGemSocleTOInventory(parent.gameObject);
                Gemcomponent scriptGemme = parent.gameObject.GetComponent<Gemcomponent>();

                // V�rifier si l'objet d�tect� est un CubeGemme (CG) ou un CubeVide (CV)
                if (scriptGemme != null) // C'est un CG
                {
                    // Si un CG est d�j� en m�moire, on ne fait rien (ne pas interagir avec un autre CG)
                    if (objetMemory != null)
                    {
                        Debug.Log("Un CubeGemme est d�j� s�lectionn�, rien � faire.");
                        return; // Ne fait rien si un CG est d�j� en m�moire
                    }

                    // Si aucun CG n'est en m�moire, on m�morise le CG

                    MoveObject(parent);
                    objetMemory = parent.gameObject; // On garde le CG en m�moire

                    layerInitial = objetMemory.layer; // Stocker le layer initial
                    Transform SocleParent = ClimbeUpParent(objetMemory.transform);

                    SelectionSocle();
                    SoundManager.Instance.PlaySound(MusicPlaceGemme, SoundType.UI, 5f);
                    LayerChange(objetMemory, 0); // Changer temporairement le layer

                    if (ShapeBoolMain == null)
                        ShapeBoolMain = scriptGemme.GemScript.form.GetForme();
                }
                else // C'est un CV
                {
                    Debug.Log("CubeVide d�tect�.");

                    if (objetMemory != null) // Si on a d�j� d�tect� un CG avant
                    {
                        // M�moriser le CV d�tect�
                        // this.objectTouch = objectTouch;
                    }
                }
            }
        }
    }

    private GameObject DetectCV()
    {
        // V�rifie si la souris est dans l'�cran
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x < 0 || mousePos.y < 0 || mousePos.x > Screen.width || mousePos.y > Screen.height)
            return null;

        // Raycast seulement si la souris est dans l'�cran
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray,
                out hit,
                9,
                layerVoid))
        {
            // Debug.Log("Objet d�tect� : " + hit.collider.gameObject.name);
            return hit.collider.gameObject;
        }

        return null;
    }

    private void MoveObject(Transform parent, bool inverse = false)
    {
        // D�placer l'objet de 2 unit�s sur l'axe Y en coordonn�es locales
        if (!inverse)
            parent.localPosition += new Vector3(0, 2, 0);
        else
            parent.localPosition += new Vector3(0, -2, 0);
    }

    private GameObject DetectGemmeRC()
    {
        // V�rifie si le jeu est en cours d'ex�cution et si la fen�tre du jeu est focalis�e
        if (!Application.isPlaying || !Application.isFocused)
        {
            // Ne fait rien si le jeu n'est pas en mode "Play" ou si la fen�tre n'a pas le focus
            return null;
        }

        // Si le jeu est en cours d'ex�cution et la fen�tre est focalis�e, ex�cute le raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray,
                out hit,
                9,
                layerSocle))
        {
            // Debug.Log("Objet d�tect� : " + hit.collider.gameObject.name);
            return hit.collider.gameObject;
        }

        return null;
    }

    private Transform ClimbeUp2Parent(Transform objet)
    {
        Transform parent = objet.parent;

        if (parent != null)
            return parent.parent; // Remonte directement au deuxi�me parent

        return null;
    }

    private Transform ClimbeUpParent(Transform objet)
    {
        Transform parent = objet.parent;

        if (parent != null)
        {
            parent = parent.parent; // Remonte au deuxi�me parent

            if (parent != null)
                return parent.parent; // Remonte au 3em parent
        }

        return null;
    }

    private void MoveObjectToMouse(Transform objetCG)
    {
        // R�cup�rer la position de la souris
        Vector3 mousePosition = Input.mousePosition;

        // Cr�er un rayon depuis la cam�ra en direction de la position de la souris
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // Plan horizontal � la hauteur de l'objet (par exemple, au niveau de l'objet ou sol)
        float height = objetCG.position.y;
        var horizontalPlane = new Plane(Vector3.up, new Vector3(0, height, 0));

        // Calculer le point d'intersection entre le rayon et le plan
        if (horizontalPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            targetPosition.y = height; // Fixer la position Y pour �viter tout changement de hauteur

            // D�placer l'objet vers la position cible calcul�e
            objetCG.position = targetPosition;
        }
    }

    private void MoveObjectOnCV(Transform objetCG, Transform objetCV, bool nouvelGem = false)
    {
        Vector3 nouvellePositionLocale;
        Transform VoidParent = objetCV.parent;

        nouvellePositionLocale = new Vector3(
            VoidParent.localPosition.x,
            objetCG.localPosition.y,
            VoidParent.localPosition.z
        );
        // Calculer la nouvelle position en coordonn�es locales du CV

        // D�placer l'objet CG en coordonn�es locales
        objetCG.localPosition = nouvellePositionLocale;
    }

    private void LayerChange(GameObject objet, int nouveauLayer)
    {
        // Changer le layer de l'objet lui-m�me
        objet.layer = nouveauLayer;

        // Changer le layer de tous ses enfants r�cursivement
        foreach (Transform child in objet.transform)
            LayerChange(child.gameObject, nouveauLayer);
    }

    private void ResetInitialLayer(GameObject objet)
    {
        objet.layer = LayerMask.NameToLayer(layerInitialName);

        foreach (Transform child in objet.transform)
            ResetInitialLayer(child.gameObject);
    }

    public void notSelectObject(Gem gem = null, bool RemoveFromZone = true)
    {
        if (gem != null)
        {
            if (RemoveFromZone)
                ZoneUIHandler.Instance.RemoveGemSocleTOInventory();

            PowerGemObject scripSocle = Socle.GetComponent<componentPowerGemObject>().PowerGemObjectScript;
            //Debug.Log(scripSocle.GemmeList.Count);
            //Debug.Log(scripSocle.GemmeList[^1].form.GetForme());
            scripSocle.DeletedGem(gem);
        }

        if (objetMemory != null)
        {
            if (RemoveFromZone)
                ZoneUIHandler.Instance.RemoveGemSocleTOInventory();
            GemFromInventory = false;
            ResetInitialLayer(objetMemory); // R�tablir le layer initial
            objetMemory = null;             // R�initialiser l'objet en m�moire
            objectTouch = null;             // R�initialiser l'objet CV en m�moire
            ShapeBoolMain = null;
            ZoneUIHandler.Instance.ResetGemme();
        }
    }
}