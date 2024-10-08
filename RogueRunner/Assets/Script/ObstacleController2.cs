using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ObstacleController2 : MonoBehaviour
{
    //따라갈 player 오브젝트.

    int stage;

    protected GameObject player;
    protected float dynamicVal;
    protected Transform playerTrans;
    protected float speed = 100f;

    protected Rigidbody rb;
    protected Vector3 savedVelocity;              // 속도를 저장할 변수
    protected Vector3 savedAngularVelocity;       // 각속도를 저장할 변수

    void Start()
    {
        stage = GameManager.Instance.gameState.stage;
        player = GameObject.FindWithTag("PLAYER");
        playerTrans = player.transform;
        dynamicVal = 1 + GameManager.Instance.gameState.stage * 0.2f;
        rb = GetComponent<Rigidbody>();
    }

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

    protected void MoveToPlayer()
    {
        Quaternion fixedRot = gameObject.transform.localRotation;

        //dir이 특정 범위안에 (z축 기준.)
        if (transform.position.z > -245)
        {
            Vector3 dir = (playerTrans.position - transform.position).normalized;
            transform.position += dir * speed * dynamicVal * Time.deltaTime;
            transform.LookAt(playerTrans);
        }
        else
        {
            //계속 z 축으로 이동
            transform.rotation = fixedRot;
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
    }
}