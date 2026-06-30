using BlacksmithModule;
using System;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public class EquipmentTest : MonoBehaviour, Equipment
{
    [SerializeField]
    private TypeQuantity<TypeWeapon> ReloadSpeed = new(TypeWeapon.VitesseRechargement, 1f);

    [SerializeField]
    private TypeQuantity<TypeWeapon> Attack = new(TypeWeapon.Attaque, 1f);

    [SerializeField]
    public TypeQuantity<TypeWeapon> BulletSpeed = new(TypeWeapon.VitesseBalle, 1f);

    [SerializeField]
    private TypeQuantity<TypeWeapon> BulletSize = new(TypeWeapon.TailleDeBalle, 1f);

    [SerializeField]
    private TypeQuantity<TypeWeapon> FireRate = new(TypeWeapon.VitesseDeTire, 1f);

    [SerializeField]
    private TypeQuantity<TypeWeapon> AmmoCapacity = new(TypeWeapon.CapaciterDeBall, 1f);

    [SerializeField]
    public TypeQuantity<TypeWeapon> Range = new(TypeWeapon.Porter, 1f);

    [SerializeField]
    public componentGBN componentGBN;

    [SerializeField]
    public string Name;

    string Equipment.Name => Name;

    componentGBN IForgeable.componentGBN => componentGBN;

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
            new((int)ReloadSpeed.Quantite, ReloadSpeed.Type.ToString()),
            new((int)Attack.Quantite, Attack.Type.ToString()),
            new((int)BulletSpeed.Quantite, BulletSpeed.Type.ToString()),
            new((int)BulletSize.Quantite, BulletSize.Type.ToString()),
            new((int)FireRate.Quantite, FireRate.Type.ToString()),
            new((int)AmmoCapacity.Quantite, AmmoCapacity.Type.ToString()),
            new((int)Range.Quantite, Range.Type.ToString()),
        };

        var previewStats = new List<UpgradeNameData>
        {
            AfterUpgradPreviewStat(ReloadSpeed),
            AfterUpgradPreviewStat(Attack),
            AfterUpgradPreviewStat(BulletSpeed),
            AfterUpgradPreviewStat(BulletSize),
            AfterUpgradPreviewStat(FireRate),
            AfterUpgradPreviewStat(AmmoCapacity),
            AfterUpgradPreviewStat(Range),
        };

        return new UpgradeStats(baseStats,
            previewStats,
            GetCostForUpgrade(),
            Name);
    }

    // Mise à jour de la méthode AfterUpgradPreviewStat pour retourner un UpgradeNameData
    public UpgradeNameData AfterUpgradPreviewStat<T>(TypeQuantity<T> statToUpgrade) where T : Enum
    {
        float newValue = statToUpgrade.Quantite * (1 + ForgePercentage / 100f);
        int roundedValue = Mathf.CeilToInt(newValue);

        return new UpgradeNameData(roundedValue, statToUpgrade.Type.ToString());
    }

    public void UpgradeEquipment()
    {
        ReloadSpeed.Quantite = AfterUpgradPreviewStat(ReloadSpeed).quantity;
        Attack.Quantite = AfterUpgradPreviewStat(Attack).quantity;
        BulletSpeed.Quantite = AfterUpgradPreviewStat(BulletSpeed).quantity;
        BulletSize.Quantite = AfterUpgradPreviewStat(BulletSize).quantity;
        FireRate.Quantite = AfterUpgradPreviewStat(FireRate).quantity;
        AmmoCapacity.Quantite = AfterUpgradPreviewStat(AmmoCapacity).quantity;
        Range.Quantite = AfterUpgradPreviewStat(Range).quantity;
        LvlOfEquipment++;
    }

    public int GetCostForUpgrade() => LvlOfEquipment * costToUpgrade;

    public void UpdateStat() => throw new NotImplementedException();

    public TypeQuantity<Enum> AfterUpgradPreviewStat(TypeQuantity<Enum> statToUpgrade) => throw new NotImplementedException();
}