using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコアボードの表示を制御
/// </summary>
public class DrawScoreBoard : MyMonoBehaviour
{
    /// <summary>
    /// 残り敵数表示テキスト
    /// </summary>
    [SerializeField, Tooltip("残り敵数表示テキスト")]
    Text remainingMessage = default;

    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();   
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPausing) return;

        remainingMessage.text = "Remaining\n" + EnemySpawner.AllEnemies.Count;
    }
}
