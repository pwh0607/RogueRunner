using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSession : MonoBehaviour
{
    public static UserSession Instance { get; private set; }

    public string P_Id { get; set; }
    public string Id { get; set; }
    public string Nickname { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void setData(string p_id, string user_id ,string nickname)
    {
        Instance.P_Id = p_id;
        Instance.Id = user_id;
        Instance.Nickname = nickname;
    }
}