using EntityModule.Entities;
using UnityEngine;

namespace BossesModule.Golem
{
    [RequireComponent(typeof(GolemTree))]
    public class GolemEntity : BossEntity
    {
        public GameObject model;

        protected override void OnDeath(float overDamage)
        {
            model.SetActive(false);

            base.OnDeath(overDamage);
        }
    }
}