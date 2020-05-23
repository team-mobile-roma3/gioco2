using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float lifeTime;
    public bool isEnemyBomb= false;
    public int damage;
    private Vector2 playerPos;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathDelay());
        StartCoroutine(Freeze());

    }

    void FixedUpdate()
    {
        if (isEnemyBomb)
        {
            Vector2 dir = Vector2.MoveTowards(transform.position, playerPos, 1000f);
            dir.Normalize();
            GetComponent<Rigidbody2D>().velocity = dir * speed;

        }
    }

    public void GetPlayer(Transform player)
    {
        playerPos = player.position - transform.position;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
    IEnumerator Freeze()
    {
        
        yield return new WaitForSeconds(2f);
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(3f);
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.transform.GetChild(0).gameObject.SetActive(true);
        this.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }



    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Wall") && isEnemyBomb )
        {
            Debug.Log("hit wall");
            Rigidbody2D ri = gameObject.GetComponent<Rigidbody2D>();
            ri.velocity = -ri.velocity;
        }
        
        /*
        if (col.CompareTag("Enemy") && !isEnemyBomb)
        {
            col.gameObject.GetComponent<EnemyController>().DamageEnemy(GameController.AttackDamage);
            Destroy(gameObject);
        }
        */
        if (col.CompareTag("Player") && isEnemyBomb)
        {
            GameController.DamagePlayer(4);    
        }
    }
}
