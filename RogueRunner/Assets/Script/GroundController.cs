using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    private float speed = 100.0f;
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.getStart())
        {
            MoveMap();
        }

        if(this.transform.position.z <= -446)
        {
            DesMap();
        }
    }
    void MoveMap()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }
    void DesMap()
    {
        //지금 오브젝트 삭제
        //MapManager에 삭제되었음을 알린다.
        MapManager.Instance.SpawnRoad();
        Destroy(gameObject);
    }
}