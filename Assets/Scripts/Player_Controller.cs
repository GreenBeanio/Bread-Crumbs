using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //Player Objects
    public CharacterController player;
    //Look Variables
    public Camera player_camera;
    public float vertical_look_limit = 90f;
    public float mouse_sensetivity = 100f;
    float xRotation = 0f;
    //Movement variables
    public float movement_speed = 20f;
    public float falling_speed = 9.81f;
    public float jump_height = 1f;
    Vector3 velocity;
    //Ground Check
    public Transform groundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;
    bool isGrounded;
    //Shortcut Check
    public Transform shortcutCheck;
    public float shortcutDistance = 1f;
    public LayerMask shortcutMask;
    public bool shortcutInRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        //Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * (mouse_sensetivity * 10) * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * (mouse_sensetivity * 10) * Time.deltaTime;
        //Turning player
        this.transform.Rotate(Vector3.up * mouseX);
        //Turning camera
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -vertical_look_limit, vertical_look_limit);
        player_camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //Get player inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        //Moving the player
        Vector3 move = transform.right * x + transform.forward * z;
        player.Move(move * movement_speed * Time.deltaTime);
        //Gravity
        velocity.y += -falling_speed * Time.deltaTime;
        //Falling
        player.Move(velocity * Time.deltaTime);
        //Checking for ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Jumping
        if(Input.GetButtonDown("Jump") && isGrounded == true)
        {
            velocity.y = Mathf.Sqrt(jump_height * -2f * -falling_speed);
        }
        //Ending fall if grounded
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        //Checking for shortcut
        shortcutInRange = Physics.CheckSphere(shortcutCheck.position, shortcutDistance, shortcutMask);
        //Unlocking shortcut
        if(Input.GetKeyDown(KeyCode.E) && shortcutInRange == true)
        {
            Collider[] shortcuts = Physics.OverlapSphere(shortcutCheck.position, shortcutDistance, shortcutMask);
            foreach(var shortcut in shortcuts)
            {
                //shortcut.GetComponentInParent<Shortcut_Controller>().Unlock(true);
                Debug.Log(shortcut.name);
            }
        }
    }
    //Fixed Update
    private void FixedUpdate()
    {
        
    }
    //On collision
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Shortcut")
        {
            collision.gameObject.GetComponent<Shortcut_Controller>().Unlock(true);
        }
    }
}
