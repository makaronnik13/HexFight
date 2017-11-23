using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeTimeline : MonoBehaviour {

	public Gradient timeLineColorV;
	public Gradient timeLineColorH;
	public Image timeLineImage;
	private float textureHeight = 100;
	private float textureWidth = 25;
	private GameObject token
	{
		get
		{
			return (GameObject)Resources.Load<GameObject> ("InitiativeTimeline/Token");
		}
	}

	// Use this for initialization
	public void Start () 
	{
		timeLineImage.sprite = GetGradientSprite (timeLineColorV, timeLineColorH);
		GameController.Instance.onModeChanged += ModeChanged;
	}

	private void ModeChanged(GameController.GameMode mode)
	{
		Clear ();
		if (mode == GameController.GameMode.Battle) {
			foreach(BattleWarrior bw in GameController.Instance.CurrentField.Warriors)
			{
				AddWarrior (bw);
			}
			Show ();
		} else {
			Hide ();
		}
	}

	public void Show()
	{
		GetComponent<Animator> ().SetBool ("Active", true);
		Invoke ("StartTimeline", 3f);
	}

	public void Hide()
	{
		StopTimeline ();
		GetComponent<Animator> ().SetBool ("Active", false);
	}

	public void AddWarrior(BattleWarrior bw)
	{
        //Lean.Pool.LeanPool.Spawn(token, transform.GetChild(1).position, Quaternion.identity, transform.GetChild(1)).GetComponent<InitiativeToken>().Init(bw);
        Instantiate (token, transform.GetChild(1).position, Quaternion.identity ,transform.GetChild(1)).GetComponent<InitiativeToken>().Init(bw);
	}

	private void Clear()
	{
		foreach(Transform t in transform.GetChild(1))
		{
            Destroy(t.gameObject);
		}
	}

	private void StartTimeline()
	{
		InitiativeToken.speedCoef = 1;
	}

	private void StopTimeline()
	{
		InitiativeToken.speedCoef = 0;
	}

	private Sprite GetGradientSprite(Gradient gradientV, Gradient gradientH)
	{
		Texture2D texture = new Texture2D ((int)textureWidth, (int)textureHeight);
		for(int i = 0; i< textureHeight; i++)
		{
			for(int j = 0;j<textureWidth;j++)
			{
				Color c = gradientV.Evaluate(i/textureHeight);
				float x = gradientH.Evaluate (j/textureWidth).a;
				c.a *= x;
				texture.SetPixel (j, i, c);
			}
		}
		texture.Apply ();
		return Sprite.Create (texture, new Rect(0,0,texture.width, texture.height), Vector2.one/2);
	}
}
