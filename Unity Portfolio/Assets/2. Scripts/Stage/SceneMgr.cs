using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr Instance;
    private void Awake()
    {
        //씬매니져가 존재한다면
        //새로생성되는 씬매니져는 삭제하고 바로 빠져나와라
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        //인스턴스가 없을때
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string value)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(value);
    }
}
