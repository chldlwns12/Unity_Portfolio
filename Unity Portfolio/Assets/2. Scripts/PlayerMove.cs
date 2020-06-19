using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    public float playerSpeed = 5.0f;
    public Animator Anim;

    public static PlayerMove Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PlayerMove>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerMove");
                    instance = instanceContainer.AddComponent<PlayerMove>();
                }
            }
            return instance;
        }
    }
    private static PlayerMove instance;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (JoyStickMove.Instance.joyVec.x != 0 || JoyStickMove.Instance.joyVec.z != 0)
        {
            rb.velocity = new Vector3(JoyStickMove.Instance.joyVec.x * playerSpeed, 0, JoyStickMove.Instance.joyVec.y * playerSpeed) * playerSpeed;
            rb.rotation = Quaternion.LookRotation(new Vector3(JoyStickMove.Instance.joyVec.x, 0, JoyStickMove.Instance.joyVec.y));
        }
    }
}
