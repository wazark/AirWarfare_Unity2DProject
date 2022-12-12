using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Player Bullets Settings")]
    public GameObject bulletPrefab;
    public Transform playerWeapon;

    [Header("Enemy Bullets Settings")]
    public GameObject enemyBulletPrefab;
    //public Transform enemyWeapon;

    [Header("SFX Settings")]
    public GameObject explosionPrefab;

    [Header("Loots Settings")]
    public GameObject[] loot;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
