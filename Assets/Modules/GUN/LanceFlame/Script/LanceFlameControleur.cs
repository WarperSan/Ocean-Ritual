using ControllerModule.Controllers;
using ControllerModule.Controllers.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public class LanceFlameControleur : Arme ,Equipement
{
    [SerializeField] ComponantGBN ComponantGBN;
    [SerializeField] ParticuleControleur ParticuleControleurs;
    [SerializeField] TypeQuantite<TypeWeapon> ReloadSpeed = new(TypeWeapon.ReloadSpeed, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> Attack = new(TypeWeapon.attack, 1f);
    [SerializeField] public TypeQuantite<TypeWeapon> BulletSpeed = new(TypeWeapon.bulletspeed, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> BulletSize = new(TypeWeapon.bulletSize, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> FireRate = new(TypeWeapon.fireRate, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> AmmoCapacity = new(TypeWeapon.AmmoCapacity, 1f);
    [SerializeField] public TypeQuantite<TypeWeapon> Range = new(TypeWeapon.Range, 1f);

    [SerializeField] TypeQuantite<TypeWeapon> ReloadSpeedWithBoost = new(TypeWeapon.ReloadSpeed, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> AttackWithBoost = new(TypeWeapon.attack, 1f);
    [SerializeField] public TypeQuantite<TypeWeapon> BulletSpeedWithBoost = new(TypeWeapon.bulletspeed, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> BulletSizeWithBoost = new(TypeWeapon.bulletSize, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> FireRateWithBoost = new(TypeWeapon.fireRate, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> AmmoCapacityBoost = new(TypeWeapon.AmmoCapacity, 1f);
    [SerializeField] public TypeQuantite<TypeWeapon> RangeBoost = new(TypeWeapon.Range, 1f);

    [SerializeField] GameObject balleDeLF;
    [SerializeField] GameObject ConteneurBall;
    [SerializeField] GameObject ZoneDeTire;


    bool modeGlace = false;
    [SerializeField] Material feu;
    [SerializeField] Material glace;

    [SerializeField] float grosseurDExpension = 2;
    private bool secondaryShoot = false;

    [SerializeField] GameObject ChargeurBalle;

    [SerializeField] private bool isOverheated = false;

    [SerializeField] int AmmoInClip;
    public ComponantGBN ComposnantGBN
    {
        get { return ComponantGBN; }
    }

    private bool faireStat = true;


    private float reloadSpeed = 1f; // Vitesse de recharge en pourcentage par seconde
    private float timeSinceLastShot = 0f; // Temps écoulé depuis le dernier tir
    [SerializeField] private Vector3 initialScaleChargeur;
    #region Unity Methods
   
    public void StartFonction()
    {

        if (faireStat)
        {
            faireStat = false;
            ComponantGBN.GBNScript.GetStat();
            UpdateStat();
        }
        // Remplace les statistiques de base par les statistiques boostées


        reloadSpeed = ReloadSpeedWithBoost.Quantite;

        AmmoInClip = Mathf.RoundToInt(AmmoCapacityBoost.Quantite); // Initialise les munitions à la capacité maximale
        if (ChargeurBalle != null)
        {
            initialScaleChargeur = ChargeurBalle.transform.localScale;
        }
    }
   
    public void ControllerUpdate()
    {

        ParticuleControleurs.ControleParticule(!isOverheated, modeGlace);
        timeSinceLastShot += Time.deltaTime; // Met à jour le temps écoulé depuis le dernier tir

        if (!isOverheated)
        {
            if (Input.GetMouseButtonDown(2)) // Changer de mode avec le bouton molette
            {
                ChangeMode();
            }

            if (Input.GetMouseButton(0)) // Bouton gauche pour tirer, dépend du mode sélectionné
            {
                if (modeGlace)
                {
                    SecondaryShoot(); // Tire avec la glace si en mode glace
                }
                else
                {
                    MainShoot(); // Tire avec le feu si en mode feu
                }
            }
            else // Ne tire pas, donc recharge
            {
                Reload();
            }
        }
        else
        {
            Reload(); // Recharge si en surchauffe
        }
    }
    #endregion

    #region Arme Interface Methods
    public override float GetDamage()
    {
        return AttackWithBoost.Quantite;
    }

    public override void OverHeat()
    {
        // Fonction vide
    }

    public override void OverHeatIndicator()
    {
        if (ChargeurBalle != null)
        {
            // Calcule la proportion de balles restantes par rapport à la capacité totale
            float ammoRatio = (float)AmmoInClip / AmmoCapacityBoost.Quantite;
            Vector3 initialScale = ChargeurBalle.transform.localScale;
            // Réduit l'échelle sur l'axe Y en fonction du nombre de balles restantes
            Vector3 newScale = initialScaleChargeur;
            newScale.y = Mathf.Clamp(ammoRatio * initialScaleChargeur.y, 0, initialScaleChargeur.y);

            // Applique la nouvelle échelle au chargeur
            ChargeurBalle.transform.localScale = newScale;
            // Ajuste la position en Y pour garder le bas du cylindre en contact avec le sol
            // On calcule la différence de hauteur, puis on ajuste la position sur l'axe Y
            float heightDifference = initialScale.y - newScale.y;
            Vector3 newPosition = ChargeurBalle.transform.localPosition;
            newPosition.y -= heightDifference; // Ajuste la position pour maintenir le contact avec le sol
            ChargeurBalle.transform.localPosition = newPosition;
        }
        else
        {
            Debug.LogWarning("ChargeurBalle n'est pas assigné.");
        }
    }

    float ExtraTimeBullet = 0;
    public override void Reload()
    {
        // Si le chargeur n'est pas déjà plein
        if (AmmoInClip < AmmoCapacityBoost.Quantite)
        {
            // Calcule le temps nécessaire pour recharger une balle
            float timeToReloadOneBullet = 1f / reloadSpeed; // secondes par balle

            // Ajoute le temps écoulé depuis la dernière mise à jour au temps excédentaire
            ExtraTimeBullet += Time.deltaTime;

            // Calcule le nombre de balles à recharger basé sur le temps écoulé
            int bulletsToReload = Mathf.FloorToInt(ExtraTimeBullet / timeToReloadOneBullet);

            // Ajoute les balles en respectant la capacité maximale
            AmmoInClip += bulletsToReload;
            AmmoInClip = Mathf.Clamp(AmmoInClip, 0, (int)AmmoCapacityBoost.Quantite);

            // Conserve le reste du temps excédentaire après avoir rechargé les balles
            ExtraTimeBullet %= timeToReloadOneBullet;

            // Si les munitions sont complètement rechargées, on désactive la surchauffe
            if (AmmoInClip >= AmmoCapacityBoost.Quantite)
            {
                isOverheated = false; // Arrête la surchauffe une fois le chargeur plein
                ParticuleControleurs.OverHeatParticuleStop();
            }
            OverHeatIndicator();
        }
    }
    public  bool CanShoot()
    {
        // Empêche de tirer si l'arme est en surchauffe
        if (isOverheated)
        {
            return false;
        }

        // Empêche de tirer si le temps de recharge est insuffisant ou si il n'y a pas de munitions
        if (timeSinceLastShot >= 1f / FireRateWithBoost.Quantite && AmmoInClip > 0)
        {
            return true;
        }



        return false;
    }
    public override void MainShoot()
    {
        if (CanShoot())
        {
            InstantiateAmmo();
        }
    }
  
    public  void SetPositionEtRotation(GameObject balle)
    {
        if (ZoneDeTire != null && balle != null)
        {
            // Assigner la position et la rotation de ZoneDeTire à la balle
            balle.transform.position = ZoneDeTire.transform.position;
            balle.transform.rotation = ZoneDeTire.transform.rotation;
        }
        else
        {
            Debug.LogError("ZoneDeTire ou la balle est null.");
        }
    }

    public override void ChangeMode()
    {
        modeGlace = !modeGlace;

        if (ChargeurBalle != null)
        {
            Renderer chargeurRenderer = ChargeurBalle.GetComponent<Renderer>(); // Récupère le Renderer du ChargeurBalle

            if (chargeurRenderer != null)
            {
                // Applique le matériau approprié en fonction du mode
                chargeurRenderer.material = modeGlace ? glace : feu;
            }
            else
            {
                Debug.LogWarning("Renderer de ChargeurBalle introuvable.");
            }
        }
        else
        {
            Debug.LogWarning("ChargeurBalle n'est pas assigné.");
        }
    }

    public override void SecondaryShoot()
    {
        if (CanShoot())
        {
            InstantiateAmmo();
        }
    }

    public override void Rotation()
    {
        // Fonction vide
    }

    public override void InstantiateAmmo()
    {
        GameObject Balle = BallGenerator.objectPool.GetBall(balleDeLF, ConteneurBall.transform);
        if (Balle != null)
        {
            SetBulletProriety(Balle);
            SetPositionEtRotation(Balle);
            Balle.SetActive(true);

            AmmoInClip--; // Consommer une balle du chargeur
                          // Si on est à court de munitions, activer la surchauffe
            if (AmmoInClip <= 0)
            {
                isOverheated = true; // Active la surchauffe
                ParticuleControleurs.OverHeatParticulePlay();
            }
            timeSinceLastShot = 0f; // Réinitialise le temps depuis le dernier tir
            OverHeatIndicator();
        }

    }

    public override void GetEffect()
    {
        // Fonction vide
    }

    public override void SetBulletProriety(GameObject balle)
    {
        BalleLF scriptBall = balle.GetComponent<BalleLF>();
        scriptBall.ResetGrossisement();
        if (scriptBall != null)
        {
            scriptBall.Vitesse = BulletSpeedWithBoost.Quantite;
            scriptBall.ValeurGrossissement = grosseurDExpension;
            scriptBall.Range = RangeBoost.Quantite;
            scriptBall.lanceFlameScript = this;
        }
        else
        {
            Debug.LogError("Aucun script BalleLF trouvé sur cet objet.");
        }
    }
    #endregion

    #region Stat Management
    public void UpdateStat()
    {
        // Création de la liste des statistiques de base
        List<TypeQuantite<TypeWeapon>> baseStats = new List<TypeQuantite<TypeWeapon>>
        {
            ReloadSpeed,
            Attack,
            BulletSpeed,
            BulletSize,
            FireRate,
            AmmoCapacity,
            Range
        };

        // Création de la liste des statistiques boostées
        List<TypeQuantite<TypeWeapon>> boostedStats = new List<TypeQuantite<TypeWeapon>>
        {
            ReloadSpeedWithBoost,
            AttackWithBoost,
            BulletSpeedWithBoost,
            BulletSizeWithBoost,
            FireRateWithBoost,
            AmmoCapacityBoost,
            RangeBoost
        };

        // Mise à jour des statistiques avec les boosts
        ComponantGBN.GBNScript.UpdateStatsWithBoost(baseStats, boostedStats);
    }
    #endregion








    protected override void OnSwitchIn() { }
    protected override void OnSwitchOut() { }
    protected override void OnUpdate(float elapsed) { ControllerUpdate(); }
    protected override void OnStart() { StartFonction();

        ControllerManager.SwitchTo(this);
    
    }
}
