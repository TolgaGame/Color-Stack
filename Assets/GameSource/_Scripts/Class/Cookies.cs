using DG.Tweening;
using System.Collections;
using UnityEngine.Animations;
using UnityEngine;

public class Cookies : MonoBehaviour
{
    public int myColorNumber;
    public Renderer[] meshRenderer;
    private Animator animator;

    /// PARENT CONS SYSTEM
    private Transform player;
    private Vector3 velocity = Vector3.zero;
    private float movementTime;
    private CookieList cookieList;
    private ConstraintSource cs;
    [HideInInspector] public bool controlActive = false;

    // ==================================== *** START

    private void Start()
    {
        cookieList = GameObject.Find("COOKIE LIST").GetComponent<CookieList>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (controlActive == true)
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.position.x, transform.position.y, transform.position.z), 
            ref velocity, movementTime);
        }
    }

    public void ParentCons(GameObject cookie)
    {
        cookie.gameObject.GetComponent<Cookies>().movementTime = cookieList.cookie.Count * 1f;
        animator.Play("Run");

        if (cookieList.cookie.Count <= 1)
            cs.sourceTransform = player;
        else
            cs.sourceTransform = cookieList.cookie[cookieList.cookie.Count - 2].transform;

        cs.weight = 1;
        cookie.GetComponent<ParentConstraint>().AddSource(cs);
        cookie.gameObject.GetComponent<ParentConstraint>().SetTranslationOffset(0, new Vector3(0f, 0f, 1.5f));
        cookie.gameObject.GetComponent<ParentConstraint>().enabled = true;
        cookie.gameObject.GetComponent<ParentConstraint>().constraintActive = true;
        controlActive = true;

       if (cookieList.cookie.Count > 2)
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

    // ==================================== TRIGGER

    private void OnTriggerEnter(Collider other)
    {
        // COLLECT NEW COOKIE
       if (other.CompareTag("Cookie"))
       {
            Cookies cookies = other.gameObject.GetComponentInChildren<Cookies>();
            other.tag ="Untagged";
            CookieList cookieList = FindObjectOfType<CookieList>();
            cookieList.cookie.Add(other.gameObject);
            ParentCons(other.gameObject);
       }
    }

    // ====================================

    public void ChangeColor(Color color)
    {

    }

    // ==================================== *** END
}