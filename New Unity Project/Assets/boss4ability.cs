using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss4ability : MonoBehaviour
{
    public GameObject bombPrefab;
    private GameObject bomb;
    private bool coolDownAttack;
    private Vector2 whereToShoot;
    private Vector2 dir;
    private GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        if (!coolDownAttack)
        {
            
            bomb = Instantiate(bombPrefab, transform.position + new Vector3(0, 3, 0), Quaternion.identity) as GameObject;
            bomb.GetComponent<BombController>().speed = 20;
            bomb.GetComponent<BombController>().isBoss4Bomb = true;
            bomb = Instantiate(bombPrefab, transform.position + new Vector3(0, -3, 0), Quaternion.identity) as GameObject;
            bomb.GetComponent<BombController>().speed = 20;
            bomb.GetComponent<BombController>().isBoss4Bomb = true;

            bomb = Instantiate(bombPrefab, transform.position + new Vector3(3, 0, 0), Quaternion.identity) as GameObject;
            bomb.GetComponent<BombController>().speed = 20;
            bomb.GetComponent<BombController>().isBoss4Bomb = true;
            bomb = Instantiate(bombPrefab, transform.position + new Vector3(-3, 0, 0), Quaternion.identity) as GameObject;
            bomb.GetComponent<BombController>().speed = 20;
            bomb.GetComponent<BombController>().isBoss4Bomb = true;

            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(10f);
        coolDownAttack = false;
    }
}
