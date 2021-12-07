using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GetReward : MonoBehaviour
{

    [Header("Singelton")]
    public static GetReward instance;
    
    [Header("GameObjects")]
    public GameObject RewardImage;
    public GameObject radialShine;
    public GameObject collectBTN;

    [Header("Values")]
    public float minimum = 0f;
    public float maximum = 0f;
    public int coin = 100;
    public float duration = 5.0f;
    
    [HideInInspector]
    float startTime;
    private bool once = true;

    private void Awake()
    {
        instance = this;
        GetValues();
    }
    private void Start()
    {
        startTime = Time.time;
    }

    public void addAmountToFill()
    {
        maximum += RewardFillControl.instance.amountStep;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            callFillBox();
        }

        float t = (Time.time - startTime) / duration;
        RewardImage.GetComponent<Image>().fillAmount = Mathf.SmoothStep(minimum, maximum, t);
        if (RewardImage.GetComponent<Image>().fillAmount == 1f)
        {
            if (once)
            {
                RewardImage.GetComponent<Animator>().SetBool("play", true);
                radialShine.SetActive(true);
                collectBTN.SetActive(true);
              //  nothanks.SetActive(true);
                once = false;
            }
        }
        radialShine.transform.Rotate(-Vector3.forward, 0.5f);
    }

    // this function add fill amount to box;
    public void callFillBox()
    {
        startTime = 0f;
        startTime = Time.time;
        addAmountToFill();
        StartCoroutine(delayMaxMin());
    }

    // this function call coin from collect button
    public void getCoins()
    {
        resetReward();
        RewardFillControl.instance.coin += coin;
        GameManager.Instance.selectGiftPanel.SetActive(true);
        GameManager.Instance.finishPanel.SetActive(false);
        this.gameObject.SetActive(false);
    }

    // this function reset reward box fill
    public void resetReward()
    {
        collectBTN.SetActive(false);
        radialShine.SetActive(false);
        RewardImage.GetComponent<Animator>().SetBool("play", false);
        minimum = 0f;
        maximum = 0f;
        SaveValues();
        RewardImage.GetComponent<Image>().fillAmount = 0f;
        startTime = 0f;
        once = true;
    }

    // this function control fill amount after fill complete his duration
    IEnumerator delayMaxMin()
    {
        yield
        return new WaitForSeconds(duration);
        minimum = maximum;
    }

    public void SaveValues()
    {
        minimum = maximum;

        PlayerPrefs.SetFloat("Max", maximum);
        PlayerPrefs.SetFloat("Min", minimum);
    }

    void GetValues()
    {
        maximum = PlayerPrefs.GetFloat("Max");
        minimum = PlayerPrefs.GetFloat("Min");
    }
}