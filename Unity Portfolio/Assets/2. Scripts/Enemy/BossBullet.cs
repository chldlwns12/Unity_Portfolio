using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    Rigidbody rb;
    public float damage = 300f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * 10f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Wall"))
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
