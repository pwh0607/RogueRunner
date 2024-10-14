using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionController : MonoBehaviour
{
    //Assets
    public Sprite muteSprite;
    public Sprite notMuteSprite;
    public Image muteImage;
    //���� ����
    bool isMute;

    void Start()
    {
        isMute = false;
        muteImage = GetComponent<Image>();
    }

    public void OnclickMuteBtn()
    {
        isMute = !isMute;

        //Ŭ�� ���� ��, 
        if (isMute)
        {
            muteImage.sprite = muteSprite;
        }
        else
        {
            muteImage.sprite = notMuteSprite;
        }
    }
}
