using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum lootInGame
{
    Coins, Health, Bombs
}

public class LootSystem : MonoBehaviour
{
    public lootInGame lootType;
    private GameController _gameController;

    void Start()
    {
      switch(lootType)
        {
            case lootInGame.Coins:

                break;

            case lootInGame.Health:


                break;

            case lootInGame.Bombs:

                break;

        }
    }

    
    void Update()
    {
        
    }
}
