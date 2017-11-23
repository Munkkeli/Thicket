Shader "Custom/Curtain" {
  Properties {
    _Color ("Color", Color) = (1, 0, 0, 0)
    _Size ("Size", Range (0, 1)) = 0.15
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
        float2 uv : TEXTCOORD0;
      };

      fragmentInput vert (appdata_base v) {
        fragmentInput o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = v.texcoord.xy - fixed2(0.5,0.5);
        return o;
      }

      fixed4 frag(fragmentInput i) : SV_Target {
        float distance = sqrt(pow(i.uv.x, 2) + pow(i.uv.y,2));
        return (distance > _Size) ? _Color : fixed4(1, 1, 1, 0);
      }

      ENDCG
    }
  }
}