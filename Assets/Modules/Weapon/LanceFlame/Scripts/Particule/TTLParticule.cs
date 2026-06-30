using UnityEngine;

public class TTLParticule : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particule;

    // Start is called before the first frame update

    // Update is called once per frame
    private void Update()
    {
        if (!particule.GetComponent<ParticleSystem>().IsAlive())
            Destroy(gameObject);
    }
}