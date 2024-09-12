using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;
public class LanceFlame : MonoBehaviour, Equipement
{
    [SerializeField] ComponantGBN ComponantGBN;
    [SerializeField] TypeQuantite<TypeWeapon> ReloadSpeed = new(TypeWeapon.ReloadSpeed, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> Attack = new (TypeWeapon.attack, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> BulletSpeed = new (TypeWeapon.bulletspeed, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> BulletSize = new (TypeWeapon.bulletSize, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> FireRate = new (TypeWeapon.fireRate, 1f);



    [SerializeField] TypeQuantite<TypeWeapon> ReloadSpeedWithBoost = new(TypeWeapon.ReloadSpeed, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> AttackWithBoost = new(TypeWeapon.attack, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> BulletSpeedWithBoost = new(TypeWeapon.bulletspeed, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> BulletSizeWithBoost = new (TypeWeapon.bulletSize, 1f);
    [SerializeField] TypeQuantite<TypeWeapon> FireRateWithBoost = new(TypeWeapon.fireRate, 1f);
    public ComponantGBN ComposnantGBN
    {
        get { return ComponantGBN; }
    }




    public void UpdateStat()
    {
        // Création de la liste des statistiques de base
        List<TypeQuantite<TypeWeapon>> baseStats = new List<TypeQuantite<TypeWeapon>>
        {
            ReloadSpeed,
            Attack,
            BulletSpeed,
            BulletSize,
            FireRate
        };

        // Création de la liste des statistiques boostées
        List<TypeQuantite<TypeWeapon>> boostedStats = new List<TypeQuantite<TypeWeapon>>
        {
            ReloadSpeedWithBoost,
            AttackWithBoost,
            BulletSpeedWithBoost,
            BulletSizeWithBoost,
            FireRateWithBoost
        };

        // Mise à jour des statistiques avec les boosts
        ComponantGBN.GBNScript.UpdateStatsWithBoost(baseStats, boostedStats);
    }




    bool faireStat = true;


    // Start is called before the first frame update
    void Start()
    {
        if (faireStat)
        {
            faireStat = false;
            ComponantGBN.GBNScript.GetStat();
            UpdateStat();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
