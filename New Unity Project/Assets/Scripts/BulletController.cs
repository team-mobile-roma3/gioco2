using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;
    public bool isEnemyBullet = false;
    public int damage;

    private Vector2 lastPos;
    private Vector2 curPos;
    private Vector2 playerPos;
    // Start is called before the first frame update
    void Start() 
    {
        StartCoroutine(DeathDelay());
        if(!isEnemyBullet)
        { 
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        }
    }

    void Update()
    {
        if(isEnemyBullet)
        {
            curPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, 10000f);
            if(curPos == lastPos)
            {
                Destroy(gameObject);
            }
            lastPos = curPos;
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPos = player.position;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Wall") && !isEnemyBullet && GameController.IPB){
            Debug.Log("hit wall");
            Rigidbody2D ri = gameObject.GetComponent<Rigidbody2D>();
            ri.velocity = -ri.velocity;
        }


        if(col.CompareTag("Enemy") && !isEnemyBullet)
        {
            col.gameObject.GetComponent<EnemyController>().DamageEnemy(GameController.AttackDamage) ;
            Destroy(gameObject);
        }

        if(col.CompareTag("Player") && isEnemyBullet)
        {
            GameController.DamagePlayer(damage);
            Destroy(gameObject);
        }
    }
}
