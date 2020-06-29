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

    public float damage = 500;
    public float maxHp = 1000f;
    public float currentHp = 1000f;
    public GameObject player;
    //public GameObject playerBullet;
    public GameObject[] PlayerBullet;
    public GameObject itemExp;
    public int playerLv = 1;
    public float playerCurrentExp = 0f;
    public float playerLvUpExp = 500f;

    public bool playerDead = false;

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
