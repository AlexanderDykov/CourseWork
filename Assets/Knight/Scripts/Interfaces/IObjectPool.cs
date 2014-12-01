using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Knight.Interfaces
{
    interface IObjectPool
    {
        List<GameObject> Create(GameObject obj, int count);
        void Spawn(GameObject obj, Vector3 position, Quaternion rotation);
        void Remove(GameObject obj);
    }
}
