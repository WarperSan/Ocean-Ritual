using UnityEngine;
using UnityEngine.EventSystems;

public class ForgeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Fill fillScript;

    [SerializeField]
    private float amount;

    [SerializeField]
    private float timeBetweenFill = 1f; // Temps entre chaque ajout de remplissage

    private float deltaTime;  // Le temps �coul� depuis le dernier remplissage
    public bool isButtonHeld; // Indique si le bouton est maintenu enfonc�

    private void Update()
    {
        if (isButtonHeld)
        {
            deltaTime += Time.deltaTime;

            // V�rifie si le temps �coul� d�passe le temps requis pour ajouter du remplissage
            if (deltaTime >= timeBetweenFill)
            {
                AddFill();      // Appelle la m�thode pour ajouter du remplissage
                deltaTime = 0f; // R�initialise le compteur de temps
            }
        }
    }

    // M�thode appel�e lorsque le bouton est maintenu enfonc�
    public void OnPointerDown(PointerEventData eventData) => Press();

    // M�thode appel�e lorsque le bouton est rel�ch�
    public  void OnPointerUp(PointerEventData eventData) => Release();
    private void OnDisable()                             => Release();

    // Ajoute du remplissage � l'objet li�
    public void AddFill() => fillScript.AddingFill(amount);

    private void Press() => isButtonHeld = true;

    private void Release()
    {
        isButtonHeld = false;
        deltaTime = 0f; // R�initialise le temps quand le bouton est rel�ch�
    }
}