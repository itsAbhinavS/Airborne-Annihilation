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



    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Taking user input and store in input(Vector2 variable)
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
        move = new Vector3(horizontalMove, 0, verticalMove);


        //Check if Right mouse button is pressed or not
        if (Input.GetKey(KeyCode.Mouse1) || 2>1)
        {
            //set head rig to 1
            headLookRig.weight = Mathf.Lerp(headLookRig.weight, 1f, animSpeed * Time.deltaTime);

            pistolRig.weight = Mathf.Lerp(pistolRig.weight, 1f, animSpeed * Time.deltaTime);

            //set Aiming Layer to 1
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, animSpeed * Time.deltaTime));


            //Set moveSpeed high
            playerSpeed = 4f;

            //Animation Controller
            animator.SetFloat("InputX", Mathf.Lerp(animator.GetFloat("InputX"), move.x, animSpeed * Time.deltaTime * 5f ));
            animator.SetFloat("InputY", Mathf.Lerp(animator.GetFloat("InputY"), move.z, animSpeed * Time.deltaTime * 5f ));

            //Moving the controller forward where the camera is looking
            move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
            move.y = 0;
            
            controller.Move(move * Time.deltaTime * playerSpeed);

            playerVelocity.y += gravityValue * Time.deltaTime * downSpeed;
            controller.Move(playerVelocity * Time.deltaTime);

            // Rotate character in forward direction 
            Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            animator.SetBool("IsAiming", true);
            IsAiming = true;

            //Set moveSpeed
            playerSpeed = 7f;
        }
        else
        {
            //set head rig to 0
            headLookRig.weight = Mathf.Lerp(headLookRig.weight, 0f, animSpeed * Time.deltaTime);

            pistolRig.weight = Mathf.Lerp(pistolRig.weight, 0f, animSpeed * Time.deltaTime);

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
}