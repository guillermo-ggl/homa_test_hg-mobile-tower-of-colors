// Based on Legacy/Diffuse but instead of multiplying the color of the texture,
// I use lerp to show the texture as some kind of decal.
Shader "Custom/Diffuse-Decal" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200

CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
fixed4 _Color;

struct Input {
    float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	c = lerp(_Color, c, c.a);
    o.Albedo = c.rgb;
}
ENDCG
}

Fallback "Legacy Shaders/Diffuse"
}
