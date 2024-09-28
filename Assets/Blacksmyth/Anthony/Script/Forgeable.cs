using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public interface  Forgeable
{
    int CostToUpgrade { get; set; }
    int ForgePercentage { get; set; }
    int LvlOfEquipment { get; set; }
    public void UpgradeEquipment();
    public int GetCostForUpgrade();

    public (List<TypeQuantity<TypeWeapon>> baseStats, List<TypeQuantity<TypeWeapon>> previewStats, int upgradeCost) GetStatToUpgradeAndCost();

    public TypeQuantity<TypeWeapon> AfterUpgradPreviewStat(TypeQuantity<TypeWeapon> statToUpgrade);
    



}
