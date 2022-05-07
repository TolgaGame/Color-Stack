using UnityEngine;

public class FinalCookie : MonoBehaviour
{
    public Animator myAnim;
    public Vector3 scaleUp;
    public float levelTargetScale;

    private ParticleSystem confeti;
    private CookieList cookieList;

    private void Start()
    {
        cookieList = GameObject.Find("COOKIE LIST").GetComponent<CookieList>();
        confeti = GameObject.Find("Confeti").GetComponent<ParticleSystem>();
    } 

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLIDE");
        if (cookieList.cookie.Count > 1)
        {
            transform.localScale = transform.localScale + scaleUp;
            cookieList.cookie.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
        else if (cookieList.cookie.Count == 1)
        {
            if (transform.localScale.y >= levelTargetScale)
            {
                GameManager.Instance.FinishLevel();
                confeti.Play();
                myAnim.Play("Win");
            }
            else
            {
                GameManager.Instance.GameOver();
                myAnim.Play("Fail");
            }
        }

    }
}
