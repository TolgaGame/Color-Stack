using UnityEngine;
public class RewardFillControl : MonoBehaviour
{
    [Header("Singleton")]
   
    public static RewardFillControl instance;
    // Add coin vaule for step max value is 1f think if you need 5 steps use like this
    /// <summary>
    ///  ie : 0.2 * 5 = 1;
    /// </summary>
    [Header("Values")]
    public float amountStep;
    // add coin amount after win game;
    public int coin;

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

}