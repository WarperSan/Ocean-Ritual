using Unity.Collections;
using UnityEngine;

public class PhysicsContacts : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject boat;

    private void Start()
    {
        Collider[] playerCollider = player.GetComponentsInChildren<Collider>();
        Collider[] boatCollider = boat.GetComponentsInChildren<Collider>();

        if (playerCollider != null)
        {
            foreach (Collider c in playerCollider)
                c.hasModifiableContacts = true;
        }

        if (boatCollider != null)
        {
            foreach (Collider c in boatCollider)
                c.hasModifiableContacts = true;
        }

        Physics.ContactModifyEvent += OnContactModification;
    }

    private void OnContactModification(PhysicsScene scene, NativeArray<ModifiableContactPair> pairs)
    {
        foreach (ModifiableContactPair pair in pairs)
            Debug.Log("callback");
    }
}