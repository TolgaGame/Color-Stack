using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class FinishLine : MonoBehaviour
{
    private GameObject player;
    private CinemachineVirtualCamera finishCam;

    // =====================================
    private void Start() 
    {
        player = GameObject.Find("Player");
        finishCam = GameObject.Find("FinishCam").GetComponent<CinemachineVirtualCamera>();
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
        
        Debug.Log("Finish");
    }
}
