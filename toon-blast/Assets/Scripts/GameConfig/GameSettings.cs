using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{

    [SerializeField] private GameConfig gameConfig;

    public static GameConfig GameConfig => instance.gameConfig;

    public static GameSettings instance;

    private void Awake()
    {
        
        if(instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
