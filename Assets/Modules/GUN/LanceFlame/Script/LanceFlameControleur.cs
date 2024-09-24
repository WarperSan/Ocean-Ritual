using EntityModule;
using System.Collections.Generic;
using UnityEngine;
using WeaponModule;
using WeaponModule.Interfaces;
using static EnumGeneral;

/// <summary>
/// Different modes for <see cref="LanceFlameControleur"/>
/// </summary>
public enum LanceFlameModes
{
    FIRE,
    ICE
};

public class LanceFlameControleur : WeaponController, IEquipement, IOverheatable, IMultiMode<LanceFlameModes>
{
    [Header("UNSORTED MESS")]
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
    [SerializeField] GameObject ZoneDeTire;

    [SerializeField] float grosseurDExpension = 2;

    [SerializeField] int AmmoInClip;
    public ComponantGBN ComposnantGBN => ComponantGBN;

    private bool faireStat = true;

    private float reloadSpeed = 1f; // Vitesse de recharge en pourcentage par seconde

    #region Arme Interface Methods
    public override float GetDamage()
    {
        return AttackWithBoost.Quantite;
    }

    float ExtraTimeBullet = 0;
    public override void Reload()
    {
        // Si le chargeur d�j� plein
        if (AmmoInClip >= AmmoCapacityBoost.Quantite)
            return;

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

        this.UpdateOverheatIndicator();
    }

    private void SetPositionEtRotation(GameObject balle)
    {
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        // Assigner la position et la rotation de ZoneDeTire � la balle
        if (ZoneDeTire != null)
        {
            position = ZoneDeTire.transform.position;
            rotation = ZoneDeTire.transform.rotation;
        }

        balle.transform.SetPositionAndRotation(position, rotation);
    }

    public void InstantiateAmmo()
    {
        GameObject Balle = this.GetBullet(balleDeLF.name);

        if (Balle == null)
            return;

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
        this.UpdateOverheatIndicator();
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

        if (balle.TryGetComponent(out Projectile projectile))
        {
            projectile.Attribute(new Attack()
            {
                Damage = this.GetDamage(),
                Type = this.currentMode switch
                {
                    LanceFlameModes.FIRE => AttackType.FIRE,
                    LanceFlameModes.ICE => AttackType.ICE,
                    _ => AttackType.NORMAL,
                },
                TargetType = ProjectileTarget.OPPONENTS
            });
        }
    }

    #endregion

    #region Stat Management

    public void UpdateStat()
    {
        // Cr�ation de la liste des statistiques de base
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

        // Cr�ation de la liste des statistiques boost�es
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

        // Mise � jour des statistiques avec les boosts
        ComponantGBN.GBNScript.UpdateStatsWithBoost(baseStats, boostedStats);
    }

    #endregion

    #region Controller

    /// <inheritdoc/>
    protected override void OnStart()
    {
        base.OnStart();

        if (faireStat)
        {
            faireStat = false;
            ComponantGBN.GBNScript.GetStat();
            UpdateStat();
        }
        // Remplace les statistiques de base par les statistiques boost�es

        reloadSpeed = ReloadSpeedWithBoost.Quantite;

        AmmoInClip = Mathf.RoundToInt(AmmoCapacityBoost.Quantite); // Initialise les munitions � la capacit� maximale

        // --- OVERHEAT ---
        if (OverheatMeter != null)
            initialScaleOverheatMeter = OverheatMeter.localScale;
        // ---
    }

    protected override void OnUpdate(float elapsed)
    {
        base.OnUpdate(elapsed);

        this.ParticuleControleurs.ControleParticule(this.CanShoot(), this.currentMode == LanceFlameModes.ICE);
    }
    #endregion

    #region WeaponController

    /// <inheritdoc/>
    public override bool CanShoot()
    {
        // If the controller is disabled, skip
        if (!this.IsEnabled)
            return false;

        // Emp�che de tirer si l'arme est en surchauffe
        if (isOverheated)
            return false;

        // Emp�che si il n'y a pas de munitions
        if (AmmoInClip <= 0)
            return false;

        // Emp�che de tirer si le temps de recharge est insuffisant
        if (timeSinceLastShot < 1f / FireRateWithBoost.Quantite)
            return false;

        return true;
    }

    /// <inheritdoc/>
    public override void Shoot()
    {
        // Bouton gauche pour tirer, d�pend du mode s�lectionn�
        if (this.currentMode == LanceFlameModes.FIRE)
        {
            this.FireShoot(); // Tire avec le feu si en mode feu
        }
        else
        {
            this.IceShoot(); // Tire avec la glace si en mode glace
        }
    }

    #endregion

    #region IOverheatable

    [Header("IOverheatable")]
    [SerializeField]
    private bool isOverheated = false;

    [SerializeField]
    private Transform OverheatMeter;
    private Vector3 initialScaleOverheatMeter;

    /// <inheritdoc/>
    public void OnOverheat() { /* Fonction vide */ }

    /// <inheritdoc/>
    public void UpdateOverheatIndicator()
    {
        if (OverheatMeter == null)
            return;

        // Calcule la proportion de balles restantes par rapport � la capacit� totale
        float ammoRatio = AmmoInClip / AmmoCapacityBoost.Quantite;
        Vector3 initialScale = OverheatMeter.localScale;

        // R�duit l'�chelle sur l'axe Y en fonction du nombre de balles restantes
        Vector3 newScale = initialScaleOverheatMeter;
        newScale.y = Mathf.Clamp(ammoRatio * initialScaleOverheatMeter.y, 0, initialScaleOverheatMeter.y);

        // Applique la nouvelle �chelle au chargeur
        OverheatMeter.localScale = newScale;

        // Ajuste la position en Y pour garder le bas du cylindre en contact avec le sol
        // On calcule la diff�rence de hauteur, puis on ajuste la position sur l'axe Y
        float heightDifference = initialScale.y - newScale.y;
        Vector3 newPosition = OverheatMeter.localPosition;
        newPosition.y -= heightDifference; // Ajuste la position pour maintenir le contact avec le sol
        OverheatMeter.localPosition = newPosition;
    }

    /// <inheritdoc/>
    bool IOverheatable.IsOverheated() => this.isOverheated;

    #endregion

    #region IMultiMode

    [Header("IMultiMode")]
    [SerializeField]
    private LanceFlameModes currentMode;

    [SerializeField]
    private Material feu;

    [SerializeField]
    private Material glace;

    [SerializeField]
    private Renderer modeRenderer;

    /// <inheritdoc/>
    public LanceFlameModes NextMode() => this.currentMode switch
    {
        LanceFlameModes.FIRE => LanceFlameModes.ICE,
        LanceFlameModes.ICE => LanceFlameModes.FIRE,
        _ => LanceFlameModes.ICE
    };

    /// <inheritdoc/>
    public void SetMode(LanceFlameModes mode)
    {
        this.currentMode = mode;

        if (modeRenderer == null)
            return;

        // Applique le mat�riau appropri� en fonction du mode
        modeRenderer.material = mode switch
        {
            LanceFlameModes.FIRE => feu,
            LanceFlameModes.ICE => glace,
            _ => feu
        };
    }

    private void FireShoot()
    {
        this.InstantiateAmmo();
    }

    private void IceShoot()
    {
        this.InstantiateAmmo();
    }

    #endregion
}
