using UnityEngine;
using Cysharp.Threading.Tasks; // UniTask
using System.Threading;

public class BossATK : MonoBehaviour
{
    [SerializeField] BulletOrigin bulletPrefab;
    [SerializeField] int speed = 10;
    [SerializeField] int damage = 1;

    [SerializeField] GameObject bulletPrefab2; // レーザー
    [SerializeField] Transform firepoint;
    [SerializeField] Transform[] fireVectals;

    [SerializeField] float attackInterval = 2f; // 攻撃間隔(秒)

    private CancellationTokenSource cts;

    private void OnEnable()
    {
        cts = new CancellationTokenSource();
        AttackLoopAsync(cts.Token).Forget();
    }

    private void OnDisable()
    {
        cts?.Cancel();
    }

    private async UniTaskVoid AttackLoopAsync(CancellationToken token)
    {
        await UniTask.Delay(1000, cancellationToken: token); // 初回待機

        while (!token.IsCancellationRequested)
        {
            int random = Random.Range(0, 2); // 0 or 1
            if (random == 0)
                FireLevel1();
            else
                FireLevel2();

            await UniTask.Delay((int)(attackInterval * 1000), cancellationToken: token);
        }
    }

    public void FireLevel1()
    {
        for (int i = 0; i < 3; i++)
        {
            BulletOrigin bullet = Instantiate(bulletPrefab, firepoint.position, Quaternion.identity);
            bullet.transform.forward = fireVectals[i].position - firepoint.position;
            bullet.Initialize((fireVectals[i].position - firepoint.position), speed, 1, damage);
        }
    }

    public void FireLevel2()
    {
        Debug.Log("FireLevel2");
        //Instantiate(bulletPrefab2, firepoint.position, firepoint.rotation);
    }
}
