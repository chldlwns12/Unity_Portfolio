using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThree : EnemyMeleeFSM
{
    RoomCondition roomConditionGO;
    //GameObject player;
    LineRenderer lr;

    //public LayerMask layerMask;
    public Transform firePos;
    public GameObject enemyBullet;
    public bool lookAtPlayer = true;

    //public float attackCount = 4.0f;
    //float currentCount = 0.0f;

    Animator enemyAnim;

    public GameObject enemyCanvasGo;

    private new void Start()
    {
        base.Start();

        lr = GetComponent<LineRenderer>();
        roomConditionGO = transform.parent.transform.parent.gameObject.GetComponent<RoomCondition>();
        enemyAnim = GetComponent<Animator>();
        //player = GameObject.FindGameObjectWithTag("Player");

        lr.startColor = new Color(1, 0, 0, 0.5f);
        lr.endColor = new Color(1, 0, 0, 0.5f);
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;

        attackCoolTime = 4.0f;
        attackCoolTimeCacl = attackCoolTime;

        playerRealizeRange = 10f;
        attackRange = 10f;
        moveSpeed = 1f;
        StartCoroutine(Idle());

        //StartCoroutine(WaitPlayer());
    }

    private void Update()
    {
        //currentCount += Time.deltaTime;
        //if (currentCount > attackCount)
        //{
        //    StartCoroutine(WaitPlayer());
        //    lookAtPlayer = true;
        //    currentCount = 0.0f;
        //}

        if (currentHp <= 0)
        {
            rb.gameObject.SetActive(false);
            PlayerTargeting.Instance.monsterList.Remove(transform.parent.gameObject);
            PlayerTargeting.Instance.targetIndex = -1;

            Vector3 currentPosition = new Vector3(transform.position.x, 3f, transform.position.z);
            for (int i = 0; i < (StageMgr.Instance.currentStage / 10 + 2 + Random.Range(0, 3)); i++)
            {
                Debug.Log(i);
                GameObject ExpClone = Instantiate(PlayerData.Instance.itemExp, currentPosition, transform.rotation);
                ExpClone.transform.parent = gameObject.transform.parent.parent;
            }

            Destroy(transform.parent.gameObject);
            return;
        }
    }
    //-----------------------------------------------------
    protected override void InitMonster()
    {
        maxHp += (StageMgr.Instance.currentStage + 1) * 100f;
        currentHp = maxHp;
        damage += (StageMgr.Instance.currentStage + 1) * 10f;
    }

    protected override IEnumerator Attack()
    {
        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Anim.SetTrigger("Idle");
        }

        yield return null;
        nvAgent.stoppingDistance = attackRange;
        nvAgent.isStopped = true;
        transform.LookAt(player.transform.position);
        lookAtPlayer = true;

        //yield return new WaitForSeconds(0.5f);
        //StartCoroutine(SetTarget());
        //
        //yield return new WaitForSeconds(4f);
        //transform.LookAt(player.transform.position);
        //DangerMarKerShoot();
        //
        //yield return new WaitForSeconds(2f);
        //DangerMarKerDeactive();
        //canAtk = false;
        //if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        //{
        //    Anim.SetTrigger("Attack");
        //}

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SetTarget());

        yield return new WaitForSeconds(2f);
        DangerMarKerDeactive();
        shoot();

        yield return new WaitForSeconds(1.5f);
        currentState = State.Move;
        transform.Rotate(new Vector3(0, Random.Range(0f, 200f), 0));
    }

    protected override IEnumerator Move()
    {
        yield return null;
        //Move
        nvAgent.isStopped = false;
        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            Debug.Log("Walk!");
            Anim.SetTrigger("Walk");
        }

        if (distance > attackRange && canAtk)
        {
            Debug.Log("FSM Attack!");
            currentState = State.Attack;
        }
        else
        {
            Debug.Log("Move forward");
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        Debug.Log("state : " + currentState);
        Debug.Log("Distance : " + distance);
        Debug.Log("canAtk : " + canAtk);
    }
    //--------------------------------------------------------------------------
    //IEnumerator WaitPlayer()
    //{
    //    yield return null;
    //
    //    while (!roomConditionGO.playerInThisRoom)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //
    //    yield return new WaitForSeconds(0.5f);
    //    StartCoroutine(SetTarget());
    //
    //    yield return new WaitForSeconds(2f);
    //    DangerMarKerDeactive();
    //    shoot();
    //    //enemyAnim.SetTrigger("Attack");
    //}

    IEnumerator SetTarget()
    {
        while (true)
        {
            yield return null;
            if (!lookAtPlayer) break;

            //Debug.Log("Set Player.transform.position");
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

            //Debug.Log("name : " + hit.transform.name + "position : " + hit.point);

            lr.positionCount++;
            //Debug.Log("position : " + hit.point);
            lr.SetPosition(i, hit.point);

            NewPosition = hit.point;
            NewDir = Vector3.Reflect(NewDir, hit.normal);
        }
    }

    private void DangerMarKerDeactive()
    {
        lookAtPlayer = false;
        //Debug.Log("lr.positionCount : " + lr.positionCount);
        for (int i = 0; i < lr.positionCount; i++)
        {
            //Debug.Log("Set Vector3.zero");
            lr.SetPosition(i, Vector3.zero);
        }
        lr.positionCount = 0;
    }

    private void shoot()
    {
        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Anim.SetTrigger("Attack");
        }

        //Vector3 currentRotation = transform.eulerAngles + new Vector3(-90, 0, 0);
        Vector3 currentRotation = transform.eulerAngles;
        Instantiate(enemyBullet, firePos.position, Quaternion.Euler(currentRotation));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Arrow"))
        {
            enemyCanvasGo.GetComponent<EnemyHpBar>().Dmg();
            Instantiate(EffectSet.Instance.OneDmgEffect, other.transform.position, Quaternion.Euler(90, 0, 0));

            GameObject dmgTextColone = Instantiate(EffectSet.Instance.MonsterDmgText, transform.position, Quaternion.identity);

            if (Random.value < 0.5)
            {
                currentHp -= other.gameObject.GetComponent<Bullet>().damage;
                dmgTextColone.GetComponent<DmgTxt>().DisplayDamage(other.gameObject.GetComponent<Bullet>().damage, false);
            }
            else
            {
                currentHp -= other.gameObject.GetComponent<Bullet>().damage * 2;
                dmgTextColone.GetComponent<DmgTxt>().DisplayDamage(other.gameObject.GetComponent<Bullet>().damage * 2, true);
            }

            //Destroy(other.gameObject);
        }
    }
}
