using UnityEngine;

public class GroundController : MonoBehaviour
{
    private float speed = 100.0f;

    void Update()
    {
        if (GameManager.Instance.getGameCode() == "Start")
        {
            MoveMap();
        }

        if(transform.position.z <= -446)
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
        gameObject.SetActive(false);
        MapManager.Instance.SpawnRoad();
    }
}