using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MarkerGenerator : MonoBehaviour
{

    #region Singleton

    public static MarkerGenerator instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Marker Generator found!");
            return;
        }
        instance = this;
    }

    #endregion

    public GameObject markerPrefab;

    public void GenerateTextMarker(GameObject target, Vector3 offset, string text, Color color)
    {
        GameObject result = GameObject.Instantiate(markerPrefab, target.transform.position+offset, Quaternion.Euler(0,0,0), gameObject.transform);
        TextMeshPro textMesh = result.GetComponent<TextMeshPro>();
        TextMarker marker = result.GetComponent<TextMarker>();
        marker.target = target.transform;
        marker.offset = offset;
        textMesh.text = text;
        textMesh.color = color;
    }
}
