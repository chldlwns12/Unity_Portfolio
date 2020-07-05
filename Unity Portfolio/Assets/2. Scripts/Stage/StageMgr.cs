using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMgr : MonoBehaviour
{
    public GameObject Player;
    public GameObject openPotal;
    public GameObject closePotal;
    public static StageMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageMgr>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("StageMgr");
                    instance = instanceContainer.AddComponent<StageMgr>();
                }
            }
            return instance;
        }
    }
    private static StageMgr instance;


    [System.Serializable]
    public class StartPositionArray
    {
        public List<Transform> StartPosition = new List<Transform>();
    }

    public StartPositionArray[] startPositionArrays;    //0 1 2
    //startPositionArrays[0] 1~10 stage
    //startPositionArrays[1] 11~20 stage

    public List<Transform> StartPositionAngel = new List<Transform>();
    //천사방 3개
    public List<Transform> StartPositionBoss = new List<Transform>();
    //보스방 3개
    public Transform StartPositionLastBoss;
    //마지막보스 1개

    public int currentStage = 0;
    int LastStage = 20;

    public bool clear = false;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void NextStage()
    {
        currentStage++;
        if (currentStage > LastStage)
        {
            clear = true;
            UIController.Instance.EndGame();
            return;
        }
        if(currentStage % 5 != 0)   //Normal State
        {
            int arrayIndex = currentStage / 10;
            int randomIndex = Random.Range(0, startPositionArrays[arrayIndex].StartPosition.Count);
            Player.transform.position = startPositionArrays[arrayIndex].StartPosition[randomIndex].position;
            startPositionArrays[arrayIndex].StartPosition.RemoveAt(randomIndex);
        }
        else    //BossRoom or Angel
        {
            if(currentStage % 10 == 5)  //Angel
            {
                int randomIndex = Random.Range(0, StartPositionAngel.Count);
                Player.transform.position = StartPositionAngel[randomIndex].position;
            }
            else    //Boss
            {
                UIController.Instance.CheckBossRoom(true);
                if (currentStage == LastStage)   //LastBoss
                {
                    Player.transform.position = StartPositionLastBoss.position;
                }
                else
                {
                    //int randomIndex = Random.Range(0, StartPositionBoss.Count);
                    Player.transform.position = StartPositionBoss[0].position;
                    StartPositionBoss.RemoveAt(0);
                }
            }
        }
        CameraMove.Instance.CameraNextRoom();
    }
}
