using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    #region Variables

    [Header("Texts")]
    public Text coinText;
    public Text speedPrice;
    public Text scorePrice;
    public Text speedLevelText;
    public Text scoreLevelText;

    [Header("Variables")]
    [HideInInspector] public int score;
    private int coin;
    private int scoreLevel;
    private int speedLevel;

    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        UpgradeDataControl();
    }

    #endregion

    #region Other Methods

    public void UpgradeDataControl()
    {
        // COIN
        coin = PlayerPrefs.GetInt("coin", 0);

        // SPEED LEVEL
        if (PlayerPrefs.HasKey("speedLevel"))
        {
            speedLevel = PlayerPrefs.GetInt("speedLevel");
            SpeedUpControl(speedLevel);
        }
        else
        {
            PlayerPrefs.SetInt("speedLevel", 0);
            speedLevel = 0;
        }

        // SCORE LEVEL
        if (PlayerPrefs.HasKey("scoreLevel"))
        {
            scoreLevel = PlayerPrefs.GetInt("scoreLevel");
            scoreLevelControl(scoreLevel);
        }
        else
        {
            PlayerPrefs.SetInt("scoreLevel", 0);
            scoreLevel = 0;
        }
    }

    public void scoreAdd()
    {
        score += Mathf.Clamp(score, 1, scoreLevel * 2);
    }

    public void DiscardCoin(int newCoin)
    {
        GameManager.Instance.AddCoin(-newCoin);
    }

    #endregion

    #region UPGRADE SHOP CONTROLLER

    // SPEED UP SHOP
    public void SpeedBuyButton()
    {
        coin = PlayerPrefs.GetInt("coin",0);
        int costCoin = (speedLevel + 1) * 100;

        if (coin >= costCoin && speedLevel <= 8)
        {
            PlayerController.Instance.Upgrade();
            SpeedUpControl(speedLevel + 1);
            DiscardCoin(costCoin);
        }
        else
            Debug.Log("not enough");
    }

    public void SpeedUpControl(int speedLevels)
    {
        PlayerPrefs.SetInt("speedLevel", speedLevels);
        speedLevel = PlayerPrefs.GetInt("speedLevel");
        PlayerController.Instance.SpeedUp(speedLevels);
        speedPrice.text = ((speedLevels + 1) * 100) + "COIN";
        speedLevelText.text = "LV. " + (speedLevel + 1);
    }

    // SCORE UP SHOP
    public void ScoreBuyButton()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
        int costCoin = (scoreLevel + 1) * 100;

        if (coin >= costCoin && scoreLevel <= 9)
        {
            PlayerController.Instance.Upgrade();
            scoreLevelControl(scoreLevel + 1);
            DiscardCoin(costCoin);
        }              
        else
            Debug.Log("not enough");
    }

    public void scoreLevelControl(int scoreLevels)
    {
        PlayerPrefs.SetInt("scoreLevel", scoreLevels);
        score = 1;
        scoreLevel = PlayerPrefs.GetInt("scoreLevel");
        scorePrice.text = ((scoreLevels + 1) * 100) + " COIN";
        scoreLevelText.text = "X " + (scoreLevel + 1);        
    }

    #endregion
}