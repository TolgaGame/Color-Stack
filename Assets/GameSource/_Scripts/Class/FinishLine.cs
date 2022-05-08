using System.Collections;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class FinishLine : MonoBehaviour
{
    private GameObject player;
    private CinemachineVirtualCamera finishCam;
    private CookieList cookieList;

    // =====================================
    private void Start() 
    {
        player = GameObject.Find("Player");
        finishCam = GameObject.Find("FinishCam").GetComponent<CinemachineVirtualCamera>();
        cookieList = GameObject.Find("COOKIE LIST").GetComponent<CookieList>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<BoxCollider>().enabled = false;
        player.GetComponent<SwerveInput>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;
       
        player.transform.DOMoveX(0, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            player.transform.DOMoveZ(transform.position.z + 3f, 3f);
            finishCam.m_Priority = 20;
        });;
        StartCoroutine(CheckWin());
    }

    private IEnumerator CheckWin()
    {
        yield return new WaitForSeconds(2f);
        if (cookieList.cookie.Count == 1)
        {
            GameManager.Instance.GameOver();
            FindObjectOfType<FinalCookie>().myAnim.Play("Fail");
        }     
    }
}
