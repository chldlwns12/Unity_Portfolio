using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControll : MonoBehaviour
{
    public GameObject enemyCanvasGo;

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Arrow"))
        {
            //enemyCanvasGo.GetComponent<EnemyHpBar>().Dmg();
        }
    }
}
