using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//장애물 코드에서 상속받기
public class MummyController : ObstacleController2
{
    // ObstacleController2로부터 이미 세팅된 값을 사용한다.
    void Update()
    {
        if (GameManager.Instance.getGameCode() == "Start" && !StageManager.Instance.isStopSkilled)
        {
            //player를 따라 이동하기.
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
