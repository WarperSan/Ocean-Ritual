using UnityEngine;

public class LunchTest : MonoBehaviour
{
    [SerializeField]
    private float animationVitesse;

    [SerializeField]
    private float animationTime = 5;

    [SerializeField]
    private Transform lunch;

    private bool returning;
    private float timeLapse;
    private Transform aa;

    private Vector3 initialPosition;

    // Start is called before the first frame update
    private void Start()
    {
        aa = transform;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        timeLapse += Time.deltaTime;
        DoLunchAnimation();
    }

    public void DoLunchAnimation()
    {
        if (!returning)
        {
            // Avance en ligne droite
            float moveDistance = animationVitesse * Time.deltaTime;
            aa.position += aa.forward * moveDistance;

            // V�rifie si l'objet a atteint la fin de l'animation
            if (timeLapse >= animationTime)
            {
                returning = true; // Commence la phase de retour
                timeLapse = 0f;   // R�initialise le timer pour le retour
            }
        }
        else
        {
            // Retour avec effet de rebond
            float returnDistance = animationVitesse * Time.deltaTime;

            // On calcule la position cible
            Vector3 directionBack = initialPosition - aa.position;

            if (directionBack.magnitude > returnDistance)
                aa.position += directionBack.normalized * returnDistance;
            else
            {
                aa.position = initialPosition; // Remet � la position initiale
                returning = false;             // Termin�
            }
        }
    }

    // Ceci sera appel� dans l'�diteur Unity pour visualiser le raycast.
    private void OnDrawGizmos()
    {
        if (lunch != null)
        {
            // Couleur du rayon
            Gizmos.color = Color.red;

            // Dessine une ligne (rayon) depuis la position de lunch dans la direction de lunch.forward
            Gizmos.DrawRay(lunch.position, lunch.forward * 5f); // Le 5f repr�sente la longueur du rayon

            // Optionnel : Dessine une sph�re � la fin du rayon pour mieux voir la direction
            Gizmos.DrawSphere(lunch.position + lunch.forward * 5f, 0.1f);
        }
    }
}