using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickMove : MonoBehaviour
{
    public GameObject smallStick;
    public GameObject bGStick;
    Vector3 stickFirstPosition;
    public Vector3 joyVec;
    Vector3 joyStickFirstPosition;
    float stickRadius;
    public bool isPlayerMoving = false;

    public static JoyStickMove Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<JoyStickMove>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("JoyStickMove");
                    instance = instanceContainer.AddComponent<JoyStickMove>();
                }
            }
            return instance;
        }
    }
    private static JoyStickMove instance;

    void Start()
    {
        stickRadius = bGStick.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        joyStickFirstPosition = bGStick.transform.position;
    }

    public void PointDown()
    {
        bGStick.transform.position = Input.mousePosition;
        smallStick.transform.position = Input.mousePosition;
        stickFirstPosition = Input.mousePosition;

        if(!PlayerMove.Instance.Anim.GetCurrentAnimatorStateInfo (0).IsName ("Walk"))
        {
            PlayerMove.Instance.Anim.SetBool("Attack", false);
            PlayerMove.Instance.Anim.SetBool("Idle", false);
            PlayerMove.Instance.Anim.SetBool("Walk", true);
        }
        isPlayerMoving = true;
        //PlayerTargeting.Instance.GetATarget = false;
    }

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 DragPosition = pointerEventData.position;
        joyVec = (DragPosition - stickFirstPosition).normalized;
        float stickDistance = Vector3.Distance(DragPosition, stickFirstPosition);

        if(stickDistance < stickRadius)
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickDistance;
        }
        else
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickRadius;
        }
    }

    public void Drop()
    {
        joyVec = Vector3.zero;
        bGStick.transform.position = joyStickFirstPosition;
        smallStick.transform.position = joyStickFirstPosition;

        if (!PlayerMove.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            PlayerMove.Instance.Anim.SetBool("Attack", false);
            PlayerMove.Instance.Anim.SetBool("Idle", true);
            PlayerMove.Instance.Anim.SetBool("Walk", false);
        }
        isPlayerMoving = false;
    }
}
