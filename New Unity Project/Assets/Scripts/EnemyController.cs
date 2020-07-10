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
    Bouncy,
    Melee,
    Ranged,
    Boss1,
    Boss2,
    Bomber
};


public class EnemyController : MonoBehaviour
{   public EnemyOnDeathSpawner dropList = null;
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
    public Animator animator;
    public Vector2 movement;
    private bool chooseDir = false;
    private bool dead = false;
    private bool coolDownAttack = false;
    public bool notInRoom;
    private Vector3 randomDir;
    public GameObject bulletPrefab;
    private GameObject bullet;


    public void DamageEnemy(float value)
    {
        Debug.Log("preso! vita: " + health);
        health -= value;
        StartCoroutine(Flash());
    }
    // Start is called before the first frame update
    void Start()
    {
   //     Debug.Log("sono spwnato e sono " + notInRoom);
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {   if (this.enemyType == EnemyType.Boss1 
                || this.enemyType == EnemyType.Boss2)
            {
                Debug.Log(this.transform.GetChild(0));
                this.transform.GetChild(0).gameObject.SetActive(true);

                transform.GetChild(0).parent = null;
            }
            this.Death();
        }
        switch (currState)
        {
            /*case(EnemyState.Idle):
                Idle();*/
            //break;
            /*case(EnemyState.Wander):
                Wander();
            break;*/
            case(EnemyState.Follow):
                Follow();
            break;
            case(EnemyState.Die):
            break;
            case(EnemyState.Attack):
                Attack();
            break;
        }

        if (!notInRoom)
        { if (rigidbody.simulated == false)
            {
                if(enemyType == EnemyType.Boss1)            //spawna la scala dopo il primo Boss
                    this.transform.GetChild(1).gameObject.SetActive(true);
                StartCoroutine(CoolDown());
                rigidbody.simulated = true;

            }
            if (IsPlayerInRange(range) && currState != EnemyState.Die  )
            {
                currState = EnemyState.Follow;
            }
            /*else if(!IsPlayerInRange(range) && currState != EnemyState.Die && enemyType != EnemyType.Bouncy)
            {
                currState = EnemyState.Wander;
     //         Debug.Log("mi sto muovendo e sono " + name + notInRoom);
            }*/
            else if (!IsPlayerInRange(range)) 
            {
                currState = EnemyState.Idle;
            }
            if (( enemyType == EnemyType.Ranged 
                || enemyType == EnemyType.Boss2  
                || enemyType == EnemyType.Bouncy
                || enemyType == EnemyType.Bomber ) 
                && Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                
                currState = EnemyState.Attack;
            }
        }
        else
        {
            if (enemyType == EnemyType.Boss1 && this.transform.GetChild(1).gameObject.activeSelf == true)
                this.transform.GetChild(1).gameObject.SetActive(false);
            rigidbody.simulated = false;
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

    /*void Wander()
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
    }*/

    void Follow()
    {
        rigidbody.MovePosition(Vector2.MoveTowards(rigidbody.position, player.transform.position, speed * Time.deltaTime));
        Vector2 direction = (player.transform.position - transform.position).normalized;
  
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
        //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        if (!coolDownAttack)
        {
            switch (enemyType)
            {
                case (EnemyType.Melee):
                    GameController.DamagePlayer(1);
                    StartCoroutine(CoolDown());
                    break;
                case (EnemyType.Bouncy):
                  
                    Charge();
                    StartCoroutine(CoolDown());
                    break;
                case (EnemyType.Boss1):
                    GameController.DamagePlayer(2);
                    StartCoroutine(CoolDown());
                    break;
                case (EnemyType.Ranged):
                    bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                    bullet.GetComponent<BulletController>().speed = bulletSpeed;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;
                    StartCoroutine(CoolDown());
                    break;
                case (EnemyType.Bomber):
                    bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BombController>().GetPlayer(player.transform);
                    bullet.GetComponent<BombController>().speed = bulletSpeed;
                    bullet.GetComponent<BombController>().isEnemyBomb = true;
                    StartCoroutine(CoolDown());
                    break;
                case (EnemyType.Boss2):
                    bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                    bullet.GetComponent<BulletController>().speed = bulletSpeed;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;
                    bullet.transform.localScale = new Vector2(2f, 2f);
                    bullet.GetComponent<BulletController>().damage = 2;

                    if (this.transform.GetComponent<Boss2Ability>().enabled == false)
                        this.transform.GetComponent<Boss2Ability>().enabled = true;
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
    public void Charge()
    {
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
        rigidbody.velocity = Vector2.MoveTowards(transform.position, (direction * speed ), 10000f);
    }
    public void Death()
    {
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
        if (dropList != null)
        {

            Debug.Log("ho drop");
            dropList.Drop(transform.position);
        }
        Destroy(gameObject);
    }

    IEnumerator Flash()
    {
        for (int n = 0; n < 2; n++)
        {
            Color spriteColor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().color = spriteColor;
          yield return new WaitForSeconds(0.1f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && enemyType == EnemyType.Bouncy)
            GameController.DamagePlayer(2);


    }
    private void OnCollisionStay2D(Collision2D collision)
    {
       
       
        if (collision.gameObject.tag == "Player" && enemyType != EnemyType.Bouncy)
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        if (collision.gameObject.tag == "Player" && enemyType != EnemyType.Ranged)
            currState = EnemyState.Attack;
    
  
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
   
    }

}
