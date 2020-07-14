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
    private Animator animator;
    public Vector2 movement;
    private bool chooseDir = false;
    private bool dead = false;
    private bool coolDownAttack = false;
    public bool notInRoom;
    private Vector3 randomDir;
    public GameObject bulletPrefab;
    private GameObject bullet;
    private Vector2 direction;


    public void DamageEnemy(float value)
    {
        Debug.Log("preso! vita: " + health);
        health -= value;
        StartCoroutine(Flash());
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
                || enemyType == EnemyType.Boss1
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
            animator.SetBool("moving", false);
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
        Vector3 temp = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        changeAnim(temp - transform.position);
        rigidbody.MovePosition(temp);
        direction = (player.transform.position - transform.position).normalized;
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
        animator.SetBool("moving", true);
        //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        
        animator.SetBool("moving", false);
 
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
                    StartCoroutine(AttackCo());
                    StartCoroutine(CoolDown());
                    break;
                case (EnemyType.Ranged):
                    Vector2 shootingPosition = player.transform.position - transform.position;
                    animator.SetFloat("shootX", shootingPosition.x);
                    animator.SetFloat("shootY", shootingPosition.y);
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
                    StartCoroutine(AttackCo());
                    StartCoroutine(CoolDown());
                    break;
                case (EnemyType.Boss2):
                    bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                    bullet.GetComponent<BulletController>().speed = bulletSpeed;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;
                    bullet.transform.localScale = new Vector2(2f, 2f);
                    bullet.GetComponent<BulletController>().damage = 2;
                    if (this.transform.GetComponent<Boss2Ability>() != null)
                    {
                        if (this.transform.GetComponent<Boss2Ability>().enabled == false)
                            this.transform.GetComponent<Boss2Ability>().enabled = true;
                    }
              
                    StartCoroutine(CoolDown());
                    break;
            }
        }
        else animator.SetBool("attacking", false);
    }
    public void SetAnimFloat(Vector2 setVector)
    {
        animator.SetFloat("moveX", setVector.x);
        animator.SetFloat("moveY", setVector.y);
    }

    public void changeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }
            else if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
        }
    }
    private IEnumerator AttackCo()
    {
        if (!coolDownAttack)
        {
            animator.SetBool("attacking", true);
            yield return new WaitForSeconds(1f);
            animator.SetBool("attacking", false);
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
            dropList.Drop(transform.position);
        }
        GameController.ScoreChange(100);
        Destroy(gameObject);
    }

    IEnumerator Flash()
    {
        for (int n = 0; n < 2; n++)
        {
            GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0.5f);
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0f);
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
      

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
       
       
        if (collision.gameObject.tag == "Player" && enemyType != EnemyType.Bouncy && enemyType != EnemyType.Ranged)
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        if (collision.gameObject.tag == "Player" && enemyType != EnemyType.Ranged)
            currState = EnemyState.Attack;
    
  
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (collision.gameObject.tag == "Player" && enemyType == EnemyType.Bouncy)
            GameController.DamagePlayer(2);

    }

}
