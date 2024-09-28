using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public class EquipmentTest : Equipment
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

    // Propriété de l'interface
    public int ForgePercentage
    {
        get => forgePercentage; 
        set => forgePercentage = value; 
    }
    [SerializeField]
    private int lvlOfEquipment; 

    // Propriété de l'interface
    public int LvlOfEquipment
    {
        get => lvlOfEquipment; 
        set => lvlOfEquipment = value;
    }
    [SerializeField]
    private int costToUpgrade;

    // Propriété de l'interface
    public int CostToUpgrade
    {
        get => costToUpgrade;
        set => costToUpgrade = value;
    }
    public (List<TypeQuantity<TypeWeapon>> baseStats, List<TypeQuantity<TypeWeapon>> previewStats, int upgradeCost) GetStatToUpgradeAndCost()

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
        List<TypeQuantity<TypeWeapon>> PreviewStat = new List<TypeQuantity<TypeWeapon>>
        {
            AfterUpgradPreviewStat(ReloadSpeed),
            AfterUpgradPreviewStat(Attack),
            AfterUpgradPreviewStat(BulletSpeed),
            AfterUpgradPreviewStat(BulletSize),
            AfterUpgradPreviewStat(FireRate),
            AfterUpgradPreviewStat(AmmoCapacity),
            AfterUpgradPreviewStat(Range)
        };
        return (baseStats, PreviewStat, GetCostForUpgrade());
    }
    public TypeQuantity<TypeWeapon> AfterUpgradPreviewStat(TypeQuantity<TypeWeapon> statToUpgrade)
    {
        // Appliquer le pourcentage de forgeage
        float newValue = statToUpgrade.Quantite * (1 + ForgePercentage / 100f); // Calculer la nouvelle valeur
        int roundedValue = Mathf.CeilToInt(newValue); // Arrondir à l'entier supérieur

        // Retourner un nouveau TypeQuantity avec la valeur mise à jour
        return new TypeQuantity<TypeWeapon>(statToUpgrade.Type, roundedValue);
    }
    public void UpdateStat()
    {
     // equipement with gem 
    }
    public void UpgradeEquipment()
    {
        // Améliorer les statistiques de base
        ReloadSpeed = AfterUpgradPreviewStat(ReloadSpeed);
        Attack = AfterUpgradPreviewStat(Attack);
        BulletSpeed = AfterUpgradPreviewStat(BulletSpeed);
        BulletSize = AfterUpgradPreviewStat(BulletSize);
        FireRate = AfterUpgradPreviewStat(FireRate);
        AmmoCapacity = AfterUpgradPreviewStat(AmmoCapacity);
        Range = AfterUpgradPreviewStat(Range);

        // Incrémenter le niveau de l'équipement
        LvlOfEquipment++;
    }
    public int GetCostForUpgrade()
    {
        return LvlOfEquipment * costToUpgrade;
    }
    //
}
