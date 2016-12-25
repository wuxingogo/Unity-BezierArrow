using UnityEngine;
using wuxingogo.Runtime;
using wuxingogo.Reflection;
using System.Collections.Generic;



public class XBezierArrow : XMonoBehaviour
{

	public Mesh mesh = null;

	public List<Vector3> Vertices = new List<Vector3>();
	public List<Vector2> UV = new List<Vector2>();
	public List<int> Triangles = new List<int>();

	public float step = 1;

	public AnimationCurve yWidthCurve;
	[Header(" Right Top")]
	public Vector2 headRTTexCoord = Vector2.one;
	[Header(" Right Down")]
	public Vector2 headRDTexCoord = Vector2.zero;

	public float headWid = 0.05f;

	public int stepCount = 100;
	public bool isNeedUpdate = true;
	void Update()
	{
		if( isNeedUpdate ) {
			Refresh();
		}

	}

	public void Refresh()
	{
		InitMesh();
		MeshSetup();
	}
	[X]
	void InitMesh()
	{
		Vertices.Clear();
		Triangles.Clear();
		UV.Clear();

		float y = step * yWidthCurve.Evaluate( 0.02f ) / 2;
		float y2 = step * yWidthCurve.Evaluate( 0.01f ) / 2;
		Vector3 lastLT = Vector3.zero;
		Vector3 lastLD = Vector3.zero;
		Vector3 leftTop;
		Vector3 leftDown;
		Vector3 rightDown;
		Vector3 rightTop;

		var p = GetBezierCurve( position0, position1, position2, position3, -headWid );
		var np = GetBezierCurve( position0, position1, position2, position3, -0f );
//			var y = step * yWidthCurve.Evaluate( 0 ) / 2;
		leftTop = new Vector3(-step,step * y* 0.5f) + p;
		leftDown = new Vector3(-step,-y* 0.5f) + p;
		Vertices.Add( leftTop);
		Vertices.Add( leftDown);
		UV.Add(new Vector2(0,0));
		UV.Add(new Vector2(0,1));
		rightTop = new Vector3(step,step * y2 * 0.5f) + np;
		rightDown = new Vector3(step,-y2* 0.5f) + np;
		Vertices.Add( rightTop );
		Vertices.Add( rightDown );
		UV.Add(headRTTexCoord);
		UV.Add(headRDTexCoord);
		int currIndex = 0;
		Triangles.AddRange( new List<int>{currIndex , currIndex + 2, currIndex + 3});
		Triangles.AddRange( new List<int>{currIndex , currIndex + 3, currIndex + 1});


		for( int i = 0; i < stepCount; i++ ) {
			float t = i * 1.0f / stepCount;
			float nextT = ( i + 1 ) * 1.0f / stepCount;

			float leftUV = headRDTexCoord.x + t * ( 1 - headRDTexCoord.x );
			float rightUV = headRDTexCoord.x + nextT * ( 1 - headRDTexCoord.x );

			p = GetBezierCurve( position0, position1, position2, position3, t );



			y = step * yWidthCurve.Evaluate( t ) / 2;

			if( lastLT != Vector3.zero ) {
				leftTop = lastLT;
			}else{
				leftTop = new Vector3(step,step * y* 0.5f) + p;
				Vertices.Add( leftTop );
				UV.Add(new Vector2(leftUV,1));

			}

			if( lastLD != Vector3.zero ) {
				leftDown = lastLD;
			} else {
				leftDown = new Vector3(step,-y* 0.5f) + p;
				Vertices.Add( leftDown );
				UV.Add(new Vector2(leftUV,0));
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

			Vertices.Add( rightDown );
			UV.Add(new Vector2(rightUV,0));
			
			lastLT = new Vector3(rightTop.x, rightTop.y * 0.5f, rightTop.z);
			lastLD = new Vector3(rightDown.x, rightDown.y * 0.5f, rightDown.z);

			currIndex = i * 2 + 4;
			Triangles.AddRange( new List<int>{currIndex , currIndex + 2, currIndex + 3});
			Triangles.AddRange( new List<int>{currIndex , currIndex + 3, currIndex + 1});

		}
	}
	public Transform p0;
	public Transform p1;
	public Transform p2;
	public Transform p3;

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
	[X]
	void MeshSetup()
	{
		var meshFilter = gameObject.GetComponent<MeshFilter>();
		if( meshFilter.sharedMesh != null )
			DestroyImmediate( meshFilter.sharedMesh, true );
		mesh = new Mesh();
		mesh.name = name;
		meshFilter.sharedMesh = mesh;
		mesh.vertices = Vertices.ToArray();
		mesh.triangles = Triangles.ToArray();
		mesh.uv = UV.ToArray();

		var type = XReflectionUtils.GetPrefabType( gameObject );
		if( type == "Prefab" )
			XReflectionUtils.AddObjectToObject( mesh, gameObject );
	}
}