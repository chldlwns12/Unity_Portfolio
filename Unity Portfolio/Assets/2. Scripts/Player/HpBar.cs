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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position;
        hpBar.value = currentHp / maxHp;
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
