using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThreeBolt : MonoBehaviour
{
    Rigidbody rb;
    Vector3 newDir;
    int bounceCount = 3;

    public GameObject bulletRotate;
    public float rotateSpeed = 150f;

    public float damage = 160f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        newDir = transform.forward;
        rb.velocity = newDir * 10f;
    }

    private void Update()
    {
        bulletRotate.transform.Rotate(new Vector3(0, 1, 0) * -rotateSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Name : " + collision.transform.name);
        if (collision.transform.CompareTag("Wall"))
        {
            bounceCount--;
            if (bounceCount > 0)
            {
                Debug.Log("hit wall");
                newDir = Vector3.Reflect(newDir, collision.contacts[0].normal);
                rb.velocity = newDir * 10f;
            }
            else
            {
                Destroy(gameObject, 0.1f);
            }
        }
        if (collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject, 0.1f);
            HpBar.Instance.currentHp -= damage;

            if (!PlayerMove.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Dmg"))
            {
                PlayerMove.Instance.Anim.SetTrigger("Dmg");
                Instantiate(EffectSet.Instance.PlayerDmgEffect, PlayerTargeting.Instance.AttackPoint.position, Quaternion.Euler(90, 0, 0));
            }
        }
    }
}
