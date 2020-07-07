using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : MonoBehaviour
{
    public GameObject rouletteGO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rouletteGO.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
