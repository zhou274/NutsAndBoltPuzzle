Shader "Hidden/Toony Colors Pro 2/Variants/Mobile Specular Matcap Rim" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_HColor ("Highlight Color", Vector) = (0.785,0.785,0.785,1)
		_SColor ("Shadow Color", Vector) = (0.195,0.195,0.195,1)
		_MainTex ("Main Texture (RGB) Spec/MatCap Mask (A) ", 2D) = "white" {}
		[TCP2Gradient] _Ramp ("#RAMPT# Toon Ramp (RGB)", 2D) = "gray" {}
		_RampThreshold ("#RAMPF# Ramp Threshold", Range(0, 1)) = 0.5
		_RampSmooth ("#RAMPF# Ramp Smoothing", Range(0.01, 1)) = 0.1
		_BumpMap ("#NORM# Normal map (RGB)", 2D) = "bump" {}
		_SpecColor ("#SPEC# Specular Color", Vector) = (0.5,0.5,0.5,1)
		_Shininess ("#SPEC# Shininess", Range(0.01, 2)) = 0.1
		_SpecSmooth ("#SPECT# Smoothness", Range(0, 1)) = 0.05
		_RimColor ("#RIM# Rim Color", Vector) = (0.8,0.8,0.8,0.6)
		_RimMin ("#RIM# Rim Min", Range(0, 1)) = 0.5
		_RimMax ("#RIM# Rim Max", Range(0, 1)) = 1
		_RimDir ("#RIMDIR# Rim Direction", Vector) = (0,0,1,0)
		_MatCap ("#MC# MatCap (RGB)", 2D) = "black" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "TCP2_MaterialInspector"
}