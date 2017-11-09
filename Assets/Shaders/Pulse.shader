Shader "Sprites/Pulse" {
  Properties {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Highlight ("Highlight", Color) = (1,1,1,1)
    _Color ("Tint", Color) = (1,1,1,1)
    [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
  }

  SubShader {
    Tags {
      "Queue"="Transparent"
      "IgnoreProjector"="True"
      "RenderType"="Transparent"
      "PreviewType"="Plane"
      "CanUseSpriteAtlas"="True"
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
        
        struct appdata_t {
          float4 vertex : POSITION;
          float4 color : COLOR;
          float2 texcoord : TEXCOORD0;
        };

        struct v2f {
          float4 vertex : SV_POSITION;
          fixed4 color : COLOR;
          half2 texcoord : TEXCOORD0;
        };
        
        fixed4 _Color;
        fixed4 _Highlight;

        v2f vert(appdata_t IN) {
          v2f o;
          o.vertex = UnityObjectToClipPos(IN.vertex);
          o.texcoord = IN.texcoord;
          o.color = IN.color * _Color;

          #ifdef PIXELSNAP_ON
          o.vertex = UnityPixelSnap (o.vertex);
          #endif

          return o;
        }

        sampler2D _MainTex;

        fixed4 frag(v2f IN) : COLOR {
          half4 texcol = tex2D (_MainTex, IN.texcoord);
          texcol.rgb = lerp(texcol.rgb, _Highlight, (1 - min(1, pow(abs(sin(_Time[0] * 40) * 2), 2))) * _Highlight.a);
          texcol = texcol * IN.color;
          return texcol;
        }
      ENDCG
    }
  }

  Fallback "Sprites/Default"
}