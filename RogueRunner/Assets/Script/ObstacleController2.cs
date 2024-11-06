using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController2 : MonoBehaviour
{
    private ObjectPoolingSC objectPool;

    int stage;
    protected GameObject player;
    protected float dynamicVal;
    protected Transform playerTrans;
    protected float speed = 100f;

    protected Rigidbody rb;
    protected Vector3 savedVelocity;             
    protected Vector3 savedAngularVelocity;       

    void Start()
    {
        objectPool = transform.parent.GetComponent<ObjectPoolingSC>();

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
            objectPool.ReturnObject(gameObject);
        }

        if (GameManager.Instance.getGameCode() == "StageClear")
        {
            objectPool.ReturnObject(gameObject);
        }
    }

    protected void MoveToPlayer()
    {
        Quaternion fixedRot = gameObject.transform.localRotation;

        if (transform.position.z > -245)
        {
            Vector3 dir = (playerTrans.position - transform.position).normalized;
            transform.position += dir * speed * dynamicVal * Time.deltaTime;
            transform.LookAt(playerTrans);
        }
        else
        {
            transform.rotation = fixedRot;
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
    }
}