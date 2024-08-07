using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //player ������ġ
    public GameObject SpawnPos;
    private float speed = 10f;
    //ü��
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
            //���� ����.
            Debug.Log("HP ��� ����");
        }
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
  //      float moveVertical = Input.GetAxis("Vertical");
        
        // �̵� ���� ���� ���
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        // �̵�
        transform.position += movement * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //��ֹ��� �ε��� ���.
        if(collision.gameObject.tag == "OBSTACLE")
        {
            HP--;
            Debug.Log("��ֹ��� �´�Ҵ�. ���� HP" + HP);

            /*
             2�� �����ð� �߰��ϱ�
             */
        }
    }
}
