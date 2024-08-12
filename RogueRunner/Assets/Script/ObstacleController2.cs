using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController2 : MonoBehaviour
{
    //따라갈 player 오브젝트.
    private GameObject player;
    private Transform playerTrans;
    private float speed = 100f;

    private Rigidbody rb;
    private Vector3 savedVelocity; // 속도를 저장할 변수
    private Vector3 savedAngularVelocity; // 각속도를 저장할 변수

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerTrans = player.transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.getPaused())
        {
            //player를 따라 이동하기.
            if (player != null)
            {
                MoveToPlayer();
            }
        }

        if (GameManager.Instance.getPaused() || SceneManager.Instance.isStopSkilled)
        {
            if (!rb.isKinematic)
            {
                // 현재 속도와 각속도를 저장
                savedVelocity = rb.velocity;
                savedAngularVelocity = rb.angularVelocity;

                // Rigidbody를 일시적으로 멈춤
                rb.isKinematic = true;
            }
        }
        else
        {
            if (rb.isKinematic)
            {
                // Rigidbody 다시 활성화
                rb.isKinematic = false;

                // 저장된 속도와 각속도를 복원
                rb.velocity = savedVelocity;
                rb.angularVelocity = savedAngularVelocity;
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

        //dir이 특정 범위안에 (z축 기준.)
        if (transform.position.z > -245)
        {
            Vector3 dir = (playerTrans.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
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
