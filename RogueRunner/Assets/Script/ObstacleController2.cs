using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController2 : MonoBehaviour
{
    //���� player ������Ʈ.
    private GameObject player;
    private Transform playerTrans;
    private float speed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerTrans = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.getStart())
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
    }

    void MoveToPlayer()
    {
        Quaternion fixedRot = gameObject.transform.localRotation;

        //dir�� Ư�� �����ȿ� (z�� ����.)
        if (transform.position.z > -245)
        {
            Vector3 dir = (playerTrans.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
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
