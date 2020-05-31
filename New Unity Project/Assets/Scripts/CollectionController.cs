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

    public GameObject floatingTextPrefab;
    public Item item;
    public float healthChange;
    public float moveSpeedChange;
    public float attackSpeedChange;
    public float bulletSizeChange;
    public float mDamageChange;
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
        if (collision.tag == "Player" && floatingTextPrefab != null)
            showFloatingText();
        if (collision.tag == "Player" && gameObject.tag == "Potion")
        {
            Inventory.PotionsChange();
            Destroy(gameObject);
            return;
        }
        if (collision.tag == "Player" && gameObject.CompareTag("wRanged"))
        {
            if (gameObject.name != Inventory.Ranged_Weapon)
            {
                Inventory.wRangedChange(gameObject.name);
                GameController.FireRateChange(attackSpeedChange);
                GameController.BulletSizeChange(bulletSizeChange);
                GameController.IPBChange(isProjectileBouncy);
                GameController.instance.UpdateCollectedItems(this);
                Destroy(gameObject);
            }
            return;
        }
        if (collision.tag == "Player" && gameObject.CompareTag("wMelee"))
        {
            if (gameObject.name != Inventory.Melee_Weapon)
            {
                Inventory.wMeleeChange(gameObject.name);
                GameController.FireRateChange(attackSpeedChange);
                GameController.BulletSizeChange(bulletSizeChange);
                GameController.IPBChange(isProjectileBouncy);
                GameController.MAttackDamageChange(mDamageChange);
                GameController.instance.UpdateCollectedItems(this);
                Destroy(gameObject);
            }
            return;
        }
        if (collision.tag == "Player" && gameObject.CompareTag("Coin"))
        {
            Inventory.CoinsChange();
            Destroy(gameObject);
           
        }
        if (collision.tag == "Player" && gameObject.CompareTag("Boots"))
        {
            GameController.MoveSpeedChange(moveSpeedChange);
            Destroy(gameObject);
        }
    }
    public void showFloatingText()
    {
       var go = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity) as GameObject;

        go.GetComponent<TextMesh>().text = item.description;
    }

}
