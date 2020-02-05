// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Mobile/Particles/Additive Distortion" {
	Properties{
		_MainTex("Main Texture", 2D) = "FFFFFF4B" {}
		_NoiseTex("Noise Texture", 2D) = "FFFFFF4B" {}
		[HDR]_Color("Color", Color) = (1.0, 1.0, 0.0, 1.0)
		_IntensityAndScrolling("Intensity (XY), Scrolling (ZW)", Vector) = (0.1,0.1,0.1,0.1)
		_EmissiveIntensity("Emissive Intensity", float) = 1.0
	}
		SubShader{
			Tags {
				"IgnoreProjector" = "True"
				"Queue" = "Transparent"
				"RenderType" = "Transparent"
			}

			Pass {
				Blend One One //1* (generated color) + 1 * (color already present on screen)
				ZWrite Off //drawing semitransparent effects

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				
				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;
				uniform sampler2D _NoiseTex;
				uniform sampler2D _DwistTex;
				uniform float _EmissiveIntensity;
				uniform float4 _Color;
				uniform float4 _MainTex_ST; // Needed for TRANSFORM_TEX(v.texcoord0, _MainTex)
				uniform float4 _NoiseTex_ST; // Needed for TRANSFORM_TEX(v.texcoord1, _NoiseTex)
				uniform float4 _IntensityAndScrolling; // Intensité X/Y et Scrolling sur Z/W

				struct VertexInput {
					float4 vertex : POSITION;
					float2 texcoord0 : TEXCOORD0;
					float2 texcoord1 : TEXCOORD1;
					float4 vertexColor : COLOR;
				};

				struct VertexOutput {
					float4 pos : SV_POSITION;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
					float4 vertexColor : COLOR;
				};

				VertexOutput vert(VertexInput v) {
					VertexOutput o;
					/*
						Transforms 2D UV by scale/bias property
						#define TRANSFORM_TEX(tex,name) (tex.xy * name##_ST.xy + name##_ST.zw)
						It scales and offsets texture coordinates. XY values controls the texture tiling and ZW the offset.
					*/
					o.uv0 = TRANSFORM_TEX(v.texcoord0, _MainTex);
					o.uv1 = TRANSFORM_TEX(v.texcoord1, _NoiseTex);
					o.uv1 += _Time.yy * _IntensityAndScrolling.zw; // Fait évoluer la texture/shader dans le temps 
					o.vertexColor = v.vertexColor;
					o.pos = UnityObjectToClipPos(v.vertex);

					return o;
				}

				float4 frag(VertexOutput i) : COLOR {
					float4 noiseTex = tex2D(_NoiseTex, i.uv1); // Créer une texture de noise avec celle donnée en paramêtre
					float2 offset = (noiseTex.rg * 2 - 1) * _IntensityAndScrolling.rg;
					float2 uvNoise = i.uv0 + offset;
					float4 mainTex = tex2D(_MainTex, uvNoise); // Créer la main texture avec celle donnée en paramêtre
					float3 emissive = (mainTex.rgb * i.vertexColor.rgb) * (mainTex.a * i.vertexColor.a) * _Color * _EmissiveIntensity; // Ajoute une lumière émissive avec la main texture, la couleur du vertex et une couleur donnée en paramêtre

					return fixed4(emissive, 1); // Retourne la couleur émise 
				}
				ENDCG
			}
		}
			FallBack "Mobile/Particles/Additive"
}