using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Embree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    //public Rigidbody rb;
    private bool isOnGround;
    private Vector2 input;

    private CharacterController characterController;

    private Vector3 direction;

    [SerializeField]
    private float speed;

    [SerializeField]private float smoothTime = 0.05f;
    private float currentVelocity;

    private float gravity = -9.81f;
    [SerializeField]private float gravityMultiplier = 3f;
    private float velocity;

    [SerializeField]
    private float jumpPower;
    private int nbJumps;
    [SerializeField] private int maxNbJumps;

    private int moveSpeed = 2;

    public InputActionReference move;

    private GameObject ground;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        //rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x,0,input.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(!context.started) return;
        if(!IsGrounded() && nbJumps >= maxNbJumps) return;
        if(nbJumps == 0) StartCoroutine(WaitForLanding());

        nbJumps++;
        velocity += jumpPower;
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(IsGrounded);

        nbJumps = 0;
    }
    // Update is called once per frame
    void Update()
    {
        
       
    }
    private void ApplyRotation()
    {
    if(input.sqrMagnitude == 0) return;
        var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Ground")
            isOnGround = true;
        
        Debug.Log("azerty");
    }

    private void OnCollisionExit(Collision other)
    {
        isOnGround = false;
        
    }

    void FixedUpdate()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
        
    }
    private void ApplyMovement()
    {
        characterController.Move(direction * speed * Time.deltaTime);
    }
    private void ApplyGravity()
    {
        if(IsGrounded() && velocity < 0.0f)
            velocity = -1;
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
            
        }
        direction.y = velocity;
    }

    private bool IsGrounded() => characterController.isGrounded;
}
