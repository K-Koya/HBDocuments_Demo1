using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵を生成する
/// </summary>
public class EnemySpawner : MyMonoBehaviour
{
    /// <summary>
    /// 敵のオブジェクトの全格納庫
    /// </summary>
    static List<GameObject> allEnemies = new List<GameObject>();

    /// <summary>
    /// スポーンさせる敵プレハブ
    /// </summary>
    [SerializeField]
    GameObject[] enemyPrefabs = default;

    /// <summary>
    /// スポーンさせる敵プレハブの実体
    /// </summary>
    List<GameObject> enemies = new List<GameObject>();

    /// <summary>
    /// 敵を出現させるまでの距離
    /// </summary>
    [SerializeField]
    float appearDistance = 100f;
    /// <summary>
    /// 敵を見えなくさせるまでの距離
    /// </summary>
    [SerializeField]
    float disappearDistance = 50f;

    /// <summary>
    /// 地面レイヤ
    /// </summary>
    [SerializeField]
    LayerMask layerOfGround = default;

    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    GameObject player = default;

    /// <summary>
    /// true:出現済み
    /// </summary>
    bool isAppeared = false;

    public static List<GameObject> AllEnemies { get => allEnemies; set => allEnemies = value; }


    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        player = GameObject.FindGameObjectWithTag("Player");

        //一度すべての敵オブジェクトを実体化させ、その後非表示にする
        foreach (GameObject obj in enemyPrefabs)
        {
            GameObject enemy = Instantiate(obj);
            enemies.Add(enemy);
            allEnemies.Add(enemy);
            MoveForEnemy move = enemy.GetComponent<MoveForEnemy>();
            move.FollowTarget = transform;
            move.AttackTarget = player.transform;
            enemy.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPausing) return;

        //出現状況と距離状況から処理を分岐する
        float sqrDistance = Vector3.SqrMagnitude(player.transform.position - transform.position);
        if (isAppeared && sqrDistance > Mathf.Pow(disappearDistance, 2f))
        {
            //隠す
            foreach (GameObject enemy in enemies) enemy.SetActive(false);
            isAppeared = false;
        }
        else if(!isAppeared && sqrDistance < Mathf.Pow(appearDistance, 2f))
        {
            //地面探索Ray
            RaycastHit hitInfo;

            //ランダム位置に出現
            foreach (GameObject enemy in enemies)
            {
                byte timeoutCounter = 0;
                //地面が見つかるまで繰り替えす
                while (timeoutCounter < 100)
                {
                    //座標をランダム決定
                    Vector3 pos = transform.position + new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
                    //地面探索
                    bool isfound = Physics.Raycast(pos + transform.up * 20f, -transform.up, out hitInfo, 100.0f, layerOfGround);
                    //見つかったら、そこに再出現させる
                    if (isfound)
                    {
                        enemy.transform.position = hitInfo.point;
                        enemy.SetActive(true);
                        break;
                    }
                    timeoutCounter++;
                }
            }

            isAppeared = true;
        }

        //倒されてDestroyされnullになったオブジェクトを排除する
        List<GameObject> nulls = new List<GameObject>();
        foreach(GameObject enemy in enemies)
        {
            if (!enemy)
            {
                allEnemies.Remove(enemy);
                nulls.Add(enemy);
            }
        }
        foreach(GameObject nul in nulls)
        {
            enemies.Remove(nul);
        }
    }
}
