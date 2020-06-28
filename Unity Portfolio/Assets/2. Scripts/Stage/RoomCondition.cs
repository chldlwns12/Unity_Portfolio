using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCondition : MonoBehaviour
{
    List<GameObject> MonsterListInRoom = new List<GameObject>();
    public bool playerInThisRoom = false;
    public bool isClearRoom = false;

    GameObject nextGate;

    public int playerInRoomIndex;

    private void Start()
    {
        nextGate = transform.GetChild(4).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInThisRoom)
        {
            if(PlayerTargeting.Instance.monsterList.Count <= 0 && !isClearRoom)
            {
                isClearRoom = true;
                StartCoroutine(OpenDoor());
            }
        }
    }

    IEnumerator OpenDoor()
    {
        yield return null;
        Debug.Log("Clear");
        yield return new WaitForSeconds(1.5f);

        StageMgr.Instance.closePotal.SetActive(false);
        StageMgr.Instance.openPotal.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            MonsterListInRoom.Add(other.transform.parent.gameObject);
            //Debug.Log("Mob name :" + other.transform.parent.gameObject);
        }
        if(other.CompareTag("Player"))
        {
            playerInThisRoom = true;
            PlayerTargeting.Instance.monsterList = new List<GameObject>(MonsterListInRoom);
            //Debug.Log("Enter New Room! Mob Count :" + PlayerTargeting.Instance.monsterList.Count);

            StageMgr.Instance.closePotal.transform.position = nextGate.transform.position + new Vector3(0, 0.7f, 0);
            StageMgr.Instance.openPotal.transform.position = nextGate.transform.position + new Vector3(0, 0.7f, 0);
            if (StageMgr.Instance.currentStage == 5 || StageMgr.Instance.currentStage == 15)
            {
                StageMgr.Instance.closePotal.SetActive(false);
                StageMgr.Instance.openPotal.SetActive(true);
            }
            else
            {
                StageMgr.Instance.closePotal.SetActive(true);
                StageMgr.Instance.openPotal.SetActive(false);
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInThisRoom = false;
    //        Debug.Log("Player Exit!");
    //    }
    //    if (other.CompareTag("Monster"))
    //    {
    //        MonsterListInRoom.Remove(other.gameObject);
    //    }
    //}
}
