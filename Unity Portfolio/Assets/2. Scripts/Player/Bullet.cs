using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bounceCount = 2;
    public int wallBounceCount = 2;
    public Rigidbody rb;
    Vector3 newDir;
    public float damage;

    private void OnDisable()
    {
        //Debug.Log("Bullet Disable");
    }

    void Start()
    {
        //GetComponent<Rigidbody>().velocity = transform.forward * 20f;
        rb = GetComponent<Rigidbody>();

        damage = PlayerData.Instance.damage;

        //Destroy(gameObject, 5f);
        //Invoke("PoolingReset", 5f);
    }

    private void Update()
    {
        if(gameObject.activeSelf == true)
        {
            newDir = transform.forward;
            rb.velocity = newDir * 20f;
        }
    }

    void PoolingReset()
    {
        bounceCount = 2;
        wallBounceCount = 2;
        damage = PlayerData.Instance.damage;
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
                closetDis = currentDis;
                closetIndex = i;
                //Debug.Log("반동!");
                //Debug.Log("closetIndex : " + closetIndex);
            }
        }

        if(closetIndex == -1)
        {
            PoolingReset();
            PlayerTargeting.Instance.ArrowEnqueue(gameObject);
            //Destroy(gameObject, 0.2f);
            return Vector3.zero;
        }
        //Debug.Log("ResultName : " + PlayerTargeting.Instance.monsterList[closetIndex].name);
        //transform.LookAt(PlayerTargeting.Instance.monsterList[closetIndex].transform.position);
        return (PlayerTargeting.Instance.monsterList[closetIndex].transform.GetChild(0).position - transform.position).normalized;
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
                    rb.velocity = transform.forward * 20f;
                    return;
                }
            }

            rb.velocity = Vector3.zero;
            PoolingReset();
            PlayerTargeting.Instance.ArrowEnqueue(gameObject);
            //Destroy(gameObject, 0.2f);
        }
        //else if(other.transform.CompareTag("Wall") && rb != null)
        //{
        //    if(PlayerData.Instance.playerSkill[4] == 0)
        //    {
        //        rb.velocity = Vector3.zero;
        //        gameObject.SetActive(false);
        //        PlayerTargeting.Instance.ArrowEnqueue(gameObject);
        //        //Destroy(gameObject, 0.2f);
        //    }
        //}
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
                    rb.velocity = newDir * 20f;
                    return;
                }
            }
            rb.velocity = Vector3.zero;
            PoolingReset();
            PlayerTargeting.Instance.ArrowEnqueue(gameObject);
            //Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + newDir.normalized * 5f);
    }
}
