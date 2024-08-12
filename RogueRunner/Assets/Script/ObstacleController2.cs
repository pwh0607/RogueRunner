using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController2 : MonoBehaviour
{
    //���� player ������Ʈ.
    private GameObject player;
    private Transform playerTrans;
    private float speed = 100f;

    private Rigidbody rb;
    private Vector3 savedVelocity; // �ӵ��� ������ ����
    private Vector3 savedAngularVelocity; // ���ӵ��� ������ ����

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
            //player�� ���� �̵��ϱ�.
            if (player != null)
            {
                MoveToPlayer();
            }
        }

        if (GameManager.Instance.getPaused() || SceneManager.Instance.isStopSkilled)
        {
            if (!rb.isKinematic)
            {
                // ���� �ӵ��� ���ӵ��� ����
                savedVelocity = rb.velocity;
                savedAngularVelocity = rb.angularVelocity;

                // Rigidbody�� �Ͻ������� ����
                rb.isKinematic = true;
            }
        }
        else
        {
            if (rb.isKinematic)
            {
                // Rigidbody �ٽ� Ȱ��ȭ
                rb.isKinematic = false;

                // ����� �ӵ��� ���ӵ��� ����
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
