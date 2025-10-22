using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGauge : MonoBehaviour
{
    [SerializeField]
    private GameObject lifeObj;

    //　ライフゲージ全削除＆HP分作成
    public void SetLifeGauge(int life)
    {
        //　体力を一旦全削除
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        //　現在の体力数分のライフゲージを作成
        for (int i = 0; i < life; i++)
        {
            Instantiate<GameObject>(lifeObj, transform);
        }
    }

    //　ダメージ分だけ削除
    public void SetLifeGauge2(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            if (transform.childCount <= 0) break;

            // 最後のライフゲージを削除
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        }

        // 👇ライフゲージがすべて削除されたらゲーム終了
        if (transform.childCount == 1)
        {
            GameOver();
        }
    }

    public void SetLifeGauge3(int heal)
    {
        for (int i = 0; i < heal; i++)
        {
            Instantiate(lifeObj, transform);
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0f;
        // ここでゲームオーバーUI表示などを追加できる
        // 例: FindObjectOfType<GameManager>().GameOver();
    }
}