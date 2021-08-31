using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Chronos;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Timekeeper))]
public class PauseOrder : MonoBehaviour
{
    /// <summary>
    /// すべてのグローバルクロックを制御するタイムキーパー
    /// </summary>
    Timekeeper tk = default;

    /// <summary>
    /// 入力情報を持つコンポーネント
    /// </summary>
    MyInputManager input = default;


    /// <summary>
    /// ポーズ画面の際に停止させるグローバルクロックのkey
    /// </summary>
    [SerializeField]
    string clockKeyPausable = "Pausable";

    /// <summary>
    /// ポーズ・メニューボタンが押された時、ポーズメニューを起動させるメソッドを定義する
    /// </summary>
    [SerializeField]
    UnityEvent pauseActivate = default;


    void Start()
    {
        tk = GetComponent<Timekeeper>();
        input = FindObjectOfType<MyInputManager>();
    }

    void Update()
    {
        //ポーズ実行・ポーズ画面起動
        if (input.Menu.NowPushDown == PushType.onePush) pauseActivate.Invoke();
    }

    /// <summary>
    /// ポーズ要求により、対象のグローバルクロックを止めたり進めたりする
    /// </summary>
    /// <param name="isPause">ポーズするか</param>
    public void SetPause(bool isPause)
    {
        tk.Clock(clockKeyPausable).paused = isPause;
    }
}
