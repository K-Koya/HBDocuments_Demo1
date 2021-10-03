using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Chronos;

public class HPMeterController : MyMonoBehaviour 
{
    /// <summary>
    /// プレイヤーのステータス
    /// </summary>
    Status status = default;

    /// <summary>
    /// プレイヤーHPの実数表示Image
    /// </summary>
    [SerializeField, Tooltip("プレイヤーHPの実数表示Imageコンポーネント")]
    Image hpMeterNowImg = default;
    /// <summary>
    /// プレイヤーHPの余白表示Image
    /// </summary>
    [SerializeField, Tooltip("プレイヤーHPの余白表示Imageコンポーネント")]
    Image hpMeterBlankImg = default;


    /// <summary>
    /// HPの余白表示のためのHP値保管
    /// </summary>
    float beforeHPRatio = 0.0f;




    // Use this for initialization
    void Start () 
    {
        TimelineInit();
        status = GameObject.FindGameObjectWithTag("Player").GetComponent<Status>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (IsPausing) return;
        syncHPMeter();
    }

    /// <summary>
    /// HPメーターをプレイヤーのHPとして反映させる
    /// </summary>
    private void syncHPMeter()
    {
        short maxHp = status.MaxHP;
        short nowHp = status.NowHP;
        //怯み中はBlank部分を表示する
        bool doBlankHP = !status.IsFlirting;

        //HPの割合値を計算
        float hpRatio = nowHp / (float)maxHp;

        //HP実数値のメーターを設定
        hpMeterNowImg.fillAmount = hpRatio;

        //HPに応じていい感じに 青→緑→黄→赤→赤黒 に変化させていくための演算
        float hue = (4.0f * hpRatio - 1.0f) / 6.0f;
        float val = 0.9f;
        if (hue < 0.0f)
        {
            val += hue;
            hue = 0.0f;
        }
        hpMeterNowImg.color = Color.HSVToRGB(hue, 1.0f, val);

        //HPの余白表示が表示されている状態で、余白部分を減らすフラグが立っていれば減少処理
        if (beforeHPRatio > hpRatio)
        {
            if (doBlankHP) beforeHPRatio = Mathf.Clamp(beforeHPRatio - time.deltaTime, hpRatio, maxHp);
        }
        else
        {
            beforeHPRatio = hpRatio;
        }

        //余白部分を設定
        hpMeterBlankImg.fillAmount = beforeHPRatio;
    }
}
