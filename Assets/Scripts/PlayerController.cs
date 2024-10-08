using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public Animator anim;

    //public GameObject bullet;
    public Transform firePoint;

    public static PlayerController instance;

    public Gun activeGun;
    public List<Gun> allGuns = new List<Gun>();
    public int currentGun;

    public Transform adsPoint,gunHolder;
    private Vector3 gunStartPosition;
    public float adsSpeed=2;
    public AudioSource footstepFast, footstepSlow;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentGun--;
        switchGun();

        gunStartPosition = gunHolder.localPosition; 
    }

    void Update()
    {
        if (!UIController.Instance.pauseScreen.activeInHierarchy && !GameManager.Instance.ending)
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
                AudioManager.instance.playSFX(4);

            }
            else if(canDoubleJump && Input.GetKeyDown(KeyCode.Space)) 
        { 
            moveInput.y = jumpPower; 
            canDoubleJump = false;
            AudioManager.instance.playSFX(4);

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

        //Pucanje
        //Jedan metak
        if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <=0)
        {
            RaycastHit hit;
            if(Physics.Raycast(camTrans.position, camTrans.forward, out hit, 50f))
            {
                if (Vector3.Distance(camTrans.position, hit.point) > 2f)
                {
                    firePoint.LookAt(hit.point);
                }
            } else firePoint.LookAt(camTrans.position+(camTrans.forward * 30f));

            //Instantiate(bullet, firePoint.position, firePoint.rotation);
            fireShot();
        }

        //Rafal
        if(Input.GetMouseButton(0) && activeGun.canAutoFire)
        {
            if (activeGun.fireCounter <= 0)
            {
                fireShot();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switchGun();
        }


        if (Input.GetMouseButton(1))
        {
            gunHolder.position = Vector3.MoveTowards(gunHolder.position, adsPoint.position, adsSpeed * Time.deltaTime);
        }
        else
        {
            gunHolder.localPosition = Vector3.MoveTowards(gunHolder.localPosition,gunStartPosition,adsSpeed* Time.deltaTime);
        }
        if (Input.GetMouseButtonDown(1))
        {
            CameraController.instance.ZoomIn(activeGun.zoomAmount);
        }
        if (Input.GetMouseButtonUp(1))
        {
            CameraController.instance.ZoomOut();
        }

        anim.SetFloat("MoveSpeed",moveInput.magnitude);
        anim.SetBool("onGround",canJump);

        }
    }

    
    public void fireShot()
    {
        if (activeGun.currentAmmo > 0) 
        { 
            activeGun.currentAmmo--;
            UIController.Instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);
            activeGun.fireCounter = activeGun.fireRate;
        }
    }

    public void switchGun()
    {
        activeGun.gameObject.SetActive(false);
        currentGun++;

        if (currentGun >= allGuns.Count) currentGun = 0;

        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);

        UIController.Instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

        firePoint.position = activeGun.firepoint.position;
    }
}
