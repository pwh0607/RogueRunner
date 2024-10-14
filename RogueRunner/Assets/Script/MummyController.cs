using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyController : ObstacleController2
{
    void Update()
    {
        if (GameManager.Instance.getGameCode() == "Start" && !StageManager.Instance.isStopSkilled)
        {
            if (player != null)
            {
                MoveToPlayer();
            }
        }

        if (gameObject.transform.position.z <= -450)
        {
            Destroy(gameObject);
        }

        if (GameManager.Instance.getGameCode() == "StageClear")
        {
            Destroy(gameObject);
        }
    }
}
