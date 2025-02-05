Shader "TetraArts/Tatoon2/Built-in/Tatoon2_Built-in" {
	Properties {
		_ShadowSize ("ShadowSize", Range(0, 2)) = 0
		_ShadowBlend ("ShadowBlend", Range(0, 1)) = 0
		_DiffuseColor ("Diffuse Color", Vector) = (1,0.75,0.75,0)
		[Toggle] _UseRim ("UseRim", Float) = 1
		[Toggle] _UseNormalMap ("UseNormalMap", Float) = 0
		_RimColor ("Rim Color", Vector) = (0,1,0.8758622,0)
		_MainTexture ("MainTexture", 2D) = "white" {}
		[NoScaleOffset] _RimTexture ("Rim Texture", 2D) = "white" {}
		[Toggle] _RimTextureViewProjection ("Rim Texture View Projection", Float) = 0
		_NormalStrength ("Normal Strength", Range(0, 1)) = 1
		_RimTextureTiling ("Rim Texture Tiling", Float) = 0
		_ShadowColor ("Shadow Color", Vector) = (1,0,0,1)
		_RimTextureRotation ("Rim Texture Rotation", Float) = 0
		[Toggle] _RimLightColor ("Rim Light Color", Float) = 0
		_RimLightIntensity ("Rim Light Intensity", Range(0, 10)) = 0
		[NoScaleOffset] _ShadowTexture ("Shadow Texture", 2D) = "white" {}
		_RimSize ("Rim Size", Range(-1, 2)) = 0.4104842
		_RimBlend ("Rim Blend", Range(0, 10)) = 0
		[Toggle] _ShadowTextureViewProjection ("Shadow Texture View Projection", Float) = 0
		[Toggle] _UseSpecular ("UseSpecular", Float) = 1
		_ShadowTextureTiling ("Shadow Texture Tiling", Float) = 0
		[Toggle] _UseGradient ("Use Gradient", Float) = 0
		[NoScaleOffset] _SpecularMap ("Specular Map", 2D) = "gray" {}
		_ShadowTextureRotation ("Shadow Texture Rotation", Float) = 0
		[Toggle] _SpecularTextureViewProjection ("Specular Texture View Projection", Float) = 0
		_ColorB ("Color B", Vector) = (0,0.1264467,1,0)
		_ColorA ("Color A", Vector) = (1,0,0,0)
		[Toggle] _UseShadow ("UseShadow", Float) = 0
		_SpecularTextureTiling ("Specular Texture Tiling", Float) = 0
		_SpecularTextureRotation ("Specular Texture Rotation", Float) = 0
		_GradientSize ("Gradient Size", Range(0, 10)) = 0
		_SpecularColor ("Specular Color", Vector) = (1,0.9575656,0.75,0)
		[Toggle] _SpecLightColor ("Spec Light Color", Float) = 0
		_GradientPosition ("Gradient Position", Float) = 0
		_SpecularLightIntensity ("Specular Light Intensity", Range(0, 10)) = 1
		_GradientRotation ("Gradient Rotation", Float) = 0
		_SpecularSize ("Specular Size", Range(0, 1)) = 0.005
		_SpecularBlend ("Specular Blend", Range(0, 1)) = 0
		_NormalMap ("NormalMap", 2D) = "bump" {}
		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
		[Toggle] _Level2 ("Level2", Float) = 0
		[Toggle] _Level3 ("Level3", Float) = 0
		[Toggle] _Animate ("Animate", Float) = 0
		[Toggle] _UseShadowTexture ("UseShadowTexture", Float) = 0
		_XDirectionSpeed ("XDirectionSpeed", Float) = 0
		_YDirectionSpeed ("YDirectionSpeed", Float) = 0
		[Toggle] _ChangeAxis ("ChangeAxis", Float) = 0
		_EmissiveMap ("EmissiveMap", 2D) = "white" {}
		[Toggle] _UseEmissive ("UseEmissive", Float) = 0
		[HDR] _EmissiveColor ("EmissiveColor", Vector) = (2.996078,0.0611825,0,0)
		_TextureRamp ("_TextureRamp", 2D) = "white" {}
		[Toggle] _UseRamp ("UseRamp", Float) = 0
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] __dirty ("", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "TatoonEditor"
}