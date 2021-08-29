using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3次元空間中のGUIをカメラの方向へ向ける
/// </summary>
public class GUI3dBillboard : MonoBehaviour {

    /// <summary>
    /// メインカメラ用タグ
    /// </summary>
    [SerializeField]
    string mainCameraTag = "MainCamera";
    /// <summary>
    /// カメラオブジェクト
    /// </summary>
    GameObject mainCameraObj = default;


    // Use this for initialization
    void Start () {

        mainCameraObj = GameObject.FindWithTag(mainCameraTag);
    }
	
	// Update is called once per frame
	void Update () {

        transform.rotation = mainCameraObj.transform.rotation;
	}
}
