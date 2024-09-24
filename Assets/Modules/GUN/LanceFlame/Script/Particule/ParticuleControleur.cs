using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticuleControleur : MonoBehaviour
{
    [SerializeField] GameObject feuPrefab;  // Pr�fabriqu� pour le feu
    [SerializeField] GameObject glacePrefab; // Pr�fabriqu� pour la glace
    [SerializeField] GameObject FeuConteneur;  // Conteneur pour les effets de feu
    [SerializeField] GameObject GlaceConteneur; // Conteneur pour les effets de glace
    [SerializeField] LanceFlameControleur lanceFlameScript; // Conteneur lanceFlame
    [SerializeField] ParticleSystem OverheatParticule; // Conteneur overheatParticule
    private ParticleSystem feuInstance;
    private ParticleSystem glaceInstance;

    // M�thodes d'activation des particules
    public void ActiverFeu()
    {
        // D�sactive la glace si elle est active
        if (glaceInstance != null)
        {
            glaceInstance.Stop();
            glaceInstance = null;
        }

        // Instancie une nouvelle instance de feu si n�cessaire
        if (feuInstance == null)
        {
            GameObject feuObj = Instantiate(feuPrefab, FeuConteneur.transform.position, FeuConteneur.transform.rotation);
            feuObj.transform.parent = FeuConteneur.transform; // Place le feu sous l'objet contr�leur
            feuInstance = feuObj.GetComponent<ParticleSystem>();

            var main = feuInstance.main;
            float range = lanceFlameScript.RangeBoost.Quantite;
            float bulletSpeed = lanceFlameScript.BulletSpeedWithBoost.Quantite;

            // Ajuste la vitesse de d�part
            main.startSpeed = bulletSpeed;

            // Ajuste la dur�e de vie pour que la distance reste constante
            main.startLifetime = range / bulletSpeed; // Dur�e de vie calcul�e pour que la particule parcoure la m�me distance
        }
        else
        {
            feuInstance.gameObject.SetActive(true); // R�active l'objet de feu si n�cessaire
        }
    }

    public void ActiverGlace()
    {
        // D�sactive le feu si il est actif
        if (feuInstance != null)
        {
            feuInstance.Stop();
            feuInstance = null;
        }

        // Instancie une nouvelle instance de glace si n�cessaire
        if (glaceInstance == null)
        {
            GameObject glaceObj = Instantiate(glacePrefab, GlaceConteneur.transform.position, GlaceConteneur.transform.rotation);
            glaceObj.transform.parent = GlaceConteneur.transform; // Place la glace sous l'objet contr�leur
            glaceInstance = glaceObj.GetComponent<ParticleSystem>();

            var main = glaceInstance.main;
            float range = lanceFlameScript.RangeBoost.Quantite;
            float bulletSpeed = lanceFlameScript.BulletSpeedWithBoost.Quantite;

            // Ajuste la vitesse de d�part
            main.startSpeed = bulletSpeed;

            // Ajuste la dur�e de vie pour que la distance reste constante
            main.startLifetime = range / bulletSpeed; // Dur�e de vie calcul�e pour que la particule parcoure la m�me distance
        }
        else
        {
            glaceInstance.gameObject.SetActive(true); // R�active l'objet de glace si n�cessaire
        }
    }

    public void OverHeatParticulePlay()
    {
        if (OverheatParticule != null)
        {
            OverheatParticule.gameObject.SetActive(true);
            OverheatParticule.Play(); // D�marre les particules pour indiquer la surchauffe
        }
        else
        {
            Debug.LogWarning("OverheatParticule n'est pas assign�.");
        }
    }

    public void OverHeatParticuleStop()
    {
        if (OverheatParticule != null)
        {
            OverheatParticule.Stop();
            OverheatParticule.Clear();// Arr�te les particules quand la surchauffe est termin�e
            OverheatParticule.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("OverheatParticule n'est pas assign�.");
        }
    }

    public void DesactiverParticules()
    {
        // D�sactive
        if (feuInstance != null)
        {
            feuInstance.Stop();

            feuInstance = null;
        }
        if (glaceInstance != null)
        {
            glaceInstance.Stop();

            glaceInstance = null;
        }
    }
    void Update()
    {
       // ControleParticule();
    }

    public void ControleParticule(bool canShoot,bool modeGlace)
    {
        if (Input.GetMouseButton(0)) // Bouton gauche pour activer les particules
        {
            if (modeGlace)
            {
                ActiverGlace(); // Active la glace si en mode glace
            }
            else
            {
                ActiverFeu(); // Active le feu si en mode feu
            }
        }
        else  // Aucun bouton n'est maintenu ou pas de balles disponibles
        {
            DesactiverParticules();
        }
    }
}
