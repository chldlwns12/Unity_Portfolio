using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public bool getATarget = false;
    float currentDist = 0;                  //현재 거리
    float closetDist = 100f;                //가까운 거리
    float targetDist = 100f;                //타겟 거리
    int closeDistIndex = 0;                //가장 가까운 인덱스
    public int targetIndex = -1;                  //타겟팅 할 인덱스
    int prevTargetIndex = 0;
    public LayerMask layerMask;

    public List<GameObject> monsterList = new List<GameObject>();

    //public GameObject playerBolt;
    public Transform attackPoint;
    public Transform doublePoint1;
    public Transform doublePoint2;
    public Transform leftAttackPoint;
    public Transform rightAttackPoint;
    public Transform backAttackPoint;

    //오브젝트 풀링 
    public Queue<GameObject> arrowPool;
    //public Queue<GameObject> doubleArrowPool;
    //public Queue<GameObject> leftArrowPool;
    //public Queue<GameObject> rightArrowPool;
    //public Queue<GameObject> backArrowPool;
    int poolSize = 20;
    int fireIndex = 0;

    public void ArrowEnqueue(GameObject arrowObject)
    {
        arrowObject.SetActive(false);
        arrowPool.Enqueue(arrowObject);
        Debug.Log("Enqueued : " + arrowObject.name);
    }

    public static PlayerTargeting Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerTargeting>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerTargeting");
                    instance = instanceContainer.AddComponent<PlayerTargeting>();
                }
            }
            return instance;
        }
    }
    private static PlayerTargeting instance;

    private void Start()
    {
        //오브젝트 풀링 초기화
        InitObjectPooling();
    }
    
    private void InitObjectPooling()
    {
        arrowPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject arrow = Instantiate(PlayerData.Instance.PlayerBullet[0]);
            arrow.SetActive(false);
            arrowPool.Enqueue(arrow);
        }
        //doubleArrowPool = new Queue<GameObject>();
        //for (int i = 0; i < poolSize; i++)
        //{
        //    GameObject doubleArrow = Instantiate(PlayerData.Instance.PlayerBullet[1]);
        //    doubleArrow.SetActive(false);
        //    doubleArrowPool.Enqueue(doubleArrow);
        //}
    }

    void Update()
    {
        SetTarget();
        AttackTarget();
    }

    private void SetTarget()
    {
        if (monsterList.Count != 0)
        {
            prevTargetIndex = targetIndex;
            currentDist = 0f;
            closeDistIndex = 0;
            targetIndex = -1;

            for (int i = 0; i < monsterList.Count; i++)
            {
                if (monsterList[i] == null) { return; }
                currentDist = Vector3.Distance(transform.position, monsterList[i].transform.GetChild(0).position);

                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, monsterList[i].transform.GetChild(0).position - transform.position,
                                             out hit, 20f, layerMask);

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    if (targetDist >= currentDist)
                    {
                        targetIndex = i;
                        targetDist = currentDist;

                        if (!JoyStickMove.Instance.isPlayerMoving && prevTargetIndex != targetIndex)
                        {
                            targetIndex = prevTargetIndex;
                        }
                    }
                }

                if (closetDist >= currentDist)
                {
                    closeDistIndex = i;
                    closetDist = currentDist;
                }
            }

            if (targetIndex == -1)
            {
                targetIndex = closeDistIndex;
            }
            closetDist = 100f;
            targetDist = 100f;
            getATarget = true;
        }
    }


    private void AttackTarget()
    {
        if (targetIndex == -1 || monsterList.Count == 0)  // 추가 
        {
            PlayerMove.Instance.Anim.SetBool("Attack", false);
            return;
        }
        if (getATarget && !JoyStickMove.Instance.isPlayerMoving && monsterList.Count != 0)
        {
            transform.LookAt(monsterList[targetIndex].transform.GetChild(0));

            //Debug.Log("Attack");
            if (PlayerMove.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                PlayerMove.Instance.Anim.SetBool("Idle", false);
                PlayerMove.Instance.Anim.SetBool("Walk", false);
                PlayerMove.Instance.Anim.SetBool("Attack", true);
            }
        }
        else if (JoyStickMove.Instance.isPlayerMoving)
        {
            //currentCount = 0.0f;
            if (!PlayerMove.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                PlayerMove.Instance.Anim.SetBool("Idle", false);
                PlayerMove.Instance.Anim.SetBool("Walk", true);
                PlayerMove.Instance.Anim.SetBool("Attack", false);
            }
        }
    }

    public void Attack()
    {
        PlayerMove.Instance.Anim.SetFloat("AttackSpeed", PlayerData.Instance.atkSpeed);
        //Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]], AttackPoint.position, transform.rotation);
        //기본화살발사 및 전방화살
        if (PlayerData.Instance.playerSkill[2] > 0)
        {
            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = doublePoint1.transform.position;
                bullet.transform.forward = doublePoint1.transform.forward;
                Debug.Log("Attack Forward : " + bullet.name);
            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }

            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = doublePoint2.transform.position;
                bullet.transform.forward = doublePoint2.transform.forward;
                Debug.Log("Attack Forward : " + bullet.name);
            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }
        }
        else
        {
            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = attackPoint.transform.position;
                bullet.transform.forward = attackPoint.transform.forward;
                Debug.Log("Attack Forward : " + bullet.name);
            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }
        }
        //멀티샷
        if (PlayerData.Instance.playerSkill[1] > 0)
        {
            Invoke("MultiShot", 0.2f);
        }

        //사선화살
        if(PlayerData.Instance.playerSkill[3] > 0)
        {
            //왼쪽
            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = attackPoint.transform.position;
                bullet.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, -45f, 0));
            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }
            //오른쪽
            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = attackPoint.transform.position;
                bullet.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 45f, 0));

            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }

            //Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[3] - 1], AttackPoint.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, -45f, 0)));
            //Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[3] - 1], AttackPoint.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, 45f, 0)));
        }

        //후방화살
        if (PlayerData.Instance.playerSkill[7] > 0)
        {
            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = backAttackPoint.transform.position;
                bullet.transform.forward = backAttackPoint.transform.forward;
                Debug.Log("Attack Back : " + bullet.name);
            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }
        }

        //사이드화살
        if (PlayerData.Instance.playerSkill[11] > 0)
        {
            //왼쪽
            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = leftAttackPoint.transform.position;
                bullet.transform.forward = leftAttackPoint.transform.forward;
            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }
            //오른쪽
            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = rightAttackPoint.transform.position;
                bullet.transform.forward = rightAttackPoint.transform.forward;
                
            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }
        }
    }

    void MultiShot()
    {
        //Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]], AttackPoint.position, transform.rotation);
        if (PlayerData.Instance.playerSkill[2] > 0)
        {
            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = doublePoint1.transform.position;
                bullet.transform.forward = doublePoint1.transform.forward;
                Debug.Log("Attack Forward : " + bullet.name);
            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }

            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = doublePoint2.transform.position;
                bullet.transform.forward = doublePoint2.transform.forward;
                Debug.Log("Attack Forward : " + bullet.name);
            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }
        }
        else
        {
            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = attackPoint.transform.position;
                bullet.transform.forward = attackPoint.transform.forward;
                Debug.Log("Attack Forward : " + bullet.name);
            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }
        }

        if (PlayerData.Instance.playerSkill[3] > 0)
        {
            //왼쪽
            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = attackPoint.transform.position;
                bullet.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, -45f, 0));
            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }
            //오른쪽
            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = attackPoint.transform.position;
                bullet.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 45f, 0));

            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }
        }

        //후방화살
        if (PlayerData.Instance.playerSkill[7] > 0)
        {
            if (arrowPool.Count > 0)
            {
                GameObject bullet2 = arrowPool.Dequeue();
                bullet2.SetActive(true);
                bullet2.transform.position = backAttackPoint.transform.position;
                bullet2.transform.forward = backAttackPoint.transform.forward;
                Debug.Log("Attack Back : " + bullet2.name);
            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet2 = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet2.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet2);
            }
        }

        //사이드화살
        if (PlayerData.Instance.playerSkill[11] > 0)
        {
            //왼쪽
            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = leftAttackPoint.transform.position;
                bullet.transform.forward = leftAttackPoint.transform.forward;
            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }
            //오른쪽
            if (arrowPool.Count > 0)
            {
                GameObject bullet = arrowPool.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = rightAttackPoint.transform.position;
                bullet.transform.forward = rightAttackPoint.transform.forward;

            }
            else
            {
                //총알 오브젝트 생성한다
                GameObject bullet = Instantiate(PlayerData.Instance.PlayerBullet[PlayerData.Instance.playerSkill[2]]);
                bullet.SetActive(false);
                //생성된 총알 오브젝트를 풀에 담는다.
                arrowPool.Enqueue(bullet);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(getATarget)
        {
            for (int i = 0; i < monsterList.Count; i++)
            {
                if (monsterList[i] == null) { return; }
                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, monsterList[i].transform.GetChild(0).position - transform.position,
                                             out hit, 20f, layerMask);

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawRay(transform.position, monsterList[i].transform.GetChild(0).position - transform.position);
            }
        }
    }
}
