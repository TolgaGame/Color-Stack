using DG.Tweening;
using System.Collections;
using UnityEngine.Animations;
using UnityEngine;

public class Cookies : MonoBehaviour
{
    public int myColorNumber;
    public SkinnedMeshRenderer[] myBody;
    private Animator animator;

    /// PARENT CONS SYSTEM
    private Transform player;
    private Vector3 velocity = Vector3.zero;
    private float movementTime;
    private CookieList cookieList;
    private ConstraintSource cs;
    [HideInInspector] public bool controlActive = false;

    // ==================================== *** START

    private void Awake() 
    {
        cookieList = GameObject.Find("COOKIE LIST").GetComponent<CookieList>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        Transform x = transform.GetChild(1);
        myBody[0] = x.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        myBody[1] = x.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        
    }
    
    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.position.x, transform.position.y, transform.position.z), 
        ref velocity, movementTime);
    }

    // ==================================== METHODS

    public void ParentCons(GameObject cookie)
    {
        cookie.gameObject.GetComponent<Cookies>().enabled = true;
        cookie.gameObject.GetComponent<Cookies>().movementTime = cookieList.cookie.Count * 0.03f;
        
        if (cookieList.cookie.Count == 1)
        {
            cs.sourceTransform = player;
        }
        else
        {
            cs.sourceTransform = cookieList.cookie[cookieList.cookie.Count - 1].transform;
        }      

        cs.weight = 1;
        cookie.GetComponent<ParentConstraint>().AddSource(cs);
        cookie.gameObject.GetComponent<ParentConstraint>().SetTranslationOffset(0, new Vector3(0f, 0f, 1f));
        cookie.gameObject.GetComponent<ParentConstraint>().enabled = true;
        cookie.gameObject.GetComponent<ParentConstraint>().constraintActive = true;
        animator.Play("Run");

       if (cookieList.cookie.Count > 3)
            StartCoroutine(ScaleEffect());

    }

    private IEnumerator ScaleEffect()
    {
        for (int i = 0; i < cookieList.cookie.Count-1; i++)
        {
            if (cookieList.cookie.Count > 1)
            {
                cookieList.cookie[cookieList.cookie.Count-1-i].transform.DOScale(2.2f, 0.05f);
                
                yield return new WaitForSeconds(0.03f);

                cookieList.cookie[cookieList.cookie.Count-1-i].transform.DOScale(2f, 0.05f);
            }
        }
    }

    public void CheckColorMatch(int colorNumber, GameObject cookieX)
    {
        if (myColorNumber == colorNumber)
        {
            PlayerController.Instance.RightColor();
            cookieX.tag ="Untagged";
            cookieX.GetComponent<Cookies>().ParentCons(cookieX.gameObject);
            cookieList.cookie.Add(cookieX);
            Debug.Log("RIGHT COLOR");
        }
        else
        {   
            Destroy(cookieList.cookie[cookieList.cookie.Count - 1].transform.gameObject);
            cookieList.cookie.Remove(cookieList.cookie[cookieList.cookie.Count - 1].transform.gameObject); 
            Destroy(cookieX);
            PlayerController.Instance.WrongColor();
            Debug.Log("WRONG COLOR");
        }
    }

    // ==================================== TRIGGER

    private void OnTriggerEnter(Collider other)
    {
        // COLLECT NEW COOKIE
       if (other.CompareTag("Cookie"))
       {
            Cookies cookies = other.gameObject.GetComponentInChildren<Cookies>();
            CheckColorMatch(cookies.myColorNumber, other.gameObject);
       }

        // PLAYER && COLOR GATE
        if (other.CompareTag("Gate"))
        {
            Gate gate = other.GetComponent<Gate>();
            if (gate.colorNumber == 0)
            {
                myBody[0].material = cookieList.bodyColors[0];
                myBody[1].material = cookieList.bodyColors[0];  
                myColorNumber = 0;
            }
            else if (gate.colorNumber == 1)
            {
                myBody[0].material = cookieList.bodyColors[1];
                myBody[1].material = cookieList.bodyColors[1]; 
                myColorNumber = 1;
            }
            else if (gate.colorNumber == 2)
            {
                myBody[0].material = cookieList.bodyColors[2];
                myBody[1].material = cookieList.bodyColors[2];
                myColorNumber = 2;  
            }

        }
    }

    // ==================================== *** END
}