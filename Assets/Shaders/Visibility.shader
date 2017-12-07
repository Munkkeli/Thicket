Shader "Custom/Visibility" {
  Properties {
    _Color ("Color", Color) = (1, 0, 0, 0)
    _Size ("Size", float) = 8
  }

  SubShader {
    Tags {
      "Queue" = "Transparent"
      "IgnoreProjector" = "True"
      "RenderType" = "Transparent"
    }

    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

      fixed4 _Color;
      float _Size;

      struct appdata {
        float4 vertex : POSITION;
        float2 scale : TEXCOORD1;
        float2 uv : TEXCOORD0;
      };

      struct v2f {
        float4 vertex : SV_POSITION;
        float2 scale : TEXCOORD1;
        float2 uv : TEXCOORD0;
      };

      v2f vert(appdata v) {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.scale = (float2)mul(unity_ObjectToWorld, fixed2(1, 1));
        o.uv = v.uv.xy - fixed2(0.5, 0.5);
        return o;
      }

      fixed4 frag(v2f i) : SV_Target {
        float distance = sqrt(pow(i.uv.x * i.scale.x, 2) + pow(i.uv.y * i.scale.y, 2));
        return (distance > _Size) ? _Color : fixed4(1, 1, 1, 0);
      }

      ENDCG
    }
  }
}