//
// XBezierArrow2.cs
//
// Author:
//       wuxingogo <>
//
// Copyright (c) 2017 wuxingogo
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using UnityEngine;
using System.Collections.Generic;
using wuxingogo.Runtime;
using wuxingogo.Reflection;

public class XBezierArrow2 : XMonoBehaviour
{

	public MeshFilter mesh1 = null;
	public MeshFilter mesh2 = null;

	public float step = 1;

	public AnimationCurve yWidthCurve;

	public float headWid = 0.05f;
	public int stepCount = 100;
	public bool isNeedUpdate = true;

	public Transform p0 = null;
	public Transform p1 = null;
	public Transform p2 = null;
	public Transform p3 = null;

	public int UVCount = 4;
	public Gradient gradient = null;

	public MeshRenderer UpdateUVRenderer = null;
	//Cannot be set Intger, because UV's Range is [0-1]
	public Vector2 UVOffset = new Vector2(0.01f, 0);


	public Vector3 position0{
		get{
			if( p0 != null )
				return p0.localPosition;
			return _position0;
		}set{
			_position0 = value;
		}
	}
	private Vector3 _position0 = Vector3.zero;


	public Vector3 position1{
		get{
			if( p1 != null )
				return p1.localPosition;
			return _position1;
		}set{
			_position1 = value;
		}
	}
	private Vector3 _position1;

	public Vector3 position2{
		get{
			if( p2 != null )
				return p2.localPosition;
			return _position2;
		}set{
			_position2 = value;
		}
	}
	private Vector3 _position2;

	public Vector3 position3{
		get{
			if( p3 != null )
				return p3.localPosition;
			return _position3;
		}set{
			_position3 = value;
		}
	}
	private Vector3 _position3;

	[X]
	void InitMesh()
	{
		//-----------------Head-----------------//
		var mesh = new Mesh ();
		mesh1.sharedMesh = mesh;

		List<Vector3> Vertices = new List<Vector3> ();
		List<Vector2> UV = new List<Vector2> ();
		List<int> Triangles = new List<int> ();
		List<Color> Colors = new List<Color> ();

		Vector3 lastLT = Vector3.zero;
		Vector3 lastLD = Vector3.zero;
		Vector3 leftTop;
		Vector3 leftDown;
		Vector3 rightDown;
		Vector3 rightTop;

		float y = step * yWidthCurve.Evaluate( 0.02f ) / 2;
		float y2 = step * yWidthCurve.Evaluate( 0.01f ) / 2;
		var p = GetBezierCurve( position0, position1, position2, position3, -headWid );
		var np = GetBezierCurve( position0, position1, position2, position3, 0f );

		rightTop = new Vector3(step,step * y2 * 0.5f) + np;
		rightDown = new Vector3(step,-y2* 0.5f) + np;
		leftTop = new Vector3(-step,step * y* 0.5f) + p;
		leftDown = new Vector3(-step,-y* 0.5f) + p;

		Vertices.Add (leftTop);
		Vertices.Add (leftDown);
		Vertices.Add (rightTop);
		Vertices.Add (rightDown);

		UV.Add (new Vector2 (0, 1));
		UV.Add (new Vector2 (0, 0));
		UV.Add (new Vector2 (1, 1));
		UV.Add (new Vector2 (1, 0));

		Triangles.AddRange (new List<int>{ 0, 2, 3 });
		Triangles.AddRange (new List<int>{ 0, 3, 1 });


		mesh.vertices = Vertices.ToArray ();
		mesh.uv = UV.ToArray ();
		mesh.triangles = Triangles.ToArray ();
		mesh.name = mesh1.name;


		var type = XReflectionUtils.GetPrefabType( gameObject );
		if( type == "Prefab" )
			XReflectionUtils.AddObjectToObject( mesh, mesh1.gameObject );


		//-----------------Trail-----------------//
		Vertices.Clear ();
		UV.Clear ();
		Triangles.Clear ();
		Colors.Clear ();

		mesh = new Mesh ();
		mesh2.sharedMesh = mesh;


		int currIndex = 0;
		float UVDelta = 1.0f / UVCount;

		for( int i = 0; i < stepCount; i++ ) {
			float t = i * 1.0f / stepCount;
			float nextT = ( i + 1 ) * 1.0f / stepCount;

			int uvIndex = i;
			float leftUV = uvIndex * UVDelta;
			float rightUV = (uvIndex+1) * UVDelta;
			Color leftColor = gradient.Evaluate (t);
			Color rightColor = gradient.Evaluate (nextT);
			p = GetBezierCurve( position0, position1, position2, position3, t );



			y = step * yWidthCurve.Evaluate( t ) / 2;

			if( lastLT != Vector3.zero ) {
				leftTop = lastLT;
			}else{
				leftTop = new Vector3(step,step * y* 0.5f) + p;
				Vertices.Add( leftTop );
				UV.Add(new Vector2(leftUV,1));
				Colors.Add (leftColor);
			}

			if( lastLD != Vector3.zero ) {
				leftDown = lastLD;
			} else {
				leftDown = new Vector3(step,-y* 0.5f) + p;
				Vertices.Add( leftDown );
				UV.Add(new Vector2(leftUV,0));
				Colors.Add (leftColor);
			}

			if( i == 0 ) {
				rightTop = new Vector3(step,step * y* 0.5f) + p;
				rightDown = new Vector3(step,-y* 0.5f) + p;

			} else {
				rightTop = new Vector3(step,step * y * 0.5f) + p;
				rightDown = new Vector3(step,-y* 0.5f) + p;
			}

			Vertices.Add( rightTop );
			UV.Add(new Vector2(rightUV,1));
			Colors.Add (rightColor);

			Vertices.Add( rightDown );
			UV.Add(new Vector2(rightUV,0));
			Colors.Add (rightColor);

			lastLT = new Vector3(rightTop.x, rightTop.y * 0.5f, rightTop.z);
			lastLD = new Vector3(rightDown.x, rightDown.y * 0.5f, rightDown.z);

			currIndex = i * 2;
			Triangles.AddRange( new List<int>{currIndex , currIndex + 2, currIndex + 3});
			Triangles.AddRange( new List<int>{currIndex , currIndex + 3, currIndex + 1});

		}
		mesh.vertices = Vertices.ToArray ();
		mesh.uv = UV.ToArray ();
		mesh.triangles = Triangles.ToArray ();
		mesh.name = mesh2.name;
		mesh.colors = Colors.ToArray();

		type = XReflectionUtils.GetPrefabType( gameObject );
		if( type == "Prefab" )
			XReflectionUtils.AddObjectToObject( mesh, mesh2.gameObject );
	}


	public bool isDrawGizmos = false;
	void OnDrawGizmos()
	{
		if( isDrawGizmos) {
			var m = Gizmos.matrix;
			Gizmos.matrix = transform.localToWorldMatrix;
			for( int i = 0; i < stepCount; i++ ) {
				float t = i * 1.0f / stepCount;
				var p = GetBezierCurve( position0, position1, position2, position3, t );

				Gizmos.DrawCube( p, Vector3.one );
			}
			Gizmos.matrix = m;
		}

	}

	public Vector3 GetBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		Vector3 resultPoint = Vector3.zero;
		resultPoint.x = ( 1 - t ) * ( ( 1 - t ) * ( ( 1 - t ) * p0.x + t * p1.x ) + t * ( ( 1 - t ) * p1.x + t * p2.x ) ) + t * ( ( 1 - t ) * ( ( 1 - t ) * p1.x + t * p2.x ) + t * ( ( 1 - t ) * p2.x + t * p3.x ) );
		resultPoint.y = ( 1 - t ) * ( ( 1 - t ) * ( ( 1 - t ) * p0.y + t * p1.y ) + t * ( ( 1 - t ) * p1.y + t * p2.y ) ) + t * ( ( 1 - t ) * ( ( 1 - t ) * p1.y + t * p2.y ) + t * ( ( 1 - t ) * p2.y + t * p3.y ) );
		return resultPoint;
	}

	void Update()
	{
		if( isNeedUpdate ) {
			Refresh();
		}
		UVUpdate ();
	}

	public void Refresh()
	{
		InitMesh();

	}

	public void UVUpdate()
	{
		if (UpdateUVRenderer != null) {
			var offset = UpdateUVRenderer.material.GetTextureOffset ("_Texture");
			offset += UVOffset;
			UpdateUVRenderer.material.SetTextureOffset("_Texture", offset);
		}
	}


}


