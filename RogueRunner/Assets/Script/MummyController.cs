using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ֹ� �ڵ忡�� ��ӹޱ�
public class MummyController : ObstacleController2
{
    // ObstacleController2�κ��� �̹� ���õ� ���� ����Ѵ�.
    void Update()
    {
        if (GameManager.Instance.getGameCode() == "Start" && !StageManager.Instance.isStopSkilled)
        {
            //player�� ���� �̵��ϱ�.
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
