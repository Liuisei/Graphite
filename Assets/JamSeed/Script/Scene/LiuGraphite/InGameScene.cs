using Cysharp.Threading.Tasks;
using JamSeed.Foundation;
using System;
using System.Diagnostics;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
public class InGameScene : SceneSingleton<InGameScene>
{

    public Transform _playerSpawnPoint;
    public GameObject _playerPrefab;
    private PlayerMovement _player;


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


    private void Start()
    {
        GameStart().Forget();
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
        _player.IsMove = true;
    }

    private void PlayerSpawn()
    {
        _player = Instantiate(_playerPrefab, _playerSpawnPoint).GetComponent<PlayerMovement>();
    }

    private void EnemySpawn()
    {
        BossCont newBossCont = Instantiate(BossCont, _enemySpawnPoint);
        newBossCont.bossMove.SetTarget(MoveRangeA, MoveRangeB);
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
    // ===== イベント =====
    public event Action<PlayerLevel> OnPlayerLevelChanged;
    public event Action<PlayerState> OnPlayerStateChanged;
    public event Action<float> OnCloneCurrentTimeChanged;
    public event Action<float> OnFibarCloneCurrentTimeChanged;
    public event Action<int> OnMaxPlayerHPChanged;
    public event Action<int> OnCurrentPlayerHPChanged;

    // ===== フィールド =====
    private PlayerLevel _playerLevel = PlayerLevel.Lv1;
    private PlayerState _playerState = PlayerState.normal;
    public float _cloneCooldownTime = 5f;
    private float _cloneCurrentTime = 0f;
    public float _fibarCloneCooldownTime = 2f;
    private float _fibarCloneCurrentTime = 0f;
    public int _maxPlayerHP = 10;
    private int _currentPlayerHP = 10;

    // ===== プロパティ =====
    public PlayerLevel Level
    {
        get => _playerLevel;
        set
        {
            if (_playerLevel != value)
            {
                _playerLevel = value;
                OnPlayerLevelChanged?.Invoke(value);
            }
        }
    }

    public PlayerState State
    {
        get => _playerState;
        set
        {
            if (_playerState != value)
            {
                _playerState = value;
                OnPlayerStateChanged?.Invoke(value);
            }
        }
    }



    public float CloneCurrentTime
    {
        get => _cloneCurrentTime;
        set
        {
            if (Math.Abs(_cloneCurrentTime - value) > float.Epsilon)
            {
                _cloneCurrentTime = value;
                OnCloneCurrentTimeChanged?.Invoke(value);
            }
        }
    }

    public float FibarCloneCurrentTime
    {
        get => _fibarCloneCurrentTime;
        set
        {
            if (Math.Abs(_fibarCloneCurrentTime - value) > float.Epsilon)
            {
                _fibarCloneCurrentTime = value;
                OnFibarCloneCurrentTimeChanged?.Invoke(value);
            }
        }
    }

    public int CurrentPlayerHP
    {
        get => _currentPlayerHP;
        set
        {
            if (_currentPlayerHP != value)
            {
                _currentPlayerHP = value;
                OnCurrentPlayerHPChanged?.Invoke(value);
            }
        }
    }

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

