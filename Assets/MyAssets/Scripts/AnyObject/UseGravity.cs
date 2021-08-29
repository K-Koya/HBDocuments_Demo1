using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// 任意の方向に重力をかける
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class UseGravity : MyMonoBehaviour
{
    /// <summary>
    /// 使用するリジッドボディ
    /// </summary>
    RigidbodyTimeline3D rb = default;

    /// <summary>
    /// 重力ベクトル
    /// </summary>
    Vector3 size = new Vector3(0.0f, -9.8f, 0.0f);


    public Vector3 Size { get => size; set => size = value; }



    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        rb = time.rigidbody;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPausing) return;

        rb.AddForce(size * time.deltaTime, ForceMode.Acceleration);
    }
}
