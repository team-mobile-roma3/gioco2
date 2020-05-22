using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DeathDelay());
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") )
        {
            col.gameObject.GetComponent<EnemyController>().DamageEnemy(GameController.MAttackDamage);
 
        }
     
    }
}
