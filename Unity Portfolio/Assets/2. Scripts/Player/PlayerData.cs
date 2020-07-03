using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerData>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerData");
                    instance = instanceContainer.AddComponent<PlayerData>();
                }
            }
            return instance;
        }
    }
    private static PlayerData instance;

    public List<int> playerSkill = new List<int>();
    //0 : 몬스터 반동
    //1 : 멀티샷
    //2 : 전방화살
    //3 : 사선화살
    //4 : 벽반동
    //5 : 공격속도
    //6 : 공격력
    //7 : 후방화살
    //8 : 관통
    //9 : 피통Up
    //10 : 무적
    //11 : 사이드화살

    public float damage = 320f;
    public float maxHp = 1000f;
    public float currentHp = 1000f;
    public GameObject player;
    //public GameObject playerBullet;
    public GameObject[] PlayerBullet;
    public GameObject itemExp;
    public int playerLv = 1;
    public float playerCurrentExp = 0f;
    public float playerLvUpExp = 500f;
    public float playerSpeed = 2.0f;
    public float atkSpeed = 1.4f;

    public bool playerDead = false;
    public bool playerSkillUp = false;
    public bool playerImmotal = false;
    public int resultIndex;

    private void Update()
    {
        if(!playerDead && currentHp <= 0)
        {
            currentHp = 0;
            playerDead = true;
            Debug.Log("Player Die");
            PlayerMove.Instance.Anim.SetTrigger("Dead");
            UIController.Instance.EndGame();
            return;
        }
        if(playerSkillUp)
        {
            Debug.Log("ResultIndex : " + resultIndex);
            for (int i = 0; i < playerSkill.Count; i++)
            {
                if(resultIndex == i)
                {
                    playerSkill[i] = 1;
                    playerSkillUp = false;
                }
            }
        }

        PlayerSkillSet();
    }

    private void PlayerSkillSet()
    {
        float immotalRandom = Random.Range(0, 50f);
        float immotalCount = 0;
        float count = 0;
        if(playerSkill[5] == 1)     //공속
        {
            atkSpeed += 0.1f;
            playerSkill[5] = 0;
        }
        if (playerSkill[6] == 1)    //공격력
        {
            damage += 75f;
            playerSkill[6] = 0;
        }
        if (playerSkill[9] == 1)    //피통
        {
            HpBar.Instance.GetHpBoost();
            playerSkill[9] = 0;
        }
        if (playerSkill[10] == 1)   //무적
        {
            immotalCount += Time.deltaTime;
            if(immotalCount > immotalRandom)
            {
                playerImmotal = true;

                count += Time.deltaTime;
                if(count > 1.5f)
                {
                    playerImmotal = false;
                }
            }
        }
        if(playerSkill[12] == 1)
        {
            playerCurrentExp += 500f;
            playerSkill[12] = 0;
        }
        if (playerSkill[13] == 1)
        {
            playerCurrentExp += 350f;
            playerSkill[13] = 0;
        }
        if (playerSkill[14] == 1)
        {
            currentHp += 240f;
            playerSkill[14] = 0;
        }
    }

    public void PlayerExpCalc(float exp)
    {
        playerCurrentExp += exp;
        if(playerCurrentExp >= playerLvUpExp)
        {
            playerLv++;
            playerCurrentExp -= playerLvUpExp;
            playerLvUpExp *= 1.3f;
            StartCoroutine(PlayerLevelUp());
        }
    }

    IEnumerator PlayerLevelUp()
    {
        yield return null;
        EffectSet.Instance.PlayerLvUpEffect.transform.position = player.transform.position;
        EffectSet.Instance.PlayerLvUpEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        UIController.Instance.PlayerLvUp(true);
        yield return new WaitForSeconds(1.5f);
        EffectSet.Instance.PlayerLvUpEffect.SetActive(false);
    }
}
