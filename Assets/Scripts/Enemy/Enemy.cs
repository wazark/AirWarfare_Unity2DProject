using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameController _gameController;
    

    void Start()
    {
    _gameController = FindObjectOfType(typeof(GameController)) as GameController;    
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
}
