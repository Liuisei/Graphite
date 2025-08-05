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


