using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    public float playerSpeed = 15.0f;
    public VariableJoystick joystick;

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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (moveHorizontal == 0 && moveVertical == 0)
        {
            moveHorizontal = joystick.Horizontal;
            moveVertical = joystick.Vertical;
        }

        Debug.Log("moveHorizontal : " + moveHorizontal + " / moveVertical : " + moveVertical);

        Vector3 dir = new Vector3(moveHorizontal, 0, moveVertical);

        transform.Translate(dir * playerSpeed * Time.deltaTime);

        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, -2.5f, 2.6f);
        position.z = Mathf.Clamp(position.z, -7f, 6.8f);
        transform.position = position;
    }
}
