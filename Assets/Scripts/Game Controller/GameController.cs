using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum tagBullets
{
    Player, Enemy
}
public class GameController : MonoBehaviour
{
    [Header("Private Settings")]
    public CharacterController _characterController;

    [Header("Player Bullets Settings")]
    public GameObject[] bulletPrefab;
    

    [Header("Enemy Bullets Prefabs")]
    public GameObject[] enemyBulletPrefab;

    [Header("SFX Prefabs")]
    public GameObject explosionPrefab;

    [Header("Loots Settings")]
    public GameObject[] lootPrefabs;
    public float cooldownToShowLoots;

    [Header("Player Settings")]
    public GameObject[] playerPrefab;

    public int idPlayerPlane;
    public int currentLife;
    public int maxLife;
    public float cooldownSpawnPlayer;
    public float cooldownNoDamage;
    public bool isGodModeOn;
    public bool isPlayerAlive;
  
    public Transform playerSpawnLocation;
    public Transform upLimit;
    public Transform downLimit;
    public Transform leftLimit;
    public Transform rightLimit;

    [Header("Camera Settings")]
    public Camera mainCamera;
    public Transform leftCameraLimit;
    public Transform rightCameraLimit;
    public Transform lastCameraPosition;
    public float cameraSpeedMove;
    public float sceneMoveSpeed;


    void Start()
    {
        
    }

    void Update()
    {
        if(isPlayerAlive == true)
        {
            playerMoveLimit();
            //cameraPositionControll();
        }
        

    }
    private void FixedUpdate()
    {
        if (isPlayerAlive == true)
        {
           
            cameraPositionControll();
        }
    }

    private void LateUpdate()
    {
        //cameraPositionControll();
        

        
        sceneMovement();
    }

    void cameraPositionControll()
    {
        if (mainCamera.transform.position.x > leftCameraLimit.position.x && mainCamera.transform.position.x < rightCameraLimit.position.x)
        {
            moveCamera();
        }
        else if (mainCamera.transform.position.x <= leftCameraLimit.position.x && _characterController.transform.position.x > leftCameraLimit.position.x)
        {
            moveCamera();
        }
        else if (mainCamera.transform.position.x >= rightCameraLimit.position.x && _characterController.transform.position.x < rightCameraLimit.position.x)
        {
            moveCamera();
        }
    }

    void moveCamera()
    {
            Vector3 newCameraPosition = new Vector3(_characterController.transform.position.x, mainCamera.transform.position.y, -10f);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newCameraPosition, cameraSpeedMove * Time.deltaTime);
    }

    void sceneMovement()
    {
        mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, new Vector3(mainCamera.transform.position.x, lastCameraPosition.position.y, -10f), sceneMoveSpeed * Time.deltaTime);
    }

    void playerMoveLimit()
    {
        float posY = _characterController.transform.position.y;
        float posX = _characterController.transform.position.x;

        if (posY > upLimit.transform.position.y)
        {
            _characterController.transform.position = new Vector3(posX, upLimit.transform.position.y, 0);
        }
        else if (posY < downLimit.transform.position.y)
        {
            _characterController.transform.position = new Vector3(posX, downLimit.transform.position.y, 0);
        }

        if (posX > rightLimit.transform.position.x)
        {
            _characterController.transform.position = new Vector3(rightLimit.transform.position.x, posY, 0);
        }
        else if (posX < leftLimit.transform.position.x)
        {
            _characterController.transform.position = new Vector3(leftLimit.transform.position.x, posY, 0);
        }
    }
    public string tagAplication(tagBullets tag)
    {
        string returno = null;

        switch (tag)
        {
            case tagBullets.Player:
                returno = "playerShoot";
                break;
            case tagBullets.Enemy:
                returno = "enemyShoot";
                break;
        }
        return returno;
    }
    public void hitPlayer()
    {
        Instantiate(explosionPrefab, _characterController.transform.position, explosionPrefab.transform.localRotation);        
        if (isGodModeOn == false)
        {
            Destroy(_characterController.gameObject);
            isPlayerAlive = false;
            currentLife--;

            if (currentLife >= 0)
            {
                StartCoroutine("delaySpawnPlayer");
                //yield return new WaitForSecondsRealtime(cooldownSpawnPlayer);
                //Instantiate(playerPrefab[idPlayerPlane], playerSpawnLocation.position, playerSpawnLocation.localRotation);
                //isPlayerAlive = true;
            }
            else
            {
                print("Game Over");
            }
        }       
    }
    IEnumerator delaySpawnPlayer()
    {
        yield return new WaitForSecondsRealtime(cooldownSpawnPlayer);
        GameObject temp= Instantiate(playerPrefab[idPlayerPlane], playerSpawnLocation.position, playerSpawnLocation.localRotation);
        yield return new WaitForEndOfFrame();
        _characterController.StartCoroutine("spawnNoDamage");
        isPlayerAlive = true;
    }
}