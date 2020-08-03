Shader "MetaGoo/DrawStructuredBuffer" 
{
	SubShader 
	{
		Pass 
		{
			Tags {"LightMode"="ForwardBase"}
			Cull back

			CGPROGRAM
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			#include "UnityShadowLibrary.cginc"
			#include "AutoLight.cginc"
			#pragma target 5.0
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma multi_compile _ SHADOWS_SCREEN
			
			struct Vert
			{
				float4 vertex;
				float3 normal;
			};

			uniform StructuredBuffer<Vert> _Buffer;

			struct v2f
			{
				float4  pos : SV_POSITION;
				float3 ambient : TEXCOORD0;
				float3 diffuse : TEXCOORD1;
				SHADOW_COORDS(2)
			};

			v2f vert(uint id : SV_VertexID)
			{
				Vert v = _Buffer[id];
				v2f o;

				// position
				o.pos = UnityWorldToClipPos(float4(v.vertex.xyz, 1));

				// lighting
				float3 worldNormal = v.normal;
				float3 ndotl = saturate(dot(worldNormal, _WorldSpaceLightPos0.xyz));
				float3 ambient = ShadeSH9(float4(worldNormal, 1.0f));
				float3 diffuse = (ndotl * _LightColor0.rgb);
				o.ambient = ambient;
				o.diffuse = diffuse;
				
				// shadows
				TRANSFER_SHADOW(o)
				
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET
			{
				fixed3 colour = fixed3(1,0,0);
				
				fixed shadow = SHADOW_ATTENUATION(i);
				float3 lighting = (i.diffuse * shadow) + i.ambient;
				return fixed4(lighting * colour, 1);
			}

			ENDCG
		}
		
		Pass
		{
			Tags {"LightMode"="ShadowCaster"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"
			
			struct Vert
			{
				float4 vertex;
				float3 normal;
			};
			
			uniform StructuredBuffer<Vert> _Buffer;

			float4 vert(uint id : SV_VertexID) : SV_POSITION
			{
				Vert v = _Buffer[id];
				float4 position = UnityClipSpaceShadowCasterPos(v.vertex.xyz, v.normal);
				return UnityApplyLinearShadowBias(position);
			}

			half4 frag () : SV_TARGET
			{
				return 0;
			}
			
			ENDCG
		}
	}
}