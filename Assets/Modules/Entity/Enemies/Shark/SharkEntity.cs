using EntityModule.Entities;
using UnityEngine;

namespace EntityModule.Enemies
{
    public class SharkEntity : EntityBehaviour
    {
        #region EntityBehaviour

        [SerializeField]
        private Sprite sprite;

        [SerializeField]
        private float chance = 100;

        [SerializeField]
        private int min = 1;

        [SerializeField]
        private int max = 4;

        protected override void OnStart() => base.OnStart();

        protected override void OnDeath(float overDamage)
        {
            (bool, int) value = GeneratorGem.randomlvl(min, max, chance);

            if (value.Item1)
            {
                GemData TheGemme = GeneratorGem.GenerateRandomGemme(value.Item2);
                TheGemme.sprite = sprite;
                Inventory.Instance.AddItem(TheGemme);
            }

            gameObject.SetActive(false);
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