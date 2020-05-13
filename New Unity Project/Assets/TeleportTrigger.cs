using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = new Vector2(200, 0);
            GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.GetChild(1).gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.GetChild(3).gameObject.SetActive(false);
            StartCoroutine(Wait());
        }
    }
    IEnumerator Wait()
    {

        yield return new WaitForSeconds(3f);

        GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.GetChild(1).gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.GetChild(3).gameObject.SetActive(true);

    }
}
