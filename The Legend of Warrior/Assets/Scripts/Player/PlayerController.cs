using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PhysicsCheck physicsCheck;
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;
    [Header("Basic Parameter")] 
    public float speed;
    public float jumpForce;
  

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
        physicsCheck = GetComponent<PhysicsCheck>();
        inputControl.Gameplay.Jump.started += Jump;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable(){
        inputControl.Enable();
    }

    private void OnDisable() {
        inputControl.Disable();
    }

    private void Update() {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate() {
        Move();
    }

    public void Move(){
        rb.velocity = new Vector2(inputDirection.x*speed*Time.deltaTime, rb.velocity.y);
        
        if (inputDirection.x > 0)
            spriteRenderer.flipX = false;
        if (inputDirection.x < 0)
            spriteRenderer.flipX = true;
    }

    private void Jump(InputAction.CallbackContext obj){
        if (physicsCheck.isGround)
            rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
    }
}
