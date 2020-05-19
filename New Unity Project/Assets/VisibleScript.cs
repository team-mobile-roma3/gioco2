using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleScript : MonoBehaviour
{
    void OnBecameVisible()
    {
        Debug.Log(this + "sono visible");
 //     this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
        void OnBecameInvisible()
    {
        Debug.Log(this +" non sono visible");
      //  this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
