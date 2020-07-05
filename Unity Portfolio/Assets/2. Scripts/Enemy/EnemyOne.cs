using System.Collections;
using UnityEngine;

public class EnemyOne : EnemyMeleeFSM
{
    public GameObject enemyCanvasGo;
    public GameObject meleeAtkArea;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerRealizeRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void Start()
    {
        base.Start();
        attackCoolTime = 2f;
        attackCoolTimeCacl = attackCoolTime;

        attackRange = 3f;
        nvAgent.stoppingDistance = 1f;

        StartCoroutine(ResetAtkArea());
    }

    IEnumerator ResetAtkArea()
    {
        while (true)
        {
            yield return null;
            if (!meleeAtkArea.activeInHierarchy && currentState == State.Attack)
            {
                yield return new WaitForSeconds(attackCoolTime);
                meleeAtkArea.SetActive(true);
            }
        }
    }

    protected override void InitMonster()
    {
        maxHp += (StageMgr.Instance.currentStage + 1) * 100f;
        currentHp = maxHp;
        damage += (StageMgr.Instance.currentStage + 1) * 10f;
    }

    protected override void AtkEffect()
    {
        Instantiate(EffectSet.Instance.OneAtkEffect, transform.position, Quaternion.Euler(90, 0, 0));
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
            for (int i = 0; i < (StageMgr.Instance.currentStage / 10 + 2 + Random.Range(0,3)); i++)
            {
                //Debug.Log(i);
                GameObject ExpClone = Instantiate(PlayerData.Instance.itemExp, currentPosition, transform.rotation);
                ExpClone.transform.parent = gameObject.transform.parent.parent;
            }

            Destroy(transform.parent.gameObject);
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Arrow"))
        {
            enemyCanvasGo.GetComponent<EnemyHpBar>().Dmg(other.gameObject);
            Instantiate(EffectSet.Instance.OneDmgEffect, other.transform.position, Quaternion.Euler(90, 0, 0));

            GameObject dmgTextClone = Instantiate(EffectSet.Instance.MonsterDmgText, transform.position, Quaternion.identity);

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
        }
    }
}
