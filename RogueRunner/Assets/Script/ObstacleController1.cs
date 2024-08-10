using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController1 : MonoBehaviour
{
    float force = 8000f;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 forceDir = new Vector3(0, 0, -1);
        gameObject.GetComponent<Rigidbody>().AddForce(force * forceDir);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.getStart())
        {
            if (gameObject.transform.position.z <= -450)
            {
                Destroy(gameObject);
            }
        }
    }
}
