using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController1 : MonoBehaviour
{
    float force = 6000f;
    float dynamicVal = 1f;
    int stage;
    private Rigidbody rb;
    private Vector3 savedVelocity; // �ӵ��� ������ ����
    private Vector3 savedAngularVelocity; // ���ӵ��� ������ ����

    // Start is called before the first frame update
    void Start()
    {
        stage = GameManager.Instance.gameState.stage;
        rb = GetComponent<Rigidbody>();
        Vector3 forceDir = new Vector3(0, 0, -1);
        dynamicVal = 100 * GameManager.Instance.gameState.stage * 0.5f;
        rb.AddForce((force + dynamicVal) * forceDir);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= -450)
        {
            Destroy(gameObject);
        }

        if (GameManager.Instance.getGameCode() == "Pause" || StageManager.Instance.isStopSkilled)
        {
            if (!rb.isKinematic)
            {
                // ���� �ӵ��� ���ӵ��� ����
                savedVelocity = rb.velocity;
                savedAngularVelocity = rb.angularVelocity;

                // Rigidbody�� �Ͻ������� ����
                rb.isKinematic = true;
            }
        }else
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

        if(GameManager.Instance.getGameCode() == "StageClear")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "BOMB")
        {
            //��ź�� �ε����� ��ü ����
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
