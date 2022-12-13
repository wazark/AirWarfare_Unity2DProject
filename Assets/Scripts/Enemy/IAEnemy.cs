using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public enum direction 
{
    Up, Down, Left, Right
}

public class IAEnemy : MonoBehaviour
{
    [Header("AI Movement Settings")]
    public float speedMove;
    public float[] placeToCurve;
    public float curveDegress;
    public float increment;
    public direction movDirection;

    [Header("AI Checks")]
    public bool isMultiplesCurves; // working in progress;    
    

    [Header("Privates")]    
    private float incremented;    
    private float zRotation;    
    private bool isCurve;    
    private bool isArrived;
    private bool leftSide;


    void Start()
    {
        zRotation = transform.eulerAngles.z;
    }

    
    void Update()
    {
        movement();
    }

    void movement()
    {
        
        switch (isMultiplesCurves)
        {
            case false:
                singleMovementationCurve();
                break;

                case true:

                break;
        }
        

    }
    void singleMovementationCurve()
    {
        switch(movDirection)
        {
            case direction.Up:
                if (transform.position.y >= placeToCurve[0] && isCurve == false)
                {
                    isCurve = true;
                }
                locomotionLogic();
                break;
                                
            case direction.Down:
                if (transform.position.y <= placeToCurve[0] && isCurve == false)
                {
                    isCurve = true;
                }
                locomotionLogic();
                break;

            case direction.Left:
                if (transform.position.x >= placeToCurve[0] && isCurve == false)
                {
                    isCurve = true;
                }
                locomotionLogic();
                break;

            case direction.Right:
                if (transform.position.x <= placeToCurve[0] && isCurve == false)
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

    
}
