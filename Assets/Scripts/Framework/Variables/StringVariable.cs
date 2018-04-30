using System;
using Framework.Variables.Generic;
using UnityEngine;

namespace Framework.Variables
{
    [Serializable]
    [CreateAssetMenu(fileName = "StringVariable", menuName = "Variables/StringVariable")]
    public class StringVariable : Variable<string>
    {
    }
}