using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteMgr : MonoBehaviour
{
    public GameObject roulettePlate;
    public GameObject roulettePanel;
    public Transform needle;

    public Sprite[] skillSprite;
    public Image[] displayItemSlot;

    List<int> startList = new List<int>();
    List<int> resultIndexList = new List<int>();
    int itemCount = 15;

    bool isClick = false;

    void Start()
    {
        isClick = false;
        for (int i = 0; i < itemCount; i++)
        {
            startList.Add(i);
        }

        for (int i = 0; i < displayItemSlot.Length - 1; i++)
        {
            int randomIndex = Random.Range(0, startList.Count);
            resultIndexList.Add(startList[randomIndex]);
            displayItemSlot[i].sprite = skillSprite[startList[randomIndex]];
            startList.RemoveAt(randomIndex);
        }

        //StartCoroutine(StartRoulette());
    }

    IEnumerator StartRoulette()
    {
        yield return new WaitForSeconds(2f);
        float randomSpd = Random.Range(1.0f, 5.0f);
        float rotateSpeed = 100f * randomSpd;

        while (true)
        {
            yield return null;
            if (rotateSpeed <= 0.01f) break;

            rotateSpeed = Mathf.Lerp(rotateSpeed, 0, Time.deltaTime * 2f);
            roulettePlate.transform.Rotate(0, 0, rotateSpeed);
        }
        yield return new WaitForSeconds(1f);
        Result();
    }

    private void Result()
    {
        int closetIndex = -1;
        float closetDis = 500f;
        float currentDis = 0f;

        for (int i = 0; i < displayItemSlot.Length - 1; i++)
        {
            currentDis = Vector2.Distance(displayItemSlot[i].transform.position, needle.position);
            if(closetDis > currentDis)
            {
                closetDis = currentDis;
                closetIndex = i;
            }
        }
        Debug.Log("closetIndex : " + closetIndex);
        if(closetIndex == -1)
        {
            Debug.Log("Something is wrong");
        }
        //displayItemSlot[6].sprite = displayItemSlot[closetIndex].sprite;

        Debug.Log("ResultIndex : " + resultIndexList[closetIndex]);

        RouletteOut(closetIndex);
    }

    private void RouletteOut(int index)
    {
        Debug.Log("ResultIndex : " + resultIndexList[index]);
        PlayerData.Instance.resultIndex = resultIndexList[index];
        PlayerData.Instance.playerSkillUp = true;
        gameObject.SetActive(false);
    }

    public void OnRouletteButton()
    {
        if (isClick == false)
        {
            isClick = true;
            StartCoroutine(StartRoulette());
        }
    }
}
