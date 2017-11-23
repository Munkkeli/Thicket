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

      struct fragmentInput {
        float4 pos : SV_POSITION;
        float2 scale : TEXTCOORD1;
        float2 uv : TEXTCOORD0;
      };

      fragmentInput vert (appdata_base v) {
        fragmentInput o;
        o.pos = UnityObjectToClipPos (v.vertex);
        o.scale = mul (unity_ObjectToWorld, float2(1, 1));
        o.uv = v.texcoord.xy - fixed2(0.5,0.5);
        return o;
      }

      fixed4 frag(fragmentInput i) : SV_Target {
        float distance = sqrt(pow(i.uv.x * i.scale.x, 2) + pow(i.uv.y * i.scale.y, 2));
        return (distance > _Size) ? _Color : fixed4(1, 1, 1, 0);
      }

      ENDCG
    }
  }
}