using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour {

    public string LevelName;
    public string NextLevel;

    public GameObject LoadingPanel;

    public GameObject LevelTitle;
    public GameObject WinnerSplash;
    public float TitleWait = 2.0f;
    public float TitleDuration = 5.0f;
    private float levelStartTime = 0;

    private bool LevelComplete = false;

    public LevelInfo[] Levels;
    public LevelSet CurrentLevelSet;
    public float ButtonStart = -15;
    public float ButtonSpacing = -40;
    public GameObject MenuPanel;
    public GameObject ScrollView;
    public GameObject LevelButtonPrefab;
    private bool displayMenu = false;

    public bool ShowArrow = true;
    public bool ShowMiniMap = true;
    private GameObject miniMap;

    public Button[] WinningButtons;
    public RawImage WinningButtonSelectedHighlight;
    private int currentButtonSelected = 0; //0 for Next Level, 1 for Repeat Level

    // called first
    void OnEnable()
    {
        //Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") Destroy(gameObject);
        else
        {
            levelStartTime = Time.fixedTime;
            Text levelTitleText = null;
            if (LevelTitle) levelTitleText = LevelTitle.transform.GetChild(0).GetComponent<Text>();
            if (levelTitleText) levelTitleText.text = LevelName;



            if (MenuPanel) MenuPanel.SetActive(false);

            ShowArrowToggle(ShowArrow);
            miniMap = null;
            ShowMiniMapToggle(ShowMiniMap);

        }


    }

    private void Start()
    {
        LevelHandler lh = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelHandler>();
        if(lh && lh.gameObject != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }
        

        //levelStartTime = Time.fixedTime;
        //LevelTitle.transform.GetChild(0).GetComponent<Text>().text = LevelName;

        //MenuPanel.SetActive(false);

        for(int i = 0; i < Levels.Length; i+=1)
        {
            GameObject btnObject = Instantiate(LevelButtonPrefab, ScrollView.transform.position, Quaternion.identity);
            btnObject.transform.GetChild(0).GetComponent<Text>().text = Levels[i].LevelTitle;
            btnObject.transform.SetParent(ScrollView.transform);
            float PosY = ButtonStart + ButtonSpacing * i + ScrollView.GetComponent<RectTransform>().rect.height / 2;
            btnObject.GetComponent<RectTransform>().localPosition = new Vector3(0, PosY ,0);
            Levels[i].SetButton(btnObject.GetComponent<Button>());
            
        }
        LevelButtonPrefab.SetActive(false);
    }

    private void Update()
    {
        handleLevelTitle();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            displayMenu = !displayMenu;
            MenuPanel.SetActive(displayMenu);
        }
        

        handleWinnerPanelButtonsSelection();

        
    }

    
    private void handleWinnerPanelButtonsSelection(){
        if (WinnerSplash.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                WinningButtons[currentButtonSelected].onClick.Invoke();
               
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                int newIndex = currentButtonSelected - 1;
                if (newIndex < 0) newIndex = WinningButtons.Length - 1;
                setButtonSelectedHightlighted(newIndex);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                setButtonSelectedHightlighted((currentButtonSelected + 1) % WinningButtons.Length);
            }


        }
    }

    private void setButtonSelectedHightlighted(int index){
        currentButtonSelected = index;
        WinningButtonSelectedHighlight.rectTransform.position = WinningButtons[currentButtonSelected].transform.position;
    }

    private void handleLevelTitle()
    {
        if (Time.fixedTime - levelStartTime > TitleWait + TitleDuration) {
            LevelTitle.SetActive(false);
        }
        else if(Time.fixedTime - levelStartTime > TitleWait)
        {
            LevelTitle.SetActive(true);
        }
    }

    public void LoadLevel()
    {
        LevelInfo li = getLevel(SceneManager.GetActiveScene().name);
        setupNextLevel(li);
        LevelName = getLevel(li.NextLevel).LevelTitle;

        
        if(LoadingPanel) LoadingPanel.SetActive(true);

        SceneManager.LoadScene(NextLevel);
    }

    public void ReloadLoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LevelButtonClick(Button button)
    {
        for(int i = 0; i < Levels.Length; i += 1)
        {
            if(Levels[i].GetButton() == button)
            {
                setupNextLevel(Levels[i]);   
            }
        }
    }

    private void setupNextLevel(LevelInfo level)
    {
        //NextLevel = getLevel(level.NextLevel).NextLevel;
        LevelName = level.LevelTitle;
        NextLevel = level.NextLevel;
        //LevelName = level.LevelTitle;
        SceneManager.LoadScene(level.SceneName);
    }

    private LevelInfo getLevel(string sceneName)
    {
        LevelInfo li = null;
        foreach(LevelInfo l in Levels)
        {
            if (l.SceneName == sceneName) li = l;
        }

        return li;
    }

    public void LeverTriggered(bool isTriggered)
    {
        if (!LevelComplete && isTriggered) setButtonSelectedHightlighted(0); 
        LevelComplete = isTriggered;
        WinnerSplash.SetActive(LevelComplete);
        FindObjectOfType<Player>().ControlsFrozen = isTriggered;
        //FindObjectOfType<Player>().PositionFrozen = isTriggered;

    }

    public void ShowArrowToggle(bool state)
    {
        ShowArrow = state;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().DisplayArrow = ShowArrow;
    }
    
    public void ShowMiniMapToggle(bool state)
    {
        ShowMiniMap = state;
        if(!miniMap) miniMap = GameObject.FindGameObjectWithTag("MiniMap");
        if (miniMap)
        {
            miniMap.SetActive(ShowMiniMap);
        }
        
    }
}

[System.Serializable]
public class LevelInfo
{
    public string LevelTitle;
    public string SceneName;
    
    public string NextLevel;
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