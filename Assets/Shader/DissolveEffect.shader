// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "ApcShader/DissolveEffect"
{
	Properties{
		_Diffuse("Diffuse", Color) = (1,1,1,1)
		_DissolveColorA("Dissolve Color A", Color) = (0,0,0,0)
		_DissolveColorB("Dissolve Color B", Color) = (1,1,1,1)
		_MainTex("MainTex", 2D) = "white"{}
		_DissolveMap("DissolveMap", 2D) = "white"{}
		_DissolveThreshold("DissolveThreshold", Range(0,1)) = 0
		_ColorFactorA("ColorFactorA", Range(0,1)) = 0.65
		_ColorFactorB("ColorFactorB", Range(0,1)) = 0.75
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag	

			#include "Lighting.cginc"
			uniform fixed4 _Diffuse;
			uniform fixed4 _DissolveColorA;
			uniform fixed4 _DissolveColorB;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform sampler2D _DissolveMap;
            // used to define the percentage of dissolve effect
            // 0 means no effect,
            // 1 means totally transparent
			uniform float _DissolveThreshold;
            // define the amount of color A, if less factor, more percentage of color
			uniform float _ColorFactorA;
            // define the amount of color B, if less factor, more percentage of color
			uniform float _ColorFactorB;
	

			struct vertIn {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 uv : TEXCOORD0;
			};
			struct VertOut
			{
				float4 vertex : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float2 uv : TEXCOORD1;
			};
	
			VertOut vert(vertIn v)
			{
				VertOut o;
                // Transform vertex in model space to Projection space
				o.vertex = UnityObjectToClipPos(v.vertex);
                // uses the TRANSFORM_TEX macro from UnityCG.cginc to make sure texture scale and offset is applied correctly
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // Convert Vertex position's corresponding normal into world coords
				o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
				return o;
			}
	
			fixed4 frag(VertOut i) : SV_Target
			{
				// Sample from the Dissolve Map which is a noise map
				fixed4 sampleDissolveValue = tex2D(_DissolveMap, i.uv);
				// if the r value of the sample is smaller than threshold, which
                // means this value should not be shown so that we discard it
				if (sampleDissolveValue.r < _DissolveThreshold)
				{
					discard;
				}

				// Apply Diffuse + Ambient illumination
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				fixed3 lambertC = saturate(dot(worldNormal, worldLightDir));
				fixed3 color = tex2D(_MainTex, i.uv).rgb * (lambertC * _Diffuse.xyz * _LightColor0.xyz + UNITY_LIGHTMODEL_AMBIENT.xyz);
                
                // Use division to calculate the current percentatge of margin
				float percentage = _DissolveThreshold / sampleDissolveValue.r;
                // Color A is like the inner margin color
                // Color B is like the out margin color
                // So here, always should let factor of B > factor A
                // if the percentage is greater than the factor of color A
                // then check whether percentage > factor of color B
                // if yes, return Color B,
                // else, return color A
				if (percentage > _ColorFactorA)
				{
					if (percentage > _ColorFactorB)
						return _DissolveColorB;
					return _DissolveColorA;
				}
                // else, it means the fragment should be normally drawn
				return fixed4(color, 1);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
