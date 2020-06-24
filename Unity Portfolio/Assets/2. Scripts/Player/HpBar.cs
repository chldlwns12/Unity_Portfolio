using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public GameObject Player;
    public Slider hpBar;
    public float maxHp = 1000f;
    public float currentHp = 1000f;

    public GameObject HpLineFolder;
    float unitHp = 200f;

    public Text playerHpText;

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
        transform.position = Player.transform.position;
        hpBar.value = currentHp / maxHp;
        playerHpText.text = "" + currentHp;
    }

    public void GetHpBoost()
    {
        maxHp += 210f;
        currentHp += 210f;
        float scaleX = (1000f / unitHp) / (maxHp / unitHp);
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach (Transform child in HpLineFolder.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
        }
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }
}
