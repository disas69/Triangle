using Framework.Tools.Data;
using UnityEngine;

namespace Game.Core.Data
{
    public class GameDataKeeper
    {
        private readonly JsonDataKeeper<GameData> _dataKeeper;
        public const string FileName = "GameData";

        public GameData Data { get; private set; }

        public GameDataKeeper()
        {
            _dataKeeper = new JsonDataKeeper<GameData>(Application.persistentDataPath + "/" + FileName, true);
        }

        public void Load()
        {
            Data = _dataKeeper.Load();
        }

        public void Save()
        {
            _dataKeeper.Save(Data);
        }
    }
}