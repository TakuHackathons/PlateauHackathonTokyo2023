using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonBehaviour<GameController>
{
    [SerializeField] private PlayerBall playerBall;

    // Start is called before the first frame update
    void Start()
    {
        playerBall.returnMainCamera();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerBall.followPlayerCamera();
            playerBall.ThrowBall(new Vector3(0,0,10));
        }
    }
}
