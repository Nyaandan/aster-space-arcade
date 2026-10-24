using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/////////////////
//
// THE PREPPER
//
/////////////////

public class LevelManagerXml : MonoBehaviour
{
    public static LevelManagerXml Instance { get; private set; }
    public LevelTimeline currentLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Prepare();
    }

    void LoadLevel(LevelTimeline level)
    {
        if (level == null)
        {
            Debug.LogError("LoadLevel: level is null");
            return;
        }

        currentLevel = level;

        // Rebuild runtime dictionaries
        if (currentLevel.phases != null)
        {
            foreach (var phase in currentLevel.phases)
            {
                if (phase?.events == null) continue;
                foreach (var ev in phase.events)
                    ev?.BuidParameters(); // uses existing method name in TimelineEvent.cs
            }
        }

        Debug.Log("Loaded level: " + (currentLevel.levelName ?? "<unnamed>"));
    }

    // Load by name from Assets/Resources/Levels/<levelName>.xml (omit extension)
    void LoadLevelFromResources(string levelName)
    {
        if (string.IsNullOrEmpty(levelName))
        {
            Debug.LogError("LoadLevelFromResources: levelName is empty");
            return;
        }

        var ta = Resources.Load<TextAsset>($"Levels/{levelName}");
        if (ta == null)
        {
            Debug.LogError($"Level not found in Resources/Levels: {levelName}");
            return;
        }

        XmlSerializer serializer = new(typeof(LevelTimeline));
        LevelTimeline level;
        try
        {
            using var sr = new StringReader(ta.text);
            level = (LevelTimeline)serializer.Deserialize(sr);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to deserialize level '{levelName}': {ex.Message}");
            return;
        }

        LoadLevel(level);
    }

    void SerializeLevel(LevelTimeline level)
    {
        if (level == null) return;

        // Prepare serializable lists
        if (level.phases != null)
        {
            foreach (var phase in level.phases)
            {
                if (phase?.events == null) continue;
                foreach (var ev in phase.events)
                    ev?.PrepareForSerialization();
            }
        }

        XmlSerializer ser = new(typeof(LevelTimeline));
        string xml;
        using (var sw = new StringWriter())
        {
            ser.Serialize(sw, level);
            xml = sw.ToString();
        }

        Debug.Log(xml);

        // Save to disk for inspection
        var path = Path.Combine(Application.persistentDataPath, (level.levelName ?? "level") + ".xml");
        File.WriteAllText(path, xml, Encoding.UTF8);
        Debug.Log("Saved level XML to: " + path);
    }

    void Prepare()
    {
        print("Preparing level data...");
        LoadLevelFromResources("Level0");
        //GameMaster.Instance.StartGame(currentLevel);
    }
}