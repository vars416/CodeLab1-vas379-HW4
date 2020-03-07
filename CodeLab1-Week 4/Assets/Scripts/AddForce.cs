using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    public float Thrust;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); //get rigidbody of gameobject
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * Thrust); //add force to forward axis
    }
}
