using System;
using UnityEngine;

public class PlayerHP : MonoBehaviour, IHasHp
{
    [SerializeField] private int teamId = 0;

    public event Action OnHPChanged;

    private PlayerDataLiu PlayerData => InGameScene.Instance._playerDataLiu;

    public int HP => PlayerData.CurrentPlayerHP;
    public int MaxHP => PlayerData.maxPlayerHP;
    public int TeamID => teamId;

    public void ChangeHP(int amount, GameObject attacker)
    {
        Debug.Log($"Player took damage: {amount} from {attacker.name}");
        PlayerData.CurrentPlayerHP += amount;
        PlayerData.OnPlayerDataChanged?.Invoke();
    }
}
