using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPirateBoss : EnemyMeleeFSM
{
    public static EnemyPirateBoss Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyPirateBoss>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("EnemyPirateBoss");
                    instance = instanceContainer.AddComponent<EnemyPirateBoss>();
                }
            }
            return instance;
        }
    }
    private static EnemyPirateBoss instance;

    public GameObject bossBullet;
    public Transform attackPoint;
    public LayerMask layerMaskWall;

    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);

    protected void Start()
    {
        base.Start();
        attackCoolTime = 1.5f;
        attackCoolTimeCacl = attackCoolTime;

        playerRealizeRange = 13f;
        attackRange = 13f;
        moveSpeed = 1f;
        nvAgent.stoppingDistance = 4f;
    }

    protected override void InitMonster()
    {
        maxHp = 80000f;
        currentHp = maxHp;
        damage = 200f;
        StartCoroutine(Attack());
    }

    protected override IEnumerator Attack()
    {
        yield return null;
        nvAgent.isStopped = true;
        transform.LookAt(player.transform.position);

        if (Random.value < 0.5)
        {
            if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                Anim.SetTrigger("Attack");
            }
        }
        else
        {
            if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            {
                Anim.SetTrigger("Attack2");
            }
        }
        yield return Delay500;

        canAtk = false;
        currentState = State.Idle;
    }

    public void Attack01()
    {
        Instantiate(bossBullet, attackPoint.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, -35f, 0)));
        Instantiate(bossBullet, attackPoint.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, 0)));
        Instantiate(bossBullet, attackPoint.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, 35f, 0)));
    }

    public void Attack02()
    {
        Instantiate(bossBullet, attackPoint.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, -25f, 0)));
        Instantiate(bossBullet, attackPoint.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, -10f, 0)));
        Instantiate(bossBullet, attackPoint.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, 10f, 0)));
        Instantiate(bossBullet, attackPoint.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, 25f, 0)));
    }

    protected override void AtkEffect()
    {
        Instantiate(EffectSet.Instance.oneAtkEffect, transform.position, Quaternion.Euler(90, 0, 0));
    }

    private void Update()
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
                GameObject ExpClone = Instantiate(PlayerData.Instance.itemExp, currentPosition, transform.rotation);
                ExpClone.transform.parent = gameObject.transform.parent.parent;
            }
            UIController.Instance.CheckBossRoom(false);

            Destroy(transform.parent.gameObject);
            return;
        }
        else
        {
            //Debug.Log("Current Hp : " + currentHp);
            //Debug.Log("Max Hp : " + maxHp);
            UIController.Instance.bossCurrentHp = currentHp;
            UIController.Instance.bossMaxHp = maxHp;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Arrow"))
        {
            float arrowDmg = other.gameObject.GetComponent<Bullet>().damage;

            UIController.Instance.Dmg();

            Instantiate(EffectSet.Instance.oneDmgEffect, other.transform.position, Quaternion.Euler(90, 0, 0));

            GameObject dmgTextClone = Instantiate(EffectSet.Instance.monsterDmgText, transform.position, Quaternion.identity);

            if (Random.value < 0.5)
            {
                currentHp -= other.gameObject.GetComponent<Bullet>().damage;
                dmgTextClone.GetComponent<DmgTxt>().DisplayDamage(other.gameObject.GetComponent<Bullet>().damage, false);
            }
            else
            {
                currentHp -= other.gameObject.GetComponent<Bullet>().damage * 4;
                dmgTextClone.GetComponent<DmgTxt>().DisplayDamage(other.gameObject.GetComponent<Bullet>().damage * 2, true);
            }
            //Destroy(other.gameObject);
        }
    }
}
