using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class standing : MonoBehaviour
{
    public Vector3 Movement { get; private set; }

    Vector3 lastPosition;

    void Update()
    {
        Vector3 position = transform.position;

        Movement = position - lastPosition;

        lastPosition = position;
    }
}
