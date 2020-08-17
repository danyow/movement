using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour {
    /// <summary>
    /// 最大速度
    /// </summary>
    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;

    /// <summary>
    /// 最大加速度
    /// </summary>
    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10f, maxAirAcceleration = 1f;

    /// <summary>
    /// 当前速度
    /// </summary>
    Vector3 velocity, desiredVelocity;

    /// <summary>
    /// 刚体
    /// </summary>
    Rigidbody body;

    /// <summary>
    /// 期望跳跃？
    /// </summary>
    bool desiredJump;

    [SerializeField, Range(0f, 10f)]
    float jumpHeight = 2f;

    bool onGround;

    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;

    int jumpPhase;

    void Awake() {
        body = GetComponent<Rigidbody>();
    }

    void Start () {
        
    }

    void Update () {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        // Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        // velocity = body.velocity;
        // float maxSpeedChange = maxAcceleration * Time.deltaTime;
        // velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        // velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        // body.velocity = velocity;
        desiredJump = Input.GetButtonDown("Jump");
    }

    void FixedUpdate() {
        // velocity = body.velocity;
        UpdateState();
        float accleration = onGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = accleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        // 位或运算
        desiredJump |= Input.GetButtonDown("Jump");
        if (desiredJump) {
            desiredJump = false;
            Jump();
        }
        body.velocity = velocity;
        onGround = false;
    }

    void UpdateState() {
        velocity = body.velocity;
        if (onGround) {
            jumpPhase = 0;
        }
    }

    void Jump() {
        // velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        // if (onGround) {
        if (onGround || jumpPhase < maxAirJumps) {
            jumpPhase += 1;
            // velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            if (velocity.y > 0f) {
                // jumpSpeed = jumpSpeed - velocity.y;
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        // onGround = true;
        EvaluateCollision(collision);
    }

    // private void OnCollisionExit(Collision collision) {
    //     onGround = false;
    // }

    private void OnCollisionStay(Collision collision) {
        // onGround = true;
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision collision) {
        for (int i = 0; i < collision.contactCount; i++) {
            Vector3 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= 0.9f;
        }
    }
}