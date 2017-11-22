using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(BattleTrigger))]
[CanEditMultipleObjects]
public class BattleTriggerInspector : Editor 
{
	private BattleTrigger trigger;
	private List<Vector3> cellsPositions = new List<Vector3>();
	private float height;

	void OnEnable()
	{
		trigger = (BattleTrigger)target;
	}

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck ();

		EditorGUILayout.BeginHorizontal ();
		BattleTrigger.BattleFieldType type = (BattleTrigger.BattleFieldType)EditorGUILayout.EnumPopup (trigger.fieldType);
		int width = EditorGUILayout.IntField(trigger.xSize, GUILayout.Width(60));
		int height = trigger.ySize;
		if(type == BattleTrigger.BattleFieldType.Rectangle)
		{
			EditorGUILayout.LabelField ("x", GUILayout.Width(10));
			height = EditorGUILayout.IntField(trigger.ySize, GUILayout.Width(60));
		}
		EditorGUILayout.EndHorizontal ();




		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Activation enabled", GUILayout.Width(95));
		bool activateOnEnter = EditorGUILayout.Toggle (trigger.activationEnabled);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Height", GUILayout.Width(45));
		EditorGUILayout.PropertyField (serializedObject.FindProperty("Offset"));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.PropertyField (serializedObject.FindProperty("recievingRaycastLayer"));
		EditorGUILayout.PropertyField (serializedObject.FindProperty("obstaclesLayers"));



		if(EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject (trigger, "edit");
			trigger.activationEnabled = activateOnEnter;
			trigger.fieldType = type;
			trigger.xSize = width;
			trigger.ySize = height;
			SceneView.RepaintAll ();
		}

		cellsPositions =  trigger.RecalculateHexes ();
	}
		
	protected virtual void OnSceneGUI()
	{
		if (Event.current.type == EventType.Repaint) 
		{
			foreach(Vector3 c in cellsPositions)
			{
				Vector3 cc = new Vector3 (c.x, trigger.Height, c.z);
				Vector3[] hexPoints = new Vector3[]{
					RotatePointAroundPivot(Corner(cc,trigger.CellSize/1.8f,0), cc, Vector3.up * trigger.transform.rotation.eulerAngles.y),
					RotatePointAroundPivot(Corner(cc,trigger.CellSize/1.8f,1), cc, Vector3.up * trigger.transform.rotation.eulerAngles.y),
					RotatePointAroundPivot(Corner(cc,trigger.CellSize/1.8f,2), cc, Vector3.up * trigger.transform.rotation.eulerAngles.y),
					RotatePointAroundPivot(Corner(cc,trigger.CellSize/1.8f,3), cc, Vector3.up * trigger.transform.rotation.eulerAngles.y),
					RotatePointAroundPivot(Corner(cc,trigger.CellSize/1.8f,4), cc, Vector3.up * trigger.transform.rotation.eulerAngles.y),
					RotatePointAroundPivot(Corner(cc,trigger.CellSize/1.8f,5), cc, Vector3.up * trigger.transform.rotation.eulerAngles.y)
				};

				Handles.color = new Color (180, 180 , 50, 0.3f);
				Handles.DrawAAConvexPolygon (hexPoints);
			}
		}
	}

	private Vector3 Corner(Vector3 center3d, float size, int i)
	{
		Vector2 center = new Vector2 (center3d.x, center3d.z);
		var angle_deg = 60 * i + 30;
		var angle_rad = Mathf.PI / 180 * angle_deg;
						return new Vector3 (center.x + size * Mathf.Cos (angle_rad), center3d.y,center.y + size * Mathf.Sin(angle_rad));
	}

	private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
		Vector3 dir = point - pivot; // get point direction relative to pivot
		dir = Quaternion.Euler(angles) * dir; // rotate it
		point = dir + pivot; // calculate rotated point
		return point; // return it
	}
}