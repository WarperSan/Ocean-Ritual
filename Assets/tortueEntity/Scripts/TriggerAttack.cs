using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttack : MonoBehaviour
{
 [SerializeField]   TurtleBehaviorThree Three;


    private void OnTriggerEnter(Collider other)
    {
        Three.HitSomething();
    }

}
