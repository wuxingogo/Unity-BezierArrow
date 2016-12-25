using UnityEngine;
using System.Collections;
using UnityEditor;
using wuxingogo.Editor;
using wuxingogo.Runtime;


[CustomEditor(typeof(XBezierArrow))]
public class XBezierArrowEditor : XMonoBehaviourEditor {

	public override void OnXGUI ()
	{
		base.OnXGUI ();
		DoButton ("Save Template", () => {
			SaveTemplate(target as XBezierArrow); 
		});
	}

	private void SaveTemplate(XBezierArrow script)
	{
		XBezierArrowTemplate template = XScriptableObject.Create<XBezierArrowTemplate>(null);
		template.Save (script);
		template.SaveInEditor ();
//		string path = AssetsUtilites.SaveFilePanel( "XBezierArrow Save Template", XEditorSetting.ProjectPath, script.gameObject.name + ".asset", "asset", true );
//		if( path == "" )
//			return;
//
//		path = FileUtil.GetProjectRelativePath( path ); 
//
//		XBezierArrowTemplate template = XScriptableObject.Create<XBezierArrowTemplate>();
//		template.Save (script);
//		AssetDatabase.CreateAsset( template, path );
//		AssetDatabase.SaveAssets();
	}
}
