using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 照準による動作の抽象メソッド
/// </summary>
[RequireComponent(typeof(Collider))]
public abstract class AimAreaAbstruct : MyMonoBehaviour
{
    /// <summary>
    /// 攻撃範囲コライダー
    /// </summary>
    Collider area = default;

    /// <summary>
    /// 派生先での、共通の初期化メソッド
    /// </summary>
    protected void Init()
    {
        area = GetComponent<Collider>();
    }
}