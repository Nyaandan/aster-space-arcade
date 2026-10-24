using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AsteroidSpawnMarker))]
public class AsteroidSpawnMarkerEditor : Editor
{
    private static AsteroidSpawnMarker activeMarker = null;

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

        AsteroidSpawnMarker marker = target as AsteroidSpawnMarker;
        if (marker == null) return;

        Color oldColor = GUI.backgroundColor;

        if (activeMarker == marker)
        {
            GUI.backgroundColor = Color.red;
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
        GUI.backgroundColor = oldColor;
    }

    private void HandleSceneView(SceneView sceneView)
    {
        
        AsteroidSpawnMarker marker = target as AsteroidSpawnMarker;
        if (marker == null) return;

        float handleSize = HandleUtility.GetHandleSize(marker.position);
        if (activeMarker == null)
        {
            Handles.color = Color.gold;
            Handles.DrawLine(marker.position, marker.position + marker.direction.normalized * handleSize, 2f);
            Handles.CubeHandleCap(0,
                marker.position,
                Quaternion.LookRotation(marker.direction, Vector3.forward),
                handleSize * .2f,
                EventType.Repaint
                );
            Handles.ArrowHandleCap(0,
                marker.position,
                Quaternion.LookRotation(marker.direction, Vector3.forward),
                handleSize,
                EventType.Repaint);
        }
        else
        {
            Handles.DrawLine(marker.position, marker.position + marker.direction.normalized * handleSize, 2f);

            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = Handles.PositionHandle(marker.position, Quaternion.identity);
            Vector3 handlePosition = Handles.Slider2D(marker.position + marker.direction.normalized * handleSize,
                Vector3.forward, Vector3.right, Vector3.up,
                handleSize, Handles.ArrowHandleCap, .01f);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(marker, "Edit Asteroid Spawn Marker");
                Vector3 newDirection = ((Vector2)handlePosition - marker.position).normalized;
                marker.position = newPosition;
                marker.direction = newDirection;
                EditorUtility.SetDirty(marker);
            }
        }
    }
}
