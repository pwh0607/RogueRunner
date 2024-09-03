using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLobbyAnim : MonoBehaviour
{
    float rotationSpeed = 100f;

    void Update()
    {
        //y축 기준으로 회전.
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}