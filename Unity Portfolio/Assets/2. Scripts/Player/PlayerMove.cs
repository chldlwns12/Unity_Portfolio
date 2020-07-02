using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    
    public Animator Anim;
    public GameObject playerCavasGO;

    public static PlayerMove Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PlayerMove>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerMove");
                    instance = instanceContainer.AddComponent<PlayerMove>();
                }
            }
            return instance;
        }
    }
    private static PlayerMove instance;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            PlayerTargeting.Instance.Attack();
        }
    }

    private void FixedUpdate()
    {
        if (JoyStickMove.Instance.joyVec.x != 0 || JoyStickMove.Instance.joyVec.z != 0)
        {
            rb.velocity = new Vector3(JoyStickMove.Instance.joyVec.x * PlayerData.Instance.playerSpeed, 0, JoyStickMove.Instance.joyVec.y * PlayerData.Instance.playerSpeed) * PlayerData.Instance.playerSpeed;
            rb.rotation = Quaternion.LookRotation(new Vector3(JoyStickMove.Instance.joyVec.x, 0, JoyStickMove.Instance.joyVec.y));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("NextRoom"))
        {
            //Debug.Log("Get Next Room");
            StageMgr.Instance.NextStage();
        }

        //if (other.transform.CompareTag("HpBooster"))
        //{
        //    HpBar.Instance.GetHpBoost();
        //    Destroy(other.gameObject);
        //}

        if (other.transform.CompareTag("MeleeAtk") && PlayerData.Instance.playerImmotal == false)
        {
            playerCavasGO.gameObject.GetComponent<HpBar>().Dmg();
            other.transform.parent.GetComponent<EnemyOne>().meleeAtkArea.SetActive(false);
            PlayerData.Instance.currentHp -= other.transform.parent.GetComponent<EnemyOne>().damage * 2f;
        
            if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Dmg"))
            {
                Anim.SetTrigger("Dmg");
                Instantiate(EffectSet.Instance.PlayerDmgEffect, PlayerTargeting.Instance.AttackPoint.position, Quaternion.Euler(90, 0, 0));
            }
        }

        if(other.transform.CompareTag("BossMeleeAtk") && PlayerData.Instance.playerImmotal == false)
        {
            HpBar.Instance.Dmg();
            PlayerData.Instance.currentHp -= other.transform.parent.GetComponent<EnemyStageBoss>().damage * 2f;

            if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Dmg"))
            {
                Anim.SetTrigger("Dmg");
                Instantiate(EffectSet.Instance.PlayerDmgEffect, PlayerTargeting.Instance.AttackPoint.position, Quaternion.Euler(90, 0, 0));
            }
        }

        if(PlayerTargeting.Instance.monsterList.Count <= 0 && other.transform.CompareTag("Exp"))
        {
            PlayerData.Instance.PlayerExpCalc(100f);
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("RangeAtk") && PlayerData.Instance.playerImmotal == false)
        {
            Destroy(collision.gameObject, 0.1f);

            HpBar.Instance.Dmg();
            PlayerData.Instance.currentHp -= collision.transform.GetComponent<EnemyThreeBolt>().damage;

            if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Dmg"))
            {
                Anim.SetTrigger("Dmg");
                Instantiate(EffectSet.Instance.PlayerDmgEffect, PlayerTargeting.Instance.AttackPoint.position, Quaternion.Euler(90, 0, 0));
            }
        }

        if(collision.transform.CompareTag("BossRangeAtk") && PlayerData.Instance.playerImmotal == false)
        {
            Destroy(collision.gameObject, 0.1f);

            HpBar.Instance.Dmg();
            PlayerData.Instance.currentHp -= collision.transform.GetComponent<BossBullet>().damage;

            if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Dmg"))
            {
                Anim.SetTrigger("Dmg");
                Instantiate(EffectSet.Instance.PlayerDmgEffect, PlayerTargeting.Instance.AttackPoint.position, Quaternion.Euler(90, 0, 0));
            }
        }
    }
}
