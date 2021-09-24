using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームのクリア条件、失敗条件を監視
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// true:クリアした
    /// </summary>
    static bool isCleared = false;

    /// <summary>
    /// true:失敗した
    /// </summary>
    static bool isNotCleared = false;

    /// <summary>
    /// 操作プレイヤーのステータス
    /// </summary>
    Status playerStatus = default;

    /// <summary>
    /// ポーズ制御を司るコンポーネント
    /// </summary>
    PauseOrder pauseOrder = default;

    public static bool IsCleared { get => isCleared; }
    public static bool IsNotCleared { get => isNotCleared; }

    void Start()
    {
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<Status>();
        pauseOrder = FindObjectOfType<PauseOrder>();
        isCleared = false;
        isNotCleared = false;
        EnemySpawner.AllEnemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCleared || isNotCleared) return;

        //クリア:敵の全滅
        if (EnemySpawner.AllEnemies.Count <= 0) isCleared = true;
        //失敗:プレイヤーの体力が尽きる
        else if (playerStatus.IsDefeated) isNotCleared = true;
    }
}
