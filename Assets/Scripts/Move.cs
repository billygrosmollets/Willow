using System;
using UnityEditor.Embree;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Rigidbody rb;
    private bool isOnGround;

    private string nameCollide;

    private GameObject ground;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isOnGround)
                rb.AddForce(Vector3.up*800);
        }
        
                
        if (Input.GetKey(KeyCode.Z))
        {
            if(isOnGround) 
                rb.AddForce(Vector3.forward * 5);
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            if(isOnGround) 
                rb.AddForce(Vector3.left * 5);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if(isOnGround) 
                rb.AddForce(Vector3.back * 5);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if(isOnGround) 
                rb.AddForce(Vector3.right * 5);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
            isOnGround = true;
        
        Debug.Log("prout");
    }

    private void OnCollisionExit(Collision other)
    {
        isOnGround = false;
        
    }
}
