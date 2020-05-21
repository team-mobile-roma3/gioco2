using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{
    public float hp;
    private bool ableDamage = true;
    public Transform target;

    void    Update()
    {
        if (hp <= 0)
            Destroy(gameObject);
        // Spin the object around the world origin at 20 degrees/second.
            transform.RotateAround(target.position, Vector3.back, 30 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            hp -= GameController.AttackDamage; 
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ableDamage)
        {
            GameController.DamagePlayer(1);
            StartCoroutine(CD());
        }
    }

    private    IEnumerator CD()
    {
        ableDamage = false;
        yield return new  WaitForSeconds(0.6f);
        ableDamage = true;
    }
}