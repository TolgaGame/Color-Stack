using UnityEngine;
using DG.Tweening;
using LPWAsset;
using TMPro;

public class PlayerController : Singleton<PlayerController>
{
    [Header("GameObjects")]
    [Space]
    public Animator playerAnim;
    [SerializeField] Color flashColor;
    [SerializeField] ParticleSystem upgradeFx;
    [SerializeField] UpgradeManager up;
    [SerializeField] Material[] colors;
    [SerializeField] TextMeshProUGUI plusText;
    [SerializeField] LowPolyWaterScript water;

    [Header("Variables")]
    [Space]
    [SerializeField] Transform _target;
    private float _moveSpeed = 10f;
    private float _slideSpeed = 5f;
    private float _smoothness = 50;
    private float _firstValue;
    private float _lastValue;
    private float _distance;
    private float _calculatedValue;

    // Private variables
    [HideInInspector] public int perfectCounter;
    [HideInInspector] public int terribleCounter;
    [HideInInspector] public bool canPickBonus;
    private Color currentColor;
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
    }

    private void Update()
    {
        if (GameManager.Instance.isGameStarted)
        {
            transform.Translate(transform.forward * _moveSpeed * Time.deltaTime, Space.World);

            if (Input.GetMouseButtonDown(0))
                _firstValue = Input.mousePosition.x;
            else if (Input.GetMouseButton(0))
            {
                _lastValue = Input.mousePosition.x;
                _distance = _lastValue - _firstValue;
                _calculatedValue = (_distance / Screen.width) * _smoothness;

                MoveX(_calculatedValue);
                _firstValue = _lastValue;
            }
        }
    }

    private void MoveX(float value)
    {
        transform.Translate(transform.right * _slideSpeed * value * Time.deltaTime, Space.World);
    }
  
   // ===================================== METHODSS

    public void SpeedUp(int addSpeed)
    {
       _moveSpeed += addSpeed;
    }

    private bool MatchColor(Color targetColor)
    {
        if (targetColor == currentColor)
            return true;
        else
            return false;
    }

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

    private void FallDown()
    {
        GameManager.Instance.GameOver();
    }

    private void ChangeColor(Color color,int index)
    {
        water.material.DOColor(color, .3f);
        currentColor = colors[index].color;
    }

    public void Upgrade()
    {
        upgradeFx.Play();
    }

    // ===================================== TRIGGER

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Cookie"))
        {
            Cookies cookies = other.gameObject.GetComponent<Cookies>();
            other.tag ="Untagged";
            cookies.ParentCons(other.gameObject);

            CookieList cookieList = FindObjectOfType<CookieList>();
            cookieList.cookie.Add(other.gameObject);

            // Color cookieColor = cookies.GetColor();
            // if (MatchColor(cookieColor))
            //     RightColor();
            // else
            // {
            //     Destroy(other.gameObject);
            //     WrongColor();
            // }
            HapticManager.Instance.Vibrate();
        }

        // PLAYER && COLOR GATE
        if (other.CompareTag("Gate"))
        {
            Gate gate = other.GetComponent<Gate>();
            ChangeColor(gate.myColor, gate.index);
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






    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Cookie"))
    //     {
    //         HapticManager.Instance.Vibrate();
           
    //         Cookies cookie = other.gameObject.GetComponent<Cookies>();
    //         Color cookieColor = cookie.GetColor();
    //         if (MatchColor(cookieColor))
    //             RightColor();
    //         else
    //         {
    //             Destroy(other.gameObject);
    //             WrongColor();
    //         }
    //     }
    // }