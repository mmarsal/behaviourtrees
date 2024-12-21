using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedGameObject : SharedVariable<GameObject>
    {
        internal T GetComponent<T>()
        {
            throw new NotImplementedException();
        }

        public static implicit operator SharedGameObject(GameObject value) { return new SharedGameObject { mValue = value }; }
    }
}