using System.Collections;
using UnityEngine;

public enum direction
{
    Up, Down, Left, Right
}
public enum shootTimerSelect
{
    Fixed, Random
}
public enum reloadTimerSelect
{
    Fixed, Random
}
public enum ammoReloadSelect
{
    Fixed, Random
}
public enum startShoot
{
    immediately, wait
}

public class AIPlaneEnemy : MonoBehaviour
{
    [Header("Private Settings")]
    private GameController _gameController;

    [Header("AI Movement Settings")]
    public float speedMove;
    public float placeToCurve;
    public float curveDegress;
    public float increment;
    public direction movDirection;

    [Header("Privates")]
    private float incremented;
    private float zRotation;
    private bool isCurve;
    private bool isArrived;
    private bool leftSide;
    private bool isShooting;
    private bool isAiVisible;

    [Header("Weapon Transform")]
    public Transform enemyWeapon;

    [Header("Enemy Bullet Settings")]
    public tagBullets tagShot;
    public startShoot timerToStartShoot;
    public float bulletSize;
    public int idBullet;
    public float startShootCooldown;
    public float bulletSpeed;
    public float shootTimer;
    public float reloadCooldown;
    public int amountShoot;
    public int maxShoot;

    [Header("Type of Shoot Settings")]

    public ammoReloadSelect amountAmmoReloaded;
    public int[] amountAmmoToReload;
    public shootTimerSelect shotTimerType;
    public float[] randShootTimer;
    public reloadTimerSelect reloadTimerType;
    public float[] randReloadTimer;


    void Start()
    {
        _gameController = FindObjectOfType(typeof(GameController)) as GameController;



        zRotation = transform.eulerAngles.z;
    }


    void Update()
    {
        singleMovementationCurve();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "playerShoot":

                Destroy(collision.gameObject);
                GameObject temp = Instantiate(_gameController.explosionPrefab, transform.position, _gameController.explosionPrefab.transform.localRotation);

                spawnLoot();

                Destroy(this.gameObject);

                break;

            case "Player":

                temp = Instantiate(_gameController.explosionPrefab, transform.position, _gameController.explosionPrefab.transform.localRotation);

                spawnLoot();
                Destroy(this.gameObject);

                break;
        }
    }

    private void OnBecameVisible()
    {
        StartCoroutine("enemyStartShoot");
        isAiVisible = true;
    }
    private void OnBecameInvisible()
    {
        isAiVisible = false;
    }
    void spawnLoot()
    {
        int idItem;
        int rand = Random.Range(0, 100);
        if (rand < 50)
        {
            rand = Random.Range(0, 100);
            if (rand > 85)
            {
                idItem = 2; // BombBox
            }
            else if (rand > 50)
            {
                idItem = 1; // HealthBox
            }
            else
            {
                idItem = 0; // CoinBox
            }

            Instantiate(_gameController.lootPrefabs[idItem], transform.position, transform.localRotation = new Quaternion(0, 0, 0, 0));
        }
    }

    void singleMovementationCurve()
    {
        switch (movDirection)
        {
            case direction.Up:
                if (transform.position.y >= placeToCurve && isCurve == false)
                {
                    isCurve = true;
                }
                locomotionLogic();
                break;

            case direction.Down:
                if (transform.position.y <= placeToCurve && isCurve == false)
                {
                    isCurve = true;
                }
                locomotionLogic();
                break;

            case direction.Left:
                if (transform.position.x >= placeToCurve && isCurve == false)
                {
                    isCurve = true;
                }
                locomotionLogic();
                break;

            case direction.Right:
                if (transform.position.x <= placeToCurve && isCurve == false)
                {
                    isCurve = true;
                }
                locomotionLogic();
                break;
        }

        transform.Translate(Vector3.down * speedMove * Time.deltaTime);
    }

    void locomotionLogic()
    {
        if (isCurve == true && incremented < curveDegress)
        {
            zRotation += increment;
            transform.rotation = Quaternion.Euler(0, 0, zRotation);

            if (increment < 0)
            {
                incremented += (increment * -1);
            }
            else
                incremented += increment;

            if (isCurve == true && incremented == curveDegress)
            {
                isCurve = false;
            }
        }
    }

    void Shoot()
    {

        if (isShooting == false && amountShoot > 0 && _gameController.isPlayerAlive == true && isAiVisible == true)
        {
            enemyWeapon.right = _gameController._characterController.transform.position - transform.position;
            amountShoot--;
            GameObject temp = Instantiate(_gameController.enemyBulletPrefab[idBullet], enemyWeapon.position, enemyWeapon.localRotation);
            temp.transform.tag = _gameController.tagAplication(tagShot);
            temp.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
            temp.GetComponent<Rigidbody2D>().velocity = enemyWeapon.right * bulletSpeed;
            temp.transform.up = _gameController._characterController.transform.position - transform.position;

            if (amountShoot <= 0)
            {
                amountShoot = 0;
                isShooting = true;
                StartCoroutine("enemyReloadWeapon");
            }

            StartCoroutine("enemyShootTime");
        }
    }

    IEnumerator enemyStartShoot()
    {
        switch (timerToStartShoot)
        {
            case startShoot.immediately:

                yield return new WaitForSecondsRealtime(0.2f);

                break;

            case startShoot.wait:

                yield return new WaitForSecondsRealtime(startShootCooldown);

                break;
        }
        StartCoroutine("enemyShootTime");
    }

    IEnumerator enemyShootTime()  //delay entre cada tiro - random ou não
    {
        switch (shotTimerType)
        {
            case shootTimerSelect.Random:
                yield return new WaitForSecondsRealtime(Random.Range(randShootTimer[0], randShootTimer[1]));
                break;

            case shootTimerSelect.Fixed:
                yield return new WaitForSecondsRealtime(shootTimer);
                break;
        }

        Shoot();
    }
    IEnumerator enemyReloadWeapon() //recarrega os tiros do inimigo conforme a bool de ser random ou não.
    {
        switch (reloadTimerType)
        {
            case reloadTimerSelect.Random:
                yield return new WaitForSecondsRealtime(Random.Range(randReloadTimer[0], randReloadTimer[1]));
                break;

            case reloadTimerSelect.Fixed:
                yield return new WaitForSecondsRealtime(reloadCooldown);
                break;
        }

        switch (amountAmmoReloaded)
        {
            case ammoReloadSelect.Random:

                amountShoot = Random.Range(amountAmmoToReload[0], amountAmmoToReload[1]);
                isShooting = false;
                Shoot();

                break;

            case ammoReloadSelect.Fixed:
                amountShoot = maxShoot;
                isShooting = false;
                Shoot();
                break;
        }

    }

}
