﻿Shader "Playtime Painter/Editor/Brush/AdditiveUV_Alpha" {

	Category{
		Tags{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Blend One Zero, SrcAlpha One 


		ColorMask RGBA
		Cull off
		ZTest off
		ZWrite off

		SubShader{
			Pass{

				CGPROGRAM

				#include "PlaytimePainter_cg.cginc"

				#pragma multi_compile BRUSH_3D BRUSH_3D_TEXCOORD2

				#pragma multi_compile ___  _qcPp_USE_DEPTH_FOR_PROJECTOR

				#pragma vertex vert
				#pragma fragment frag

				struct v2f {
					float4 pos : POSITION;
					float4 worldPos : TEXCOORD0;
					float4 shadowCoords : TEXCOORD1;
					float2 srcTexAspect : TEXCOORD2;
				};

				v2f vert(appdata_brush_qc v) {

					v2f o;
					float4 worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0f));

					o.worldPos = worldPos;

					o.shadowCoords = mul(pp_ProjectorMatrix, worldPos);

					#if BRUSH_3D_TEXCOORD2
						v.texcoord.xy = v.texcoord1.xy;
					#endif

					float2 suv = _qcPp_SourceTexture_TexelSize.zw;
					o.srcTexAspect = max(1, float2(suv.y / suv.x, suv.x / suv.y));

					float atY = floor(v.texcoord.z / _qcPp_brushAtlasSectionAndRows.z);
					float atX = v.texcoord.z - atY * _qcPp_brushAtlasSectionAndRows.z;
					v.texcoord.xy = (float2(atX, atY) + v.texcoord.xy) / _qcPp_brushAtlasSectionAndRows.z
						* _qcPp_brushAtlasSectionAndRows.w + v.texcoord.xy * (1 - _qcPp_brushAtlasSectionAndRows.w);

					worldPos.xyz = _qcPp_RTcamPosition.xyz;
					worldPos.z += 100;
					worldPos.xy += (v.texcoord.xy*_qcPp_brushEditedUVoffset.xy + _qcPp_brushEditedUVoffset.zw - 0.5) * 256;

					v.vertex = mul(unity_WorldToObject, float4(worldPos.xyz, v.vertex.w));

					o.pos = UnityObjectToClipPos(v.vertex);

					return o;
				}


				float4 frag(v2f o) : COLOR {

					o.shadowCoords.xy /= o.shadowCoords.w;

					float alpha = prepareAlphaSphere(o.shadowCoords.xy, o.worldPos.xyz);

					alpha *= ProjectorSquareAlpha(o.shadowCoords);

					float2 pUv =  (o.shadowCoords.xy + 1) * 0.5;

					#if _qcPp_USE_DEPTH_FOR_PROJECTOR
					alpha *= ProjectorDepthDifference(o.shadowCoords, o.worldPos, pUv);
					#endif

					pUv *= o.srcTexAspect;

					alpha *= BrushClamp(pUv);

					clip(alpha-0.001);

					return float4(pUv, 0, alpha);

				}
				ENDCG
			}
		}
	}
}