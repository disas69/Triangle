using System;
using Framework.UI.Structure.Base.Model;
using Framework.Variables;
using UnityEngine;

namespace Game.UI.Screens.Replay
{
    [Serializable]
    [CreateAssetMenu(fileName = "ReplayPageModel", menuName = "UI/Models/ReplayPageModel")]
    public class ReplayPageModel : PageModel
    {
        public IntVariable BestScore;
        public IntVariable CurrentScore;
    }
}