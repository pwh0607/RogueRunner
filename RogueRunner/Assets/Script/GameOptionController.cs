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
    //상태 변수
    bool isMute;

    void Start()
    {
        isMute = false;
        muteImage = GetComponent<Image>();
    }

    public void OnclickMuteBtn()
    {
        isMute = !isMute;

        //클릭 했을 때, 
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
