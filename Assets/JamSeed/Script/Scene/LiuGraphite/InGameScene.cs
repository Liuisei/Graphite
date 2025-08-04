using Cysharp.Threading.Tasks;
using System.Diagnostics;
using TMPro;
using UnityEngine;
public class InGameScene : MonoBehaviour
{

    public Transform _playerSpawnPoint;
    public GameObject _playerPrefab;


    public Transform _enemySpawnPoint;
    public BossCont BossCont;
    public Transform MoveRangeA;
    public Transform MoveRangeB;


    public TextMeshProUGUI _gameStateText;


    public int _maxFibarTime = 100;
    public int _currentFibarTime = 0;

    public int _maxOkawariTime = 100;
    public int _currentOkawariTime = 0;


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
    }

    private void PlayerSpawn()
    {
        Instantiate(_playerPrefab, _playerSpawnPoint);
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
