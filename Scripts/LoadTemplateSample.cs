using UnityEngine;
using System.Collections;
using wuxingogo.Runtime;

public class LoadTemplateSample : XMonoBehaviour {

	public XBezierArrowTemplate template = null;
	void Start()
	{
		var meshFilter = gameObject.AddComponent<MeshFilter> ();
		var bezierArrow = gameObject.AddComponent<XBezierArrow> ();
		template.CreateFromTemplate (bezierArrow);


	}


}
