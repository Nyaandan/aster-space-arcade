using TMPro;
using UnityEngine;

public class DebuggerConsole : MonoBehaviour
{
    [Tooltip("The panel containing the text boxes for debugging")]
    public static DebuggerConsole Instance { get; private set; }
    TextMeshProUGUI[] textMeshes;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        textMeshes = GetComponentsInChildren<TextMeshProUGUI>();
    }

    // THE BOXES ARE 1-INDEXED!!!!
    public static void WriteToBox(int box, string msg)
    {
        if (Instance != null) Instance.InstanceWriteToBox(box - 1, msg);
    }

    private void InstanceWriteToBox(int box, string msg)
    {
        if (textMeshes[box]) textMeshes[box].text = msg;
    }
}
