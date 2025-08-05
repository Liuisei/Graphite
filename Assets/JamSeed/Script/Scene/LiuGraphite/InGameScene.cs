using Cysharp.Threading.Tasks;
using JamSeed.Foundation;
using System;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Windows;
public class InGameScene : SceneSingleton<InGameScene>
{
    public InputSystem_Actions InputActions { get; private set; }

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
    public int _currentFibarTime = 0;

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


    }
    private async UniTaskVoid GameStart()
    {
        PlayerSpawn();
        await PrepareCountDawn();
        EnemySpawn();
    }

    private async UniTask PrepareCountDawn()
    {
        _gameStateText.text = "Reaty?";
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
        PlayerCont player = Instantiate(_playerPrefab, _playerSpawnPoint);
        player.playerMovement.IsMove = true;
        currentplayer.playerMovement.IsMove = false; // 元のプレイヤーは動かない
        currentplayer = player; // 新しいプレイヤーを現在のプレイヤーとして設定
    }

    private float nextFireTime;
    [SerializeField] private float fireCooldown = 0.2f;

    private void FixedUpdate()
    {
        if (isFiring && Time.time >= nextFireTime)
        {
            Debug.Log("Player Fireaaa");
            fireAction.Invoke();
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


