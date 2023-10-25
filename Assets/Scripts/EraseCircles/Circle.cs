using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunBase.EraseCircles
{
    /// <summary>
    /// Circle class attached on each Spawned Circle
    /// </summary>
    public class Circle : MonoBehaviour, ICircle
    {
        public void OnCollideWithLine()
        {
            throw new System.NotImplementedException();
        }
    }
}