using System;

namespace BlacksmithModule
{
    public interface IForgeable
    {
        int          CostToUpgrade   { get; set; }
        int          ForgePercentage { get; set; }
        int          LvlOfEquipment  { get; set; }
        componentGBN componentGBN    { get; }

        void         UpgradeEquipment();
        int          GetCostForUpgrade();
        UpgradeStats GetStatToUpgradeAndCost();

        UpgradeNameData AfterUpgradPreviewStat<T>(TypeQuantity<T> statToUpgrade) where T : Enum;
    }
}