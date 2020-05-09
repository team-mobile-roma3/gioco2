using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Wander,
    Follow,
    Die,
    Attack
};

public enum EnemyType
{
    Melee,
    Ranged,
    Boss1
};

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rigidbody;
    GameObject player;
    public EnemyState currState = EnemyState.Idle;
    public EnemyType enemyType;
    public float range;
    public float speed;
    public float attackRange;
    public float bulletSpeed;
    public float coolDown;
    public   float health;
    private bool chooseDir = false;
    private bool dead = false;
    private bool coolDownAttack = false;
    public bool notInRoom = false;
    private Vector3 randomDir;
    public GameObject bulletPrefab;


    public void DamageEnemy(float value)
    {
        Debug.Log("preso! vita: " + health);
        health -= value;
    }
    // Start is called before the first frame update
    void Start()
    {
      
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            
            this.Death();
        }
        switch (currState)
        {
            //case(EnemyState.Idle):
            //    Idle();
            //break;
            case(EnemyState.Wander):
                Wander();
            break;
            case(EnemyState.Follow):
                Follow();
            break;
            case(EnemyState.Die):
            break;
            case(EnemyState.Attack):
                Attack();
            break;
        }

        if(!notInRoom)
        {
            if(IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Follow;
            }
            else if(!IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Wander;
            }
        
                    if ( enemyType == EnemyType.Ranged && Vector3.Distance(transform.position, player.transform.position) <= attackRange)
                    {
                        currState = EnemyState.Attack;
                    }
        }
        else
        {
            currState = EnemyState.Idle;
        }      
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        randomDir = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;
    }

    void Wander()
    {
        if(!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        transform.position += -transform.right * speed * Time.deltaTime;
        if(IsPlayerInRange(range))
        {
            currState = EnemyState.Follow;
        }
    }

    void Follow()
    {
        rigidbody.MovePosition(Vector2.MoveTowards(rigidbody.position, player.transform.position, speed * Time.deltaTime));
 //       transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        if(!coolDownAttack)
        {
            switch(enemyType)
            {
                case(EnemyType.Melee):
                    GameController.DamagePlayer(1);
                    StartCoroutine(CoolDown());
                break;
                case (EnemyType.Boss1):
                    GameController.DamagePlayer(2);
                    StartCoroutine(CoolDown());
                    break;
                case (EnemyType.Ranged):
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                   
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;
                    StartCoroutine(CoolDown());
                break;
            }
        }
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    public void Death()
    {
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag== "Player" && enemyType != EnemyType.Ranged)
        {
            currState= EnemyState.Attack;
            
        }
  
    }

}
