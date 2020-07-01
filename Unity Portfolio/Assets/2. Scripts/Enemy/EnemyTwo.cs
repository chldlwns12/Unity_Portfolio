using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwo : EnemyMeleeFSM
{
    //GameObject player;
    RoomCondition roomConditionGO;

    //public LayerMask layerMask;

    public GameObject dangerMarker;
    public GameObject enemyBolt;

    public Transform boltGenPosition;

    Animator enemyAnim;

    public GameObject enemyCanvasGo;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerRealizeRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected void Start()
    {
        base.Start();
        roomConditionGO = transform.parent.transform.parent.gameObject.GetComponent<RoomCondition>();
        enemyAnim = GetComponent<Animator>();

        attackCoolTime = 5.0f;
        attackCoolTimeCacl = attackCoolTime;

        playerRealizeRange = 10f;
        attackRange = 10f;
        moveSpeed = 1f;
        StartCoroutine(Idle());
    }

    void Update()
    {
        if (currentHp <= 0)
        {
            nvAgent.isStopped = true;

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
        Debug.Log("Two Attack!");
        yield return null;
        //Atk
        nvAgent.stoppingDistance = attackRange;
        nvAgent.isStopped = true;
        transform.LookAt(player.transform.position);
        yield return new WaitForSeconds(4f);
        transform.LookAt(player.transform.position);
        DangerMaKerShoot();

        yield return new WaitForSeconds(2f);
        canAtk = false;
        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Anim.SetTrigger("Attack");
        }
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
        Vector3 CurrentRotation = transform.eulerAngles + new Vector3(-90, 0, 0);
        Instantiate(enemyBolt, boltGenPosition.position, Quaternion.Euler(CurrentRotation));
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

    protected override IEnumerator CalcCoolTime()
    {
        while (true)
        {
            yield return null;
            if (!canAtk)
            {
                Debug.Log("CoolTime");
                attackCoolTimeCacl -= Time.deltaTime;
                if (attackCoolTimeCacl <= 0)
                {
                    attackCoolTimeCacl = attackCoolTime;
                    canAtk = true;
                }
            }
        }
    }
}
