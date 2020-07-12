using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3ability : MonoBehaviour
{
    public GameObject turretPrefab;
    private GameObject[] turret = new GameObject[4];
    private bool coolDownAttack;
    private Vector2[] turretSpawn = { new Vector2(-7, -4), new Vector2(7, 4), new Vector2(-7, 4), new Vector2(7, -4) };
    private Vector2 roomCentre;

    // Update is called once per frame
    void Update()
    {
        roomCentre = RoomController.instance.currRoom.GetRoomCentre();
        if (!this.GetComponent<EnemyController>().notInRoom )
        {
            if (!checkTurret(turret))
                this.GetComponent<SpriteRenderer>().enabled = true;
            if (!coolDownAttack && !checkTurret(turret))
            {
                this.GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(CoolDown());
                for (int i = 0; i < 4; i++)
                {
                    turret[i] = Instantiate(turretPrefab, roomCentre + turretSpawn[i], Quaternion.identity) as GameObject;
                    turret[i].GetComponent<EnemyController>().notInRoom = false;
                }

            }
        }
    }
    public Boolean checkTurret(GameObject[] turrets)
    {
        Boolean check = false;
        foreach(GameObject tur in turrets)
        {
            if (tur != null)
                check = true;
        }

        return check;
    }
    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(20f);
        coolDownAttack = false;
    }
}
