using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLobbyAnim : MonoBehaviour
{
    float rotationSpeed = 100f;

    void Update()
    {
        //y�� �������� ȸ��.
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}