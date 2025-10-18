Shader "Hidden/TerrainEngine/BillboardTree" {
    Properties {
        _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
    }

    SubShader {
        Tags { "Queue" = "Transparent-100" "IgnoreProjector"="True" "RenderType"="TreeBillboard" }

        Pass {
            ColorMask rgb
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
//            #pragma multi_compile_fog
            #include "../GlobalSnowDeferredOptions.cginc"
            #include "UnityCG.cginc"
            #include "TerrainEngine.cginc"
            #include "SnowedTreeBillboardLibrary.cginc"

            v2f vert (appdata_tree_billboard v) {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                TerrainBillboardTree(v.vertex, v.texcoord1.xy, v.texcoord.y);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv.x = v.texcoord.x;
                o.uv.y = v.texcoord.y > 0;
                o.color = v.color;
//                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            sampler2D _MainTex;
            fixed4 frag(v2f input) : SV_Target
            {
                fixed4 col = tex2D( _MainTex, input.uv);
                col.rgb *= input.color.rgb;
                clip(col.a);
//                UNITY_APPLY_FOG(input.fogCoord, col);
				SetTreeBillboardCoverage(input, col);
                return col;
            }
            ENDCG
        }
    }

    Fallback Off
}
