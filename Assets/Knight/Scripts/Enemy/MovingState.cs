using UnityEngine;
using System.Collections;
using Assets.Knight.Interfaces;

public class MovingState : IState{
   // private bool isFacingRight = true;
    public void MonsterUpdate(Monster _monster)
    {
      //  distance = Vector2.Distance( _monster.currTarget.position, _monster.monster.position);
     //   if (distance > _monster.attackDistance)
     //   {
        _monster.animator.SetBool("Attack", false);
        _monster.monster.transform.Translate(Vector2.right * _monster.moveSpeed * Time.smoothDeltaTime);
      //  }        
    }

    public void MonsterOnTriggerEnter(Monster _monster)
    {
        _monster.currTarget = _monster.targets[0].transform;
        //_monster.ChangeState(new AttackState());
    }

    public void MonsterOnTriggerExit(Monster _monster)
    {
        //_monster.currTarget = 
    }

    public virtual void MonsterToTargetDistance(Monster _monster, float distance) 
    { 
        if(distance <= _monster.attackDistance)
        {
            _monster.ChangeState(new AttackState());
        }
        else if (distance >= _monster.boringDistance)
        {
            _monster.currTarget = _monster.targets[1].transform;
        }
    }
}
