using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public interface Forgeable
{
    int CostToUpgrade { get; set; }
    int ForgePercentage { get; set; }
    int LvlOfEquipment { get; set; }
    componentGBN componentGBN { get; }
    public void UpgradeEquipment();
    public int GetCostForUpgrade();
    public UpgradeStats GetStatToUpgradeAndCost();

    public UpgradeNameData AfterUpgradPreviewStat(TypeQuantity<TypeWeapon> statToUpgrade);
}
