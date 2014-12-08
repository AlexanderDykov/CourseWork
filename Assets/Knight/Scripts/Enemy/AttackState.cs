using UnityEngine;
using System.Collections;
using Assets.Knight.Interfaces;

public class AttackState : IState {
    private float tmpFireTime;
    public virtual void MonsterUpdate(Monster _monster)
    {
        _monster.animator.SetBool("Attack", true);
        if (tmpFireTime >= _monster.attackSpeed)
        {
            switch (_monster.monsterType)
            {
                case MonsterType.Skeleton:
                    PlayerStats.ChangeHP(-15f);
                    tmpFireTime = 0;
                    break;
                case MonsterType.Spider:
                    for (int i = 0; i < _monster.bullets.Count; i++)
                    {

                        if (!_monster.bullets[i].activeInHierarchy)
                        {
                            ObjectPool.Instance.Spawn(_monster.bullets[i], _monster.spawnBulletPosition.position, _monster.spawnBulletPosition.rotation);
                            break;
                        }
                    }
                    tmpFireTime = 0;
                    break;
            }
            
           // _monster.animator.SetTrigger(attack);
           
        }
        else
        {
            tmpFireTime += 1 * Time.deltaTime;
        }
    }


    public virtual void MonsterOnTriggerEnter(Monster _monster)
    {
        _monster.currTarget = _monster.targets[0].transform;
    }

    public virtual void MonsterOnTriggerExit(Monster _monster)
    {
      //  _monster.animation.PlayQueued("GoToSleep", QueueMode.CompleteOthers);
        _monster.ChangeState(new MovingState());
    }
    public virtual void MonsterToTargetDistance(Monster _monster, float distance) 
    {
        if (distance >= _monster.attackDistance)
        {
            _monster.ChangeState(new MovingState());
        }
    }
}
