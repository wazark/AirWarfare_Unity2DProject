using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class EnemyJourney : MonoBehaviour
{
    public Transform enemy;
    public Transform[] checkPoints;
    public float speedMove;
    public float delayStoped;

    private int idCheckPoint;
    private bool isMoving;
    void Start()
    {
        StartCoroutine("startMove");
    }

    
    void Update()
    {
        if(isMoving == true)
        {
            enemy.position = Vector3.MoveTowards(enemy.position, checkPoints[idCheckPoint].position, speedMove * Time.deltaTime);
            if(enemy.position == checkPoints[idCheckPoint].position )
            {
                isMoving = false;
                StartCoroutine("startMove");
            }
        }
    }

    IEnumerator startMove()
    {
        idCheckPoint++;
        if (idCheckPoint == checkPoints.Length)
        {
            idCheckPoint = 0;
        }
        yield return new WaitForSeconds(delayStoped);
        
        isMoving= true;
    }
}
