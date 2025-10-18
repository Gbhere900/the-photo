Shader "Hidden/TerrainEngine/Details/WavingDoublePass" {
Properties {
    _WavingTint ("Fade Color", Color) = (.7,.6,.5, 0)
    _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
    _WaveAndDistance ("Wave and distance", Vector) = (12, 3.6, 1, 1)
    _Cutoff ("Cutoff", float) = 0.5
}

SubShader {
    Tags {
        "Queue" = "Geometry+200"
        "IgnoreProjector"="True"
        "RenderType"="Grass"
        "DisableBatching"="True"
    }
    Cull Off
    LOD 200
    ColorMask RGB

CGPROGRAM
#pragma surface surf Lambert vertex:WavingGrassVert exclude_path:deferred // addshadow
#include "../GlobalSnowDeferredOptions.cginc"
#include "TerrainEngine.cginc"
#include "SnowedGrassLibrary.cginc"

sampler2D _MainTex;
fixed _Cutoff;

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
    o.Albedo = c.rgb;
    o.Alpha = c.a;
    clip (o.Alpha - _Cutoff);
    o.Alpha *= IN.color.a;
    SetGrassCoverage(IN, o);
}
ENDCG
}
  
    Fallback Off
}
