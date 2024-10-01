using EntityModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField]
        GameObject boat;
         

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Death")
            {
                Debug.Log("respawn");
                this.transform.position = boat.transform.position + new Vector3(0,2,0);
                
            }
        }
    }
}

