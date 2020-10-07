using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBoxGenerator : MonoBehaviour
{

    #region Singleton

    public static DialogBoxGenerator instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Dialog Box Generator found!");
            return;
        }
        instance = this;
    }

    #endregion

    public GameObject dialogBoxPrefab;

    public GameObject GenerateDialogBox(GameObject parent, Vector2 offset, string text, float decayTime)
    {
        GameObject newDialogBox = Instantiate(dialogBoxPrefab, parent.transform.position + new Vector3(offset.x, offset.y, 0), parent.transform.rotation);
        newDialogBox.transform.SetParent(parent.transform);
        newDialogBox.GetComponent<DialogBoxController>().SetText(text);
        newDialogBox.GetComponent<DialogBoxController>().StartDecay(decayTime);
        return newDialogBox;
    }
}
