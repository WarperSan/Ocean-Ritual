using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticuleControleur : MonoBehaviour
{
    [SerializeField] GameObject feuPrefab;  // Préfabriqué pour le feu
    [SerializeField] GameObject glacePrefab; // Préfabriqué pour la glace
    [SerializeField] GameObject FeuConteneur;  // Conteneur pour les effets de feu
    [SerializeField] GameObject GlaceConteneur; // Conteneur pour les effets de glace
    [SerializeField] LanceFlameControleur lanceFlameScript; // Conteneur lanceFlame
    [SerializeField] ParticleSystem OverheatParticule; // Conteneur overheatParticule
    private ParticleSystem feuInstance;
    private ParticleSystem glaceInstance;

    // Méthodes d'activation des particules
    public void ActiverFeu()
    {
        // Désactive la glace si elle est active
        if (glaceInstance != null)
        {
            glaceInstance.Stop();
            glaceInstance = null;
        }

        // Instancie une nouvelle instance de feu si nécessaire
        if (feuInstance == null)
        {
            GameObject feuObj = Instantiate(feuPrefab, FeuConteneur.transform.position, FeuConteneur.transform.rotation);
            feuObj.transform.parent = FeuConteneur.transform; // Place le feu sous l'objet contrôleur
            feuInstance = feuObj.GetComponent<ParticleSystem>();

            var main = feuInstance.main;
            float range = lanceFlameScript.RangeBoost.Quantite;
            float bulletSpeed = lanceFlameScript.BulletSpeedWithBoost.Quantite;

            // Ajuste la vitesse de départ
            main.startSpeed = bulletSpeed;

            // Ajuste la durée de vie pour que la distance reste constante
            main.startLifetime = range / bulletSpeed; // Durée de vie calculée pour que la particule parcoure la même distance
        }
        else
        {
            feuInstance.gameObject.SetActive(true); // Réactive l'objet de feu si nécessaire
        }
    }

    public void ActiverGlace()
    {
        // Désactive le feu si il est actif
        if (feuInstance != null)
        {
            feuInstance.Stop();
            feuInstance = null;
        }

        // Instancie une nouvelle instance de glace si nécessaire
        if (glaceInstance == null)
        {
            GameObject glaceObj = Instantiate(glacePrefab, GlaceConteneur.transform.position, GlaceConteneur.transform.rotation);
            glaceObj.transform.parent = GlaceConteneur.transform; // Place la glace sous l'objet contrôleur
            glaceInstance = glaceObj.GetComponent<ParticleSystem>();

            var main = glaceInstance.main;
            float range = lanceFlameScript.Range.Quantite;
            float bulletSpeed = lanceFlameScript.BulletSpeed.Quantite;

            // Ajuste la vitesse de départ
            main.startSpeed = bulletSpeed;

            // Ajuste la durée de vie pour que la distance reste constante
            main.startLifetime = range / bulletSpeed; // Durée de vie calculée pour que la particule parcoure la même distance
        }
        else
        {
            glaceInstance.gameObject.SetActive(true); // Réactive l'objet de glace si nécessaire
        }
    }

    public void OverHeatParticulePlay()
    {
        if (OverheatParticule != null)
        {
            OverheatParticule.gameObject.SetActive(true);
            OverheatParticule.Play(); // Démarre les particules pour indiquer la surchauffe
        }
        else
        {
            Debug.LogWarning("OverheatParticule n'est pas assigné.");
        }
    }

    public void OverHeatParticuleStop()
    {
        if (OverheatParticule != null)
        {
            OverheatParticule.Stop();
            OverheatParticule.Clear();// Arrête les particules quand la surchauffe est terminée
            OverheatParticule.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("OverheatParticule n'est pas assigné.");
        }
    }

    public void DesactiverParticules()
    {
        // Désactive
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
        if (canShoot && Input.GetMouseButton(0)) // Bouton gauche pour activer les particules
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
