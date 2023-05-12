using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{
    [Space(30)]
    [Header("PLAYER VARIABLES")]
    public float playerSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravityValue = -9.81f;
    public float downSpeed = 1.5f;
    public float animSpeed = 5f;
    public bool IsAiming = false;
    public Animator animator;

    private float Speed;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector3 move;
    private Transform cameraTransform;

    [Space(30)]
    [Header("USER INPUTS")]
    public float horizontalMove;
    public float verticalMove;

    [Space(30)]
    [Header("RIGGING COMPONENTS")]
    public Rig headLookRig;
    public Rig pistolRig;
    public Rig pistolOnHandRig;


    [Space(30)]
    [Header("SHOOTING COMPONENTS")]
    public float fireRate = 15f;
    public float nextTimeToFire = 0f;
    public bool fire = false;
    public LayerMask aimColliderLayerMask = new LayerMask();
    public Transform pfBulletProjectile;
    public Transform spawnBulletPosition;
    private Transform crossHair;
    private Vector3 mouseWorldPosition;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        mouseWorldPosition = Vector3.zero;

        Vector3 origin = spawnBulletPosition.position;
        Vector3 direction = spawnBulletPosition.TransformDirection(Vector3.forward);

        Transform hitTransform = null;
        if (Physics.Raycast(origin, direction, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            //debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        MovementAimShoot();
    }

    public void MovementAimShoot() 
    {
        // Taking user input and store in input(Vector2 variable)
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
        move = new Vector3(horizontalMove, 0, verticalMove);


        //Check if Right mouse button is pressed or not
        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse0))
        {
            //________________________________PISTOL ANIMATION__________________________________________
            headLookRig.weight = Mathf.Lerp(headLookRig.weight, 1f, animSpeed * 2 * Time.deltaTime);
            pistolRig.weight = Mathf.Lerp(pistolRig.weight, 1f, animSpeed * 2 * Time.deltaTime);
            pistolOnHandRig.weight = Mathf.Lerp(pistolOnHandRig.weight, 0f, animSpeed * 2 * Time.deltaTime);

            //set Aiming Layer to 1
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, animSpeed * Time.deltaTime));


            //Set moveSpeed high
            playerSpeed = 4f;

            //Animation Controller
            animator.SetFloat("InputX", Mathf.Lerp(animator.GetFloat("InputX"), move.x, animSpeed * Time.deltaTime * 5f));
            animator.SetFloat("InputY", Mathf.Lerp(animator.GetFloat("InputY"), move.z, animSpeed * Time.deltaTime * 5f));

            //Moving the controller forward where the camera is looking
            move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
            move.y = 0;

            controller.Move(move * Time.deltaTime * playerSpeed);

            playerVelocity.y += gravityValue * Time.deltaTime * downSpeed;
            controller.Move(playerVelocity * Time.deltaTime);

            // Rotate character in forward direction 
            Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 20f);

            animator.SetBool("IsAiming", true);
            IsAiming = true;

            //Set moveSpeed
            playerSpeed = 7f;

            //Spawn a bullet
            if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }

        }
        else
        {
            //________________________________PISTOL ANIMATION__________________________________________
            headLookRig.weight = Mathf.Lerp(headLookRig.weight, 0f, animSpeed * Time.deltaTime);
            pistolRig.weight = Mathf.Lerp(pistolRig.weight, 0f, animSpeed * Time.deltaTime);
            pistolOnHandRig.weight = Mathf.Lerp(pistolOnHandRig.weight, 1f, animSpeed * 100 * Time.deltaTime);

            //set Aiming Layer to 0
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, animSpeed * Time.deltaTime));

            //Determining Speed
            Speed = Mathf.Abs(horizontalMove) > Mathf.Abs(verticalMove) ? Mathf.Abs(horizontalMove) : Mathf.Abs(verticalMove);

            //Apply Speed as blend tree value
            animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), Speed, animSpeed * Time.deltaTime));

            //Moving the controller forward where the camera is looking
            move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
            move.y = 0;

            controller.Move(move * Time.deltaTime * playerSpeed);

            playerVelocity.y += gravityValue * Time.deltaTime * downSpeed;
            controller.Move(playerVelocity * Time.deltaTime);


            animator.SetBool("IsAiming", false);
            IsAiming = false;
        }

        // Rotate character in direction of movement
        if (move.magnitude > 0 && IsAiming == false)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }


    public void Shoot() 
    {
        //shoot a bullet from spawn position to aimDir position
        Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        fire = false;
    }

}





