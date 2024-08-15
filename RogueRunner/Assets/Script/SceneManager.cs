using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    public static SceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Find the SceneManager in the scene
                instance = FindObjectOfType<SceneManager>();

                if (instance == null)
                {
                    // If not found, create a new one
                    GameObject obj = new GameObject("SceneManager");
                    instance = obj.AddComponent<SceneManager>();
                }
            }
            return instance;
        }
    }

    float SceneTime;
    public bool isStopSkilled { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        SceneTime = 0;
        isStopSkilled = false;

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        //������ �������� ������ ��츸 ����(isPaused == false)
        if (GameManager.Instance.getGameCode() == "Start")
        {
            SceneTime += Time.deltaTime;
        }

        //���� �ð��� 100�ʰ� �ʰ��� ���...
        if(SceneTime >= 100)
        {
            //�������� �Ϸ�.
            GameManager.Instance.StageClear();
        }

        if(SceneTime % 10 == 0)
        {
            Debug.Log("SceneTime : " + SceneTime);
        }
    }
}