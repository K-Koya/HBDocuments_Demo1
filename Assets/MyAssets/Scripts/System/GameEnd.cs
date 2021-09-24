using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームを終了するコンポーネント
/// </summary>
public class GameEnd : MonoBehaviour
{
    /// <summary>
    /// 終了処理
    /// </summary>
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   // UnityEditorの実行を停止する処理
#else
        Application.Quit();                                // ゲームを終了する処理
#endif
    }
}
