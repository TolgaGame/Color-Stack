using UnityEngine;

public class BonusPart : MonoBehaviour
{

    [SerializeField] private int multipier;
    [SerializeField] private Color myColor;

    private MeshRenderer renderer;

    // ===================================== START
    
    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && PlayerController.Instance.canPickBonus)
        {
            var cookie = collision.transform.GetComponent<Cookies>();
            if (cookie != null)
            {

            }
            Paint();
        }
    }

    private  void Paint()
    {
        renderer.material.color = myColor;
        GameManager.Instance.SetBonus(multipier);
        Destroy(this);
    }
}
