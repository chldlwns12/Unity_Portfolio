using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject joyStickGo;
    public GameObject joyStickZoneGo;
    public GameObject slotMachineGo;
    public GameObject rouletteGo;
    public GameObject endGameGo;

    public Text clearRoomCnt;

    public Slider playerExpBar;
    public Slider bossHpBar;
    public Slider bossBackHpSlider;
    public bool backHpHit = false;
    public bool bossRoom = false;

    public Text playerLvText;

    public float bossCurrentHp;
    public float bossMaxHp;

    public static UIController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIController>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("UIController");
                    instance = instanceContainer.AddComponent<UIController>();
                }
            }
            return instance;
        }
    }
    private static UIController instance;

    void Start()
    {
        Debug.Log("playerExp : " + PlayerData.Instance.playerCurrentExp);
        Debug.Log("playerMaxExp : " + PlayerData.Instance.playerLvUpExp);
        playerExpBar.value = PlayerData.Instance.playerCurrentExp / PlayerData.Instance.playerLvUpExp;
        playerExpBar.gameObject.SetActive(true);
        bossHpBar.gameObject.SetActive(false);
        bossBackHpSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        if(!bossRoom)
        {
            playerExpBar.value = Mathf.Lerp(playerExpBar.value, PlayerData.Instance.playerCurrentExp / PlayerData.Instance.playerLvUpExp, 0.75f);
            playerLvText.text = "Lv." + PlayerData.Instance.playerLv;
        }
        else
        {
            bossHpBar.value = Mathf.Lerp(bossHpBar.value, bossCurrentHp / bossMaxHp, 5f * Time.deltaTime);

            if(backHpHit)
            {
                bossBackHpSlider.value = Mathf.Lerp(bossBackHpSlider.value, bossHpBar.value, 10f * Time.deltaTime);
                if(bossHpBar.value >= bossBackHpSlider.value - 0.01f)
                {
                    backHpHit = false;
                    bossBackHpSlider.value = bossHpBar.value;
                }
            }
        }
    }

    public void Dmg()
    {
        Invoke("BackHpFun", 0.5f);
    }
    void BackHpFun()
    {
        backHpHit = true;
    }

    public void CheckBossRoom(bool isBossRoom)
    {
        bossRoom = isBossRoom;

        if(isBossRoom)
        {
            playerExpBar.gameObject.SetActive(false);
            playerLvText.gameObject.SetActive(false);
            bossHpBar.gameObject.SetActive(true);
            bossBackHpSlider.gameObject.SetActive(true);
        }
        else
        {
            playerExpBar.gameObject.SetActive(true);
            playerLvText.gameObject.SetActive(true);
            bossHpBar.gameObject.SetActive(false);
            bossBackHpSlider.gameObject.SetActive(false);
        }
    }

    public void PlayerLvUp(bool isSlotMachineOn)
    {
        if (isSlotMachineOn)
        {
            joyStickGo.SetActive(false);
            joyStickZoneGo.SetActive(false);
            slotMachineGo.SetActive(true);
        }
        else
        {
            joyStickGo.SetActive(true);
            joyStickZoneGo.SetActive(true);
            slotMachineGo.SetActive(false);
        }
    }

    public void EndGame()
    {
        joyStickGo.SetActive(false);
        joyStickZoneGo.SetActive(false);
        StartCoroutine(EndGamePopUp());
    }

    IEnumerator EndGamePopUp()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        endGameGo.SetActive(true);
        if(StageMgr.Instance.clear)
        {
            clearRoomCnt.text = "     Chapter Clear!";
        }
        else
        {
            clearRoomCnt.text = "Stage : " + (StageMgr.Instance.currentStage - 1);
        }
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
