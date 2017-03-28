Shader "Custom/Strip" {
    Properties {
       _Color("Color & Transparency", Color) = (0, 0, 0, 0.5)
       _Texture("Texture",2D) = "white"{}
    }
    SubShader {
       Lighting Off
       ZWrite On
//       Cull Off
       Blend SrcAlpha OneMinusSrcAlpha
       Tags {"Queue"="Transparent+100"   "IgnoreProjector"="True"  "RenderType"="Transparent"}
       LOD 200
       Color[_Color]
       
       Pass {
       		SetTexture [_Texture] {
                Combine Primary * Texture
            }
       }
    }
    
    FallBack "Unlit/Transparent"
}