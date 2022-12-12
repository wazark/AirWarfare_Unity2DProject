using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Private Settings")]
    private Rigidbody2D playerRB;

    [Header("Player Settings")]
    public float speedMove;
    
    
    void Start()
    {
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
    }

    void buffSpeddMove(float buffSpeedMove)
    {
        speedMove += buffSpeedMove;
    }
}
