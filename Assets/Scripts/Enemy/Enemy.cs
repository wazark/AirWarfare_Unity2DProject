using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Private Settings")]
    private GameController _gameController;
    private CharacterController _characterController;

    [Header("Enemy Bullet Settings")]
    public float bulletSpeed;
    public float shootTimer;
    public float reloadCooldown;

    public int amountShoot;
    public int maxShoot;

    private bool isShooting;
    public bool isRandomReload;

    

    void Start()
    {
    _gameController = FindObjectOfType(typeof(GameController)) as GameController;
    _characterController = FindObjectOfType(typeof(CharacterController)) as CharacterController;
        StartCoroutine("enemyShootTime");
    }
    
    void Update()
    {
        

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "playerShoot":

                Destroy(collision.gameObject);
                GameObject temp = Instantiate(_gameController.explosionPrefab, transform.position, transform.localRotation);
                Destroy(temp, 0.5f);

                spawnLoot();

                Destroy(this.gameObject);

                break;

            case "Player":

                temp = Instantiate(_gameController.explosionPrefab, transform.position, transform.localRotation);
                Destroy(temp, 0.5f);

                Destroy(this.gameObject);

                break;
        }
    }

    void spawnLoot()
    {
        int idItem;
        int rand = Random.Range(0, 100);
        if (rand < 50)
        {
           rand = Random.Range (0,100);
            if(rand > 85) 
            {
                idItem = 2; // BombBox
            }
            else if( rand > 50)
            {
                idItem= 1; // HealthBox
            }
            else
            {
                idItem = 0; // CoinBox
            }

            Instantiate(_gameController.loot[idItem], transform.position, transform.localRotation);
        }        
    }

    void Shoot()
    {
        if (isShooting == false && amountShoot > 0)
        {
            _gameController.enemyWeapon.right = _characterController.transform.position - transform.position;
            amountShoot--;
            GameObject temp = Instantiate(_gameController.enemyBulletPrefab, _gameController.enemyWeapon.position, _gameController.enemyWeapon.localRotation);
            temp.GetComponent<Rigidbody2D>().velocity = _gameController.enemyWeapon.right * bulletSpeed;
            temp.transform.up = _characterController.transform.position - transform.position;
            if(amountShoot <=0)
            {
                amountShoot = 0;
                isShooting= true;
                StartCoroutine("enemyReloadWeapon");
            }

            StartCoroutine("enemyShootTime");
        }
    }

    IEnumerator enemyShootTime()  //delay entre cada tiro
    {
        yield return new WaitForSecondsRealtime(shootTimer);                
        Shoot();
    }
    IEnumerator enemyReloadWeapon() //recarrega os tiros do inimigo conforme a bool de ser random ou não.
    {
        yield return new WaitForSecondsRealtime(reloadCooldown);

        switch(isRandomReload)
        {
            case true:

                amountShoot = Random.Range(1, maxShoot);
                isShooting = false;
                Shoot();

                break;

            case false:
                amountShoot = maxShoot;
                isShooting = false;
                Shoot();
                break;
        }
        
    }


}
