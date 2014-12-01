using UnityEngine;
using System.Collections;

namespace Assets.Knight.Interfaces
{
    public interface IWeapone 
    {
        void Attack(GameObject target,float damage);
    }
}