using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float xConstOffset, yConstOffset;
    private float xOffset;
    private float yOffset;

    // Start is called before the first frame update
    void Start()
    {
        xOffset = xConstOffset;
        yOffset = yConstOffset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(xOffset, yOffset, -10f);
    }
}
