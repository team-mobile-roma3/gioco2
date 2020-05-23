using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFarAway : MonoBehaviour
{

    // --------------------------------------------------
    // Variables:

    private GameObject itemActivatorObject;
    private ItemActive activationScript;

    // --------------------------------------------------

    void Start()
    {
        itemActivatorObject = GameObject.Find("ItemActivator");
        activationScript = itemActivatorObject.GetComponent<ItemActive>();
 //       StartCoroutine("Wait");
        StartCoroutine("AddToList");
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(15f);
    }
    IEnumerator AddToList()
    {
        yield return new WaitForSeconds(0.1f);

        activationScript.addList.Add(new ActivatorItem { item = this.gameObject });
    }
}