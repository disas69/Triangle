using System;
using Framework.Variables.Generic;
using UnityEngine;

namespace Framework.Variables
{
    [Serializable]
    [CreateAssetMenu(fileName = "IntVariable", menuName = "Variables/IntVariable")]
    public class IntVariable : Variable<int>
    {
    }
}