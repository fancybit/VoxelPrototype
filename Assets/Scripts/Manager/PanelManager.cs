using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LuaInterface;

namespace SimpleFramework.Manager {

    public class PanelManager : MonoBehaviour {
        private Transform parent;

        Transform Parent {
            get {
                if (parent == null) {
                    GameObject go = GameObject.FindWithTag("GuiCamera");
                    if (go != null) parent = go.transform;
                }
                return parent;
            }
        }

        /// <summary>
        /// ������壬������Դ������
        /// </summary>
        /// <param name="type"></param>
        public void CreatePanel(string name, LuaFunction func = null) {
            StartCoroutine(OnCreatePanel(name, func));
        }

        IEnumerator OnCreatePanel(string name, LuaFunction func = null) {
            yield return StartCoroutine(Initialize());

            string assetName = name + "Panel";
            // Load asset from assetBundle.
            string abName = name.ToLower() + ".unity3d";
            AssetBundleAssetOperation request = ResourceManager.LoadAssetAsync(abName, assetName, typeof(GameObject));
            if (request == null) yield break;
            yield return StartCoroutine(request);

            // Get the asset.
            GameObject prefab = request.GetAsset<GameObject>();

            if (Parent.FindChild(name) != null || prefab == null) {
                yield break;
            }
            GameObject go = Instantiate(prefab) as GameObject;
            go.name = assetName;
            go.layer = LayerMask.NameToLayer("Default");
            go.transform.SetParent(Parent);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.AddComponent<LuaBehaviour>();

            if (func != null) func.Call(go);
            Debug.LogWarning("CreatePanel::>> " + name + " " + prefab);
        }

        IEnumerator Initialize() {
            ResourceManager.BaseDownloadingURL = Util.GetRelativePath();

            // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
            var request = ResourceManager.Initialize(AppConst.AssetDirname);
            if (request != null)
                yield return StartCoroutine(request);
        }
    }
}