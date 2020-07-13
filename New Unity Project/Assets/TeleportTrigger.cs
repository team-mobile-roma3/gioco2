using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportTrigger : MonoBehaviour
{
    public Vector2 pos;
    public Boolean toBS;
    public Boolean tutorial;
    private Boolean activated;
    public Animator animator;
    private void Awake()
    {
        activated = false;
        if (toBS)
        {    
            pos = new Vector2(4, 216);
            GameObject.Find("BS").GetComponent<TeleportTrigger>().pos = new Vector2(this.transform.position.x, this.transform.position.y);
        }
        StartCoroutine(spawnDelay());

    }
    public void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated)
        {
            if (collision.CompareTag("Player") )
            {
                if (!tutorial)
                {
                    collision.transform.position = pos;
                    GameObject.Find("leftLeg").transform.position = pos;
                    GameObject.Find("rightLeg").transform.position = pos;
                    if(GameObject.Find("ErPet") != null)
                    GameObject.Find("ErPet").transform.position = pos;
                    GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(true);
                    GameObject.Find("Canvas").transform.GetChild(1).gameObject.SetActive(false);
                    GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(false);
                    GameObject.Find("Canvas").transform.GetChild(3).gameObject.SetActive(false);
                    GameObject.Find("Canvas").transform.GetChild(4).gameObject.SetActive(false);
                    collision.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    StartCoroutine(Wait(collision));
                }
                else
                {
                    SceneManager.LoadScene(19);
                }

            }

        }
    }
    IEnumerator spawnDelay()
    {
 
        yield return new WaitForSeconds(2f);
        activated = true;
        

    }
    IEnumerator Wait(Collider2D collision)
    {

        yield return new WaitForSeconds(3f);

        GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.GetChild(1).gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.GetChild(3).gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.GetChild(4).gameObject.SetActive(true);
        collision.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        if (toBS)
            Destroy(gameObject); 

    }
}
