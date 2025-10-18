	
	uniform float4    _GS_SnowData1;	// x = relief, y = occlusion, z = glitter, w = brightness
	uniform float4    _GS_SnowData2;	// x = minimum altitude for vegetation
	uniform float4    _GS_SnowData4;    // w = billboard coverage
    uniform float4    _GS_SnowCamPos;
    uniform sampler2D_float _GS_DepthTexture;

    uniform sampler2D _GS_DepthMask;
    uniform float4 _GS_DepthMaskWorldSize;

	struct v2f {
		float4 pos : SV_POSITION;
        fixed4 color : COLOR0;
		float3 worldPos : TEXCOORD0;
		float2 uv: TEXCOORD1;
        UNITY_VERTEX_OUTPUT_STEREO
	};

	// get snow coverage on tree billboards
	void SetTreeBillboardCoverage(v2f input, inout fixed4 col) { 
		
		float minAltitude = saturate((input.worldPos.y - _GS_SnowData2.x) * 0.1);
		float solid       = col.g * 2.0 - _GS_SnowData4.w;
		float snowCover   = minAltitude * saturate(solid) * 0.95;

        // mask support
        #if defined(GLOBALSNOW_MASK)
            #if defined(USE_ZENITHAL_DEPTH)
                float4 st = float4(input.worldPos.xz - _GS_SnowCamPos.xz, 0, 0);
                st *= _GS_SnowData2.z;
                st += 0.5;
                float zmask = tex2Dlod(_GS_DepthTexture, st).g;
                snowCover   = max(snowCover - zmask, 0);
            #else
                float2 maskUV = (input.worldPos.xz - _GS_DepthMaskWorldSize.yw) / _GS_DepthMaskWorldSize.xz + 0.5.xx;
                snowCover = tex2D(_GS_DepthMask, maskUV).a;
                if (maskUV.x<=0.01 || maskUV.x>=0.99 || maskUV.y<=0.01 || maskUV.y>=0.99) snowCover = 1.0;
            #endif
        #endif

		// pass color data to output shader
		col.rgb = lerp(col.rgb, (1.0 + col.g) * (0.25 + _GS_SnowData1.w) * solid, snowCover);
	}	
