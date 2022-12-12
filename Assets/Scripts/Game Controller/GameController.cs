using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Private Settings")]
    private CharacterController _characterController;

    [Header("Player Bullets Settings")]
    public GameObject bulletPrefab;
    public Transform playerWeapon;

    [Header("Enemy Bullets Settings")]
    public GameObject enemyBulletPrefab;    

    [Header("SFX Settings")]
    public GameObject explosionPrefab;

    [Header("Loots Settings")]
    public GameObject[] loot;

    [Header("Loots Settings")]
    public Transform upLimit;
    public Transform downLimit;
    public Transform leftLimit;
    public Transform rightLimit;

    [Header("Camera Settings")]
    public Camera mainCamera;
    public Transform leftCameraLimit;
    public Transform rightCameraLimit;
    public float cameraSpeedMove;

    
    

    void Start()
    {
        _characterController = FindObjectOfType(typeof(CharacterController)) as CharacterController;
    }
        
    void Update()
    {
        playerMoveLimit();
    }

    private void LateUpdate()
    {
        cameraPositionControll();   
    }
    void cameraPositionControll()
    {
       if(mainCamera.transform.position.x > leftCameraLimit.position.x && mainCamera.transform.position.x < rightCameraLimit.position.x)
        {
            moveCamera();
        }
       else if (mainCamera.transform.position.x <= leftCameraLimit.position.x && _characterController.transform.position.x > leftCameraLimit.position.x )
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

    void playerMoveLimit()
    {
        float posY = _characterController.transform.position.y;
        float posX = _characterController.transform.position.x;

        if(posY > upLimit.transform.position.y)
        {
            _characterController.transform.position = new Vector3(posX, upLimit.transform.position.y, 0);
        }
        else if(posY < downLimit.transform.position.y)
        {
            _characterController.transform.position = new Vector3(posX, downLimit.transform.position.y, 0);
        }

        if(posX > rightLimit.transform.position.x) 
        {
            _characterController.transform.position = new Vector3(rightLimit.transform.position.x, posY, 0);
        }
        else if (posX < leftLimit.transform.position.x)
        {
            _characterController.transform.position = new Vector3(leftLimit.transform.position.x, posY, 0);
        }
    }
}
