using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "IttyGrave/LevelReference")]
public class LevelRef : ScriptableObject {

    public string SceneName;
    public string LevelTitle;
    //public string NextLevel;
    private Button button;

    public void SetButton(Button button)
    {
        this.button = button;
    }

    public Button GetButton()
    {
        return button;
    }
}
