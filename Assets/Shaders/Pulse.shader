Shader "Sprites/Pulse" {
  Properties {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Highlight ("Highlight", Color) = (1, 1, 1, 1)
    _Color ("Tint", Color) = (1, 1, 1, 1)
    [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
  }

  SubShader {
    Tags {
      "Queue" = "Transparent"
      "IgnoreProjector" = "True"
      "RenderType" = "Transparent"
      "PreviewType" = "Plane"
      "CanUseSpriteAtlas" = "True"
    }

    Cull Off
    Lighting Off
    ZWrite Off
    Fog { Mode Off }
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile DUMMY PIXELSNAP_ON

      #include "UnityCG.cginc"

      fixed4 _Color;
      fixed4 _Highlight;
      sampler2D _MainTex;

      struct appdata {
        float4 vertex : POSITION;
        float4 color : COLOR;
        float2 uv : TEXCOORD0;
      };

      struct v2f {
        float4 vertex : SV_POSITION;
        fixed4 color : COLOR;
        half2 uv : TEXCOORD0;
      };

      v2f vert(appdata v) {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;
        o.color = v.color * _Color;

        #ifdef PIXELSNAP_ON
        o.vertex = UnityPixelSnap(o.vertex);
        #endif

        return o;
      }

      fixed4 frag(v2f i) : COLOR {
        half4 texcol = tex2D(_MainTex, i.uv);
        texcol.rgb = lerp(texcol.rgb, _Highlight, (1 - min(1, pow(abs(sin(_Time[0] * 40) * 2), 2))) * _Highlight.a);
        texcol = texcol * i.color;
        return texcol;
      }

      ENDCG
    }
  }

  Fallback "Sprites/Default"
}