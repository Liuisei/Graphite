using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

[RequireComponent(typeof(Rigidbody))]
public class BossMove : MonoBehaviour
{
    [Header("移動範囲の設定")]
    public Transform transformA;
    public Transform transformB;

    [Header("移動設定")]
    public float moveSpeed = 5f;
    public float changeDirectionInterval = 2f; // 方向を変える間隔（秒）
    public float arrivalThreshold = 0.5f; // 目標地点に到達したと判定する距離

    private Rigidbody rb;
    private Vector3 targetPosition;
    private Vector3 minBounds;
    private Vector3 maxBounds;
    private CancellationTokenSource cancellationTokenSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cancellationTokenSource = new CancellationTokenSource();

        // 初期設定があれば移動を開始
        if (transformA != null && transformB != null)
        {
            StartMovementAsync(cancellationTokenSource.Token).Forget();
        }
    }

    public void SetTarget(Transform newTransformA, Transform newTransformB)
    {
        transformA = newTransformA;
        transformB = newTransformB;

        // 既存の移動をキャンセル
        cancellationTokenSource?.Cancel();
        cancellationTokenSource = new CancellationTokenSource();

        // 新しい移動を開始
        StartMovementAsync(cancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid StartMovementAsync(CancellationToken cancellationToken)
    {
        // 移動範囲の境界を計算
        CalculateBounds();

        // 最初のランダム目標地点を設定
        SetRandomTarget();

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // 目標地点への移動をチェック
                await CheckAndUpdateTarget(cancellationToken);

                // フレーム待機
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
        }
        catch (System.OperationCanceledException)
        {
            // キャンセルされた場合は正常終了
        }
    }

    private async UniTask CheckAndUpdateTarget(CancellationToken cancellationToken)
    {
        float startTime = Time.time;

        while (!cancellationToken.IsCancellationRequested)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            float elapsedTime = Time.time - startTime;

            // 目標地点に到達したか、一定時間経過したら新しい目標を設定
            if (distanceToTarget < arrivalThreshold || elapsedTime >= changeDirectionInterval)
            {
                SetRandomTarget();
                break;
            }

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
        }
    }

    void FixedUpdate()
    {
        // 目標地点に向かって移動
        if (targetPosition != Vector3.zero)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Y座標の移動を無視
            direction.y = 0;

            // Rigidbodyを使って移動
            rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);
        }
    }

    void CalculateBounds()
    {
        if (transformA == null || transformB == null) return;

        // 2つのtransformの位置から移動範囲の境界を計算
        Vector3 posA = transformA.position;
        Vector3 posB = transformB.position;

        float minX = Mathf.Min(posA.x, posB.x);
        float maxX = Mathf.Max(posA.x, posB.x);
        float minZ = Mathf.Min(posA.z, posB.z);
        float maxZ = Mathf.Max(posA.z, posB.z);

        minBounds = new Vector3(minX, transform.position.y, minZ);
        maxBounds = new Vector3(maxX, transform.position.y, maxZ);
    }

    void SetRandomTarget()
    {
        if (minBounds == Vector3.zero && maxBounds == Vector3.zero) return;

        // 範囲内でランダムな位置を生成（Y座標は現在の位置を保持）
        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomZ = Random.Range(minBounds.z, maxBounds.z);

        targetPosition = new Vector3(randomX, transform.position.y, randomZ);
    }

    // 移動を停止
    public void StopMovement()
    {
        cancellationTokenSource?.Cancel();
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
    }

    // 移動を再開
    public void ResumeMovement()
    {
        if (transformA != null && transformB != null)
        {
            cancellationTokenSource = new CancellationTokenSource();
            StartMovementAsync(cancellationTokenSource.Token).Forget();
        }
    }

    void OnDestroy()
    {
        // オブジェクト破棄時にキャンセル
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
    }

    // デバッグ用：移動範囲と目標地点を可視化
    void OnDrawGizmos()
    {
        if (transformA != null && transformB != null)
        {
            // 移動範囲を描画
            Gizmos.color = Color.yellow;
            Vector3 center = (minBounds + maxBounds) / 2;
            Vector3 size = maxBounds - minBounds;
            size.y = 0.1f; // 薄い箱として描画
            Gizmos.DrawWireCube(center, size);

            // 2つの基準点を描画
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transformA.position, 0.2f);
            Gizmos.DrawSphere(transformB.position, 0.2f);
        }

        // 現在の目標地点を描画
        if (Application.isPlaying && targetPosition != Vector3.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(targetPosition, 0.3f);

            // 現在位置から目標地点への線を描画
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, targetPosition);
        }
    }
}