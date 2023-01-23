using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameManager
{
    static GameManager _instance = new GameManager();
    static public GameManager Instance => _instance;
    GameManager() { }

    SaveData _save = null;

    public static bool IsNewGame => _instance._save == null;
    public static SaveData SaveData => _instance._save;

    public static void Load()
    {
        _instance._save = LocalData.Load<SaveData>("save.json");
    }
    public static void UserCreateSave(string uuid, string name, int level)
    {
        if(_instance._save == null)
        {
            _instance._save = new SaveData();
        }

        _instance._save.UUID = uuid;
        _instance._save.Name = name;
        _instance._save.Player.Level = level;
        LocalData.Save<SaveData>("save.json", _instance._save);
    }

    
}
