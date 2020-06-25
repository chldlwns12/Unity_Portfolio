using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThree : MonoBehaviour
{
    RoomCondition roomConditionGO;
    GameObject player;
    LineRenderer lr;

    public LayerMask layerMask;
    public Transform firePos;
    public GameObject enemyBullet;
    public bool lookAtPlayer = true;

    public float attackCount = 4.0f;
    float currentCount = 0.0f;

    Animator enemyAnim;

    //public GameObject enemyCanvasGo;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        roomConditionGO = transform.parent.transform.parent.gameObject.GetComponent<RoomCondition>();

        lr.startColor = new Color(1, 0, 0, 0.5f);
        lr.endColor = new Color(1, 0, 0, 0.5f);
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;

        enemyAnim = GetComponent<Animator>();

        StartCoroutine(WaitPlayer());
    }

    private void Update()
    {
        currentCount += Time.deltaTime;
        if (currentCount > attackCount)
        {
            StartCoroutine(WaitPlayer());
            lookAtPlayer = true;
            currentCount = 0.0f;
        }
    }

    IEnumerator WaitPlayer()
    {
        yield return null;

        while (!roomConditionGO.playerInThisRoom)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SetTarget());

        yield return new WaitForSeconds(2f);
        DangerMarKerDeactive();
        shoot();
        //enemyAnim.SetTrigger("Attack");
    }

    IEnumerator SetTarget()
    {
        while (true)
        {
            yield return null;
            if (!lookAtPlayer) break;

            Debug.Log("Set Player.transform.position");
            transform.LookAt(player.transform.position);
            DangerMarKerShoot();
        }
    }

    private void DangerMarKerShoot()
    {
        Vector3 NewPosition = firePos.position;
        Vector3 NewDir = transform.forward;
        lr.positionCount = 1;
        lr.SetPosition(0, transform.position);
        for (int i = 1; i < 4; i++)
        {
            Physics.Raycast(NewPosition, NewDir, out RaycastHit hit, 30f, layerMask);

            Debug.Log("name : " + hit.transform.name + "position : " + hit.point);

            lr.positionCount++;
            Debug.Log("position : " + hit.point);
            lr.SetPosition(i, hit.point);

            NewPosition = hit.point;
            NewDir = Vector3.Reflect(NewDir, hit.normal);
        }
    }

    private void DangerMarKerDeactive()
    {
        lookAtPlayer = false;
        Debug.Log("lr.positionCount : " + lr.positionCount);
        for (int i = 0; i < lr.positionCount; i++)
        {
            Debug.Log("Set Vector3.zero");
            lr.SetPosition(i, Vector3.zero);
        }
        lr.positionCount = 0;
    }

    private void shoot()
    {
        //Vector3 currentRotation = transform.eulerAngles + new Vector3(-90, 0, 0);
        Vector3 currentRotation = transform.eulerAngles;
        Instantiate(enemyBullet, firePos.position, Quaternion.Euler(currentRotation));
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.CompareTag("Arrow"))
    //    {
    //        enemyCanvasGo.GetComponent<EnemyHpBar>().Dmg();
    //        currentHp -= 300f;
    //        Instantiate(EffectSet.Instance.OneDmgEffect, collision.contacts[0].point, Quaternion.Euler(90, 0, 0));
    //    }
    //}
}
