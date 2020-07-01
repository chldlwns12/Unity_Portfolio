using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bounceCount = 2;
    public int wallBounceCount = 2;
    Rigidbody rb;
    Vector3 newDir;
    public float damage;

    void Start()
    {
        //GetComponent<Rigidbody>().velocity = transform.forward * 20f;
        rb = GetComponent<Rigidbody>();
        newDir = transform.forward;
        rb.velocity = newDir * 5f;

        damage = PlayerData.Instance.damage;

        Destroy(gameObject, 5f);
    }

    Vector3 ResultDir(int index)
    {
        int closetIndex = -1;
        float closetDis = 500f;
        float currentDis = 0f;

        for (int i = 0; i < PlayerTargeting.Instance.monsterList.Count; i++)
        {
            if (i == index) continue;

            currentDis = Vector3.Distance(PlayerTargeting.Instance.monsterList[i].transform.GetChild(0).position, transform.position);

            if (currentDis > 5f) continue;

            //Debug.Log("currentDis : " + currentDis);
            //Debug.Log("closetDis : " + closetDis);
                Debug.Log("i : " + i);
            if(closetDis > currentDis)
            {
                Collider[] cols = Physics.OverlapSphere(transform.position, 5f);
                closetDis = currentDis;
                closetIndex = i;
                //Debug.Log("반동!");
                //Debug.Log("closetIndex : " + closetIndex);
            }
        }

        if(closetIndex == -1)
        {
            Destroy(gameObject, 0.2f);
            return Vector3.zero;
        }
        //Debug.Log("ResultName : " + PlayerTargeting.Instance.monsterList[closetIndex].name);
        //transform.LookAt(PlayerTargeting.Instance.monsterList[closetIndex].transform.position);
        return (PlayerTargeting.Instance.monsterList[closetIndex].transform.position - transform.position).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Monster"))
        {
            if(PlayerData.Instance.playerSkill[0] != 0 && PlayerTargeting.Instance.monsterList.Count >= 2)
            {
                int myIndex = PlayerTargeting.Instance.monsterList.IndexOf(other.gameObject.transform.parent.gameObject);

                if(bounceCount > 0)
                {
                    bounceCount--;
                    damage *= 0.7f;
                    //Debug.Log("myIndex : " + myIndex);
                    newDir = ResultDir(myIndex) ;
                
                    transform.rotation = Quaternion.LookRotation(newDir);
                    rb.velocity = transform.forward * 5f;
                    return;
                }
            }
            rb.velocity = Vector3.zero;
            Destroy(gameObject, 0.2f);
        }
        else if(other.transform.CompareTag("Wall") && rb != null)
        {
            if(PlayerData.Instance.playerSkill[4] == 0)
            {
                rb.velocity = Vector3.zero;
                Destroy(gameObject, 0.2f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall") && rb != null)
        {
            if(PlayerData.Instance.playerSkill[4] != 0)
            {
                if(wallBounceCount > 0)
                {
                    wallBounceCount--;
                    damage *= 0.5f;
                    newDir = Vector3.Reflect(newDir, collision.contacts[0].normal);
                    transform.rotation = Quaternion.LookRotation(newDir);
                    rb.velocity = newDir * 5f;
                    return;
                }
            }
            rb.velocity = Vector3.zero;
            Destroy(gameObject);
        }
    }
}
