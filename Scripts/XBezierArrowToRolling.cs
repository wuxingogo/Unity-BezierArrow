using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wuxingogo.Runtime;
using wuxingogo;
using wuxingogo.tools;
public class XBezierArrowToRolling : XMonoBehaviour {
	[X]
	public void Rolling(XBezierArrow sourceArrow)
	{
		Destroy (sourceArrow.GetComponent<MeshFilter> ());
		Destroy (sourceArrow.GetComponent<MeshRenderer> ());

		var destArrow = gameObject.AddComponent<XBezierArrow2> ();
		destArrow.step = sourceArrow.step;
		destArrow.yWidthCurve = sourceArrow.yWidthCurve;
		destArrow.headWid = sourceArrow.headWid;
		destArrow.p0 = sourceArrow.p0;
		destArrow.p1 = sourceArrow.p1;
		destArrow.p2 = sourceArrow.p2;
		destArrow.p3 = sourceArrow.p3;

		destArrow.stepCount = sourceArrow.stepCount;
		destArrow.UVCount = 40;
		Gradient g = new Gradient ();
		g.colorKeys = new GradientColorKey[]{
			new GradientColorKey(Color.white, 0),
			new GradientColorKey(Color.white, 1),

		};
		g.alphaKeys = new GradientAlphaKey[]{
			new GradientAlphaKey(1, 0),
			new GradientAlphaKey(1, 0.8f),
			new GradientAlphaKey(0, 1)
		};
		destArrow.gradient = g;
		destArrow.UVOffset = new Vector2 (0.01f, 0);
		destArrow.mesh1 = CreateMesh ("Mesh1", "Strip");
		destArrow.mesh2 = CreateMesh ("Mesh2", "StripLoop");

		destArrow.UpdateUVRenderer = destArrow.mesh2.GetComponent<MeshRenderer> ();
	}

	MeshFilter CreateMesh(string name, string materialName){
		GameObject g = new GameObject (name);
		g.transform.SetParent (transform);
		GameObjectUtilities.AlignTransform (g.transform, transform);

		var renderer = g.AddComponent<MeshRenderer> ();
		var mat = Resources.Load<Material>(materialName);
		renderer.material = mat;
		return g.AddComponent<MeshFilter> ();
	}
}
