    uniform float4    _GS_SnowData1;	// x = relief, y = occlusion, z = glitter, w = brightness
	uniform float4    _GS_SnowData2;	// x = minimum altitude, y = altitude scattering, z = coverage extension
	uniform float4    _GS_SnowData3;	// x = Sun occlusion, y = sun atten, z = ground coverage, w = grass coverage
    uniform float4    _GS_SnowCamPos;
    uniform sampler2D_float _GS_DepthTexture;

    uniform sampler2D _GS_DepthMask;
    uniform float4 _GS_DepthMaskWorldSize;

struct Input {
	float2 uv_MainTex;
	fixed4 color : COLOR;
	float3 worldPos;
};


	// get snow coverage on grass
	void SetGrassCoverage(Input IN, inout SurfaceOutput o) { 
		// prevent snow on sides and below minimum altitude
		float minAltitude = saturate( IN.worldPos.y - _GS_SnowData2.x);
		float snowCover   = minAltitude * saturate(IN.uv_MainTex.y + _GS_SnowData3.w);

        // mask support
        #if defined(GLOBALSNOW_MASK)
            #if defined(USE_ZENITHAL_DEPTH)
                float4 st = float4(IN.worldPos.xz - _GS_SnowCamPos.xz, 0, 0);
                st *= _GS_SnowData2.z;
                st += 0.5;
                float zmask = tex2Dlod(_GS_DepthTexture, st).g;
                snowCover   = max(snowCover - zmask, 0);
            #else
                float2 maskUV = (IN.worldPos.xz - _GS_DepthMaskWorldSize.yw) / _GS_DepthMaskWorldSize.xz + 0.5.xx;
                snowCover = tex2D(_GS_DepthMask, maskUV).a;
                if (maskUV.x<=0.01 || maskUV.x>=0.99 || maskUV.y<=0.01 || maskUV.y>=0.99) snowCover = 1.0;
            #endif
        #endif

		// pass color data to output shader
		o.Albedo = lerp(o.Albedo, _GS_SnowData1.www, snowCover * 0.96);
	}
	
	
	
