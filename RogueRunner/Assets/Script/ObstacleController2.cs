using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ObstacleController2 : MonoBehaviour
{
    //���� player ������Ʈ.

    int stage;

    protected GameObject player;
    protected float dynamicVal;
    protected Transform playerTrans;
    protected float speed = 100f;

    protected Rigidbody rb;
    protected Vector3 savedVelocity;              // �ӵ��� ������ ����
    protected Vector3 savedAngularVelocity;       // ���ӵ��� ������ ����

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

    protected void MoveToPlayer()
    {
        Quaternion fixedRot = gameObject.transform.localRotation;

        //dir�� Ư�� �����ȿ� (z�� ����.)
        if (transform.position.z > -245)
        {
            Vector3 dir = (playerTrans.position - transform.position).normalized;
            transform.position += dir * speed * dynamicVal * Time.deltaTime;
            transform.LookAt(playerTrans);
        }
        else
        {
            //��� z ������ �̵�
            transform.rotation = fixedRot;
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
    }
}