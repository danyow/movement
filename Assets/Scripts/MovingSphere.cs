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
    float maxAcceleration = 10f;

    /// <summary>
    /// 可活动区域
    /// </summary>
    [SerializeField]
    Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);

    /// <summary>
    /// 弹力系数
    /// </summary>
    [SerializeField, Range(0f, 1f)]
    float bounciness = 0.5f;

    /// <summary>
    /// 当前速度
    /// </summary>
    Vector3 velocity;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        Vector2 playerInput;
        // playerInput.x = 0;
        // playerInput.y = 0;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        // playerInput.Normalize();
        // ClampMagnitude --> 使得在一个半径为1的圆内随意走动
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        // transform.localPosition = new Vector3(playerInput.x, 0.5f, playerInput.y);

        // displacement-> 位移增量
        // Vector3 displacement = new Vector3(playerInput.x, 0f, playerInput.y);
        // transform.localPosition += displacement;

        // Vector3 velocity = new Vector3(playerInput.x, 0f, playerInput.y);
        // Vector3 displacement = velocity * Time.deltaTime;
        // transform.localPosition += displacement;

        // Vector3 velocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        // Vector3 displacement = velocity * Time.deltaTime;
        // transform.localPosition += displacement;

        // acceleration 加入了加速度之后 更加的平滑
        // Vector3 acceleration = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        // velocity += acceleration * Time.deltaTime;
        // Vector3 displacement = velocity * Time.deltaTime;
        // transform.localPosition += displacement;

        // desiredVelocity --> 期望速度
        Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        // if (velocity.x < desiredVelocity.x) {
        //     // 这行代码会导致增量过大
        //     // velocity.x += maxSpeedChange;
        //     velocity.x = Mathf.Min(velocity.x + maxSpeedChange, desiredVelocity.x);
        // } else if (velocity.x > desiredVelocity.x) {
        //     velocity.x = Mathf.Max(velocity.x - maxSpeedChange, desiredVelocity.x);
        // }
        // MoveTowards的含义与上面的if elseif 一致
        // Moves a value current towards target.(将当前值移向目标。)
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        Vector3 displacement = velocity * Time.deltaTime;
        // transform.localPosition += displacement;
        Vector3 newPosition = transform.localPosition + displacement;
        // if (!allowedArea.Contains(newPosition)) {
        //     newPosition = transform.localPosition;
        // }
        // 上面错误 因为y值不对
        // if (!allowedArea.Contains(new Vector3(newPosition.x, newPosition.z))) {
        //     // newPosition = transform.localPosition;
        //     // 使用Clamp 来限制 上面那行代码会导致粘住不动
        //     newPosition.x = Mathf.Clamp(newPosition.x, allowedArea.xMin, allowedArea.xMax);
        //     newPosition.z = Mathf.Clamp(newPosition.z, allowedArea.yMin, allowedArea.yMax);
        // }
        // 上面的if 会导致加速度的方向继续朝着边界 需要消除对应方向的加速度
        if (newPosition.x < allowedArea.xMin) {
            newPosition.x = allowedArea.xMin;
            // velocity.x = 0f;
            // velocity.x = -velocity.x;
            velocity.x = -velocity.x * bounciness;
        } else if (newPosition.x > allowedArea.xMax) {
            newPosition.x = allowedArea.xMax;
            // velocity.x = 0f;
            // velocity.x = -velocity.x;
            velocity.x = -velocity.x * bounciness;
        }
        if (newPosition.z < allowedArea.yMin) {
            newPosition.z = allowedArea.yMin;
            // velocity.z = 0f;
            // velocity.z = -velocity.z;
            velocity.z = -velocity.z * bounciness;
        } else if (newPosition.z > allowedArea.yMax) {
            newPosition.z = allowedArea.yMax;
            // velocity.z = 0f;
            // velocity.z = -velocity.z;
            velocity.z = -velocity.z * bounciness;
        }
        transform.localPosition = newPosition;
    }
}