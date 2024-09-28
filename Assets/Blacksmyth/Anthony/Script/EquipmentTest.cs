using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DhafinFawwaz.AnimationUILib.Sequence;
using static EnumGeneral;

public class EquipmentTest : MonoBehaviour,Equipment
{
    [SerializeField] TypeQuantity<TypeWeapon> ReloadSpeed = new(TypeWeapon.ReloadSpeed, 1f);
    [SerializeField] TypeQuantity<TypeWeapon> Attack = new(TypeWeapon.attack, 1f);
    [SerializeField] public TypeQuantity<TypeWeapon> BulletSpeed = new(TypeWeapon.bulletspeed, 1f);
    [SerializeField] TypeQuantity<TypeWeapon> BulletSize = new(TypeWeapon.bulletSize, 1f);
    [SerializeField] TypeQuantity<TypeWeapon> FireRate = new(TypeWeapon.fireRate, 1f);
    [SerializeField] TypeQuantity<TypeWeapon> AmmoCapacity = new(TypeWeapon.AmmoCapacity, 1f);
    [SerializeField] public TypeQuantity<TypeWeapon> Range = new(TypeWeapon.Range, 1f);
    public componentGBN componentGBN => throw new System.NotImplementedException();

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
        List<UpgradeNameData> baseStats = new List<UpgradeNameData>
    {
        new((int)ReloadSpeed.Quantite, ReloadSpeed.Type.ToString()),
        new ((int)Attack.Quantite, Attack.Type.ToString()),
        new ((int)BulletSpeed.Quantite, BulletSpeed.Type.ToString()),
        new ((int)BulletSize.Quantite, BulletSize.Type.ToString()),
        new ((int)FireRate.Quantite, FireRate.Type.ToString()),
        new ((int)AmmoCapacity.Quantite, AmmoCapacity.Type.ToString()),
        new ((int)Range.Quantite, Range.Type.ToString())
    };


        List<UpgradeNameData> previewStats = new List<UpgradeNameData>
    {
        AfterUpgradPreviewStat(ReloadSpeed),
        AfterUpgradPreviewStat(Attack),
        AfterUpgradPreviewStat(BulletSpeed),
        AfterUpgradPreviewStat(BulletSize),
        AfterUpgradPreviewStat(FireRate),
        AfterUpgradPreviewStat(AmmoCapacity),
        AfterUpgradPreviewStat(Range)
    };

        return new UpgradeStats(baseStats, previewStats, GetCostForUpgrade());
    }

    // Mise à jour de la méthode AfterUpgradPreviewStat pour retourner un UpgradeNameData
    public UpgradeNameData AfterUpgradPreviewStat(TypeQuantity<TypeWeapon> statToUpgrade)
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

    public int GetCostForUpgrade()
    {
        return LvlOfEquipment * costToUpgrade;
    }

    public void UpdateStat() => throw new System.NotImplementedException();
   
    public TypeQuantity<Enum> AfterUpgradPreviewStat(TypeQuantity<Enum> statToUpgrade) => throw new NotImplementedException();
}

