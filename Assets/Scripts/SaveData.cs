using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int Level;
    public Vector3 Position;
    public Vector3 Rotation;
}

[Serializable]
public class SaveData
{
    public int GameVersion = 1;
    public string UUID = "";
    public string Name = "";
    public PlayerData Player = new PlayerData();
}
