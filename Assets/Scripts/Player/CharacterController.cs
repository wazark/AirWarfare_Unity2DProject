using System.Collections;
using UnityEngine;


public class CharacterController : MonoBehaviour
{
    [Header("Private Settings")]
    private GameController _gameController;
    private AIPlaneEnemy _aiEnemy;
    public Rigidbody2D playerRB;
    private SpriteRenderer playerSR;

    [Header("Player GameObjects")]
    public Transform playerWeapon;
    public Transform gasFog;
    public GameObject playerShadow;


    [Header("Player Bullets")]
    public tagBullets tagShot;
    public float bulletSize;
    public int idBullet;
    public float bulletSpeed;
    public float bulletShootTimer;
    public float gasFogSpeed;
    private bool isShooting;
    public Color noDamgeColor;
    public Color gasFogColor;
    public Color initialGasFogColor;

    [Header("Player Stats")]
    public float speedMove;
    public bool isPlayerControlling;


    void Start()
    {
        _gameController = FindObjectOfType(typeof(GameController)) as GameController;

        _gameController._characterController = this;
        _gameController.isPlayerAlive = true;


        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();

    }


    void Update()
    {
        playerLocomotion();
        if (Input.GetKeyDown(KeyCode.Space)) // suicide for teste
        {
            _gameController.hitPlayer();
        }
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

        if (_gameController.currentState == gameState.GamePlay)
        {
            isPlayerControlling = true;
            playerRB.velocity = new Vector2(horizontal * speedMove, vertical * speedMove);
        }

        if (Input.GetButton("Fire1") && isShooting == false && _gameController.currentState == gameState.GamePlay)
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
        StartCoroutine("shootCooldown");
    }

    IEnumerator shootCooldown()
    {
        yield return new WaitForSecondsRealtime(bulletShootTimer);
        isShooting = false;
    }

    IEnumerator spawnNoDamage()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;
        playerSR.color = noDamgeColor;
        gasFog.GetComponent<SpriteRenderer>().color = noDamgeColor;
        StartCoroutine("respawnVisualEffect");

        yield return new WaitForSecondsRealtime(_gameController.cooldownNoDamage);
        col.enabled = true;
        playerSR.color = Color.white;
        gasFog.GetComponent<SpriteRenderer>().color = gasFogColor;
        playerSR.enabled = true;
        gasFog.GetComponent<SpriteRenderer>().enabled = true;
        StopCoroutine("respawnVisualEffect");


    }
    IEnumerator respawnVisualEffect()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        playerSR.enabled = !playerSR.enabled;
        gasFog.GetComponent<SpriteRenderer>().enabled = !gasFog.GetComponent<SpriteRenderer>().enabled;

        StartCoroutine("respawnVisualEffect");

    }

}
