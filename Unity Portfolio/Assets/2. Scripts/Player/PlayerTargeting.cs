using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public bool getATarget = false;
    float currentDist = 0;                  //현재 거리
    float closetDist = 100f;                //가까운 거리
    float targetDist = 100f;                //타겟 거리
    int closeDistIndex = 0;                //가장 가까운 인덱스
    public int targetIndex = -1;                  //타겟팅 할 인덱스
    int prevTargetIndex = 0;
    public LayerMask layerMask;

    public List<GameObject> monsterList = new List<GameObject>();

    public GameObject playerBolt;
    public Transform AttackPoint;

    public float atkSpeed = 1.0f;

    public static PlayerTargeting Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerTargeting>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerTargeting");
                    instance = instanceContainer.AddComponent<PlayerTargeting>();
                }
            }
            return instance;
        }
    }
    private static PlayerTargeting instance;

    void Update()
    {
        SetTarget();
        AttackTarget();
    }

    private void SetTarget()
    {
        if (monsterList.Count != 0)
        {
            prevTargetIndex = targetIndex;
            currentDist = 0f;
            closeDistIndex = 0;
            targetIndex = -1;

            for (int i = 0; i < monsterList.Count; i++)
            {
                if (monsterList[i] == null) { return; }
                currentDist = Vector3.Distance(transform.position, monsterList[i].transform.GetChild(0).position);

                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, monsterList[i].transform.GetChild(0).position - transform.position,
                                             out hit, 20f, layerMask);

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    if (targetDist >= currentDist)
                    {
                        targetIndex = i;
                        targetDist = currentDist;

                        if (!JoyStickMove.Instance.isPlayerMoving && prevTargetIndex != targetIndex)
                        {
                            targetIndex = prevTargetIndex;
                        }
                    }
                }

                if (closetDist >= currentDist)
                {
                    closeDistIndex = i;
                    closetDist = currentDist;
                }
            }

            if (targetIndex == -1)
            {
                targetIndex = closeDistIndex;
            }
            closetDist = 100f;
            targetDist = 100f;
            getATarget = true;
        }
    }


    private void AttackTarget()
    {
        if (targetIndex == -1 || monsterList.Count == 0)  // 추가 
        {
            PlayerMove.Instance.Anim.SetBool("Attack", false);
            return;
        }
        if (getATarget && !JoyStickMove.Instance.isPlayerMoving && monsterList.Count != 0)
        {
            transform.LookAt(monsterList[targetIndex].transform.GetChild(0));

            Debug.Log("Attack");
            if (PlayerMove.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                PlayerMove.Instance.Anim.SetBool("Idle", false);
                PlayerMove.Instance.Anim.SetBool("Walk", false);
                PlayerMove.Instance.Anim.SetBool("Attack", true);
            }
        }
        else if (JoyStickMove.Instance.isPlayerMoving)
        {
            //currentCount = 0.0f;
            if (!PlayerMove.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                PlayerMove.Instance.Anim.SetBool("Idle", false);
                PlayerMove.Instance.Anim.SetBool("Walk", true);
                PlayerMove.Instance.Anim.SetBool("Attack", false);
            }
        }
    }

    void Attack()
    {
        PlayerMove.Instance.Anim.SetFloat("AttackSpeed", atkSpeed);
        Instantiate(playerBolt, AttackPoint.position, transform.rotation);
    }

    private void OnDrawGizmos()
    {
        if(getATarget)
        {
            for (int i = 0; i < monsterList.Count; i++)
            {
                if (monsterList[i] == null) { return; }
                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, monsterList[i].transform.GetChild(0).position - transform.position,
                                             out hit, 20f, layerMask);

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawRay(transform.position, monsterList[i].transform.GetChild(0).position - transform.position);
            }
        }
    }
}
