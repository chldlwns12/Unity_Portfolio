using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgTxt : MonoBehaviour
{
    public TextMesh dmgText;

    void Start()
    {
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        transform.Translate(Vector3.up * 2f * Time.deltaTime);
    }

    public void DisplayDamage(float arrowDmg, bool isCritical)
    {
        if(isCritical)
        {
            dmgText.text = "<color=#ff0000>" + "-" + arrowDmg + "</color>";
        }
        else
        {
            dmgText.text = "<color=#ffffff>" + "-" + arrowDmg + "</color>";
        }

        //if (PlayerData.Instance.playerSkill[2] > 0)
        //{
        //    StartCoroutine(DoubleShot(arrowDmg, isCritical));
        //}
    }

    //IEnumerator DoubleShot(float arrowDmg, bool isCritical)
    //{
    //    yield return new WaitForSeconds(1f);
    //    
    //    if (isCritical)
    //    {
    //        dmgText.text = "<color=#ff0000>" + "-" + arrowDmg + "</color>";
    //    }
    //    else
    //    {
    //        dmgText.text = "<color=#ffffff>" + "-" + arrowDmg + "</color>";
    //    }
    //}
}
