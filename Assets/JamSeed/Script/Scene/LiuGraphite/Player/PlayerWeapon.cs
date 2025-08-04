using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameObject bulletPrefab; // 弾のプレハブ
    [SerializeField] GameObject bulletPrefab2;//レーザー
    [SerializeField] Transform firepoint;
    [SerializeField] Transform[] fireVectals; // 発射方向
}
