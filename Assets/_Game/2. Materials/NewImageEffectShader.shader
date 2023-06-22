Shader "Hidden/NewImageEffectShader"
{
	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" {}
		_FillAmount("Fill Amount", Range(0, 1)) = 1
		_Color("Color", Color) = (1, 1, 1, 1)
	}

		SubShader
		{
			Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				  #pragma multi_compile_instancing
				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float _FillAmount;
				fixed4 _Color;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// Calculate the distance from the center of the sprite
					float2 center = float2(0.5, 0.5);
					float2 dir = i.uv - center;
					float distance = length(dir);

					// Calculate the angle of the pixel
					float angle = atan2(dir.y, dir.x);

					// Convert the angle to a normalized value between 0 and 1
					float normalizedAngle = (angle + 3.14159) / (2 * 3.14159);

					// Determine the fill amount based on the normalized angle
					float fill = normalizedAngle <= _FillAmount ? 1 : 0;

					// Apply spherical mask to retain the original sprite shape
					float mask = distance <= 0.5 ? 1 : 0;

					// Apply sprite color to the final color
					fixed4 spriteColor = tex2D(_MainTex, i.uv) * _Color;

					// Output the final color with fill, mask, and sprite color applied
					return spriteColor * fill * mask;
				}
				ENDCG
			}
		}
}
