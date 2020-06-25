using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwo : MonoBehaviour
{
    GameObject player;
    RoomCondition roomConditionGO;

    public LayerMask layerMask;

    public GameObject dangerMarker;
    public GameObject enemyBolt;

    public Transform boltGenPosition;

    public float attackCount = 4.0f;
    float currentCount = 0.0f;

    Animator enemyAnim;

    //public GameObject enemyCanvasGo;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        roomConditionGO = transform.parent.transform.parent.gameObject.GetComponent<RoomCondition>();
        enemyAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        currentCount += Time.deltaTime;
        if (currentCount > attackCount)
        {
            StartCoroutine(WaitPlayer());
            currentCount = 0.0f;
        }
    }

    IEnumerator WaitPlayer()
    {
        yield return null;

        while (!roomConditionGO.playerInThisRoom)
        {
            yield return new WaitForSeconds(0.5f);
            enemyAnim.SetTrigger("Idle");
        }

        yield return new WaitForSeconds(4f);
        transform.LookAt(player.transform.position);
        DangerMaKerShoot();

        yield return new WaitForSeconds(2f);
        Shoot();
    }

    private void DangerMaKerShoot()
    {
        Vector3 NewPosition = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        Physics.Raycast(NewPosition, transform.forward, out RaycastHit hit, 30f, layerMask);

        if(hit.transform.CompareTag("Wall"))
        {
            GameObject dangerMarkerClone = Instantiate(dangerMarker, NewPosition, transform.rotation);
            dangerMarkerClone.GetComponent<DangerLine>().EndPosition = hit.point;
        }
    }

    private void Shoot()
    {
        enemyAnim.SetTrigger("Attack");
        Vector3 CurrentRotation = transform.eulerAngles + new Vector3(-90, 0, 0);
        Instantiate(enemyBolt, boltGenPosition.position, Quaternion.Euler(CurrentRotation));
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
