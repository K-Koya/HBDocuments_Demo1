using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の倒された時の動作
/// </summary>
[RequireComponent(typeof(Status))]
public class EnemysDefeated : MyMonoBehaviour
{
    /// <summary>
    /// 倒されたときに発生させるエフェクト
    /// </summary>
    [SerializeField, Tooltip("倒されたときに発生させるエフェクト")]
    GameObject effect = default;

    /// <summary>
    /// 該当キャラクターのステータスコンポーネント
    /// </summary>
    Status status = default;

    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        status = GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPausing) return;

        //やられたら、消えるまでのルーティンを走らせる
        if (status.IsDefeated)
        {
            StartCoroutine(DestroySequence());
        }
    }

    /// <summary>
    /// 倒され、エフェクトを出して消えるまでのフロー
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroySequence()
    {
        yield return time.WaitForSeconds(1.2f);
        GameObject obj = Instantiate(effect);
        obj.transform.position = transform.position;
        Destroy(gameObject);
    }
}
