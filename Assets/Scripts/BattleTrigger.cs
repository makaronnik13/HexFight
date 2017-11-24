using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using GridSystem;

public class BattleTrigger : MonoBehaviour
{

	public enum BattleFieldType
	{
		Hex,
		Rectangle,
	}
	public float Height;
	public BattleFieldType fieldType;
	public bool activationEnabled = true;
	private GameObject GridBattleField {
		get {
			return (GameObject)Resources.Load<GameObject> ("HexBattleSystem/HexField");
		}
	}
	private List<Vector2> cellsCoordinates = new List<Vector2> ();
	private HexField field;
	public RangeFloat Offset = new RangeFloat (-10, 10);

	public float CellSize
	{
		get
		{
			return transform.localScale.x;	
		}
	}
	public LayerMask recievingRaycastLayer;
	public LayerMask obstaclesLayers;
	public int xSize = 10, ySize = 10;

	public void StartBattle ()
	{
		if (activationEnabled) 
		{
			GameController.Instance.Warrior = null;
			field = Instantiate (GridBattleField, transform.position, Quaternion.identity, transform).GetComponent<HexField> ();
			GameController.Instance.CurrentField = field;
			List<Vector3> positions = RecalculateHexes ();



			field.GenerateCells (positions, cellsCoordinates);
		}

		field.Warriors.Clear ();
		//not so good
		foreach(BattleWarrior bw in FindObjectsOfType<BattleWarrior>())
		{
			Cell c = field.GetClosestCell (bw.transform.position, true);
			c.cellWarrior = bw;
			NavMeshHit hit;
			NavMesh.SamplePosition (field.CellCoordToWorld (c),out hit, 1, NavMesh.AllAreas);
			bw.GetComponent<FakeController> ().GoTo (hit.position, false);
			field.Warriors.Add (bw);
		}

		GameController.Instance.Mode = GameController.GameMode.Battle;
	}



	public List<Vector3> RecalculateHexes ()
	{
		List<Vector3> result = new List<Vector3> ();
		cellsCoordinates.Clear ();
		switch (fieldType) {
		case BattleFieldType.Hex:
			for (int i = -xSize * 3; i < xSize * 3; i++) {
				for (int j = -xSize * 3; j < xSize * 3; j++) {
					if (UnityEngine.Random.Range (0, 10) >= 0 && Mathf.Abs (i + j) < xSize && Mathf.Abs (j) < xSize && Mathf.Abs (i) < xSize) {
						cellsCoordinates.Add (new Vector2 (i, j));
					}
				}
			}
			break;
		case BattleFieldType.Rectangle:
			for (int i = Mathf.CeilToInt (-(float)xSize / 2); i < Mathf.CeilToInt ((float)xSize / 2); i++) {
				
				for (int j = Mathf.CeilToInt (-(float)ySize / 2); j < Mathf.CeilToInt ((float)ySize / 2); j++) {
					if (j > 0 && j % 2 == 1) {
						cellsCoordinates.Add (new Vector2 (-1 + i - Mathf.CeilToInt (j / 2), j));
					} else {
						cellsCoordinates.Add (new Vector2 (i - Mathf.CeilToInt (j / 2), j));
					}
				}
			}
			break;
		}

		List<Vector2> actualRoordinates = new List<Vector2> ();

		foreach (Vector2 c in cellsCoordinates) {
			Vector2 cell2DCoord = CellCoordToWorld (c);
			Vector3 cellPosition = RotatePointAroundPivot (new Vector3 (cell2DCoord.x, transform.position.y, cell2DCoord.y), transform.position, transform.rotation.eulerAngles);

			Vector3 r = Raycast (cellPosition, Offset.Min, Offset.Max, obstaclesLayers, recievingRaycastLayer);

			if (r.x!=Mathf.Infinity) {
				result.Add (r);
				actualRoordinates.Add (c);
			}


		}

		cellsCoordinates = actualRoordinates;

		return result;
	}

	private Vector2 CellCoordToWorld (Vector2 cellCoord)
	{
		float x =  CellSize * (float)Mathf.Sqrt (3) * (cellCoord.x + cellCoord.y / 2);
		float y =  CellSize * 3 / 2 * -cellCoord.y;
		Vector3 pos = transform.position + new Vector3 (x, 0, y) * 0.6f;
		return new Vector2 (pos.x, pos.z);
	}

	private Vector3 RotatePointAroundPivot (Vector3 point, Vector3 pivot, Vector3 angles)
	{
		Vector3 dir = point - pivot; // get point direction relative to pivot
		dir = Quaternion.Euler (angles) * dir; // rotate it
		point = dir + pivot; // calculate rotated point
		return point; // return it
	}

	private Vector3 Raycast (Vector3 fromPosition, float minOffset, float maxOffset, LayerMask obstacleLayer, LayerMask raycastingLayer)
	{
		RaycastHit hit;
		if (Physics.Raycast (fromPosition, Vector3.down, out hit, 1000, raycastingLayer.value)) { //obstacleLayer.value | raycastingLayer.value 
			Collider[] hitColliders = Physics.OverlapCapsule (hit.point + Vector3.down * CellSize * 3, hit.point + Vector3.up * CellSize * 3, CellSize/2f);

			if (hitColliders.ToList ().Where (c => Contains (obstacleLayer, c.gameObject.layer)).Count () == 0) {//if(Contains(raycastingLayer, hit.collider.gameObject.layer))
				float ofset = hit.point.y - hit.collider.gameObject.transform.position.y;

				if ((ofset >= 0 && ofset < maxOffset) || (ofset < 0 && -ofset < minOffset)) {
					Height = hit.collider.gameObject.transform.position.y;	
					return hit.point;
				}
			}
			return Vector3.one*Mathf.Infinity;
		} 
		return Vector3.one*Mathf.Infinity;
	}

	private bool Contains (LayerMask mask, int layer)
	{
		return mask == (mask | (1 << layer));
	}
}
