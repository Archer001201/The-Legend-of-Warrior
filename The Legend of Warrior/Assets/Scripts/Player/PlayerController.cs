using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;
    [Header("Basic Parameter")] 
    public float speed;
    public float jumpForce;
  

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
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
        move();
    }

    public void move(){
        rb.velocity = new Vector2(inputDirection.x*speed*Time.deltaTime, rb.velocity.y);
        
        if (inputDirection.x > 0)
            spriteRenderer.flipX = false;
        if (inputDirection.x < 0)
            spriteRenderer.flipX = true;
    }

    private void Jump(InputAction.CallbackContext obj){
        rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
    }
}
