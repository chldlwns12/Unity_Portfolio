using System.Collections;
using UnityEngine;

public class EnemyRangeFSM : EnemyBase
{
    public enum State
    {
        Idle,
        Move,
        Attack,
        Dead
    };

    public State currentState = State.Idle;

    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);

    protected void Start()
    {
        base.Start();
        parentRoom = transform.parent.transform.parent.gameObject;
        Debug.Log("Start - State :" + currentState.ToString());

        StartCoroutine(FSM());
    }
    protected virtual void InitMonster() { }

    protected virtual IEnumerator FSM()
    {
        yield return null;

        while (!parentRoom.GetComponent<RoomCondition>().playerInThisRoom)
        {
            yield return Delay500;
        }

        InitMonster();

        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    protected virtual IEnumerator Idle()
    {
        yield return null;
        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Anim.SetTrigger("Idle");
        }

        if (CanAtkStateFun())
        {
            if (canAtk)
            {
                currentState = State.Attack;
            }
            else
            {
                currentState = State.Idle;
                transform.LookAt(player.transform.position);
            }
        }
        else
        {
            currentState = State.Move;
        }
    }

    protected virtual void AtkEffect() { }

    protected virtual IEnumerator Attack()
    {
        yield return null;
        //Atk
        nvAgent.speed = moveSpeed;
        nvAgent.stoppingDistance = attackRange;
        nvAgent.isStopped = true;
        nvAgent.SetDestination(player.transform.position);
        yield return Delay500;

        canAtk = false;

        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Anim.SetTrigger("Attack");
        }
        AtkEffect();
        yield return Delay500;
        currentState = State.Move;
    }

    protected virtual IEnumerator Move()
    {
        yield return null;
        //Move
        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            Anim.SetTrigger("Walk");
        }
        if (CanAtkStateFun() && canAtk)
        {
            currentState = State.Attack;
        }
        else if (distance > playerRealizeRange)
        {
            nvAgent.SetDestination(transform.parent.position - Vector3.forward * 5f);
        }
        else
        {
            nvAgent.SetDestination(player.transform.position);
        }
    }
}
