using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public GameObject Player;
    public Image fadeInOut;

    public float offsetY = 40f;
    public float offsetZ = -30f;

    Vector3 cameraPosition;

    public static CameraMove Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraMove>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("CameraMove");
                    instance = instanceContainer.AddComponent<CameraMove>();
                }
            }
            return instance;
        }
    }
    private static CameraMove instance;

    private void Start()
    {
        CameraNextRoom();
        
    }

    private void LateUpdate()
    {
        cameraPosition.y = Player.transform.position.y + offsetY;
        cameraPosition.z = Player.transform.position.z + offsetZ;

        transform.position = cameraPosition;
    }

    public void CameraNextRoom()
    {
        StartCoroutine(FadeInOut());
        cameraPosition.x = Player.transform.position.x;
    }

    IEnumerator FadeInOut()
    {
        float alpha = 1;
        fadeInOut.color = new Vector4(1, 1, 1, alpha);
        yield return new WaitForSeconds(0.3f);

        while (alpha >= 0)
        {
            fadeInOut.color = new Vector4(1, 1, 1, alpha);
            alpha -= 0.02f;
            yield return null;
        }
    }
}
