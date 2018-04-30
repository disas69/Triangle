using System;
using Framework.UI.Structure.Base.Model;
using Framework.Variables;
using UnityEngine;

namespace Game.UI.Screens.Play
{
    [Serializable]
    [CreateAssetMenu(fileName = "PlayPageModel", menuName = "UI/Models/PlayPageModel")]
    public class PlayPageModel : PageModel
    {
        public IntVariable CurrentScore;
    }
}