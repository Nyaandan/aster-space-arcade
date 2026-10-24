using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GammaRaySpawnMarker))]
public class GammaRaySpawnMarkerEditor : Editor
{
    private static GammaRaySpawnMarker activeMarker = null;

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

        GammaRaySpawnMarker marker = target as GammaRaySpawnMarker;
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
        GammaRaySpawnMarker marker = target as GammaRaySpawnMarker;
        if (marker == null) return;

        Vector2 orientVector = new(Mathf.Cos(marker.orientation * Mathf.Deg2Rad),
                                    Mathf.Sin(marker.orientation * Mathf.Deg2Rad));
        Vector3 position = orientVector * marker.distanceToCenter;
        float handleSize = HandleUtility.GetHandleSize(Vector3.zero);

        if (activeMarker == null)
        {
            Vector2 parallelVector = new(-orientVector.y, orientVector.x);
            Vector2 mark0 = orientVector * (marker.distanceToCenter - .5f * marker.magnitude);
            Vector2 mark1 = orientVector * (marker.distanceToCenter + .5f * marker.magnitude);

            Handles.color = Color.gold;
            Handles.DrawLine(mark0, mark1, 2f);
            Handles.DrawLine(mark0 + .5f * parallelVector, mark0 - .5f * parallelVector, 2f);
            Handles.DrawLine(mark1 - .5f * parallelVector, mark1 + .5f * parallelVector, 2f);
        }
        else
        {
            Handles.DrawLine(Vector3.zero, orientVector * marker.distanceToCenter, 2f);

            EditorGUI.BeginChangeCheck();
            Vector3 handlePosition = Handles.Slider2D(position,
                Vector3.forward, Vector3.right, Vector3.up,
                handleSize, Handles.ArrowHandleCap, .01f);
            Vector3 magnitudeSlider = Handles.Slider(
                handlePosition + .5f * marker.magnitude * (Vector3)orientVector,
                orientVector, handleSize * .2f, Handles.CubeHandleCap, .1f);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(marker, "Edit Gamma Ray Spawn Marker");
                marker.distanceToCenter = handlePosition.magnitude;
                marker.orientation = Mathf.Atan2(handlePosition.y, handlePosition.x) * Mathf.Rad2Deg;
                marker.magnitude = (magnitudeSlider - handlePosition).magnitude * 2;
                EditorUtility.SetDirty(marker);
            }
        }
    }

}
