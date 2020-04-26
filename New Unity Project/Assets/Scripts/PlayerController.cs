using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float rotateSpeed = 0;
    Rigidbody2D rigidbody;
    public Text collectedText;
    public static int collectedAmount = 0;

    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;

    private float lastFlipShoot;

    public bool joyMove;

    public FloatingJoystick move;

    public FloatingJoystick act;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;
      
        float horizontal, vertical;
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
        
        if((shootHor != 0 || shootVert != 0) && Time.time > lastFire + fireDelay)
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

        rigidbody.velocity = new Vector3(horizontal * speed, vertical * speed, 0);
        collectedText.text = "Items Collected: " + collectedAmount;
    }

    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed
        );
    }
}
