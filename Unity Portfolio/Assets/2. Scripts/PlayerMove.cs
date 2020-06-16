using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public static PlayerMove Instance;
    private void Awake()
    {
        if(Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    Rigidbody rb;
    public float playerSpeed = 25.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Debug.Log("moveHorizontal : " + moveHorizontal + " / moveVertical : " + moveVertical);

        Vector3 dir = new Vector3(moveHorizontal, 0, moveVertical);

        transform.Translate(dir * playerSpeed * Time.deltaTime);
    }
}
