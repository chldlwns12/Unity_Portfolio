﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public bool getATarget = false;
    float currentDist = 0;
    float closetDist = 100f;
    float targetDist = 100f;
    int closeDistIndex = 0;
    int targetIndex = -1;
    public LayerMask layerMask;

    public List<GameObject> monsterList = new List<GameObject>();

    public GameObject playerBolt;
    public Transform AttackPoint;

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
       if(monsterList.Count != 0)
        {
            currentDist = 0f;
            closeDistIndex = 0;
            targetIndex = -1;

            for (int i = 0; i < monsterList.Count; i++)
            {
                currentDist = Vector3.Distance(transform.position, monsterList[i].transform.position);

                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, monsterList[i].transform.position - transform.position,
                    out hit, 20f, layerMask);

                if(isHit && hit.transform.CompareTag("Monster"))
                {
                    if(targetDist >= currentDist)
                    {
                        targetIndex = i;
                        targetDist = currentDist;
                    }
                }

                if(closetDist >= currentDist)
                {
                    closeDistIndex = i;
                    closetDist = currentDist;
                }
            }

            if(targetIndex == -1)
            {
                targetIndex = closeDistIndex;
            }
            closetDist = 100f;
            targetDist = 100f;
            getATarget = true;
        }

       if(getATarget && !JoyStickMove.Instance.isPlayerMoving)
        {
            transform.LookAt(new Vector3(monsterList[targetIndex].transform.position.x, transform.position.y, monsterList[targetIndex].transform.position.z));
            Attack();

            if(PlayerMove.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                PlayerMove.Instance.Anim.SetBool("Idle", false);
                PlayerMove.Instance.Anim.SetBool("Walk", false);
                PlayerMove.Instance.Anim.SetBool("Attack", true);
            }
        }
       else if(JoyStickMove.Instance.isPlayerMoving)
        {
            if(!PlayerMove.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                PlayerMove.Instance.Anim.SetBool("Idle", false);
                PlayerMove.Instance.Anim.SetBool("Walk", true);
                PlayerMove.Instance.Anim.SetBool("Attack", false);
            }
        }
    }

    void Attack()
    {
        Instantiate(playerBolt, AttackPoint.position, transform.rotation);
    }
}