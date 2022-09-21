using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Movement Variables")]
    [SerializeField] [Range(0.0f, 1.0f)] private float _moveSmoothTime;
    [SerializeField] private float _walkSpeed = 10.0f;
    [SerializeField] private float _jumpHeight = 4.0f;
    [SerializeField] private CharacterController _controller;
    private Vector2 _currentDir = Vector2.zero;
    private Vector2 _currentDirVelocity = Vector2.zero;

    [Header("Mouse Variables")]
    [SerializeField] [Range(0.0f, 1.0f)] private float _mouseSmoothTime;
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private float _mouseSensitivity = 4.0f;
    [SerializeField] private bool _lockCursor = true;
    private Vector2 _currentMouseDelta = Vector2.zero;
    private Vector2 _currentMouseDeltaVelocity = Vector2.zero;
    private float _cameraPitch = 0.0f;

    [Header("Gravity Variables")]
    [SerializeField] private float _gravity = -13.0f;
    [SerializeField] private float _velocityY = 0.0f;

    #endregion
    #region Main Functions
    private void Start()
    {
        if (_lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
    }

    private void Update()
    {
        CheckMouse();
        CheckMove();
        CheckJump();
    }

    #endregion
    #region Movement Functions
    private void CheckMouse()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector2 targetMouseDelta = new Vector2(mouseX, mouseY);
        _currentMouseDelta = Vector2.SmoothDamp(_currentMouseDelta, targetMouseDelta, ref _currentMouseDeltaVelocity, _mouseSmoothTime);
        _cameraPitch -= _currentMouseDelta.y * _mouseSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90.0f, 90.0f);
        _playerCamera.localEulerAngles = Vector3.right * _cameraPitch;
        transform.Rotate(Vector3.up * _currentMouseDelta.x * _mouseSensitivity);
    }
    private void CheckMove()
    {
        float _moveX = Input.GetAxisRaw("Horizontal");
        float _moveY = Input.GetAxisRaw("Vertical");

        Vector2 _targetDir = new Vector2(_moveX, _moveY);
        _targetDir.Normalize();
        _currentDir = Vector2.SmoothDamp(_currentDir, _targetDir, ref _currentDirVelocity, _moveSmoothTime);
        Vector3 velocity = (transform.forward * _targetDir.y + transform.right * _targetDir.x) * _walkSpeed + Vector3.up * _velocityY;
        _controller.Move(velocity * Time.deltaTime);

        if (_controller.isGrounded)
        {
            _velocityY = 0.0f;
        }

        _velocityY += _gravity * Time.deltaTime;
    }
    private void CheckJump()
    {
        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
        {
            _velocityY = Mathf.Sqrt(_jumpHeight * -2.0f * _gravity);
        }
    }

    #endregion
    #region Triggers / Colliders
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
            Singleton.Instance.TakeDamage(25);
            Debug.Log("TAKE DAMAGE");
        }

        if (other.gameObject.CompareTag("Health"))
        {
            Debug.Log("GOT HEALTH");
            Singleton.Instance.GiveHealth(25);
            Destroy(other.gameObject, 0.1f);
        }

        if (other.gameObject.CompareTag("Collect"))
        {
            Debug.Log("GOT COLLECTIBLE");
            Singleton.Instance.Collect();
        }
    }
    #endregion
}
