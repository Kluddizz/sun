Shader "Unlit/SunShader"
{
    Properties
    {
        _displacement ("Displacement", Float) = 0.01
        _color1 ("Bright Color", Color) = (0,0,0,1)
        _color2 ("Dark Color", Color) = (1,0,0,1)
        _octaves ("Octaves", Int) = 4
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            float4 _color1, _color2;
            float _displacement;
            int _octaves;

            float hash (float2 n)
            {
              return frac(sin(dot(n,float2(123.456789,987.654321)))*54321.9876 );
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 u = smoothstep(0.0,1.0,frac(p));
                float a = hash(i+float2(0,0));
                float b = hash(i+float2(1,0));
                float c = hash(i+float2(0,1));
                float d = hash(i+float2(1,1));
                float r = lerp(lerp(a,b,u.x), lerp(c,d,u.x),u.y);
                return r*r;
            }

            float fbm(float2 p , int octaves)
            {
                float value = 0.0;
                float amplitude = 0.45;
                float e = 4.0;

                for (int i = 0; i < octaves; i++)
                {
                    value += amplitude * noise(p);
                    p = p * e;
                    amplitude *= 0.55;
                    e *= 0.85;
                }

                return value;
            }

            float sunTexture(float2 uv)
            {
              return fbm(uv + fbm(5 * uv + _Time.x, _octaves), _octaves);
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
              float3 position = v.vertex.xyz;
              float3 direction = normalize(position);

              // Calculate heightmap using FBM like in the fragment shader.
              // Based on the resulting FBM texture, vertices will be
              // displaced. Darker regions are nearer the center while
              // brighter regions are more far away from the center
              // (displacement).
              position += direction * sunTexture(v.uv) * _displacement;

              v2f o;
              o.vertex = UnityObjectToClipPos(float4(position, 1.0));
              o.uv = v.uv;

              return o;
            }

            float4 frag (v2f i) : SV_Target
            {
              float texColor = sunTexture(i.uv);
              float4 color = lerp(_color1, _color2, 2.0 * texColor);
              return color;
            }
            ENDCG
        }
    }
}
