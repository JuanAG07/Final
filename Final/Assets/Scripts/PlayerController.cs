using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float jumpForce = 10f;
    public float gravityModifier = 1f;
    public float mouseSensitivity = 1f;
    public GameObject bullet;
    public Transform firePoint;
    public Transform theCamera;
    public Transform groundCheckpoint;
    public LayerMask whatIsGround;
    public GameObject object1, object2, object3, object4;
    private bool _canPlayerJump;
    private Vector3 _moveInput;
    private CharacterController _characterController;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Store the y velocity
        float yVeclocity = _moveInput.y;

        //Player movement
        //_moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //_moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        Vector3 forwardDirection = transform.forward * Input.GetAxis("Vertical");
        Vector3 horizontalDirection = transform.right * Input.GetAxis("Horizontal");

        _moveInput = (forwardDirection + horizontalDirection).normalized;
        _moveInput *= moveSpeed;

        //Player jumping setup
        _moveInput.y = yVeclocity;
        _moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

        if (_characterController.isGrounded)
        {
            _moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        //Checking to see if player can jump
        _canPlayerJump = Physics.OverlapSphere(groundCheckpoint.position, 0.50f, whatIsGround).Length > 0;

        //Apply a jump force to player
        if (Input.GetKeyDown(KeyCode.Space) && _canPlayerJump)
        {
            _moveInput.y = jumpForce;
        }

        _characterController.Move(_moveInput * Time.deltaTime);

        //Control camera rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        //Player Rotation
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        //Camera Rotation
        theCamera.rotation = Quaternion.Euler(theCamera.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("object1"))
        {
            object1.SetActive(false);
        }
        if (other.gameObject.CompareTag("object2"))
        {
            object2.SetActive(false);
        }
        if (other.gameObject.CompareTag("object3"))
        {
            object3.SetActive(false);
        }
        if (other.gameObject.CompareTag("object4"))
        {
            object4.SetActive(false);
        }
        if (!object1.activeSelf && !object2.activeSelf)
        {
            _characterController.enabled = false;
            _characterController.transform.position = new Vector3(-4.5f, 0, -25);
            _characterController.enabled = true;
        }

        if (!object3.activeSelf && !object4.activeSelf)
        {
            _characterController.enabled = false;
            _characterController.transform.position = new Vector3(47.5f, 1, -37);
            _characterController.enabled = true;
        }
    }

}