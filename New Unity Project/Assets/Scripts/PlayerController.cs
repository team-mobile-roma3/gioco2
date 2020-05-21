using System;
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
    private float lastFire,lastSwing, lastMove;
    public float fireDelay,swingDelay;//, actionDelay;
    private bool stance;
    private static float ableTeleportDoor;
    private float horizontal,vertical,shootHor,shootVert;
    private float lastFlipShoot;

    /******implementa i joypad*********/
    public FloatingJoystick move;

    public FloatingJoystick act;

    /**********************************/

    public  static float AbleTeleportDoor { get => ableTeleportDoor; set => ableTeleportDoor = value; }
    
    void Start()
    {
       
       rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.freezeRotation = true;
        ableTeleportDoor = Time.time-2f;
    }
    void Update()
    {
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;
        stance = GameController.Stance;

        horizontal = Input.GetAxis("Horizontal") + move.Horizontal;
        vertical =Input.GetAxis("Vertical") + move.Vertical;
        shootHor = Input.GetAxis("ShootHorizontal") + act.Horizontal;
        shootVert =Input.GetAxis("ShootVertical") + act.Vertical;
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Inventory.PotionUse();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            stance = !stance;
        }

   /*     if (horizontal > 0 && (Time.time > lastFlipShoot + 1.0f || lastFlipShoot == 0))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (horizontal < 0 && (Time.time > lastFlipShoot + 1.0f || lastFlipShoot == 0))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }*/
        //if (horizontal != 0 && vertical != 0)
        Move(horizontal, vertical);
       
        if ((shootHor != 0 || shootVert != 0) && (((Time.time > lastFire + fireDelay) && !stance) || ((Time.time > lastSwing + swingDelay) && stance)))
        {
            /*****GIRA IL PG*********
            if (shootHor > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (shootHor < 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }*/
            

            if (!stance)        //ranged
            {
                Shoot(shootHor, shootVert);
                lastFire = Time.time;
                lastFlipShoot = Time.time;
            }

            else if (stance)        //melee
            {
                Melee(shootHor, shootVert);
                lastSwing = Time.time;
                lastFlipShoot = Time.time;
            }
        } 
    }
/*
    public void PlayerAttack(float x, float y)
    {
        Debug.Log("sono qui");
        if ((x != 0 || y != 0) && (((Time.time > lastFire + fireDelay) && !stance) || ((Time.time > lastSwing + swingDelay && stance) )))
        {
            if (!stance)        //ranged
            {
                Shoot(x, y);
                lastFire = Time.time;
                lastFlipShoot = Time.time;
            }

            else if (stance)        //melee
            {
                Melee(x, y);
                lastSwing = Time.time;
                lastFlipShoot = Time.time;
            }
        }
    }*/
    public void Move(float x, float y)
    {
        if (x==0 && y==0) {
            rigidbody.velocity = new Vector2(0,0);}
        else
        {
            rigidbody.velocity = new Vector2(x * speed, y * speed);
        }
        
    }

   public  void Shoot(float x, float y)
    {
        if (x == 0 || y == 0) {return;}
        else
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
            Vector2 whereToShoot = new Vector2(x, y);
            Vector2 dir = Vector2.MoveTowards(bullet.transform.position, whereToShoot, 10000f); ;
            dir.Normalize();
            bullet.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;
        }
    }
    
    void Melee(float x, float y)
    {
        if (x == 0 && y == 0) {return;}
        else
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
}