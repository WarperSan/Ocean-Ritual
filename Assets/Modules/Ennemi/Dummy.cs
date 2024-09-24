
using UnityEngine;

public class Dummy : MonoBehaviour, EnnemieGenerique
{
    #region Propriétés
    public float Life { get; set; }
    public float Damage { get; set; }
    public float AttackSpeed { get; set; }
    #endregion
    [SerializeField] float vie;
    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        // Initialisation de l'ennemi
        vie = 2000f;
        Life = 2000f;
        Damage = 10f;
        AttackSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        // Gestion des actions à chaque frame
    }
    #endregion

    #region Interface Methods
    public void Movement()
    {
        // Logique pour le mouvement de l'ennemi
    }

    public void TakeHit(float damage)
    {
        Debug.Log("aille!");
        // Réduction de la vie lors de la prise de dégâts
        Life -= damage;
        vie-= damage;
        if (Life <= 0)
        {
            // L'ennemi meurt
            Destroy(gameObject);
        }
    }

    public void DoHit()
    {
        // Logique pour infliger des dégâts à la cible
    }
    #endregion
}