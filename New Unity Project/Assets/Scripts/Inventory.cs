using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    private static string melee_weapon = "default";
    private static string ranged_weapon = "default";
    public Text potionsText;
    private static int nPotions = 0 ;
    public Text coinsText;
    public static int CoinsCollected = 0;



    public static string Melee_Weapon { get => melee_weapon; set => melee_weapon = value; }
    public static string Ranged_Weapon { get => ranged_weapon; set => ranged_weapon = value; }

    private void Start()
    {
        melee_weapon = "default";
        ranged_weapon = "default";
        nPotions = 0;
        CoinsCollected = 0;
    }

void Update()
    {
        potionsText.text = "Potions: " + nPotions;
        coinsText.text = "Coins Collected: " + CoinsCollected;
    }

    public  static  void PotionsChange()
    {
        nPotions ++;
    }
    public static void CoinsChange()
    {

        CoinsCollected ++;
    }

    public static void wRangedChange(string name)
    {
        ranged_weapon = name;
    }

    public static void wMeleeChange(string name)
    {
        melee_weapon = name;
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
