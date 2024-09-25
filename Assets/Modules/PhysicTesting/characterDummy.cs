using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterDummy : MonoBehaviour
{
    public float Speed = 5f;
    Vector2 movementDirection;
    Vector2 movementRecorded;
    moving Moving;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), 0) + movementRecorded;
        //rb.AddForce(Vector2.up * jumpforce);

    }
    private void LateUpdate()
    {
        if (Moving != null)
        {
            movementRecorded = Moving.Movement;
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(movementDirection * Speed * Time.deltaTime);
        //rb.MovePosition(rb.position + movement * Speed * Time.deltaTime);

    }
    private void OnCollisionEnter(Collision collision)
    {
        Moving=  collision.gameObject.GetComponent<moving>();
        Debug.Log("Touched");
        Debug.Log(Moving.Movement.ToString());
    }
    
}
