using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine;

public class Cookies : MonoBehaviour
{
    public int[] colorIndex;
    public int[] skinIndex;
    private Renderer[] meshRenderer;
    private Animator animator;
    private Color currentColor;

    /// PARENT CONS SYSTEM
    private Transform player;
    private Vector3 velocity = Vector3.zero;
    private float movementTime;
    private CookieList cookieList;
    private ConstraintSource cs;

    // ====================================

    private void Start()
    {
        if (player == false)
        {
            GameObject x = transform.GetChild(1).transform.gameObject;
            meshRenderer = x.GetComponentsInChildren<Renderer>();
            animator = GetComponent<Animator>();
            currentColor = meshRenderer[skinIndex[0]].materials[colorIndex[0]].color; 
        }
        cookieList = GameObject.Find("COOKIE LIST").GetComponent<CookieList>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
      
    public void ParentCons(GameObject cookie)
    {
        cookie.gameObject.GetComponent<Cookies>().enabled = true;
        cookie.gameObject.GetComponent<Cookies>().movementTime = cookieList.cookie.Count * 0.06f;

        if (cookieList.cookie.Count <= 1)
            cs.sourceTransform = player;
        else
            cs.sourceTransform = cookieList.cookie[cookieList.cookie.Count - 2].transform;

        cs.weight = 1;
        cookie.GetComponent<ParentConstraint>().AddSource(cs);
        cookie.gameObject.GetComponent<ParentConstraint>().SetTranslationOffset(0, new Vector3(0f, 0f, 2f));
        cookie.gameObject.GetComponent<ParentConstraint>().enabled = true;
        cookie.gameObject.GetComponent<ParentConstraint>().constraintActive = true;

       // if (cookieList.cookie.Count > 2)
            //StartCoroutine(ScaleEffect());

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

    // ====================================

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

    public Color GetColor()
    {
        return currentColor;
    }

    public void ChangeColor(Color color)
    {
        for (int i = 0; i < colorIndex.Length; i++)
        {
            for (int j = 0; j < skinIndex.Length; j++)
                meshRenderer[skinIndex[j]].materials[colorIndex[i]].DOColor(color, .25f);
        }
    }

    public void FlashEffect(Color flashColor,Color _currentColor)
    {
        for (int i = 0; i < colorIndex.Length; i++)
        {
            for (int j = 0; j < skinIndex.Length; j++)
            {
                meshRenderer[skinIndex[j]].material.DOColor(flashColor, .05f);
                StartCoroutine(ChangeToDefaultColor(_currentColor,j));
            }
        }
    }

    private IEnumerator ChangeToDefaultColor(Color _currentColor,int index)
    {
        yield return new WaitForSeconds(.0f);
        meshRenderer[skinIndex[index]].material.DOColor(_currentColor, .05f);
    }

    public void Run()
    {
        animator.Play("Run");
    }

}

