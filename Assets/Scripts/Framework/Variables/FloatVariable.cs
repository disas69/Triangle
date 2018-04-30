using System;
using Framework.Variables.Generic;
using UnityEngine;

namespace Framework.Variables
{
    [Serializable]
    [CreateAssetMenu(fileName = "FloatVariable", menuName = "Variables/FloatVariable")]
    public class FloatVariable : Variable<float>
    {
    }
}