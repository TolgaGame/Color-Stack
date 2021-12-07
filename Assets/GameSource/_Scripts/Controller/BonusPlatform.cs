using DG.Tweening;
using UnityEngine;

public class BonusPlatform : MonoBehaviour
{
    #region Variables

    [SerializeField] private float targetYPos;

    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        transform.DOLocalMoveY(targetYPos, .75f);
    }

    #endregion
}
