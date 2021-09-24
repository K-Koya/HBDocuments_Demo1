using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    /// <summary>
    /// ジャンプ先のシーン名
    /// </summary>
    [SerializeField, Tooltip("ジャンプ先のシーン名")]
    string sceneName = default;

    /// <summary>
    /// ジャンプ先のシーン名へジャンプする
    /// </summary>
    public void Jump()
    {
        StartCoroutine(ChangeFlow());
    }

    IEnumerator ChangeFlow()
    {
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene(sceneName);
    }
}
