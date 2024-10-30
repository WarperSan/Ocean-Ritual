using BlacksmithModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public class CannonStats : MonoBehaviour, Equipment
{
    #region IEquipement

    [Header("IEquipement")]
    [SerializeField] private TypeQuantity<TypeWeapon> BASE_RELOAD_SPEED = new(TypeWeapon.ReloadSpeed, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BASE_ATTACK = new(TypeWeapon.attack, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BASE_BULLET_SPEED = new(TypeWeapon.bulletspeed, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BASE_BULLET_SIZE = new(TypeWeapon.bulletSize, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BASE_FIRERATE = new(TypeWeapon.fireRate, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BASE_AMMO_CAPACITY = new(TypeWeapon.AmmoCapacity, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BASE_RANGE = new(TypeWeapon.Range, 1f);

    [SerializeField] private TypeQuantity<TypeWeapon> BOOSTED_RELOAD_SPEED = new(TypeWeapon.ReloadSpeed, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BOOSTED_ATTACK = new(TypeWeapon.attack, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BOOSTED_BULLET_SPEED = new(TypeWeapon.bulletspeed, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BOOSTED_BULLET_SIZE = new(TypeWeapon.bulletSize, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BOOSTED_FIRERATE = new(TypeWeapon.fireRate, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BOOSTED_AMMO_CAPACITY = new(TypeWeapon.AmmoCapacity, 1f);
    [SerializeField] private TypeQuantity<TypeWeapon> BOOSTED_RANGE = new(TypeWeapon.Range, 1f);

    [SerializeField] private componentGBN ComponantGBN;
    public componentGBN componentGBN => ComponantGBN;

    

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

    #region Upgrades
    
    [SerializeField] public string Name;

    string Equipment.Name
    {
        get { return Name; }
    }
    
    [SerializeField]
    private int forgePercentage;

    public int ForgePercentage
    {
        get => forgePercentage;
        set => forgePercentage = value;
    }

    [SerializeField]
    private int lvlOfEquipment;

    public int LvlOfEquipment
    {
        get => lvlOfEquipment;
        set => lvlOfEquipment = value;
    }

    [SerializeField]
    private int costToUpgrade;

    public int CostToUpgrade
    {
        get => costToUpgrade;
        set => costToUpgrade = value;
    }

    public UpgradeStats GetStatToUpgradeAndCost()
    {
        // Crée des listes pour les statistiques de base et d'aperçu
        var baseStats = new List<UpgradeNameData>
        {
            new((int)BASE_RELOAD_SPEED.Quantite, BASE_RELOAD_SPEED.Type.ToString()),
            new ((int)BASE_ATTACK.Quantite, BASE_ATTACK.Type.ToString()),
            new ((int)BASE_BULLET_SPEED.Quantite, BASE_BULLET_SPEED.Type.ToString()),
            new ((int)BASE_BULLET_SIZE.Quantite, BASE_BULLET_SIZE.Type.ToString()),
            new ((int)BASE_FIRERATE.Quantite, BASE_FIRERATE.Type.ToString()),
            new ((int)BASE_AMMO_CAPACITY.Quantite, BASE_AMMO_CAPACITY.Type.ToString()),
            new ((int)BASE_RANGE.Quantite, BASE_RANGE.Type.ToString())
        };


        var previewStats = new List<UpgradeNameData>
        {
            AfterUpgradPreviewStat(BASE_RELOAD_SPEED),
            AfterUpgradPreviewStat(BASE_ATTACK),
            AfterUpgradPreviewStat(BASE_BULLET_SPEED),
            AfterUpgradPreviewStat(BASE_BULLET_SIZE),
            AfterUpgradPreviewStat(BASE_FIRERATE),
            AfterUpgradPreviewStat(BASE_AMMO_CAPACITY),
            AfterUpgradPreviewStat(BASE_RANGE)
        };

        return new UpgradeStats(baseStats, previewStats, GetCostForUpgrade(), Name);
    }

    // Mise à jour de la méthode AfterUpgradPreviewStat pour retourner un UpgradeNameData
    public UpgradeNameData AfterUpgradPreviewStat<T>(TypeQuantity<T> statToUpgrade) where T: Enum
    {
        float newValue = statToUpgrade.Quantite * (1 + ForgePercentage / 100f);
        int roundedValue = Mathf.CeilToInt(newValue);

        return new UpgradeNameData(roundedValue, statToUpgrade.Type.ToString());
    }

    public void UpgradeEquipment()
    {
        BASE_RELOAD_SPEED.Quantite = AfterUpgradPreviewStat(BASE_RELOAD_SPEED).quantity;
        BASE_ATTACK.Quantite = AfterUpgradPreviewStat(BASE_ATTACK).quantity;
        BASE_BULLET_SPEED.Quantite = AfterUpgradPreviewStat(BASE_BULLET_SPEED).quantity;
        BASE_BULLET_SIZE.Quantite = AfterUpgradPreviewStat(BASE_BULLET_SIZE).quantity;
        BASE_FIRERATE.Quantite = AfterUpgradPreviewStat(BASE_FIRERATE).quantity;
        BASE_AMMO_CAPACITY.Quantite = AfterUpgradPreviewStat(BASE_AMMO_CAPACITY).quantity;
        BASE_RANGE.Quantite = AfterUpgradPreviewStat(BASE_RANGE).quantity;
        LvlOfEquipment++;
    }

    public int GetCostForUpgrade()
    {
        return LvlOfEquipment * costToUpgrade;
    }
    #endregion
}
