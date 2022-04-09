using UnityEngine;
using DG.Tweening;
using LPWAsset;
using TMPro;

public class PlayerController : Singleton<PlayerController>
{
    [Header("GameObjects")]
    [Space]
    public Animator playerAnim;
    [SerializeField] ParticleSystem upgradeFx;
    [SerializeField] SkinnedMeshRenderer[] myBody;
    
    [Header("Other")]
    [Space]
    [SerializeField] TextMeshProUGUI plusText;
    [SerializeField] UpgradeManager up;
    [SerializeField] LowPolyWaterScript water;
    private CookieList cookieList;

    [Header("Variables")]
    [Space]
    public int playerColorNumber;
    [HideInInspector] public int perfectCounter;
    [HideInInspector] public int terribleCounter;
    [HideInInspector] public bool canPickBonus;
    private Vector3 targetOffset;

   // ===================================== START
   
    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Color color = new Color32(0, 111, 255, 255);
        water.material.DOColor(color, 1f);
        cookieList = GameObject.Find("COOKIE LIST").GetComponent<CookieList>();
    } 
  
   // ================== UPGRADE METHODS

    public void SpeedUp(int addSpeed)
    {
       //_moveSpeed += addSpeed;
    }
    
    public void Upgrade()
    {
        upgradeFx.Play();
    }

    // ================== COLOR MATCH METHODS

    private void RightColor()
    {
        PerfectCounterSystem(true);
        up.scoreAdd();
        PlusSpawner(true);
    }

    public void WrongColor()
    {
        PerfectCounterSystem(false);
        TerribleSpanwer();
        PlusSpawner(false);
    }

    // ===================================== TRIGGER

    private void OnTriggerEnter(Collider other)
    {
        // PLAYER && FIRST COOKIE
        if (other.gameObject.CompareTag("Cookie"))
        {
            Cookies cookies = other.gameObject.GetComponent<Cookies>();
            other.tag ="Untagged";
            cookies.ParentCons(other.gameObject);
            int x = cookies.myColorNumber;

            CookieList cookieList = FindObjectOfType<CookieList>();
            cookieList.cookie.Add(other.gameObject);

            if (playerColorNumber == x)
            {
                RightColor();
                Debug.Log("RIGHT COLOR");
            }
            else
            {
                WrongColor();
                Debug.Log("WRONG COLOR");
            }
            HapticManager.Instance.Vibrate();
        }

        // PLAYER && COLOR GATE
        if (other.CompareTag("Gate"))
        {
            Gate gate = other.GetComponent<Gate>();
            if (gate.colorNumber == 0)
            {
                myBody[0].material = cookieList.bodyColors[0];
                myBody[1].material = cookieList.bodyColors[0];
                playerColorNumber = 0;
            }
            else if (gate.colorNumber == 1)
            {
                myBody[0].material = cookieList.bodyColors[1];
                myBody[1].material = cookieList.bodyColors[1];
                playerColorNumber = 1;
            }
            else if (gate.colorNumber == 2)
            {
                myBody[0].material = cookieList.bodyColors[2];
                myBody[1].material = cookieList.bodyColors[2]; 
                playerColorNumber = 2;
            }

        }
        // PLAYER && FINISH
        if (other.gameObject.CompareTag("Finish"))
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
            FinishLine.Instance.myEvent.Invoke();
            Color color = new Color32(0, 111, 255, 255);
            water.material.DOColor(color, 1f);
        }
        // PLAYER && BONUS LINE
        if (other.CompareTag("BonusLine"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.FinishLevel();
        }
        // PLAYER && COIN
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.AddCoin(1);
        }
    }
    
    // ===================================== UI

    public void PlusSpawner(bool isTrue)
    {
        if (isTrue)
        {
            plusText.outlineColor = Color.green;
            plusText.text = "+1";
        }
        else
        {
            plusText.outlineColor = Color.red;
            plusText.text = "-1";
        }

        plusText.gameObject.SetActive(true);
        plusText.rectTransform.DOScale(1.25f, .3f).SetLoops(1, LoopType.Yoyo).OnComplete(() =>
        {
            plusText.rectTransform.DOScale(.75f, .3f);
            plusText.gameObject.SetActive(false);
        });
    }

    public void PerfectCounterSystem(bool isTrue)
    {
        if (isTrue)
        {
            perfectCounter++;
            terribleCounter = 0;

            if (perfectCounter % 3 == 0)
            {
                GameManager.Instance.Amazer();
                perfectCounter = 0;
            }
        }
        else
        {
            terribleCounter++;
            perfectCounter = 0;

            if(terribleCounter % 3 == 0)
            {
                GameManager.Instance.Terribler();
                terribleCounter = 0;
            }
        }
    }

    public void TerribleSpanwer()
    {
        perfectCounter = 0;
    }

}