using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class MenuSkillList : MonoBehaviour
{
    public static MenuSkillList Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MenuSkillList>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("MenuSkillList");
                    instance = instanceContainer.AddComponent<MenuSkillList>();
                }
            }
            return instance;
        }
    }
    private static MenuSkillList instance;

    public Sprite[] skillSprite;
    public Image[] skillList;
    public List<int> skillIndex;
    bool[] isSkillSet;
    float alpha = 1;

    private void Start()
    {
        skillIndex = new List<int>();
    }

    private void OnEnable()
    {
        isSkillSet = new bool[skillIndex.Count];

        if (PlayerData.Instance.playerSkill.Count > 0)
        {
            for (int i = 0; i < PlayerData.Instance.playerSkill.Count; i++)
            {
                if (PlayerData.Instance.playerSkill[i] > 0)
                {
                    skillIndex.Add(i);
                }
            }
        }

        if(skillIndex.Count > 0)
        {
            for (int i = 0; i < skillIndex.Count; i++)
            {
                skillList[i].sprite = skillSprite[skillIndex[i]];
                skillList[i].color = new Vector4(1, 1, 1, alpha);
            }
        }
    }

    private void OnDisable()
    {
        skillIndex.Clear();
    }
}
