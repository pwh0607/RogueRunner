using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController1 : MonoBehaviour
{
    private ObjectPoolingSC objectPool;

    float force = 6000f;
    float dynamicVal = 1f;
    int stage;
    private Rigidbody rb;
    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        objectPool = transform.parent.GetComponent<ObjectPoolingSC>();
        stage = GameManager.Instance.gameState.stage;
        dynamicVal = 100 * GameManager.Instance.gameState.stage;
    }

    void Update()
    {
        if (transform.position.z <= -450)
        {
            objectPool.ReturnObject(gameObject);
        }

        if (GameManager.Instance.getGameCode() == "Pause" || StageManager.Instance.isStopSkilled)
        {
            if (!rb.isKinematic)
            {
                savedVelocity = rb.velocity;
                savedAngularVelocity = rb.angularVelocity;
                rb.isKinematic = true;
            }
        }
        else
        {
            if (rb.isKinematic)
            {
                rb.isKinematic = false;

                rb.velocity = savedVelocity;
                rb.angularVelocity = savedAngularVelocity;
            }
        }

        if(GameManager.Instance.getGameCode() == "StageClear")
        {
            objectPool.ReturnObject(gameObject);
        }
    }

    private void OnEnable()
    {
        Vector3 forceDir = new Vector3(0, 0, -1);
        rb.AddForce((force + dynamicVal) * forceDir);
    }

    private void OnDisable()
    {
        if(rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "BOMB")
        {
            objectPool.ReturnObject(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
