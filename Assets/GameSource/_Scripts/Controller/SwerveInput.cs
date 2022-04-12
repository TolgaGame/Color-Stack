using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SwerveInput : MonoBehaviour
{
    private CharacterController characterController;

    private float lastFrameFingerPositionX;
    private float moveInX;
    private Vector3 swerveAmount;
    public float _moveSpeed;

    [Space(10)]
    [SerializeField] private bool isRotate;

    [Space(10)]
    [SerializeField] private bool isClampX;
    [SerializeField] private float clampingBoundaryInX;

    [Header("Speed Components")]
    public float speedInXAxis;
    [SerializeField] private float speedInYAxis;
    [SerializeField] private float speedInZAxis;

    [Space(10)]
    [SerializeField] private bool applyGravity;
    private float gravity = 9.8f;
    private float verticalSpeed;

    // ============================

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (GameManager.Instance.isGameStarted == true)
        {
            characterController.Move(transform.forward * Time.deltaTime * _moveSpeed);

            SwerveHandler();

            characterController.Move(new Vector3(SwerveAmount() * speedInXAxis * Time.fixedDeltaTime, // X movement jitters in deltatime
                                        verticalSpeed * Time.deltaTime,
                                        speedInZAxis * Time.deltaTime));

            if (isRotate)
            {
                if (SwerveAmount() != 0)
                    RotateToFinger();
                else
                    RotateToZero();
            }

            if (isClampX)
                ClampPosition();

            if (applyGravity)
                ApplyGravity();
        }
    }

    // ============================

    private void SwerveHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastFrameFingerPositionX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            moveInX = Input.mousePosition.x - lastFrameFingerPositionX;
            lastFrameFingerPositionX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            moveInX = 0f;
            
        }

        swerveAmount.x = moveInX;
    }

    public float SwerveAmount()
    {
        return swerveAmount.x;
    }

    private void RotateToFinger()
    {
        var targetPos = new Vector3(SwerveAmount(), transform.position.y, transform.position.z + 1);

        Quaternion rotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);
    }

    private void RotateToZero()
    {
        var targetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);

        Quaternion rotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);
    }

    private void ClampPosition()
    {
        var x = Mathf.Clamp(transform.position.x, -clampingBoundaryInX, clampingBoundaryInX);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
            verticalSpeed -= gravity * Time.fixedDeltaTime;
        else
            verticalSpeed = speedInYAxis;
    }

}