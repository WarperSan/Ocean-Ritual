using EntityModule;
using System;
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

public class LanceFlameControleur : WeaponController, Equipment, IOverheatable, IMultiMode<LanceFlameModes> 
{
    #region Controller

    /// <inheritdoc/>
    protected override void OnStart()
    {
        base.OnStart();

        // --- STATS ---
        ComponantGBN.GBNScript.GetStat();
        this.UpdateStat();
        // ---

        // --- OVERHEAT ---
        if (OverheatMeter != null)
            initialScaleOverheatMeter = OverheatMeter.localScale;
        // ---

        this.remainingBullets = this.GetClipSize();
    }

    /// <inheritdoc/>
    protected override void OnSwitchIn()
    {
        SetCursorLock(true);
        Cursor.visible = false;
    }
    
    /// <inheritdoc/>
    protected override void OnSwitchOut()
    {
        SetCursorLock(false);
        Cursor.visible = true;
    }

    /// <inheritdoc/>
    protected override void OnUpdate(float elapsed)
    {
        base.OnUpdate(elapsed);

        this.ParticuleControleurs.ControleParticule(this.CanShoot(), this.currentMode == LanceFlameModes.ICE);
    }

    #endregion

    #region Shoot

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
        if (this.remainingBullets <= 0)
            return false;

        // Emp�che de tirer si le temps de recharge est insuffisant
        if (timeSinceLastShot < 1f / BOOSTED_FIRERATE.Quantite)
            return false;

        return true;
    }

    /// <inheritdoc/>
    protected override void SetupProjectile(Projectile projectile)
    {
        // If unexpected projectile, skip
        if (projectile is not BalleLF balleLF)
            return;

        // Set properties
        balleLF.SetProperties(
            this.GetBulletSpeed(),
            this.GetRange()
        );

        this.PlaceBullet(balleLF);

        timeSinceLastShot = 0f; // R�initialise le temps depuis le dernier tir
        this.UpdateOverheatIndicator();
    }

    /// <inheritdoc/>
    protected override Attack GetAttack() => new()
    {
        Damage = this.GetDamage(),
        Type = this.currentMode switch
        {
            LanceFlameModes.FIRE => AttackType.FIRE,
            LanceFlameModes.ICE => AttackType.ICE,
            _ => AttackType.NORMAL,
        },
        TargetType = ProjectileTarget.OPPONENTS
    };

    #endregion

    #region Reload

    /// <inheritdoc/>
    protected override uint GetClipSize() => (uint)BOOSTED_AMMO_CAPACITY.Quantite;

    /// <inheritdoc/>
    protected override float GetReloadSpeed() => BOOSTED_RELOAD_SPEED.Quantite;

    /// <inheritdoc/>
    protected override void OnReloadStart()
    {
        isOverheated = true; // Active la surchauffe
        ParticuleControleurs.OverHeatParticulePlay();
    }

    /// <inheritdoc/>
    protected override void OnReload() => this.UpdateOverheatIndicator();

    /// <inheritdoc/>
    protected override void OnReloadCompleted()
    {
        isOverheated = false; // Arr�te la surchauffe une fois le chargeur plein
        ParticuleControleurs.OverHeatParticuleStop();
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
        float ammoRatio = this.remainingBullets / (float)this.GetClipSize();
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

    #endregion

    #region IEquipement

    [Header("IEquipement")]
    [SerializeField] private TypeQuantity<TypeWeapon> BASE_RELOAD_SPEED = new(TypeWeapon.VitesseRechargement, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BASE_ATTACK = new(TypeWeapon.Attaque, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BASE_BULLET_SPEED = new(TypeWeapon.VitesseBalle, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BASE_BULLET_SIZE = new(TypeWeapon.TailleDeBalle, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BASE_FIRERATE = new(TypeWeapon.VitesseDeTire, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BASE_AMMO_CAPACITY = new(TypeWeapon.CapaciterDeBall, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BASE_RANGE = new(TypeWeapon.Porter, 1f);

    [SerializeField] private TypeQuantity<TypeWeapon> BOOSTED_RELOAD_SPEED = new(TypeWeapon.VitesseRechargement, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BOOSTED_ATTACK = new(TypeWeapon.Attaque, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BOOSTED_BULLET_SPEED = new(TypeWeapon.VitesseBalle, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BOOSTED_BULLET_SIZE = new(TypeWeapon.TailleDeBalle, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BOOSTED_FIRERATE = new(TypeWeapon.VitesseDeTire, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BOOSTED_AMMO_CAPACITY = new(TypeWeapon.CapaciterDeBall, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BOOSTED_RANGE = new(TypeWeapon.Porter, 1f);

    [SerializeField] private componentGBN ComponantGBN;
    public componentGBN componentGBN => ComponantGBN;

    public int CostToUpgrade { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int ForgePercentage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int LvlOfEquipment { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public string Name => throw new System.NotImplementedException();

    /// <inheritdoc/>
    public void UpdateStat()
    {
        // Cr�ation de la liste des statistiques de base
        var baseStats = new List<TypeQuantity<TypeWeapon>>
        {
            BASE_RELOAD_SPEED,
            BASE_ATTACK,
            BASE_BULLET_SPEED,
            BASE_BULLET_SIZE,
            BASE_FIRERATE,
            BASE_AMMO_CAPACITY,
            BASE_RANGE
        };

        // Cr�ation de la liste des statistiques boost�es
        var boostedStats = new List<TypeQuantity<TypeWeapon>>
        {
            BOOSTED_RELOAD_SPEED,
            BOOSTED_ATTACK,
            BOOSTED_BULLET_SPEED,
            BOOSTED_BULLET_SIZE,
            BOOSTED_FIRERATE,
            BOOSTED_AMMO_CAPACITY,
            BOOSTED_RANGE
        };

        // Mise � jour des statistiques avec les boosts
        ComponantGBN.GBNScript.UpdateStatsWithBoost(baseStats, boostedStats);
    }

    /// <summary>
    /// Fetches the range of this weapon
    /// </summary>
    public float GetRange(bool getBoosted = true) => getBoosted ? this.BOOSTED_RANGE.Quantite : this.BASE_RANGE.Quantite;

    /// <summary>
    /// Fetches the bullet speed of this weapon
    /// </summary>
    public float GetBulletSpeed(bool getBoosted = true) => getBoosted ? this.BOOSTED_BULLET_SPEED.Quantite : this.BASE_BULLET_SPEED.Quantite;

    /// <summary>
    /// Fetches the bullet damage of this weapon
    /// </summary>
    public float GetDamage(bool getBoosted = true) => getBoosted ? this.BOOSTED_ATTACK.Quantite : this.BASE_ATTACK.Quantite;

    /// <inheritdoc/>
    public uint GetAmmoCapacity(bool getBoosted = true) => (uint)(getBoosted ? this.BOOSTED_AMMO_CAPACITY.Quantite : this.BASE_AMMO_CAPACITY.Quantite);

    #endregion

    #region Muzzle

    [Header("Muzzle")]
    [SerializeField]
    private Transform shootOrigin;

    private void PlaceBullet(Projectile bullet)
    {
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        // Assigner la position et la rotation de ZoneDeTire � la balle
        if (this.shootOrigin != null)
        {
            position = this.shootOrigin.position;
            rotation = this.shootOrigin.rotation;
        }

        bullet.transform.SetPositionAndRotation(position, rotation);
    }

    public void UpgradeEquipment() => throw new System.NotImplementedException();
    public int GetCostForUpgrade() => throw new System.NotImplementedException();
    public UpgradeStats GetStatToUpgradeAndCost() => throw new System.NotImplementedException();

    public UpgradeNameData AfterUpgradPreviewStat<T>(TypeQuantity<T> statToUpgrade) where T : Enum
    {
        return null;
    }


        #endregion

        #region Effects

    [Header("Effects")]
    [SerializeField] private ParticuleControleur ParticuleControleurs;

    #endregion
}
