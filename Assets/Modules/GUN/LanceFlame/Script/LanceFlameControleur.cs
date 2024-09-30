using ControllerModule.Controllers;
using ControllerModule.Controllers.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public class LanceFlameControleur : Arme 
{
    [SerializeField] componentGBN GBNComponent;
    [SerializeField] ParticuleControleur ParticuleControleurs;
    [SerializeField] TypeQuantity<TypeWeapon> ReloadSpeed = new(TypeWeapon.ReloadSpeed, 1f);
    [SerializeField] TypeQuantity<TypeWeapon> Attack = new(TypeWeapon.attack, 1f);
    [SerializeField] public TypeQuantity<TypeWeapon> BulletSpeed = new(TypeWeapon.bulletspeed, 1f);
    [SerializeField] TypeQuantity<TypeWeapon> BulletSize = new(TypeWeapon.bulletSize, 1f);
    [SerializeField] TypeQuantity<TypeWeapon> FireRate = new(TypeWeapon.fireRate, 1f);
    [SerializeField] TypeQuantity<TypeWeapon> AmmoCapacity = new(TypeWeapon.AmmoCapacity, 1f);
    [SerializeField] public TypeQuantity<TypeWeapon> Range = new(TypeWeapon.Range, 1f);

    [SerializeField] TypeQuantity<TypeWeapon> ReloadSpeedWithBoost = new(TypeWeapon.ReloadSpeed, 1f);
    [SerializeField] TypeQuantity<TypeWeapon> AttackWithBoost = new(TypeWeapon.attack, 1f);
    [SerializeField] public TypeQuantity<TypeWeapon> BulletSpeedWithBoost = new(TypeWeapon.bulletspeed, 1f);
    [SerializeField] TypeQuantity<TypeWeapon> BulletSizeWithBoost = new(TypeWeapon.bulletSize, 1f);
    [SerializeField] TypeQuantity<TypeWeapon> FireRateWithBoost = new(TypeWeapon.fireRate, 1f);
    [SerializeField] TypeQuantity<TypeWeapon> AmmoCapacityBoost = new(TypeWeapon.AmmoCapacity, 1f);
    [SerializeField] public TypeQuantity<TypeWeapon> RangeBoost = new(TypeWeapon.Range, 1f);

    [SerializeField] GameObject balleDeLF;
    [SerializeField] GameObject ConteneurBall;
    [SerializeField] GameObject ZoneDeTire;


    bool modeGlace = false;
    [SerializeField] Material feu;
    [SerializeField] Material glace;

    [SerializeField] float grosseurDExpension = 2;
  //  private bool secondaryShoot = false;

    [SerializeField] GameObject ChargeurBalle;

    [SerializeField] private bool isOverheated = false;

    [SerializeField] int AmmoInClip;
    public componentGBN componentGBN
    {
        get { return GBNComponent; }
    }

    public int CostToUpgrade { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int ForgePercentage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int LvlOfEquipment { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private bool faireStat = true;


    private float reloadSpeed = 1f; // Vitesse de recharge en pourcentage par seconde
    private float timeSinceLastShot = 0f; // Temps �coul� depuis le dernier tir
    [SerializeField] private Vector3 initialScaleChargeur;
    #region Unity Methods
   
    public void StartFonction()
    {

        if (faireStat)
        {
            faireStat = false;
            componentGBN.GBNScript.GetStat();
            UpdateStat();
        }
        // Remplace les statistiques de base par les statistiques boost�es


        reloadSpeed = ReloadSpeedWithBoost.Quantite;

        AmmoInClip = Mathf.RoundToInt(AmmoCapacityBoost.Quantite); // Initialise les munitions � la capacit� maximale
        if (ChargeurBalle != null)
        {
            initialScaleChargeur = ChargeurBalle.transform.localScale;
        }
    }
   
    public void ControllerUpdate()
    {

        ParticuleControleurs.ControleParticule(!isOverheated, modeGlace);
        timeSinceLastShot += Time.deltaTime; // Met � jour le temps �coul� depuis le dernier tir

        if (!isOverheated)
        {
            if (Input.GetMouseButtonDown(2)) // Changer de mode avec le bouton molette
            {
                ChangeMode();
            }

            if (Input.GetMouseButton(0)) // Bouton gauche pour tirer, d�pend du mode s�lectionn�
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
            // Calcule la proportion de balles restantes par rapport � la capacit� totale
            float ammoRatio = (float)AmmoInClip / AmmoCapacityBoost.Quantite;
            Vector3 initialScale = ChargeurBalle.transform.localScale;
            // R�duit l'�chelle sur l'axe Y en fonction du nombre de balles restantes
            Vector3 newScale = initialScaleChargeur;
            newScale.y = Mathf.Clamp(ammoRatio * initialScaleChargeur.y, 0, initialScaleChargeur.y);

            // Applique la nouvelle �chelle au chargeur
            ChargeurBalle.transform.localScale = newScale;
            // Ajuste la position en Y pour garder le bas du cylindre en contact avec le sol
            // On calcule la diff�rence de hauteur, puis on ajuste la position sur l'axe Y
            float heightDifference = initialScale.y - newScale.y;
            Vector3 newPosition = ChargeurBalle.transform.localPosition;
            newPosition.y -= heightDifference; // Ajuste la position pour maintenir le contact avec le sol
            ChargeurBalle.transform.localPosition = newPosition;
        }
        else
        {
            Debug.LogWarning("ChargeurBalle n'est pas assign�.");
        }
    }

    float ExtraTimeBullet = 0;
    public override void Reload()
    {
        // Si le chargeur n'est pas d�j� plein
        if (AmmoInClip < AmmoCapacityBoost.Quantite)
        {
            // Calcule le temps n�cessaire pour recharger une balle
            float timeToReloadOneBullet = 1f / reloadSpeed; // secondes par balle

            // Ajoute le temps �coul� depuis la derni�re mise � jour au temps exc�dentaire
            ExtraTimeBullet += Time.deltaTime;

            // Calcule le nombre de balles � recharger bas� sur le temps �coul�
            int bulletsToReload = Mathf.FloorToInt(ExtraTimeBullet / timeToReloadOneBullet);

            // Ajoute les balles en respectant la capacit� maximale
            AmmoInClip += bulletsToReload;
            AmmoInClip = Mathf.Clamp(AmmoInClip, 0, (int)AmmoCapacityBoost.Quantite);

            // Conserve le reste du temps exc�dentaire apr�s avoir recharg� les balles
            ExtraTimeBullet %= timeToReloadOneBullet;

            // Si les munitions sont compl�tement recharg�es, on d�sactive la surchauffe
            if (AmmoInClip >= AmmoCapacityBoost.Quantite)
            {
                isOverheated = false; // Arr�te la surchauffe une fois le chargeur plein
                ParticuleControleurs.OverHeatParticuleStop();
            }
            OverHeatIndicator();
        }
    }
    public  bool CanShoot()
    {
        // Emp�che de tirer si l'arme est en surchauffe
        if (isOverheated)
        {
            return false;
        }

        // Emp�che de tirer si le temps de recharge est insuffisant ou si il n'y a pas de munitions
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
            // Assigner la position et la rotation de ZoneDeTire � la balle
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
            Renderer chargeurRenderer = ChargeurBalle.GetComponent<Renderer>(); // R�cup�re le Renderer du ChargeurBalle

            if (chargeurRenderer != null)
            {
                // Applique le mat�riau appropri� en fonction du mode
                chargeurRenderer.material = modeGlace ? glace : feu;
            }
            else
            {
                Debug.LogWarning("Renderer de ChargeurBalle introuvable.");
            }
        }
        else
        {
            Debug.LogWarning("ChargeurBalle n'est pas assign�.");
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
                          // Si on est � court de munitions, activer la surchauffe
            if (AmmoInClip <= 0)
            {
                isOverheated = true; // Active la surchauffe
                ParticuleControleurs.OverHeatParticulePlay();
            }
            timeSinceLastShot = 0f; // R�initialise le temps depuis le dernier tir
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
            Debug.LogError("Aucun script BalleLF trouv� sur cet objet.");
        }
    }
    #endregion

    #region Stat Management
    public void UpdateStat()
    {
        // Cr�ation de la liste des statistiques de base
        List<TypeQuantity<TypeWeapon>> baseStats = new List<TypeQuantity<TypeWeapon>>
        {
            ReloadSpeed,
            Attack,
            BulletSpeed,
            BulletSize,
            FireRate,
            AmmoCapacity,
            Range
        };

        // Cr�ation de la liste des statistiques boost�es
        List<TypeQuantity<TypeWeapon>> boostedStats = new List<TypeQuantity<TypeWeapon>>
        {
            ReloadSpeedWithBoost,
            AttackWithBoost,
            BulletSpeedWithBoost,
            BulletSizeWithBoost,
            FireRateWithBoost,
            AmmoCapacityBoost,
            RangeBoost
        };

        // Mise � jour des statistiques avec les boosts
        componentGBN.GBNScript.UpdateStatsWithBoost(baseStats, boostedStats);
    }
    #endregion








    protected override void OnSwitchIn() { }
    protected override void OnSwitchOut() { }
    protected override void OnUpdate(float elapsed) { ControllerUpdate(); }
    protected override void OnStart() { StartFonction();

        ControllerManager.SwitchTo(this);
    
    }

    public void UpgradeEquipment() => throw new System.NotImplementedException();
    public void GetUpgradeStat() => throw new System.NotImplementedException();
    public int GetCostForUpgrade() => throw new System.NotImplementedException();
    public (List<TypeQuantity<TypeWeapon>> baseStats, List<TypeQuantity<TypeWeapon>> previewStats, int upgradeCost) GetStatToUpgradeAndCost() => throw new System.NotImplementedException();
    public TypeQuantity<TypeWeapon> AfterUpgradPreviewStat(TypeQuantity<TypeWeapon> statToUpgrade) => throw new System.NotImplementedException();
}
