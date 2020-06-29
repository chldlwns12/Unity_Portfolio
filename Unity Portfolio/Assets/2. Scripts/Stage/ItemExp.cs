using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExp : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerData.Instance.player;

        StartCoroutine(WaitClearRoom());
    }

    IEnumerator WaitClearRoom()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            while (transform.parent.gameObject.GetComponent<RoomCondition>().isClearRoom)
            {
                transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.2f);
                yield return null;
            }
        }
    }
}
