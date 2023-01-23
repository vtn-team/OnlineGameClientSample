using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Title : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    void Start()
    {
        GameManager.Load();

        if(!GameManager.IsNewGame)
        {
            _text.text = "Continue";
        }
    }

    public void Next()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)(GameManager.IsNewGame ? SCENEID.UserCreate : SCENEID.Field));
    }
}
