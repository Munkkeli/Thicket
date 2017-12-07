Shader "Custom/Clouds" {
  Properties {
    _MainTex ("Texture", 2D) = "white" {}
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

      sampler2D _MainTex;
      float4 _MainTex_ST;

      struct appdata {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
      };

      struct v2f {
        float4 vertex : SV_POSITION;
        float2 uv : TEXCOORD0;
      };

      v2f vert(appdata v) {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = (float2)mul(unity_ObjectToWorld, TRANSFORM_TEX(v.uv, _MainTex) * 0.015625);
        return o;
      }
      
      fixed4 frag(v2f i) : SV_Target {
        return tex2D(_MainTex, i.uv);
      }

      ENDCG
    }
  }
}
