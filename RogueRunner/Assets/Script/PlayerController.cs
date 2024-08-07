using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //player 스폰위치
    public GameObject SpawnPos;
    private float speed = 10f;
    //체력
    private int HP;
    
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.position = SpawnPos.transform.position;
        HP = 3;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckHP();
    }
    
    void CheckHP()
    {
        if (HP <= 0)
        {
            //게임 종료.
            Debug.Log("HP 모두 소진");
        }
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
  //      float moveVertical = Input.GetAxis("Vertical");
        
        // 이동 방향 벡터 계산
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        // 이동
        transform.position += movement * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //장애물에 부딪힌 경우.
        if(collision.gameObject.tag == "OBSTACLE")
        {
            HP--;
            Debug.Log("장애물에 맞닿았다. 남은 HP" + HP);

            /*
             2초 무적시간 추가하기
             */
        }
    }
}
