using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    [Header("GameObjects")]
    [Space]
    public GameObject player;
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
    public GameObject bonusWinPanel;
    public Image progressBar;
    // ======== TEXT =========
    public Text levelText;
    public Text coinText;
    public Text scoreText;
    public Text bonusText;

    [Header(("Perfect Sprites"))]
    [Space]
    [SerializeField] Sprite[] perfectSprites;
    [SerializeField] Sprite[] terribleSprites;
    public Image amazing;
    public Image perfect;
    public Image terrible;

    [Header(("Variables"))]
    [HideInInspector] public bool isGameStarted;
    [HideInInspector] public int level;
    [SerializeField] private int sceneIndex;
    private int coin;
    private int clickNum = 0;
    private int bonusMultipier;

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
        isGameStarted = false;
    }

    public IEnumerator FinishPanel()
    {
        yield return new WaitForSeconds(1f);
        bonusText.text = "+" + bonusMultipier;
        gamePanel.SetActive(false);
        finishPanel.SetActive(true);
        AddCoin(bonusMultipier);
        GetReward.instance.callFillBox();
        level++;
        PlayerPrefs.SetInt("level", level);
    }

    public void GameOver()
    {
        isGameStarted = false;
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
   
    public void BonusLevelClaimButton()
    {
        AddCoin(50);
        SceneLoad();
    }

    public void RewardButton()
    {
        AddCoin(50);
        SceneLoad();
    }

    public void SetBonus(int value)
    {
        if(value > bonusMultipier)
            bonusMultipier = value;
    }

    public float GetBonusMultipier()
    {
        return bonusMultipier;
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
        SceneManager.LoadScene(sceneIndex);
    }

    // ===================================== PEFRECT SYSTEM

    public void Perfector()
    {
        perfect.gameObject.SetActive(true);
        perfect.transform.DOScale(5, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            perfect.transform.DOScale(1, 0);
            perfect.gameObject.SetActive(false);
        });
    }
   
    public void Amazer()
    {
        int random = Random.Range(0, perfectSprites.Length);
        amazing.sprite = perfectSprites[random];
        amazing.gameObject.SetActive(true);
        amazing.transform.DOScale(4, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            amazing.transform.DOScale(1, 0);
            amazing.gameObject.SetActive(false);
        });
    }
    
    public void Terribler()
    {
        int random = Random.Range(0, terribleSprites.Length);
        terrible.sprite = terribleSprites[random];
        terrible.gameObject.SetActive(true);
        terrible.transform.DOScale(4, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            terrible.transform.DOScale(1, 0);
            terrible.gameObject.SetActive(false);
        });
    }

}