Shader "Unlit/Outline Simple" {
	Properties {
		_OColor ("Color", Vector) = (1,1,1,1)
		_OWidth ("Width", Float) = 0
		_ZFlatCorrect ("ZFlatCorrect", Float) = 0
		_ZOffsetCorrect ("ZOffsetCorrect", Float) = 0
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
}