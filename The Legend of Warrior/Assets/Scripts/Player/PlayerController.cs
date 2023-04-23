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
    private CapsuleCollider2D coll;

    [Header("基本参数")] 
    public float speed;
    private float runSpeed;
    private float walkSpeed => speed / 2.5f;
    public float jumpForce;
    public bool isCrouch;
    private Vector2 originalOffset;
    private Vector2 originalSize;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<CapsuleCollider2D>();

        originalOffset = coll.offset;
        originalSize = coll.size;

        inputControl = new PlayerInputControl();
        inputControl.Gameplay.Jump.started += Jump;


        #region 强制走路
        runSpeed = speed;
        inputControl.Gameplay.WalkButton.performed += ctx =>{
            if (physicsCheck.isGround)
                speed = walkSpeed;
        };

        inputControl.Gameplay.WalkButton.canceled += ctx =>{
            if (physicsCheck.isGround)
                speed = runSpeed;
        };
        #endregion;
    }

    private void OnEnable() {
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

    public void Move() {
        if (!isCrouch)
            rb.velocity = new Vector2(inputDirection.x*speed*Time.deltaTime, rb.velocity.y);
        
        float faceDir = transform.localScale.x;
        if (inputDirection.x > 0 && faceDir < 0)
            faceDir *= -1;
            //spriteRenderer.flipX = false;
        if (inputDirection.x < 0 && faceDir > 0)
            faceDir *= -1;
            //spriteRenderer.flipX = true;

            transform.localScale = new Vector3(faceDir,1,1);

        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;
        if (isCrouch){
            coll.offset = new Vector2(-0.12f, 0.8f);
            coll.size = new Vector2(0.75f, 1.6f);
        }else{
            coll.offset = originalOffset;
            coll.size = originalSize;
        }
    }

    private void Jump(InputAction.CallbackContext obj) {
        if (physicsCheck.isGround)
            rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
    }
}
