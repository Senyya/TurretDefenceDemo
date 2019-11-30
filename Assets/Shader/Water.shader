Shader "Unlit/Water"
{
	Properties
	{
		_MainTex("MainTex",2D)="white"{}
		_MainColor("MainColor",COLOR)=(1,1,1,1)
		_DistortionTex("DistortionTex",2D)="white"{}
		_DistortionFactor("DistortionFactor",Range(0.1,1.0))=0.713
		_AlphaScale("Alpha Scale", Range(0, 1)) = 0.412
	}
	SubShader
	{
		Tags{"RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True"}
		Pass
		{
			Tags { "LightMode" = "ForwardBase" } // set the light mode
			ZWrite Off // close ZWrite to make transparency
            Blend SrcAlpha OneMinusSrcAlpha // set as Traditional transparency

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
 
			float4 _MainColor;
			sampler2D _MainTex;
			sampler2D _DistortionTex;
			float4 _MainTex_ST;
			float _DistortionFactor;
			fixed _AlphaScale;

			struct vertIn
			{
				float4 vertex:POSITION;
				float4 normal : NORMAL;
				float2 uv:TEXCOORD0;
			};

			struct vertOut
			{
				float4 vertex:POSITION;
				float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
				float2 uv:TEXCOORD2;
			};

			vertOut vert(vertIn v)
			{
				vertOut o;
                
                // Transform vertex in model space to Projection space
				o.vertex = UnityObjectToClipPos(v.vertex);
                // Convert Vertex position's corresponding normal into world coords
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
                // Convert Vertex position into world coords and assign to worldPos
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                // uses the TRANSFORM_TEX macro from UnityCG.cginc to make sure texture scale and offset is applied correctly
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
 
 
			float4 frag(vertOut o):SV_Target
			{
				// normalize the worldNomal
                fixed3 worldNormal = normalize(o.worldNormal);
                // normalize the direction to light in world space
                fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(o.worldPos));
                // Time since level load in t/20
                // To make the distortion of water using a smooth changing value which is the gb value of the image
				float2 distortion = (tex2D(_DistortionTex,o.uv.xy + float2(0, _Time.x)).gb + tex2D(_DistortionTex, o.uv.xy + float2(_Time.x, 0)).gb) - 1;
                // Multiple with the distortion factor
				float2 ruv = o.uv.xy + distortion.xy * _DistortionFactor;
                // using the new uv to tex texture
				half4 textColor = tex2D(_MainTex, ruv) * _MainColor;

                // apply ambient and diffuse illumination
                // define the albedo of the material
				fixed3 Ka = textColor.rgb * _MainColor.rgb;
                // use the direction light in world as source to apply diffuse illumination
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * Ka;
                // use the direction light in world space as source to apply diffuse illumination based on the lab
				fixed3 diffuse = _LightColor0.rgb * Ka * saturate(dot(worldNormal, worldLightDir));
				return fixed4(ambient + diffuse, textColor.a * _AlphaScale);
			}
			ENDCG
		}
	}
}
