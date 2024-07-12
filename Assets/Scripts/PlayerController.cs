using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed, gravityModifier, jumpPower, runSpeed = 12f;
    public CharacterController charCon;

    private Vector3 moveInput;

    public Transform camTrans;
    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;

    private bool canJump,canDoubleJump;
    public Transform groundCheckpoint;
    public LayerMask whatIsGround; 

    void Start()
    {
        
    }

    void Update()
    {
        //Kretanje igraca

        //moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        //za gravitaciju
        float yStore = moveInput.y;

        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = horiMove + vertMove;
        moveInput.Normalize();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveInput = moveInput*runSpeed;
        }
        else { 
            moveInput = moveInput * moveSpeed;
        }

        moveInput.y = yStore;
        moveInput.y += Physics.gravity.y * gravityModifier*Time.deltaTime;

        if (charCon.isGrounded)
        {
            moveInput.y = Physics.gravity.y*gravityModifier*Time.deltaTime ;
        }

        //Dodavanje skoka
        canJump = Physics.OverlapSphere(groundCheckpoint.position, .25f, whatIsGround).Length > 0;
        if(canJump)
        {
            canDoubleJump = false;
        }

        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            moveInput.y = jumpPower;
            canDoubleJump = true;
        }else if(canDoubleJump && Input.GetKeyDown(KeyCode.Space)) 
        { 
            moveInput.y = jumpPower; canDoubleJump = false; 
        }

        charCon.Move(moveInput * Time.deltaTime);

        //Kontrolisanje rotacije kamere

        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        if (invertX)
        {
            mouseInput.x = -mouseInput.x;
        }
        if (invertY)
        {
            mouseInput.y = -mouseInput.y;
        }
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y+mouseInput.x, transform.rotation.eulerAngles.z);

        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y,0f,0f));
    }
}
