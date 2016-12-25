using UnityEngine;
using System.Collections;
using wuxingogo.Runtime;

public class XBezierArrowTemplate : XScriptableObject {
	public Vector3 position;
	public Quaternion rotation;
	public Vector3 localScale;

	public Vector3 position0;
	public Vector3 position1;
	public Vector3 position2;
	public Vector3 position3;

	public float step = 1;

	public AnimationCurve yWidthCurve;
	[Header(" Right Top")]
	public Vector2 headRTTexCoord = Vector2.one;
	[Header(" Right Down")]
	public Vector2 headRDTexCoord = Vector2.zero;

	public float headWid = 0.05f;

	public int stepCount = 100;

	public void Save(XBezierArrow script)
	{
		
		this.position = script.transform.position;
		this.rotation = script.transform.rotation;
		this.localScale = script.transform.localScale;

		position0 = script.position0;
		position1 = script.position1;
		position2 = script.position2;
		position3 = script.position3;

		step = script.step;
		yWidthCurve = script.yWidthCurve;
		headRDTexCoord = script.headRDTexCoord;
		headRTTexCoord = script.headRTTexCoord;
		headWid = script.headWid;
		stepCount = script.stepCount;

	}

	public void CreateFromTemplate(XBezierArrow script){
		script.transform.position = position;
		script.transform.rotation = rotation;
		script.transform.localScale = localScale;

		script.position0 = position0;
		script.position1 = position1;
		script.position2 = position2;
		script.position3 = position3;

		script.step = step;
		script.yWidthCurve = yWidthCurve;
		script.headRTTexCoord = headRTTexCoord;
		script.headRDTexCoord = headRDTexCoord;
		script.headWid = headWid;
		script.stepCount = stepCount;

		script.Refresh();

		script.isNeedUpdate = false;

	}
}
