Shader "LowPoly/SimpleWater"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 16
		_WaterNormal("Water Normal", 2D) = "bump" {}
		_NormalScale("Normal Scale", Float) = 0
		_DeepColor("Deep Color", Color) = (0,0,0,0)
		_ShalowColor("Shalow Color", Color) = (1,1,1,0)
		_WaterDepth("Water Depth", Float) = 0
		_WaterFalloff("Water Falloff", Float) = 0
		_WaterSpecular("Water Specular", Float) = 0
		_WaterSmoothness("Water Smoothness", Float) = 0
		//_Distortion("Distortion", Float) = 0.5
		_Foam("Foam", 2D) = "white" {}
		_FoamDepth("Foam Depth", Float) = 0
		//_FoamFalloff("Foam Falloff", Float) = 0
		_FoamSpecular("Foam Specular", Float) = 0
		_FoamSmoothness("Foam Smoothness", Float) = 0
		_WavesAmplitude("WavesAmplitude", Float) = 0.1
		_WavesAmount("WavesAmount", Float) = 8.87
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" }
		Cull Back
		GrabPass{ } //utilise la texture derrière l'objet en seconde texture
		CGPROGRAM
		#include "UnityStandardUtils.cginc" //enable blendNormals
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 4.6
		#pragma surface surf StandardSpecular keepalpha vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float _NormalScale;
		uniform sampler2D _WaterNormal;
		uniform float4 _WaterNormal_ST;
		uniform float4 _DeepColor;
		uniform float4 _ShalowColor;
		uniform sampler2D _CameraDepthTexture;
		uniform float _WaterDepth;
		uniform float _WaterFalloff;
		uniform float _FoamDepth;
		uniform float _FoamFalloff;
		uniform sampler2D _Foam;
		uniform float4 _Foam_ST;
		uniform sampler2D _GrabTexture;
		uniform float _Distortion;
		uniform float _WaterSpecular;
		uniform float _FoamSpecular;
		uniform float _WaterSmoothness;
		uniform float _FoamSmoothness;
		uniform float _WavesAmount;
		uniform float _WavesAmplitude;
		uniform float _TessValue;

		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v ) //données vertex : pos, tan, norm, tex_coords, color
		{
			float3 vertex3Pos = v.vertex.xyz;
			float3 vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( sin( ( ( _WavesAmount * vertex3Pos.z ) + _Time.y ) ) * vertexNormal ) * _WavesAmplitude );
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o ) //i renvoie au struct: tex_coords et screenPos
		{
			float2 uv_WaterNormal = i.uv_texcoord * _WaterNormal_ST.xy + _WaterNormal_ST.zw;
			float2 uv_WaterBase = ( uv_WaterNormal + 1.0 * _Time.y * float2( -0.03,0 )); //base normal map
			float2 uv_WaterSec = ( uv_WaterNormal + 1.0 * _Time.y * float2( 0.04,0.04 )); //secondary normal map
			float3 blendNormals = BlendNormals( UnpackScaleNormal( tex2D( _WaterNormal, uv_WaterBase) ,_NormalScale ) , UnpackScaleNormal( tex2D( _WaterNormal, uv_WaterSec) ,_NormalScale ) );
			o.Normal = blendNormals;

			float4 screenPos = i.screenPos.xyzw;
			float eyeDepth = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(screenPos))));
			float depthTex = abs(( eyeDepth - screenPos.w )); //effet de profondeur de la texture en fonction de la pos
			float depthWaterCoeff = saturate( pow((depthTex + _WaterDepth ) , _WaterFalloff ));
			float4 texWaterColor = lerp( _DeepColor , _ShalowColor , depthWaterCoeff);
			float2 uv_FoamNormal = i.uv_texcoord * _Foam_ST.xy + _Foam_ST.zw;
			float2 uv_Foam = ( uv_FoamNormal + 1.0 * _Time.y * float2( -0.01,0.01 ));
			float depthFoamCoeff = ( saturate( pow( (depthTex + _FoamDepth ) , _FoamFalloff ) ) * tex2D( _Foam, uv_Foam ).r );
			float4 texFoamColor = lerp(texWaterColor, float4(1,1,1,0) , depthFoamCoeff);
			float4 screen_Pos = screenPos;

			#if UNITY_UV_STARTS_AT_TOP // true pour les directx platform, contre l'image flipping
			float uv_norm = -1.0;
			#else
			float uv_norm = 1.0;
			#endif

			float halfScreen = screen_Pos.w * 0.5;
			screen_Pos.y = (screen_Pos.y - halfScreen) * _ProjectionParams.x* uv_norm + halfScreen;
			screen_Pos.xyzw /= screen_Pos.w;

			float4 texEffect = tex2D( _GrabTexture, ( float3( (screen_Pos).xy ,  0.0 ) + (blendNormals * _Distortion ) ).xy );
			float4 texAlbedo = lerp( texFoamColor , texEffect, depthWaterCoeff);
			o.Albedo = texAlbedo.rgb;

			float texSpecular = lerp( _WaterSpecular , _FoamSpecular , depthFoamCoeff);
			float3 texSpecular_x = (texSpecular).xxx;
			o.Specular = texSpecular_x;

			float texSmoothness = lerp( _WaterSmoothness , _FoamSmoothness , depthFoamCoeff);
			o.Smoothness = texSmoothness;
			o.Alpha = 1;
		}
		ENDCG
	}
	Fallback "Diffuse"
}