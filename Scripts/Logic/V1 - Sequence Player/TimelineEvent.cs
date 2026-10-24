using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

////////////////////
//
// THE SHEET MUSIC
//
////////////////////

[Serializable]
public class EventParameter
{
    [XmlAttribute]
    public string key;
    [XmlAttribute]
    public float value;
}

[Serializable]
public class TimelineEvent
{
    [XmlAttribute]
    public float timestamp; // in seconds
    [XmlAttribute]
    public string id;
    [XmlArray("parameters")] [XmlArrayItem("Param")]
    public List<EventParameter> parameterList;
    [NonSerialized] [XmlIgnore] public Dictionary<string, float> parameters; // configuration package for speed, duration...

    public void PrepareForSerialization()
    {
        if (parameters == null)
        {
            parameterList = null;
            return;
        }

        parameterList = new List<EventParameter>(parameters.Count);
        foreach (var kv in parameters)
            parameterList.Add(new EventParameter { key = kv.Key, value = kv.Value });
    }

    public void BuidParameters()
    {
        parameters = parameterList.ToDictionary(p => p.key, p => p.value) ?? new Dictionary<string, float>();
    }
}

[Serializable]
public class PhaseTimeline
{
    [XmlAttribute("name")]
    public string phaseName;
    [XmlAttribute("duration")]
    public float phaseDuration; // in seconds
    [XmlArrayItem("Event")]
    public List<TimelineEvent> events; // sorted by timestamp ascending
}

[Serializable]
[XmlRoot("Level")]
public class LevelTimeline
{
    [XmlAttribute("name")]
    public string levelName;
    [XmlArrayItem("Phase")]
    public List<PhaseTimeline> phases; // sorted by phase order
}