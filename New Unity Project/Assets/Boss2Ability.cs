using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Ability : MonoBehaviour
{
    public GameObject bulletPrefab;
    private GameObject bullet;
    private bool coolDownAttack;
    private Vector2 whereToShoot;
    private Vector2 dir;



    // Update is called once per frame
    void Update()
    {
        if (!coolDownAttack)
        {
            bullet = Instantiate(bulletPrefab, transform.position +   new Vector3(0, 2, 0), Quaternion.identity) as GameObject;   
            bullet.GetComponent<BulletController>().isEnemyBullet = true;
            bullet.GetComponent<BulletController>().lifeTime = 7;

            bullet = Instantiate(bulletPrefab, transform.position  + new Vector3(0, -2, 0), Quaternion.identity) as GameObject;
            bullet.GetComponent<BulletController>().isEnemyBullet = true;
            bullet.GetComponent<BulletController>().lifeTime = 7;

            bullet = Instantiate(bulletPrefab, transform.position + new Vector3(2, 0, 0), Quaternion.identity) as GameObject;
            bullet.GetComponent<BulletController>().isEnemyBullet = true;
            bullet.GetComponent<BulletController>().lifeTime = 7;


            bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-2, 0, 0), Quaternion.identity) as GameObject;
            bullet.GetComponent<BulletController>().isEnemyBullet = true;
            bullet.GetComponent<BulletController>().lifeTime = 7;


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
