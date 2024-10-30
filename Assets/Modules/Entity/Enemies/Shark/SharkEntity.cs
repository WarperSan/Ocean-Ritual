using EntityModule.Entities;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;


namespace EntityModule.Enemies
{
    public class SharkEntity : EntityBehaviour
    {
        #region EntityBehaviour
        [SerializeField] UnityEngine.Sprite sprite;
        [SerializeField] float chance = 100;
        [SerializeField] int min = 1;
        [SerializeField] int max = 4;
        
        protected override void OnStart()
        {
            base.OnStart();
        }

        
          protected override void OnDeath(float overDamage)
        {
            (bool, int) value = GeneratorGem.randomlvl(min,max,chance);
            if (value.Item1)
            {
                GemData TheGemme = GeneratorGem.GenerateRandomGemme(value.Item2);
                TheGemme.sprite = sprite;
                Inventory.Instance.AddItem(TheGemme);

            }
            
            this.gameObject.SetActive(false);
        }


        //private void OnTriggerEnter(Collider other)
        //{
        //    UnityEngine.Debug.Log("touched");
        //    if (other.tag == "Player")
        //    {
        //        UnityEngine.Debug.Log("Hit Player");
        //    }
        //}
        #endregion
    }
}

