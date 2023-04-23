using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        transform.position = new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y, -10);
    }
}
