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
    public Image[] DisplayItemSlot;

    List<int> StartList = new List<int>();
    List<int> ResultIndexList = new List<int>();
    int itemCount = 6;

    public float endCount = 2.0f;
    float currentCount = 0.0f;

    void Start()
    {
        for (int i = 0; i < itemCount; i++)
        {
            StartList.Add(i);
        }

        for (int i = 0; i < itemCount; i++)
        {
            int randomIndex = Random.Range(0, StartList.Count);
            ResultIndexList.Add(StartList[randomIndex]);
            DisplayItemSlot[i].sprite = skillSprite[StartList[randomIndex]];
            StartList.RemoveAt(randomIndex);
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
            if (rotateSpeed <= 0.01f)
            {
                gameObject.SetActive(false);
                break;
            }
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

        for (int i = 0; i < itemCount; i++)
        {
            currentDis = Vector2.Distance(DisplayItemSlot[i].transform.position, needle.position);
            if(closetDis > currentDis)
            {
                closetDis = currentDis;
                closetIndex = i;
            }
        }
        Debug.Log("LV UP Index : " + closetIndex);
        if(closetIndex == -1)
        {
            Debug.Log("Something is wrong");
        }
        DisplayItemSlot[itemCount].sprite = DisplayItemSlot[closetIndex].sprite;

        Debug.Log("LV UP Index : " + ResultIndexList[closetIndex]);
    }

    public void OnRouletteButton()
    {
        StartCoroutine(StartRoulette());
    }
}
