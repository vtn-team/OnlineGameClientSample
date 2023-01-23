using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class CreateUser : MonoBehaviour
{
    [SerializeField] TMP_InputField _input;

    bool _isCreate = false;

    class UserCreateResult
    {
        public string uuid;
        public int level;
    }

    private void Start()
    {
        _input.Select();
    }


    public void UserCreate()
    {
        string name = _input.text;
        if (name == "") return;
        if (_isCreate) return;

        _isCreate = true;

        GameAPI.API.CreateUser(name, (data) =>
        {
            string json = Encoding.UTF8.GetString(data);
            UserCreateResult result = JsonUtility.FromJson<UserCreateResult>(json);

            GameManager.UserCreateSave(result.uuid, name, result.level);
            
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)SCENEID.Field);
        });
    }
}
