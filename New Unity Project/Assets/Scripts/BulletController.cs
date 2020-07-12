using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;
    public bool isEnemyBullet = false;
    public int damage;
    public float speed;
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
            Vector2 dir = Vector2.MoveTowards(transform.position, playerPos, 1000f);
            dir.Normalize();
            GetComponent<Rigidbody2D>().velocity = dir * speed;
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPos = player.position -transform.position;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Wall")  && !GameController.IPB)
        {
            Destroy(gameObject);
        }
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
