using EntityModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public class BoatStats : MonoBehaviour, Equipment
{
    [SerializeField] private componentGBN ComponantGBN;
    public componentGBN componentGBN => ComponantGBN;
    // Start is called before the first frame update
    void Start()
    {
        this.UpdateStat();
    }

    

    #region Stats

    [SerializeField] private TypeQuantity<TypeBoat> BASE_LIFE = new(TypeBoat.Life, 1f);
    [SerializeField] private TypeQuantity<TypeBoat> BASE_SPEED = new(TypeBoat.navigateSpeed, 4f);
    [SerializeField] private TypeQuantity<TypeBoat> BASE_HANDLING = new(TypeBoat.Handling, 10f);

    [SerializeField] private TypeQuantity<TypeBoat> BOOST_LIFE = new(TypeBoat.Life, 1f);
    [SerializeField] private TypeQuantity<TypeBoat> BOOST_SPEED = new(TypeBoat.navigateSpeed, 4f);
    [SerializeField] private TypeQuantity<TypeBoat> BOOST_HANDLING = new(TypeBoat.Handling, 10f);

    

    public void UpdateStat()
    {
        var baseStats = new List<TypeQuantity<TypeBoat>>
        {
            BASE_LIFE,
            BASE_SPEED, BASE_HANDLING
        };

        // Cr�ation de la liste des statistiques boost�es
        var boostedStats = new List<TypeQuantity<TypeBoat>>
        {
            BOOST_LIFE, BOOST_SPEED, BOOST_HANDLING
        };

        // Mise � jour des statistiques avec les boosts
        ComponantGBN.GBNScript.UpdateStatsWithBoost(baseStats, boostedStats);
    }

    public float GetLife(bool getBoosted = true) => getBoosted ? this.BOOST_LIFE.Quantite : this.BASE_LIFE.Quantite;
    public float GetSpeed(bool getBoosted = true) => getBoosted ? this.BOOST_SPEED.Quantite : this.BASE_SPEED.Quantite;
    public float GetHandling(bool getBoosted = true) => getBoosted ? this.BOOST_HANDLING.Quantite : this.BASE_HANDLING.Quantite;

    #endregion

    #region Upgrades
    public string Name { get; }
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


    public void UpgradeEquipment()
    {
        // à implémenter une fois IEquipment a été fix

        //BASE_LIFE.Quantite = AfterUpgradPreviewStat(BASE_LIFE).quantity;
        //BASE_SPEED.Quantite = AfterUpgradPreviewStat(BASE_SPEED).quantity;
        //BASE_HANDLING.Quantite = AfterUpgradPreviewStat(BASE_HANDLING).quantity;
        LvlOfEquipment++;
    }
    public int GetCostForUpgrade()
    {
        return LvlOfEquipment * costToUpgrade;
        
    }
    public UpgradeStats GetStatToUpgradeAndCost()
    {
        var baseStats = new List<UpgradeNameData>
        {
            new((int)BASE_LIFE.Quantite, BASE_LIFE.Type.ToString()),
            new ((int)BASE_SPEED.Quantite, BASE_SPEED.Type.ToString()),
            new ((int)BASE_HANDLING.Quantite, BASE_HANDLING.Type.ToString()),
            
        };


        var previewStats = new List<UpgradeNameData>
        {
            // à implémenter une fois IEquipment a été fix

            //AfterUpgradPreviewStat(BASE_LIFE),
            //AfterUpgradPreviewStat(BASE_SPEED),
            //AfterUpgradPreviewStat(BASE_HANDLING),

        };

        return new UpgradeStats(baseStats, previewStats, GetCostForUpgrade(), Name);
    }

    public UpgradeNameData AfterUpgradPreviewStat(TypeQuantity<TypeWeapon> statToUpgrade )
    {
        // à implémenter une fois IEquipment a été fix
        throw new NotImplementedException();
    }

    #endregion
}
