using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Chronos;

/// <summary>
/// ポーズ状態を制御するコンポーネント
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
    /// true:ゲーム終了
    /// </summary>
    bool isGameEnd = false;


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

    /// <summary>
    /// ポーズ中にポーズ・メニューボタンが押された時、ポーズメニューを閉じるメソッドを定義する
    /// </summary>
    [SerializeField]
    UnityEvent pauseEnactivate = default;

    /// <summary>
    /// ゲームクリア時、クリアメニューを起動させるメソッドを定義する
    /// </summary>
    [SerializeField]
    UnityEvent clearMenuActivate = default;

    /// <summary>
    /// ゲームオーバー時、ゲームオーバーメニューを起動させるメソッドを定義する
    /// </summary>
    [SerializeField]
    UnityEvent notClearMenuActivate = default;


    void Start()
    {
        tk = GetComponent<Timekeeper>();
        input = FindObjectOfType<MyInputManager>();
    }

    void Update()
    {
        if (isGameEnd) return;

        //ゲームオーバー
        if(GameManager.IsNotCleared)
        {
            //ゲームオーバーメニュー起動
            notClearMenuActivate.Invoke();
            isGameEnd = true;
        }
        //ゲームクリア
        else if(GameManager.IsCleared)
        {
            //ゲームクリアメニュー起動
            clearMenuActivate.Invoke();
            isGameEnd = true;
        }
        //ポーズ処理
        else if (input.Menu.NowPushDown == PushType.onePush)
        {
            //ポーズ解除・ポーズ画面終了
            if (tk.Clock(clockKeyPausable).paused) pauseEnactivate.Invoke();
            //ポーズ実行・ポーズ画面起動
            else pauseActivate.Invoke();
        }
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
