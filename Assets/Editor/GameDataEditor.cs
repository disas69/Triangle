using Game.Core.Data;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class GameDataEditor
    {
        [MenuItem("Triangle/Clear Game Data")]
        public static void ClearData()
        {
            FileUtil.DeleteFileOrDirectory(Application.persistentDataPath + "/" + GameDataKeeper.FileName);
        }
    }
}