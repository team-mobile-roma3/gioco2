using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    private static float health = 6;
    private static int maxHealth = 6;
    private static float moveSpeed = 5f;
    private static float fireRate = 0.5f;
    private static float bulletSize = 0.5f;
    private static bool isProjectileBouncy = false;
    private static float attackDamage = 1;
    private static float mAttackDamage = 2;
    private static bool stance = false;

    private bool bootCollected = false;
    private bool screwCollected = false;

    public List<string> collectedNames = new List<string>();

    public static float Health { get => health; set => health = value; }

    public static float AttackDamage { get => attackDamage; set => attackDamage = value; }
    public static float MAttackDamage { get => mAttackDamage; set => mAttackDamage = value; }
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static float BulletSize { get => bulletSize; set => bulletSize = value; }
    public static bool IPB { get => isProjectileBouncy; set => isProjectileBouncy = value; }
    public static bool Stance { get => stance; set => stance = value; }



    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        health = 6;
        maxHealth = 6;
        moveSpeed = 5f;
        fireRate = 0.5f;
        bulletSize = 0.5f;
        isProjectileBouncy = false;
        attackDamage = 1;
        mAttackDamage = 2;
        stance = false;
        bootCollected = false;
        screwCollected = false;
}
    // Update is called once per frame


    public static void DamagePlayer(int damage)
    {
        health -= damage;

        if(Health <= 0)
        {
            KillPlayer();
        }
    }

    public static void HealPlayer(float healAmount)
    {
        health = Mathf.Min(maxHealth, health + healAmount);
    }

    public static void AttackDamageChange(float value)
    {
        attackDamage = value;
    }
    public static void MAttackDamageChange(float value)
    {
        mAttackDamage = value;
    }
    public static void MoveSpeedChange(float speed)
    {
        moveSpeed = speed;
    }

    public static void FireRateChange(float rate)
    {
        fireRate = rate;
    }
    public static void BulletSizeChange(float size)
    {
        bulletSize = size;
    }
    public static void IPBChange(bool cond)

    {
        isProjectileBouncy = cond;
    }

    public void UpdateCollectedItems(CollectionController item)
    {
        collectedNames.Add(item.item.name);

        foreach(string i in collectedNames)
        {
            switch(i)
            {
                case "Boot":
                    bootCollected = true;
                break;
                case "Screw":
                    screwCollected = true;
                break;
            }
        }

        if(bootCollected && screwCollected)
        {
            FireRateChange(0.25f);
        }
    }

    private static void KillPlayer()
    {
        SceneManager.LoadScene(0);
    }
}
