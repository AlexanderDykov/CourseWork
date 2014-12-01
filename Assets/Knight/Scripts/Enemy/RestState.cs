using UnityEngine;
using System.Collections;
using Assets.Knight.Interfaces;

public class RestState : IState {

    public virtual void MonsterUpdate(Monster _monster)
    {
       /* if (!_monster.animation.isPlaying)
        {
          //  _monster.animation.PlayQueued("Sleep", QueueMode.CompleteOthers);
        }*/
    }

    public virtual void MonsterOnTriggerEnter(Monster _monster)
    {
       // _monster.animation.PlayQueued("GetUp", QueueMode.CompleteOthers);
        _monster.ChangeState(new AttackState());
    }

    public virtual void MonsterOnTriggerExit(Monster _monster) { }

    public virtual void MonsterToTargetDistance(Monster _monster, float distance) { }
}
