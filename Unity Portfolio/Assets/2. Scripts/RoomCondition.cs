using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCondition : MonoBehaviour
{
    List<GameObject> MonsterListInRoom = new List<GameObject>();
    public bool playerInThisRoom = false;
    public bool isClearRoom = false;

    // Update is called once per frame
    void Update()
    {
        if(playerInThisRoom)
        {
            if(MonsterListInRoom.Count <= 0 && !isClearRoom)
            {
                isClearRoom = true;
                Debug.Log("Clear");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //플레이어가 방에 들어오면 이방의 몹리스트를 링크(복사)시킨다.
            playerInThisRoom = true;
            PlayerTargeting.Instance.monsterList = new List<GameObject>(MonsterListInRoom);
            Debug.Log("Enter New Room! Mob Count :" + PlayerTargeting.Instance.MonsterList.Count);
            //Debug.Log("Player Enter New Room!");
        }
        if(other.CompareTag("Monster"))
        {
            MonsterListInRoom.Add(other.gameObject);
            Debug.Log("Mob name :" + other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInThisRoom = false;
            Debug.Log("Player Exit!");
        }
        if (other.CompareTag("Monster"))
        {
            MonsterListInRoom.Remove(other.gameObject);
        }
    }
}
