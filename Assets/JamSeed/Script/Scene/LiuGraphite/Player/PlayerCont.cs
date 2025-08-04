using UnityEngine;



public class PlayerCont : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerWeapon playerWeapon;

    private void Start()
    {
        playerMovement.onFire += playerWeapon.PlayerFire;
    }

}
