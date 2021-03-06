﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSet : MonoBehaviour
{
    public static EffectSet Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EffectSet>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("EffectSet");
                    instance = instanceContainer.AddComponent<EffectSet>();
                }
            }
            return instance;
        }
    }
    private static EffectSet instance;
    //public float dmgCount = 0f;

    [Header("Monster")]
    public GameObject oneAtkEffect;
    public GameObject oneDmgEffect;
    public GameObject monsterDmgText;

    [Header("Player")]
    public GameObject playerAtkEffect;
    public GameObject playerDmgEffect;
    
    
}
