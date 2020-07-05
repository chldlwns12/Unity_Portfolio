using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMgr : MonoBehaviour
{
    public void OnTouchToScreen()
    {
        SceneMgr.Instance.LoadScene("GameScene");
    }
}
