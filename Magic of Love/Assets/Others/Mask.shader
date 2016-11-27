Shader "ImageEffect/MaskIcon"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Mask("Base (RGB)", 2D) = "white" {}

		_Scale("Scale", float) = 1
		_Color("Tint", Color) = (1,1,1,1)
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Stencil
	{
		Ref[_Stencil]
		Comp[_StencilComp]
		Pass[_StencilOp]
		ReadMask[_StencilReadMask]
		WriteMask[_StencilWriteMask]
	}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"
#include "UnityUI.cginc"

#pragma multi_compile __ UNITY_UI_ALPHACLIP

	struct a2v
	{
		fixed2 uv : TEXCOORD0;
		half4 vertex : POSITION;
		float4 color    : COLOR;
	};

	fixed4 _Color;

	struct v2f
	{
		fixed2 uv : TEXCOORD0;
		half4 vertex : SV_POSITION;
		float4 color    : COLOR;
	};

	sampler2D _MainTex;
	sampler2D _Mask;
	float _Scale;

	v2f vert(a2v i)
	{
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, i.vertex);
		o.uv = i.uv;

		o.color = i.color * _Color;
		return o;
	}

	fixed4 frag(v2f i) : COLOR
	{
		float delta = 0.5 * _Scale - 0.5;
		half4 color = tex2D(_MainTex, i.uv) * i.color;
		half4 mask = tex2D(_Mask, i.uv * _Scale - fixed2(delta, delta));
		
		color.a *= mask.a;
		return color;
	}
		ENDCG
	}
	}
}