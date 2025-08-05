using Cysharp.Threading.Tasks;
using JamSeed.Foundation;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Windows;

public class InGameScene : SceneSingleton<InGameScene>
{
    public InputSystem_Actions InputActions { get; private set; }

    public fibarState _fibarState = fibarState.Normal;

    private bool isFiring;

    private void Awake()
    {
        InputActions = new InputSystem_Actions();
        InputActions.Enable();
    }

    private void OnDestroy()
    {
        InputActions.Disable();
    }

    public Transform _playerSpawnPoint;
    public PlayerCont _playerPrefab;
    private PlayerCont currentplayer;

    public Transform _enemySpawnPoint;
    public BossCont BossCont;
    public Transform MoveRangeA;
    public Transform MoveRangeB;

    public TextMeshProUGUI _gameStateText;

    public int _maxFibarTime = 100;

    public int _maxOkawariTime = 100;
    public int _currentOkawariTime = 0;

    public PlayerDataLiu _playerDataLiu = new PlayerDataLiu();

    public Action fireAction;

    private void Start()
    {
        Debug.LogWarning("InGameScene Start");
        GameStart().Forget();
        InputActions.Player.Jump.performed += _ => CLonePlayer();

        InputActions.Player.Attack.performed += _ => isFiring = true;
        InputActions.Player.Attack.canceled += _ => isFiring = false;

        HelthCloneCoolD().Forget();
        FibarCont().Forget(); // フィーバー管理を開始
    }

    private async UniTaskVoid FibarCont()
    {
        while (true)
        {
            await UniTask.Delay(100); // 0.1秒ごとに更新

            if (_fibarState == fibarState.Normal)
            {
                // フィーバー状態の回復処理
                if (_playerDataLiu.fibarCloneCurrentTime < _playerDataLiu.fibarCloneCooldownTime)
                {
                    _playerDataLiu.fibarCloneCurrentTime += 0.01f;
                }
                else
                {
                    _playerDataLiu.fibarCloneCurrentTime = _playerDataLiu.fibarCloneCooldownTime;
                    _fibarState = fibarState.Fibar; // フィーバー状態を終了
                }
            }
            else if (_fibarState == fibarState.Fibar)
            {
                // フィーバー状態の消費処理
                _playerDataLiu.fibarCloneCurrentTime -= 0.1f;
                if (_playerDataLiu.fibarCloneCurrentTime <= 0)
                {
                    _fibarState = fibarState.Normal;

                    _playerDataLiu.fibarCloneCurrentTime = 0f; // クールダウンをリセット
                }
            }

            _playerDataLiu.OnPlayerDataChanged?.Invoke();
        }
    }

    private async UniTaskVoid HelthCloneCoolD()
    {
        while (true)
        {
            await UniTask.Delay(100);
            if (_playerDataLiu.cloneCurrentTime < _playerDataLiu.cloneCooldownTime)
            {
                _playerDataLiu.cloneCurrentTime += 0.1f;
            }
            else
            {
                _playerDataLiu.cloneCurrentTime = _playerDataLiu.cloneCooldownTime;
            }
            _playerDataLiu.OnPlayerDataChanged?.Invoke();
        }
    }

    private async UniTaskVoid GameStart()
    {
        PlayerSpawn();
        await PrepareCountDawn();
        EnemySpawn();
    }

    private async UniTask PrepareCountDawn()
    {
        _gameStateText.text = "Ready?";
        await UniTask.Delay(1000);
        _gameStateText.text = "Go!";
        await UniTask.Delay(1000);
        _gameStateText.text = "";
        currentplayer.playerMovement.IsMove = true;
    }

    private void PlayerSpawn()
    {
        PlayerCont playerC = Instantiate(_playerPrefab, _playerSpawnPoint);
        currentplayer = playerC;
    }

    private void EnemySpawn()
    {
        BossCont newBossCont = Instantiate(BossCont, _enemySpawnPoint);
        newBossCont.bossMove.SetTarget(MoveRangeA, MoveRangeB);
    }

    public void CLonePlayer()
    {
        if(_fibarState == fibarState.Normal)
        {
            if (_playerDataLiu.cloneCurrentTime < _playerDataLiu.cloneCooldownTime)
            {
                Debug.Log("クローンできない");
                return;
            }
            else
            {
                _playerDataLiu.cloneCurrentTime = 0f; // クールダウンをリセット
            }
            PlayerCont player = Instantiate(_playerPrefab, _playerSpawnPoint);
            player.playerMovement.IsMove = true;
            Destroy(currentplayer.gameObject, 5f);
            currentplayer.playerMovement.IsMove = false; // 元のプレイヤーは動かない
            currentplayer = player; // 新しいプレイヤーを現在のプレイヤーとして設定
        }
        else if (_fibarState == fibarState.Fibar)
        {
            PlayerCont player = Instantiate(_playerPrefab, _playerSpawnPoint);
            player.playerMovement.IsMove = true;
            Destroy(currentplayer.gameObject, 5f);
            currentplayer.playerMovement.IsMove = false; // 元のプレイヤーは動かない
            currentplayer = player; // 新しいプレイヤーを現在のプレイヤーとして設定
        }
    }


    public async UniTaskVoid ClonePlayerAsync()
    {
        // 連打防止用
        if (_isCloneProcessing) return;
        _isCloneProcessing = true;

        if (_fibarState == fibarState.Normal)
        {
            if (_playerDataLiu.cloneCurrentTime < _playerDataLiu.cloneCooldownTime)
            {
                Debug.Log("クローンできない");
                _isCloneProcessing = false;
                return;
            }
            else
            {
                _playerDataLiu.cloneCurrentTime = 0f; // クールダウンをリセット
            }
            CreateClone();
        }
        else if (_fibarState == fibarState.Fibar)
        {
            CreateClone();
        }

        // 0.3秒クールタイム
        await UniTask.Delay(300);
        _isCloneProcessing = false;
    }

    private bool _isCloneProcessing = false;

    private void CreateClone()
    {
        PlayerCont player = Instantiate(_playerPrefab, _playerSpawnPoint);
        player.playerMovement.IsMove = true;

        Destroy(currentplayer.gameObject, 5f);
        currentplayer.playerMovement.IsMove = false; // 元のプレイヤーは動かない
        currentplayer = player; // 新しいプレイヤーを現在のプレイヤーとして設定
    }


    // フィーバー状態を開始するメソッド
    public void StartFibarState()
    {
        if (_playerDataLiu.fibarCloneCurrentTime >= _playerDataLiu.fibarCloneCooldownTime)
        {
            _fibarState = fibarState.Fibar;
            _playerDataLiu.fibarCloneCurrentTime = 0;
            Debug.Log("フィーバー状態開始！");
        }
        else
        {
            Debug.Log("フィーバー状態を開始できません（クールダウン中）");
        }
    }

    private float nextFireTime;
    [SerializeField] private float fireCooldown = 0.2f;

    private void FixedUpdate()
    {
        if (isFiring && Time.time >= nextFireTime)
        {
            Debug.Log("Player Fire");
            fireAction?.Invoke();
            nextFireTime = Time.time + fireCooldown;
        }
    }

    private enum SceneState
    {
        None,
        Prepare,
        Game,
        GameOver,
        GameClear,
    }

    //フェーズ
    private enum phase
    {
        None,
        Start,
        BossFight,
        End,
    }

    public enum fibarState
    {
        Normal,
        Fibar,
    }
}

public class PlayerDataLiu
{
    // ===== フィールド =====
    public PlayerLevel playerLevel = PlayerLevel.Lv1;
    public PlayerState playerState = PlayerState.normal;

    public float cloneCooldownTime = 5f;
    public float cloneCurrentTime = 0f;

    public float fibarCloneCooldownTime = 2f;
    public float fibarCloneCurrentTime = 0f;

    public int maxPlayerHP = 10;

    private int _currentPlayerHP = 10; // 内部値
    public int CurrentPlayerHP
    {
        get => _currentPlayerHP;
        set
        {
            int clamped = Math.Clamp(value, 0, maxPlayerHP); // 0〜maxPlayerHP に制限
            if (_currentPlayerHP != clamped)
            {
                _currentPlayerHP = clamped;
            }
        }
    }

    public Action OnPlayerDataChanged;

    // ===== 列挙型 =====
    public enum PlayerState
    {
        normal,
        clone,
    }

    public enum PlayerLevel
    {
        Lv1,
        Lv2,
        Lv3,
    }
}