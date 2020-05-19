using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float rotateSpeed = 0;
    Rigidbody2D rigidbody;
  

    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;

    private float lastSwing;
    public float swingDelay;

    private bool stance;
    float horizontal, vertical;


    private static float ableTeleportDoor;
    
    

    private float lastFlipShoot;

    public bool joyMove;

    public FloatingJoystick move;

    public FloatingJoystick act;

    public  static float AbleTeleportDoor { get => ableTeleportDoor; set => ableTeleportDoor = value; }
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.freezeRotation = true;
        ableTeleportDoor = Time.time-2f;
    }

    // Update is called once per frame
    void Update()
    {
        stance = GameController.Stance;
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Inventory.PotionUse();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            stance = !stance;
        }


       
        if (joyMove)
        {
            horizontal = move.Horizontal;
            vertical = move.Vertical;

            if (horizontal > 0 && (   Time.time > lastFlipShoot + 1.0f || lastFlipShoot == 0))
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (horizontal < 0 && (Time.time > lastFlipShoot + 1.0f || lastFlipShoot == 0))
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }

        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical =Input.GetAxis("Vertical");


            if (horizontal > 0 && (Time.time > lastFlipShoot + 1.0f || lastFlipShoot == 0))
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (horizontal < 0 && (Time.time > lastFlipShoot + 1.0f || lastFlipShoot == 0))
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }


        }
        
        float shootHor, shootVert;
        
        if (joyMove)
        {
            shootHor = act.Horizontal;
            shootVert = act.Vertical;    
        }
        else
        {
            shootHor = Input.GetAxis("ShootHorizontal");
            shootVert =Input.GetAxis("ShootVertical");
        }
        if (!stance)  // cioe' se sono ranged
        {
            if ((shootHor != 0 || shootVert != 0) && Time.time > lastFire + fireDelay)
            {
                if (shootHor > 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
                else if (shootHor < 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }


                Shoot(shootHor, shootVert);
                lastFire = Time.time;
                lastFlipShoot = Time.time;
            }
        }
        else
        {
            if ((shootHor != 0 || shootVert != 0) && Time.time > lastSwing + swingDelay)
            {
                if (shootHor > 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
                else if (shootHor < 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }


                Melee(shootHor, shootVert);
                lastSwing = Time.time;
                lastFlipShoot = Time.time;
            }
        }

        

    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector3(horizontal * speed, vertical * speed, 0);
    }

    void Shoot(float x, float y)
    {
        if (x == 0 || y == 0)
            return;
        //     Debug.Log(x + " " + y);
        else
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
            Vector2 whereToShoot = new Vector2(x, y);
            Vector2 dir = Vector2.MoveTowards(bullet.transform.position, whereToShoot, 100f);
            dir.Normalize();

            bullet.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;
            //       Debug.Log("ho sparato con " + Inventory.Ranged_Weapon + "direzione " + dir.x + "|" + dir.y);
        }
    }
    
    void Melee(float x, float y)
    {
        
        if(y < 0)
        {
            if(math.abs(x) < math.abs(y))
            transform.GetChild(0).gameObject.SetActive(true);   
        }

        if (y > 0)
        {
            if (math.abs(x) < math.abs(y))
                transform.GetChild(1).gameObject.SetActive(true);
        }

        if (x > 0)
        {
            if (math.abs(y) < math.abs(x))
                transform.GetChild(3).gameObject.SetActive(true);
        }
        if (x < 0)
        {
            if (math.abs(y) < math.abs(x))
                transform.GetChild(2).gameObject.SetActive(true);
        }


    }
}
