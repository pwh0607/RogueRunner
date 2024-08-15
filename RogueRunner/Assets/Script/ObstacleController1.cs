using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController1 : MonoBehaviour
{
    float force = 8000f;
    private Rigidbody rb;
    private Vector3 savedVelocity; // 속도를 저장할 변수
    private Vector3 savedAngularVelocity; // 각속도를 저장할 변수

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 forceDir = new Vector3(0, 0, -1);
        rb.AddForce(force * forceDir);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= -450)
        {
            Destroy(gameObject);
        }

        if (GameManager.Instance.getGameCode() =="Pause" || SceneManager.Instance.isStopSkilled)
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "BOMB")
        {
            //폭탄에 부딪히면 물체 삭제
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
