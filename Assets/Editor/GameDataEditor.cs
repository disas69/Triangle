using Game.Core.Data;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class GameDataEditor
    {
        [MenuItem("Game/Clear Data")]
        public static void ClearData()
        {
            FileUtil.DeleteFileOrDirectory(Application.persistentDataPath + "/" + GameDataKeeper.FileName);
        }
    }
}