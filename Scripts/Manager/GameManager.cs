﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    [Header("GameObjects")]
    [Space]
    public GameObject blocker;
    public GameObject giftContinuebutton;
    public UpgradeManager upgradeManager;

    [Header("UI Panel")]
    [Space]
    public GameObject finishPanel;
    public GameObject gameOverPanel;
    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject selectGiftPanel;

    // ======== TEXT =========
    public Text levelText;
    public Text coinText;

    [Header(("Variables"))]
    [HideInInspector] public bool isGameStarted;
    [HideInInspector] public int level;
    private int coin;
    private int clickNum = 0;

    // ===================================== START

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;       
        GetDatas();
        LevelGenerate();
    }

    // ===================================== GAME EVENTS

    public void FinishLevel()
    {
        PlayerController.Instance.playerAnim.Play("Win");
        isGameStarted = false;
        StartCoroutine(FinishPanel());
    }

    public IEnumerator FinishPanel()
    {
        yield return new WaitForSeconds(3f);
        gamePanel.SetActive(false);
        finishPanel.SetActive(true);
        AddCoin(50);
        GetReward.instance.callFillBox();
        level++;
        PlayerPrefs.SetInt("level", level);
    }

    public void GameOver()
    {
        isGameStarted = false;
        PlayerController.Instance.playerAnim.Play("Fail");
        StartCoroutine(OverPanel());
    }

    private IEnumerator OverPanel()
    {
        yield return new WaitForSeconds(3f);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    // ===================================== UI BUTTON

    public void StartButton()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        PlayerController.Instance.playerAnim.Play("Run");
        isGameStarted = true;
    }
    
    public void NextLevelButton()
    {
        GetReward.instance.SaveValues();
        SceneLoad();
    }

    public void RestartButton()
    {
        SceneLoad();
    }
   
    public void RewardButton()
    {
        AddCoin(50);
        SceneLoad();
    }

    // ===================================== LEVEL

    private void LevelGenerate()
    {
        int i = level - 1;
        LevelGenerator.Instance.SpawnLevel(i);
        coinText.text = coin.ToString();
        levelText.text = "LEVEL " + level.ToString();        
    }

    public void GetDatas()
    {
        // LEVEL
        if (PlayerPrefs.HasKey("level"))
        {
            level = PlayerPrefs.GetInt("level");
        }
        else
        {
            PlayerPrefs.SetInt("level", 1);
            level = 1;
        }

        // GEM
        if (PlayerPrefs.HasKey("coin"))
        {
            coin = PlayerPrefs.GetInt("coin");
        }
        else
        {
            PlayerPrefs.SetInt("coin", coin);
        }

        // SOUND
        if (!PlayerPrefs.HasKey("sound"))
        {
            PlayerPrefs.SetInt("sound", 1);
        }
    }

    public void AddCoin(int newCoin)
    {
        int prevCoin = PlayerPrefs.GetInt("coin");
        PlayerPrefs.SetInt("coin", prevCoin + newCoin);
        coin = PlayerPrefs.GetInt("coin");
        coinText.text = coin.ToString();
    }

    public void AddGiftCoin(int newCoin)
    {    
        clickNum++;
        Debug.Log(clickNum);
        if(clickNum >= 3)
        {
            blocker.SetActive(true);
            giftContinuebutton.SetActive(true);
        }
        int prevCoin = PlayerPrefs.GetInt("coin");
        PlayerPrefs.SetInt("coin", prevCoin + newCoin);
        coin = PlayerPrefs.GetInt("coin");
        coinText.text = coin.ToString();
    }

    public void SceneLoad()
    {
        SceneManager.LoadScene(0);
    }


}