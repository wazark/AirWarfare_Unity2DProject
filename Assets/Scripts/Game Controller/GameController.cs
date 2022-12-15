using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum tagBullets
{
    Player, Enemy
}
public enum gameState
{
    Intro, GamePlay
}

public class GameController : MonoBehaviour
{
    [Header("Private Settings")]
    public CharacterController _characterController;

    [Header("Game State Settings")]
    public gameState currentState;

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
    public int currentCoins;
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

    [Header("Intro Settings")]
    public float initialPlaneSize;
    public float planeIncrementSize;
    public float shadowInitialSize;
    public float shadowIncrementSize;
    public float takeoffSpeed;
    public float flyingSpeed;
    public float cooldownToTakeOff;
    public float cooldownToGameplay;
    public bool isTakeOff;
    private bool isAutoPilot;
    private float currentSpeed;
    public Vector3 placeDefaultSize;
    public Vector3 ShadowDefaultSize;
    public Transform planeInitialPosition;

    [Header("User Interface")]
    public Text txtCoins;
    public Text txtLifes;






    void Start()
    {
        StartCoroutine("introGame");
        isAutoPilot = true;
        updateLifeHuD();
        txtCoins.text = currentCoins.ToString();
    }

    void Update()
    {
        if (isPlayerAlive == true)
        {
            playerMoveLimit();
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
        introToGameplay();
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
           updateLifeHuD();

            if (currentLife >= 0)
            {
                StartCoroutine("delaySpawnPlayer");
            }
            else
            {
                print("Game Over");
            }
        }
    }

    void updateLifeHuD()
    {
        if(currentLife <0)
        {
            currentLife= 0;
        }
        else if(currentLife > maxLife) 
        {
            currentLife= maxLife;
        }
        txtLifes.text = "x" + currentLife.ToString();
    }

    public void addCoins(int Points)
    {
        currentCoins += Points;
        txtCoins.text = currentCoins.ToString();
    }

    void introToGameplay()
    {
        if (currentState == gameState.GamePlay)
        {
            

            if (isAutoPilot == true)
            {
                StartCoroutine("automaticPilot");
            }
            else
                sceneMoveSpeed = 0.5f;
            if(mainCamera.orthographicSize <= 5f)
            {
                mainCamera.orthographicSize += 0.01f;
            } 
        }
        else if (currentState == gameState.Intro)
        {
            StartCoroutine("planeTakeOff");
        }
    }
    IEnumerator delaySpawnPlayer()
    {
        yield return new WaitForSecondsRealtime(cooldownSpawnPlayer);
        GameObject temp = Instantiate(playerPrefab[idPlayerPlane], playerSpawnLocation.position, playerSpawnLocation.localRotation);
        yield return new WaitForEndOfFrame();
        _characterController.StartCoroutine("spawnNoDamage");
        isPlayerAlive = true;
    }
    IEnumerator introGame()
    {
        _characterController.gasFog.GetComponent<SpriteRenderer>().enabled = false;
        _characterController.transform.localScale = new Vector3(initialPlaneSize, initialPlaneSize, initialPlaneSize);
        _characterController.transform.position = planeInitialPosition.position;
        _characterController.playerShadow.transform.localScale = new Vector3(shadowInitialSize, shadowInitialSize, shadowInitialSize);

        return null;
    }

    IEnumerator planeTakeOff()
    {
        if (isTakeOff == true)
        {
            takeoffSpeed += 0.005f;
            mainCamera.orthographicSize = 2.0f;
            _characterController.playerRB.velocity = new Vector2(0, takeoffSpeed);

            yield return new WaitForSecondsRealtime(cooldownToTakeOff);

            if (takeoffSpeed >= 2.0f)
            {
                isTakeOff = false;

            }
            StartCoroutine("planeTakeOff");
        }
        _characterController.playerRB.velocity = new Vector2(0, flyingSpeed);
        StartCoroutine("planeSizeEffect");
        _characterController.gasFog.GetComponent<SpriteRenderer>().enabled = true;
    }

    IEnumerator planeSizeEffect()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        if (_characterController.transform.localScale != placeDefaultSize)
        {
            _characterController.transform.localScale += new Vector3(planeIncrementSize, planeIncrementSize, planeIncrementSize);
            _characterController.playerShadow.transform.localScale += new Vector3(shadowIncrementSize, shadowIncrementSize, shadowIncrementSize);
            StartCoroutine("planeSizeEffect");
        }
        else
            currentState = gameState.GamePlay;
        StopCoroutine("planeSizeEffect");
    }

    IEnumerator automaticPilot()
    {
        if (_characterController.transform.position.y >= 15.0f)
        {
            isAutoPilot = false;
            StopCoroutine("automaticPilot");
        }
        else
            _characterController.playerRB.velocity = new Vector2(0, flyingSpeed);
        sceneMoveSpeed = 2.5f;

        yield return null;
    }

}