using EntityModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityModule
{
    public class PlayerEntity : Entity
    {
        protected override void OnPostAttack()
        {
            //knockback?
        }
        protected override void OnDeath(float overDamage)
        {
            //tue le joueur

            //soit afficher un menu game over
            //soit faire respawn le joueur direct après un certain temps
        }
    }
}

