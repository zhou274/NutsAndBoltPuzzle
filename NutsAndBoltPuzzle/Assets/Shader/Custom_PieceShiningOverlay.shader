Shader "Custom/PieceShiningOverlay" {
	Properties {
		[PerRendererData] _ShineColor ("Shine Color", Vector) = (1,1,1,1)
		[PerRendererData] _ShinePower ("Shine Power", Range(0, 1)) = 0
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