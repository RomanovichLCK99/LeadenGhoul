using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement instance;



    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private Camera cam = null;
    [SerializeField] private Transform gun = null;
    [SerializeField] private Transform gunGrab = null;
    [SerializeField] FieldOfView fieldOfView = null;

    [Space(10)]
    [SerializeField] private Animator topAnimator = null;
    [SerializeField] private Animator bottomAnimator = null;

    [Space(10)]
    [Header("Variables")]
    public float speed;
    public Transform[] myTransforms;



    // Input of the player
    float horizontalInput;
    float verticalInput;


    // Normalized input
    Vector2 moveVelocity;
    [HideInInspector]
    public Vector2 playerInput;

    // Used to move the gun and the player
    Vector2 mousePos;
    Vector2 gunGrabPos;
    

    //Direction calculated from the substraction of two vectors;
    Vector3 aimDir;


    // Used on the Player dash script 
    [HideInInspector]
    public bool isDashing = false;


    #region Singleton

    private void Awake()
    {
        instance = this;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        playerInput = new Vector2(horizontalInput, verticalInput);
        moveVelocity = playerInput.normalized;

        bottomAnimator.SetFloat("RunX", moveVelocity.x);
        bottomAnimator.SetFloat("RunY", moveVelocity.y);
        bottomAnimator.SetFloat("RunMagnitude", moveVelocity.magnitude);


        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        gunGrabPos = gunGrab.position;

        aimDir = (mousePos - gunGrabPos).normalized;

        topAnimator.SetFloat("AimX", aimDir.x);
        topAnimator.SetFloat("AimY", aimDir.y);
        topAnimator.SetFloat("AimMagnitude", aimDir.magnitude);

        if (!PauseScreen.instance.isPaused) {
            fieldOfView.SetAimDirection(aimDir);
            fieldOfView.SetOrigin(transform.position);

            gun.position = gunGrabPos;

            Vector2 lookDir = mousePos - gunGrabPos;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            gun.rotation = Quaternion.Euler(0, 0, angle);
        }
        

    }
    // Used to perform movement
    void FixedUpdate() 
    {

        if (isDashing == false)
        {
            rb.MovePosition(rb.position + moveVelocity * speed * Time.deltaTime);
        }

    }

}
