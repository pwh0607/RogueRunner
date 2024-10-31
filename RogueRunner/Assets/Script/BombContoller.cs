using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BombContoller : MonoBehaviour
{
    float force = 2000f;
    void Start()
    {
        Vector3 dir = new Vector3(0, 0.3f, 1);
        gameObject.GetComponent<Rigidbody>().AddForce(force * dir);

        //2초뒤 제거.
        Destroy(gameObject, 3.0f);
    }

    void Update()
    {
        if (GameManager.Instance.getGameCode() != "Paused" || !StageManager.Instance.isStopSkilled)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
