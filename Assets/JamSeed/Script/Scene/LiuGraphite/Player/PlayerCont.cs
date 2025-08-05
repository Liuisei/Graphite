using UnityEngine;
using UnityEngine.UI;



public class PlayerCont : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerWeapon playerWeapon;

    //ui
    [SerializeField] Slider sliderClone;
    [SerializeField] Slider sliderFebarl;

    private void Start()
    {
        playerMovement.onFire += playerWeapon.PlayerFire;
    }

}
