Shader "Game/Sprite Unlit"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		
		_ZWrite ("Depth Write", Float) = 0.0
		_Cutoff ("Depth alpha cutoff", Range(0,1)) = 0.0
		_ShadowAlphaCutoff ("Shadow alpha cutoff", Range(0,1)) = 0.1
		
		_OverlayColor ("Overlay Color", Color) = (0,0,0,0)
		_Hue("Hue", Range(-0.5,0.5)) = 0.0
		_Saturation("Saturation", Range(0,2)) = 1.0	
		_Brightness("Brightness", Range(0,2)) = 1.0	
		
		_BlendTex ("Blend Texture", 2D) = "white" {}
		_BlendAmount ("Blend", Range(0,1)) = 0.0
		
		[HideInInspector] _BlendMode ("__mode", Float) = 0.0
		[HideInInspector] _SrcBlend ("__src", Float) = 1.0
		[HideInInspector] _DstBlend ("__dst", Float) = 0.0
		[HideInInspector] _RenderQueue ("__queue", Float) = 0.0
		[HideInInspector] _Cull ("__cull", Float) = 0.0
	}
	
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100
		
		Pass
		{
			Blend [_SrcBlend] [_DstBlend]
			Lighting Off
			ZWrite [_ZWrite]
			ZTest LEqual
			Cull [_Cull]
			Lighting Off
			
			CGPROGRAM			
				#pragma shader_feature _ _ALPHAPREMULTIPLY_ON _ADDITIVEBLEND _ADDITIVEBLEND_SOFT _MULTIPLYBLEND _MULTIPLYBLEND_X2
				#pragma shader_feature _ALPHA_CLIP
				#pragma shader_feature _TEXTURE_BLEND
				#pragma shader_feature _COLOR_ADJUST
				#pragma shader_feature _FOG
				
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma multi_compile_fog
				
				#pragma vertex vert
				#pragma fragment frag
				
				#include "SpriteUnlit.cginc"
			ENDCG
		}
		Pass
		{
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }
			Offset 1, 1
			
			Fog { Mode Off }
			ZWrite On
			ZTest LEqual
			Cull Off
			Lighting Off
			
			CGPROGRAM		
				#pragma multi_compile_shadowcaster
				#pragma fragmentoption ARB_precision_hint_fastest
				
				#pragma vertex vert
				#pragma fragment frag
				
				#include "SpriteShadows.cginc"
			ENDCG
		}
	}
	
	CustomEditor "SpriteShaderGUI"
}
