using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 地点通過ごとに指定したイベントメソッドを実行するコンポーネント
/// </summary>
[RequireComponent(typeof(Collider))]
public class SectionKeeper : MonoBehaviour
{
    /// <summary>
    /// 本地点へ進入時に実行されるメソッドが格納されている
    /// </summary>
    [SerializeField, Tooltip("本地点へ進入時に実行するメソッドを指定")]
    UnityEvent OnEnter = default;

    /// <summary>
    /// 本地点（に対応するコライダー）に接触したときメソッドを実行
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        OnEnter.Invoke();
    }
}
