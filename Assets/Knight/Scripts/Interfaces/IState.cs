using UnityEngine;
using System.Collections;

namespace Assets.Knight.Interfaces
{
    public enum MonsterType
    {
        Skeleton,
        Spider
    }
    public interface IState
    {
        void MonsterUpdate(Monster _monster);
        void MonsterOnTriggerEnter(Monster _monster);
        void MonsterOnTriggerExit(Monster _monster);
        void MonsterToTargetDistance(Monster _monster, float distance);
    }
}
