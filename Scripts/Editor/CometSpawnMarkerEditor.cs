using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

[CustomEditor(typeof(CometSpawnMarker))]
public class CometSpawnMarkerEditor: Editor
{
    private static CometSpawnMarker activeMarker = null;

    private void OnEnable()
    {
        SceneView.duringSceneGui += HandleSceneView;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= HandleSceneView;
        if (activeMarker != null)
        {
            activeMarker = null;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CometSpawnMarker marker = target as CometSpawnMarker;
        if (marker == null) return;

        Color bgColor = GUI.backgroundColor;

        if (activeMarker == marker)
        {
            GUI.backgroundColor = Color.crimson;
            if (GUILayout.Button("Confirm Marker"))
            {
                activeMarker = null;
                SceneView.RepaintAll();
            }

        }
        else
        {
            GUI.backgroundColor = Color.forestGreen;
            if (GUILayout.Button("Edit Marker"))
            {
                activeMarker = marker;
                SceneView.RepaintAll();
            }
        }
        GUI.backgroundColor = bgColor;
    }

    private void HandleSceneView(SceneView sceneView)
    {
        CometSpawnMarker marker = target as CometSpawnMarker;
        if (marker == null) return;

        float handleSize = HandleUtility.GetHandleSize(marker.position);
        Color bgColor = GUI.backgroundColor;

        if (activeMarker == null)
        {
            Handles.color = Color.gold;
            Handles.DrawLine(
                marker.position,
                marker.position + (marker.target - marker.position).normalized * handleSize,
                2f);
            Handles.SphereHandleCap(0,
                marker.position,
                Quaternion.identity,
                handleSize * .2f,
                EventType.Repaint);
            Handles.ArrowHandleCap(0,
                marker.position,
                Quaternion.LookRotation(marker.target - marker.position, Vector3.forward),
                handleSize,
                EventType.Repaint);
            Handles.CubeHandleCap(0,
                marker.target,
                Quaternion.LookRotation(marker.target - marker.position, Vector3.forward),
                handleSize * .2f,
                EventType.Repaint);
        }
        else
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newOrigin = Handles.PositionHandle(marker.position, Quaternion.identity);
            Vector3 newDestination = Handles.PositionHandle(marker.target, Quaternion.identity);

            // Draw an arrow to visualize path
            Handles.ArrowHandleCap(0,
                newOrigin,
                Quaternion.LookRotation(newDestination - newOrigin, Vector3.forward),
                HandleUtility.GetHandleSize(marker.position) * 0.5f,
                EventType.Repaint);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(marker, "Edit Comet Spawn Marker");
                marker.position = newOrigin;
                marker.target = newDestination;
                EditorUtility.SetDirty(marker);
            }
        }

        GUI.backgroundColor = bgColor;
    }
}
