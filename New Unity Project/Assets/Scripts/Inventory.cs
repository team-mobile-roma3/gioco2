using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    private static string melee_weapon;
    private static string ranged_weapon;
    public Text potionsText;
    private static int nPotions ;  

    

    public static string Melee_Weapon { get => melee_weapon; set => melee_weapon = value; }
    public static string Ranged_Weapon { get => ranged_weapon; set => ranged_weapon = value; }



    void Update()
    {
        potionsText.text = "Potions: " + nPotions;
    }

    public  static  void PotionsChange()
    {
      
        nPotions ++;
    }



    public static void PotionUse()
    {
        if (nPotions > 0)
        {
            nPotions--;
            GameController.HealPlayer(1);
        }
    }
}
