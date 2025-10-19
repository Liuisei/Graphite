using UnityEngine;

namespace JamSeed.Foundation
{
    /// <summary>
    /// シーン内に1つだけ存在するMonoBehaviourシングルトン基底クラス
    /// シーン切り替えで破棄される通常のシングルトン用途
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // シーン内の既存インスタンスを探す
                    _instance = FindAnyObjectByType<T>();

                    if (_instance == null)
                    {
                        Debug.LogError($"[SceneSingleton] {typeof(T)} のインスタンスがシーン内に存在しません。");
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                // 既に別のインスタンスがいるなら自分を破棄
                Destroy(gameObject);
                return;
            }
            _instance = this as T;
        }
    }


}
