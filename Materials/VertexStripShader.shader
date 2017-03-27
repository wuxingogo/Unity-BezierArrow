Shader "Unlit/VertexStripShader"
{
	Properties
	{
		_Color("Color & Transparency", Color) = (0, 0, 0, 0.5)
		_Texture("Texture",2D) = "white"{}
	}
	SubShader
	{
		Tags { "Queue"="Transparent+100"   "IgnoreProjector"="True"  "RenderType"="Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			sampler2D _Texture;
			float4 _Texture_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _Texture);
				o.color = v.color;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_Texture, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col * i.color;
			}
			ENDCG
		}
	}
}
