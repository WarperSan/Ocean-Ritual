using UnityEngine;

public class BalleLF : MonoBehaviour, BalleGenerique
{
    #region Propriétés

    // Utilise SerializeField pour les valeurs que tu veux définir dans l'éditeur
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


    // Propriétés publiques pour accéder aux valeurs

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
        // Initialisation ou logique de démarrage ici
    }

    // Update is called once per frame
    void Update()
    {
        Deplacement();
        Grossisement();
        
    }

    #region Interface Methods

   public  LanceFlameControleur lanceFlameScript;
    public void EnnemiHit(Collider other)
    {
      
        
        
        
            // Vérifie si l'objet touché a un script implémentant l'interface EnnemieGenerique
            EnnemieGenerique ennemi = other.GetComponent<EnnemieGenerique>();
            if (ennemi != null && lanceFlameScript != null)
            {
                // Applique les dégâts de LanceFlame
                float damage = lanceFlameScript.GetDamage(); // Ou lanceFlameScript.AttackWithBoost.Quantite
                ennemi.TakeHit(damage);
                if (DisparaitApresHit)
                {
                    gameObject.SetActive(false); // Désactiver la balle
                }

            }
        }
    
    public void Grossisement()
    {
        // Si l'option Grossissement est activée
        if (Grossissement)
        {
           
           
                // Récupérer le premier enfant
                Transform enfant = transform.GetChild(0);

                // Grossit l'enfant en utilisant la valeur de ValeurGrossissement
                float scaleIncrement = ValeurGrossissement * Vitesse * Time.deltaTime;
                enfant.localScale += new Vector3(0, scaleIncrement, scaleIncrement);
            
            
        }
    }
    public void ResetGrossisement()
    {
        // Si l'option Grossissement est activée
        if (Grossissement)
        {


            // Récupérer le premier enfant
            Transform enfant = transform.GetChild(0);

          
            enfant.localScale = new Vector3(0.1f, 1, 1);


        }
    }

    public void Deplacement()
    {
        // Calculer la distance que la balle a parcourue depuis la dernière frame
        float distanceThisFrame = Vitesse * Time.deltaTime;

        // Ajouter cette distance à la distance totale parcourue
        distanceTravel += distanceThisFrame;

        // Si la distance parcourue dépasse la portée, désactiver la balle
        if (distanceTravel >= range)
        {
            distanceTravel = 0;
            gameObject.SetActive(false);
            
        }
        else
        {
            // Déplacer la balle vers l'avant
            transform.Translate(Vector3.forward * distanceThisFrame);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet touché appartient au layer Ennemi
        if (other.gameObject.layer == LayerMask.NameToLayer("Ennemi"))
        {
            // Appelle la logique d'impact avec l'ennemi
            EnnemiHit(other);

            // Si la balle doit disparaître après avoir touché un ennemi
            if (DisparaitApresHit)
            {
                gameObject.SetActive(false); // Désactiver la balle
            }
        }
    }
 
    #endregion
}
