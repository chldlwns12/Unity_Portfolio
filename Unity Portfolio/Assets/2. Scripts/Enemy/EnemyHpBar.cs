using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public GameObject Enemy;
    public Slider hpBar;
    public Slider hpBarBack;
    public float maxHp = 1000f;
    public float currentHp = 1000f;
    bool backHpHit = false;

    void Update()
    {
        transform.position = Enemy.transform.position;
        hpBar.value = Mathf.Lerp(hpBar.value, currentHp / maxHp, Time.deltaTime * 5f);

        if(backHpHit)
        {
            hpBarBack.value = Mathf.Lerp(hpBarBack.value, hpBar.value, Time.deltaTime * 10f);
            if (hpBar.value >= hpBarBack.value - 0.01f)
            {
                backHpHit = false;
                hpBarBack.value = hpBar.value;
            }
        }
    }

    public void Dmg()
    {
        StartCoroutine(Dameged());
    }

    IEnumerator Dameged()
    {
        currentHp -= PlayerData.Instance.damage;
        yield return new WaitForSeconds(0.5f);
        backHpHit = true;
    }
}
