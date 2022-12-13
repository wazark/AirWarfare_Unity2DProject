using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterController : MonoBehaviour
{
    [Header("Private Settings")]
    private GameController _gameController;
    private IAEnemy _aiEnemy;
    private Rigidbody2D playerRB;


    [Header("Player Bullets")]
    public Transform playerWeapon;
    public tagBullets tagShot;
    public float bulletSize;
    public int idBullet;
    public float bulletSpeed;    
    public float bulletShootTimer;
    private bool isShooting;

    [Header("Player Stats")]
    public float speedMove;
    
    
    void Start()
    {
        _gameController = FindObjectOfType(typeof(GameController)) as GameController;

        _gameController._characterController = this;        
        _gameController.isPlayerAlive = true;
        

        playerRB = GetComponent<Rigidbody2D>();        
    }

    
    void Update()
    {
        playerLocomotion();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "enemyShoot":                
                _gameController.hitPlayer();
                Destroy(collision.gameObject);                
                break;
        }
    }    
    void playerLocomotion()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

       playerRB.velocity = new Vector2(horizontal * speedMove, vertical * speedMove);

        if(Input.GetButton("Fire1") && isShooting == false)
        {
            playerShoot();
        }
    }

    void buffSpeddMove(float buffSpeedMove)
    {
        speedMove += buffSpeedMove;
    }

    void playerShoot()
    {
        isShooting = true;
        GameObject temp = Instantiate(_gameController.bulletPrefab[idBullet]);
        temp.transform.tag = _gameController.tagAplication(tagShot);
        temp.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
        temp.transform.position = playerWeapon.position;
        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
        StartCoroutine ("shootCooldown");
    }

    IEnumerator shootCooldown()
    {
        yield return new WaitForSecondsRealtime(bulletShootTimer);
        isShooting = false;
    }
   
}
