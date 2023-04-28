using System;
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
    private PlayerAnimation playerAnimation;

    [Header("基本参数")] 
    public float speed;
    private float runSpeed;
    private float walkSpeed => speed / 2.5f;
    public float jumpForce;
    public float hurtForce;
    private Vector2 originalOffset;
    private Vector2 originalSize;

    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    [Header("状态")]
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool isDefence;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();

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

        inputControl.Gameplay.Attack.started += PlayerAttack;
        
        inputControl.Gameplay.Defence.performed += ctx =>{
            isDefence = true;
        };
        inputControl.Gameplay.Defence.canceled += ctx =>{
            isDefence = false;
        };
    }

    private void OnEnable() {
        inputControl.Enable();
    }

    private void OnDisable() {
        inputControl.Disable();
    }

    private void Update() {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        CheckState();
    }

    private void FixedUpdate() {
        if (!isHurt && !isAttack && !isDefence)
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

    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        playerAnimation.PlayAttack();
        isAttack = true;
    }

    public void GetHurt(Transform attacker){
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x-attacker.transform.position.x),0).normalized;

        rb.AddForce(dir*hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead(){
        isDead = true;
        inputControl.Gameplay.Disable();
    }

    private void CheckState()
    {
        if (isDead)
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        else
            gameObject.layer = LayerMask.NameToLayer("Player");

        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }
}
