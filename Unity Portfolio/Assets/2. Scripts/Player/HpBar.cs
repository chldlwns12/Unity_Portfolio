using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public GameObject player;
    public Slider hpBar;
    public Slider hpBarBack;

    public GameObject HpLineFolder;
    float unitHp = 200f;

    public Text playerHpText;

    bool backHpHit = false;

    public static HpBar Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HpBar>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("HpBar");
                    instance = instanceContainer.AddComponent<HpBar>();
                }
            }
            return instance;
        }
    }
    private static HpBar instance;

    void Update()
    {
        transform.position = player.transform.position;
        hpBar.value = Mathf.Lerp(hpBar.value, PlayerData.Instance.currentHp / PlayerData.Instance.maxHp, Time.deltaTime * 5f);

        if (backHpHit)
        {
            hpBarBack.value = Mathf.Lerp(hpBarBack.value, hpBar.value, Time.deltaTime * 10f);
            if (hpBar.value >= hpBarBack.value - 0.01f)
            {
                backHpHit = false;
                hpBarBack.value = hpBar.value;
            }
        }
        playerHpText.text = "" + PlayerData.Instance.currentHp;
    }

    public void Dmg()
    {
        StartCoroutine(Dameged());
    }

    IEnumerator Dameged()
    {
        yield return new WaitForSeconds(0.5f);
        backHpHit = true;
    }

    public void GetHpBoost()
    {
        PlayerData.Instance.maxHp += 210f;
        PlayerData.Instance.currentHp += 210f;
        float scaleX = (1000f / unitHp) / (PlayerData.Instance.maxHp / unitHp);
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach (Transform child in HpLineFolder.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
        }
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }
}
