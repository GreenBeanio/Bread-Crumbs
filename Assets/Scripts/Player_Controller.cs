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
    //Attack Varaibles
    public Transform fire_point;
    public GameObject butter;
    public GameObject butter_hand;
    public float projectile_speed = 15f;
    public float attack_wait = 2f;
    float attack_elapsed;
    public bool attacked;
    //Damage Variables
    public float damage_wait = 5f;
    float damage_elapsed;
    bool damaged;
    //Health variables
    public float health = 100;
    public float max_health = 100;
    public int lives = 3;
    public int max_lives = 3;
    //Spawn
    public Transform spawn_point;

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
                shortcut.GetComponent<Shortcut_Controller>().Unlock(true);
                Debug.Log(shortcut.name);
            }
        }
        //Attacking
        if(Input.GetMouseButton(0))
        {
            if(attacked == false)
            {
                attacked = true;
                Attack();
            }
        }
        //Check attack status
        if(attacked == true)
        {
            attack_elapsed = attack_elapsed + Time.deltaTime;
            butter_hand.SetActive(false);
            if (attack_elapsed >= attack_wait)
            {
                attacked = false;
                attack_elapsed = 0;
                butter_hand.SetActive(true);
            }
        }
        //Check damaged status
        if(damaged == true)
        {
            damage_elapsed = damage_elapsed + Time.deltaTime;
            if(damage_elapsed >= damage_wait)
            {
                damaged = false;
                damage_elapsed = 0;
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
    //Attack
    void Attack()
    {
        GameObject butter_shot = Instantiate(butter, fire_point.position, fire_point.rotation, null);
        butter_shot.GetComponent<Rigidbody>().AddRelativeForce(butter_shot.transform.forward * projectile_speed);
    }
    //Damaged
    public void take_damage(float damage_amount)
    {
        if (damaged == false)
        {
            damaged = true;
            float new_health = health - damage_amount;
            if(health > 0)
            {
                health = new_health;
            }
            else
            {
                health = 0;
                Die();
            }
        }
    }
    //Healing
    public void healing(float healing_amount)
    {
        float new_health = health + healing_amount;
        if(new_health > max_health)
        {
            health = max_health;
        }
        else
        {
            health = new_health;
        }
    }
    //Lives
    public void lives_change(int lives_amount)
    {
        int new_lives = lives + lives_amount;
        if(new_lives > max_lives)
        {
            lives = max_lives;
        }
        else
        {
            lives = new_lives;
        }
    }
    //Die
    void Die()
    {
        int new_lives = lives - 1;
        if(new_lives >= 0)
        {
            health = max_health;
            damaged = true;
            attacked = false;
            this.transform.position = spawn_point.position;
        }
        else
        {
            //End Game
        }
    }
}
