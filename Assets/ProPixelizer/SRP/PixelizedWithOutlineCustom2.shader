Shader "ProPixelizer/SRP/PixelizedWithOutlineCustom2"
{

    Properties
    {
        [NoScaleOffset]_LightingRamp("LightingRamp", 2D) = "white" {}
        [NoScaleOffset]_PaletteLUT("PaletteLUT", 2D) = "white" {}
        [NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
        _Albedo_ST("Albedo_ST", Vector) = (1, 1, 0, 0)
        _BaseColor("Color", Color) = (1, 1, 1, 1)
        _AmbientLight("AmbientLight", Color) = (0.1, 0.1, 0.1, 0.5019608)
        _PixelSize("PixelSize", Range(1, 5)) = 1
        _PixelGridOrigin("PixelGridOrigin", Vector) = (0, 0, 0, 0)
        [Normal][NoScaleOffset]_NormalMap("Normal Map", 2D) = "bump" {}
        _NormalMap_ST("Normal Map_ST", Vector) = (1, 1, 0, 0)
        [ToggleUI]_Use2DTexture("Use2DTexture", Float) = 0
        [NoScaleOffset]_Emission("Emission", 2D) = "black" {}
        _Emission_ST("Emission_ST", Vector) = (1, 1, 0, 0)
        _AlphaClipThreshold("Alpha Clip Threshold", Float) = 0.5
        _ID("ID", Float) = 1
        _OutlineColor("OutlineColor", Color) = (1, 1, 1, 0.5019608)
        _EdgeHighlightColor("Edge Highlight Color", Color) = (0.5, 0.5, 0.5, 0.5058824)
        [HDR]_EmissionColor("EmissionColor", Color) = (1, 1, 1, 0)
        _DiffuseVertexColorWeight("DiffuseVertexColorWeight", Float) = 1
        _EmissiveVertexColorWeight("EmissiveVertexColorWeight", Float) = 0
        [Toggle]COLOR_GRADING("Use Color Grading", Float) = 1
        [Toggle]USE_OBJECT_POSITION("Use Object Position", Float) = 1
        [Toggle]RECEIVE_SHADOWS("Receive Shadows", Float) = 1
        [Toggle]PROPIXELIZER_DITHERING("Use Dithering", Float) = 0
        _Tiling("Tiling", Vector) = (1, 1, 0, 0)
        _ScrollSpeed("ScrollSpeed", Vector) = (0, 0, 0, 0)
        _ColdLavaColor("ColdLavaColor", Color) = (1, 0, 0.2055364, 0)
        _HotLavaColor("HotLavaColor", Color) = (0.2208682, 1, 0, 0)
        _Noise_Power("Noise Power", Int) = 9
        _Noise_Scale_2("Noise Scale 2", Float) = 37.06
        _Noise_Scale_1("Noise Scale 1", Float) = 6.23
        _PixelAmount("PixelAmount", Int) = 50
        [ToggleUI]_UseMyPixelisation("UseMyPixelisation", Float) = 0
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

			UsePass "ProPixelizer/Hidden/ProPixelizerBaseCustom2/UNIVERSAL FORWARD"
			UsePass "ProPixelizer/Hidden/ProPixelizerBaseCustom2/SHADOWCASTER"
			UsePass "ProPixelizer/Hidden/ProPixelizerBaseCustom2/DEPTHONLY"
			UsePass "ProPixelizer/Hidden/ProPixelizerBaseCustom2/DEPTHNORMALS"

		Pass
		{
			Name "ProPixelizerPass"
			Tags {
				"RenderPipeline" = "UniversalRenderPipeline"
				"LightMode" = "ProPixelizer"
				"DisableBatching" = "True"
			}

			ZWrite On
			Cull Off
			Blend Off

			HLSLPROGRAM
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "PixelUtils.hlsl"
			#include "PackingUtils.hlsl"
			#include "ScreenUtils.hlsl" 
			#pragma vertex outline_vert
			#pragma fragment outline_frag
			#pragma target 2.5
			#pragma multi_compile_local USE_OBJECT_POSITION_ON _
			#pragma multi_compile USE_ALPHA_ON _
			#pragma multi_compile NORMAL_EDGE_DETECTION_ON _
			#pragma multi_compile_local PROPIXELIZER_DITHERING_ON _
			
			// If you want to use the SRP Batcher:
			// The CBUFFER has to match that generated from ShaderGraph - otherwise all hell breaks loose.
			// In some cases, it might be easier to just break SRP Batching support for your outline shader.
			// Graph Properties
			CBUFFER_START(UnityPerMaterial)
 		float _Noise_Scale_1;
        float _Noise_Scale_2;
        float _Noise_Power;
        float4 _HotLavaColor;
        float4 _ColdLavaColor;
        float2 _ScrollSpeed;
        float2 _Tiling;
        float4 _LightingRamp_TexelSize;
        float4 _PaletteLUT_TexelSize;
        float4 _Albedo_TexelSize;
        float4 _Albedo_ST;
        float4 _BaseColor;
        float4 _AmbientLight;
        float _PixelSize;
        float4 _PixelGridOrigin;
        float4 _NormalMap_TexelSize;
        float4 _NormalMap_ST;
        float4 _Emission_TexelSize;
        float4 _Emission_ST;
        float _AlphaClipThreshold;
        float _ID;
        float4 _OutlineColor;
        float4 _EdgeHighlightColor;
        float4 _EmissionColor;
        float _DiffuseVertexColorWeight;
        float _EmissiveVertexColorWeight;
        float _PixelAmount;
        float _UseMyPixelisation;
        float _Use2DTexture;
			CBUFFER_END
			
			// Object and Global properties
			SAMPLER(SamplerState_Linear_Repeat);
			SAMPLER(SamplerState_Point_Clamp);
			SAMPLER(SamplerState_Point_Repeat);
			TEXTURE2D(_LightingRamp);
			SAMPLER(sampler_LightingRamp);
			TEXTURE2D(_PaletteLUT);
			SAMPLER(sampler_PaletteLUT);
			TEXTURE2D(_Albedo);
			SAMPLER(sampler_Albedo);
			TEXTURE2D(_NormalMap);
			SAMPLER(sampler_NormalMap);
			TEXTURE2D(_Emission);
			SAMPLER(sampler_Emission);

			#include "OutlinePass.hlsl"
			ENDHLSL
		}
     }
	//CustomEditor "PixelizedWithOutlineShaderGUI"
	FallBack "ProPixelizer/Hidden/ProPixelizerBaseCustom2"
}
