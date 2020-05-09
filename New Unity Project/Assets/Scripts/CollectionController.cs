using System.Collections;

using UnityEngine;


[System.Serializable]
public class Item
{
    public string name;
    public string description;
    public Sprite itemImage;
}

public class CollectionController : MonoBehaviour
{

    public Item item;
    public float healthChange;
    public float moveSpeedChange;
    public float attackSpeedChange;
    public float bulletSizeChange;
    public bool isProjectileBouncy;
    private bool coolDownPick = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
        gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!coolDownPick)
        {
            if (collision.tag == "Player" && gameObject.tag == "Potion")
            {
                Inventory.PotionsChange();
                Destroy(gameObject);
                StartCoroutine(CoolDownPick());

                return;

            }
            // devo aggiungere che se ho già l'item, non lo prendo OPPURE lo prendo e non fa effetto (distruggendolo)
            if (collision.tag == "Player")
            {

//                GameController.HealPlayer(healthChange);
                GameController.MoveSpeedChange(moveSpeedChange);
                GameController.FireRateChange(attackSpeedChange);
                GameController.BulletSizeChange(bulletSizeChange);
                GameController.IPBChange(isProjectileBouncy);
                GameController.instance.UpdateCollectedItems(this);
                Destroy(gameObject);
                StartCoroutine(CoolDownPick());
            }
        }
    }

    private IEnumerator CoolDownPick()
    {
        coolDownPick = true;
        yield return new WaitForSeconds(1f);
        coolDownPick = false;

    }
}
