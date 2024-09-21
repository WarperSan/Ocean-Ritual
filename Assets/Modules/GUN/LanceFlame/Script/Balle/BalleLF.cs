using EntityModule;
using UnityEngine;

public class BalleLF : MonoBehaviour, BalleGenerique
{
    #region Propri�t�s

    // Utilise SerializeField pour les valeurs que tu veux d�finir dans l'�diteur
    [SerializeField] public float vitesse = 10f;
    [SerializeField] private bool disparaitApresHit = false;
    [SerializeField] private bool grossissement = false;
    [SerializeField] public float valeurGrossissement = 1.5f;
    [SerializeField] private bool directionForward = true;
    [SerializeField] private bool rotation = true;
    [SerializeField] private float rotationTodo = 90f;
    [SerializeField] private bool disparaitHitObstacle = false;
    [SerializeField] private bool coupCritique = true;
    [SerializeField] private bool effetSpecial = false;
    [SerializeField] private float range = 50;


    // Propri�t�s publiques pour acc�der aux valeurs

    public float Vitesse { get => vitesse; set => vitesse = value; }
    public bool DisparaitApresHit { get => disparaitApresHit; set => disparaitApresHit = value; }
    public bool Grossissement { get => grossissement; set => grossissement = value; }
    public float ValeurGrossissement { get => valeurGrossissement; set => valeurGrossissement = value; }
    public bool DirectionForward { get => directionForward; set => directionForward = value; }
    public bool Rotation { get => rotation; set => rotation = value; }
    public float RotationTodo { get => rotationTodo; set => rotationTodo = value; }
    public bool DisparaitHitObstacle { get => disparaitHitObstacle; set => disparaitHitObstacle = value; }
    public bool CoupCritique { get => coupCritique; set => coupCritique = value; }
    public bool EffetSpecial { get => effetSpecial; set => effetSpecial = value; }
    public float Range { get => range; set => range = value; }


    float distanceTravel = 0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Initialisation ou logique de d�marrage ici
    }

    // Update is called once per frame
    void Update()
    {
        Deplacement();
        Grossisement();

    }

    #region Interface Methods

    public LanceFlameControleur lanceFlameScript;
    public void EnnemiHit(Collider other)
    {
        if (!other.TryGetComponent(out Entity entity))
            return;

        entity.UseAttack(new Attack()
        {
            Damage = lanceFlameScript.GetDamage(),
            Type = AttackType.FIRE
        });

        // D�sactiver la balle
        if (DisparaitApresHit)
            gameObject.SetActive(false);
    }

    public void Grossisement()
    {
        // Si l'option Grossissement est activ�e
        if (Grossissement)
        {


            // R�cup�rer le premier enfant
            Transform enfant = transform.GetChild(0);

            // Grossit l'enfant en utilisant la valeur de ValeurGrossissement
            float scaleIncrement = ValeurGrossissement * Vitesse * Time.deltaTime;
            enfant.localScale += new Vector3(0, scaleIncrement, scaleIncrement);


        }
    }
    public void ResetGrossisement()
    {
        // Si l'option Grossissement est activ�e
        if (Grossissement)
        {


            // R�cup�rer le premier enfant
            Transform enfant = transform.GetChild(0);


            enfant.localScale = new Vector3(0.1f, 1, 1);


        }
    }

    public void Deplacement()
    {
        // Calculer la distance que la balle a parcourue depuis la derni�re frame
        float distanceThisFrame = Vitesse * Time.deltaTime;

        // Ajouter cette distance � la distance totale parcourue
        distanceTravel += distanceThisFrame;

        // Si la distance parcourue d�passe la port�e, d�sactiver la balle
        if (distanceTravel >= range)
        {
            distanceTravel = 0;
            gameObject.SetActive(false);

        }
        else
        {
            // D�placer la balle vers l'avant
            transform.Translate(Vector3.forward * distanceThisFrame);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        // V�rifie si l'objet touch� appartient au layer Ennemi
        if (other.gameObject.layer == LayerMask.NameToLayer("Ennemi"))
        {
            // Appelle la logique d'impact avec l'ennemi
            EnnemiHit(other);

            // Si la balle doit dispara�tre apr�s avoir touch� un ennemi
            if (DisparaitApresHit)
            {
                gameObject.SetActive(false); // D�sactiver la balle
            }
        }
    }

    #endregion
}
