using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public int Hp;
    public int PlayerMoveSpeed;
    public int RefillGauge;
    private void Awakew()
    {
        Instance = this;
    }
}
