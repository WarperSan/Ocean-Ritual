using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();   
    }

    public Vector3 Movement { get; private set; }

    Vector3 lastPosition;

    void Update()
    {
        Vector3 position = transform.position;

        Movement = position - lastPosition;

        lastPosition = position;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Vector3.right*Time.fixedDeltaTime);
    }
}
