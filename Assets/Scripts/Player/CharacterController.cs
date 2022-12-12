using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Private Settings")]
    private GameController _gameController;
    private Rigidbody2D playerRB;
    

    [Header("Player Bullets")]
    public float bulletSpeed;
    public float bulletDistanceToDestroy;
    public float bulletShootTimer;
    private bool isShooting;

    [Header("Player Stats")]
    public float speedMove;
    
    
    void Start()
    {
        _gameController = FindObjectOfType(typeof(GameController)) as GameController;
        playerRB = GetComponent<Rigidbody2D>();        
    }

    
    void Update()
    {
        playerLocomotion();
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
        GameObject temp = Instantiate(_gameController.bulletPrefab);
        temp.transform.position = _gameController.weaponPosition.position;
        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
        StartCoroutine ("shootCooldown");
        Destroy(temp, bulletDistanceToDestroy);
        

    }

    IEnumerator shootCooldown()
    {
        yield return new WaitForSecondsRealtime(bulletShootTimer);
        isShooting = false;
    }
}
