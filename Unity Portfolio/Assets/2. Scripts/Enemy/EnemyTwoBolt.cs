using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwoBolt : MonoBehaviour
{
    Rigidbody rb;

    public float damage = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.up * -20f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Wall"))
        {
            Destroy(gameObject, 0.1f);
        }

        if(collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject, 0.1f);
            PlayerData.Instance.currentHp -= damage;

            if (!PlayerMove.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Dmg"))
            {
                PlayerMove.Instance.Anim.SetTrigger("Dmg");
                Instantiate(EffectSet.Instance.playerDmgEffect, PlayerTargeting.Instance.attackPoint.position, Quaternion.Euler(90, 0, 0));
            }
        }
    }
}
