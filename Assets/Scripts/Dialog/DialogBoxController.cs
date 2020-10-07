using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBoxController : MonoBehaviour
{
    public float maxSize;

    bool decaying;

    public TextMesh textMesh;
    public MeshRenderer textRenderer;

    // Start is called before the first frame update
    void Start()
    {
        decaying = false;
        textRenderer.sortingLayerName = "DialogLayer";
        textRenderer.sortingOrder = 3;
    }

    public void StartDecay(float decayTime)
    {
        if (!decaying) {
            StartCoroutine("Decay", decayTime);
            decaying = true;
        }
    }

    IEnumerator Decay(float decayTime)
    {
        yield return new WaitForSeconds(decayTime);
        Destroy(gameObject);
    }

    public void SetText(string text)
    {
        textMesh.text = string.Empty;
        string[] sections = text.Split(' ');
        string displayStorage = string.Empty;

        for (int i = 0; i < sections.Length; i++)
        {
            textMesh.text += sections[i] + " ";
            if (textMesh.GetComponent<Renderer>().bounds.extents.x > maxSize)
            {
                textMesh.text = displayStorage.TrimEnd() + System.Environment.NewLine + sections[i] + " ";
            }
            displayStorage = textMesh.text;
        }
    }
}
