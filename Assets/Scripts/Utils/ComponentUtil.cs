using UnityEngine;

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

public class ComponentUtil
{
    /// <summary>
    /// <para>初期化</para>
    /// <para>【第1引数】初期化したいTransform</para>
    /// </summary>
    public static void Normalize(Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localEulerAngles = Vector3.zero;
        t.localScale = Vector3.one;
    }

    /// <summary>
    /// <para>GameObjectを生成</para>
    /// <para>【第1引数】親となるGameObject</para>
    /// <para>【第2引数】生成したいGameObject(外部から持ってきた物)</para>
    /// <para>【戻り値】生成されたGameObject</para>
    /// </summary>
    public static GameObject InstantiateTo(GameObject parent, GameObject go)
    {
        GameObject ins = (GameObject)GameObject.Instantiate(
            go,
            parent.transform.position,
            parent.transform.rotation
        );
        ins.transform.SetParent(parent.transform);
        Normalize(ins.transform);
        return ins;
    }

    /// <summary>
    /// <para>GameObjectを生成し、そのGameObjectに張り付いている、Componentクラスを取得する</para>
    /// <para>【第1引数】親となるGameObject</para>
    /// <para>【第2引数】生成したいGameObject(外部から持ってきた物)</para>
    /// <para>【戻り値】生成されたGameObjectに張り付いてる指定したComponentクラス</para>
    /// </summary>
    public static T InstantiateTo<T>(GameObject parent, GameObject go)
        where T : Component
    {
        return InstantiateTo(parent, go).GetComponent<T>();
    }

    /// <summary>
    /// <para>Transform以下に紐付いている子のGameObjectを全て削除する</para>
    /// <para>【第1引数】削除したいTransform</para>
    /// </summary>
    public static void DeleteAllChildren(Transform parent)
    {
        List<Transform> transformList = new List<Transform>();
        foreach (Transform child in parent)
        {
            transformList.Add(child);
        }
        parent.DetachChildren();
        foreach (Transform child in transformList)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// <para>Transform以下の末尾の子のGameObjectを削除する</para>
    /// <para>【第1引数】削除したいTransform</para>
    /// </summary>
    public static void DeleteLastChild(Transform parent)
    {
        int childCount = parent.childCount;
        if (childCount > 0)
        {
            Transform lastChild = parent.GetChild(childCount - 1);
            GameObject.Destroy(lastChild.gameObject);
        }
    }

    /// <summary>
    /// <para>Transform以下の末尾の子のGameObjectを削除する</para>
    /// <para>【第1引数】削除したいTransform</para>
    /// </summary>
    public static void DeleteFirstChild(Transform parent)
    {
        int childCount = parent.childCount;
        if (childCount > 0)
        {
            Transform firstChild = parent.GetChild(0);
            GameObject.Destroy(firstChild.gameObject);
        }
    }

}