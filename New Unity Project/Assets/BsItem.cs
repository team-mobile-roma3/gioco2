using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BsItem : MonoBehaviour
{
    public int price;
    public GameObject item;
    public TextMesh textPrice;

    // Start is called before the first frame update
    private void Start()
    {
        textPrice.text = "Price: " + price;
            }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Inventory.CoinsCollected >= price)
        {

            Inventory.CoinsCollected -= price;
            item = Instantiate(item.gameObject, transform.position, Quaternion.identity) as GameObject;
            Destroy(gameObject);
        }

    }
}
