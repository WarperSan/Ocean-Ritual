using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTLParticule : MonoBehaviour
{
    [SerializeField] ParticleSystem particule;
   


    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (!particule.GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
