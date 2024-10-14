using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageTextController : MonoBehaviour
{
    public GameObject StageText;
    int stage;

    private void Start()
    {
        stage = GameManager.Instance.gameState.stage;

        StageText.GetComponent<TextMeshProUGUI>().text = $"Stage : {stage}";
    }
}
