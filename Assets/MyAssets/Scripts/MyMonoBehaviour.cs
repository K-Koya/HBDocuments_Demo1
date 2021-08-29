using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// MonoBehaviourに追加機能を設けた基底クラス
/// </summary>
[RequireComponent(typeof(Timeline))]
public class MyMonoBehaviour : MonoBehaviour
{
    /// <summary>
    /// ChronosアセットのTimelineを利用するためのアクセッサ
    /// </summary>
    protected Timeline time = default;

    /// <summary>
    /// ポーズされている
    /// </summary>
    protected bool IsPausing
    {
        get { return time.timeScale <= 0.0f; }
    }

    /// <summary>
    /// Timelineを初期化
    /// </summary>
    protected void TimelineInit()
    {
        time = GetComponent<Timeline>();
    }
}


/// <summary>
/// 拡張メソッド定義クラス
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// 3次元ベクトルのy軸の値を任意に指定する
    /// </summary>
    /// <param name="before">元のベクトル</param>
    /// <param name="valueY">指定するy軸の値</param>
    /// <returns>y軸を"valueY"にしたベクトル</returns>
    public static Vector3 YEqual(this Vector3 before, float valueY = 0.0f) { return new Vector3(before.x, valueY, before.z); }

    /// <summary>
    /// 3次元ベクトルのy軸を取り除き、xz軸の2次元ベクトルを返す
    /// </summary>
    /// <param name="before">元のベクトル</param>
    /// <returns></returns>
    public static Vector2 ConvVector2_XZ(this Vector3 before) { return new Vector2(before.x, before.z); }
}