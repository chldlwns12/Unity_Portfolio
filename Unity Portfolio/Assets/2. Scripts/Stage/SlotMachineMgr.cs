using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineMgr : MonoBehaviour
{
    public static SlotMachineMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SlotMachineMgr>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("SlotMachineMgr");
                    instance = instanceContainer.AddComponent<SlotMachineMgr>();
                }
            }
            return instance;
        }
    }
    private static SlotMachineMgr instance;

    public GameObject[] slotSkillObject;
    public Button[] slot;

    public Sprite[] skillSprite;

    [System.Serializable]
    public class DisplayItemSlot
    {
        public List<Image> slotSprite = new List<Image>();
    }
    public DisplayItemSlot[] displayItemSlots;

    public Image displayResultImage;
    

    public List<int> startList = new List<int>();
    public List<int> resultIndexList = new List<int>();
    int itemCount = 3;

    int[] answer = { 2, 3, 1 };

    void OnEnable()
    {
        //for (int i = 0; i < itemCount * slot.Length; i++)
        for (int i = 0; i < skillSprite.Length; i++)
        {
            startList.Add(i);
        }

        for (int i = 0; i < slot.Length; i++)
        {
            for (int j = 0; j < itemCount; j++)
            {
                slot[i].interactable = false;

                int randomIndex = Random.Range(0, startList.Count);
                if(i == 0 && j == 1 || i == 1 && j == 0 || i == 2 && j == 2)
                {
                    //resultIndexList.Add(randomIndex);
                    resultIndexList.Add(startList[randomIndex]);
                }
                //displayItemSlots[i].slotSprite[j].sprite = skillSprite[randomIndex];
                displayItemSlots[i].slotSprite[j].sprite = skillSprite[startList[randomIndex]];

                if(j == 0)
                {
                    //displayItemSlots[i].slotSprite[itemCount].sprite = skillSprite[randomIndex];
                    displayItemSlots[i].slotSprite[itemCount].sprite = skillSprite[startList[randomIndex]];
                }
                startList.RemoveAt(randomIndex);
            }
        }

        for (int i = 0; i < slot.Length; i++)
        {
            StartCoroutine(StartSlot(i));
        }
    }

    IEnumerator StartSlot(int slotIndex)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        for (int i = 0; i < (itemCount * (6 + slotIndex * 4) + answer[slotIndex]) * 2; i++)
        {
            slotSkillObject[slotIndex].transform.localPosition -= new Vector3(0, 50f, 0);
            if (slotSkillObject[slotIndex].transform.localPosition.y < 50f)
            {
                slotSkillObject[slotIndex].transform.localPosition += new Vector3(0, 300f, 0);
            }
            yield return new WaitForSeconds(0.02f);
        }

        slot[slotIndex].interactable = true;
    }

    public void OnClickButton(int index)
    {
        displayResultImage.sprite = skillSprite[resultIndexList[index]];
        Debug.Log("ResultIndex : " + resultIndexList[index]);
        PlayerData.Instance.resultIndex = resultIndexList[index];
        PlayerData.Instance.playerSkillUp = true;
        for (int i = 0; i < slotSkillObject.Length; i++)
        {
            slotSkillObject[i].transform.localPosition = new Vector3(0, 300f, 0);
        }

        resultIndexList.Clear();
        UIController.Instance.PlayerLvUp(false);
        //gameObject.SetActive(false);
    }
}
