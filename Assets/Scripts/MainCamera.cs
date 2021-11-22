using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject player;
    public Camera mainCamera;
    public Vector3 offset = new Vector3(0, 1.2f, -1.05f);
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = player.transform.position + offset;
    }
}
