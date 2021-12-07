using MoreMountains.NiceVibrations;

public class HapticManager : Singleton<HapticManager>
{
    #region Other Methods

    public void Vibrate()
    {
        MMVibrationManager.Vibrate();
    }

    #endregion
}
