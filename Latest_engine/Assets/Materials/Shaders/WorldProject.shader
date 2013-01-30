Shader "Custom/WorldProject"
{
        Properties
        {
                _Color ("Main Color", Color) = (1,1,1,1)
                _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
                _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
                _MainTex ("Base", 2D) = "white" {}
                _BumpMap ("Normalmap", 2D) = "bump" {}
                _ScaleX ("Scale X", float) = 1.0
                _ScaleY ("Scale Y", float) = 1.0
        }
       
        SubShader
        {
                Tags { "RenderType"="Opaque" }
                LOD 400
       
                CGPROGRAM
                #pragma target 3.0
                #pragma surface surf VAO
               
                half _Shininess;
               
                inline fixed4 LightingVAO( SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten )
                {
                        half3 h = normalize( lightDir + viewDir );
                        fixed diff = max( 0.0, dot( s.Normal, lightDir ) );
                       
                        float nh = max( 0.0, dot ( s.Normal, h ) );
                        float spec = pow( nh, s.Specular * 128.0 );
                       
                        half3 c = ( s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * _SpecColor.rgb * spec ) *
                        ( atten * 2.0 );
                       
                        return fixed4( c, 1.0 );
                }
               
                struct Input
                {
                    float4 color : COLOR;
                        float2 uv_MainTex;
                        float2 uv_BumpMap;
                        float3 worldPos;
                        float3 worldNormal; INTERNAL_DATA
                };
               
                sampler2D _MainTex;
                sampler2D _BumpMap;
                fixed4 _Color;
               
                half _ScaleX;
                half _ScaleY;
               
                void surf (Input IN, inout SurfaceOutput o)
                {
                        float3 correctWorldNormal = WorldNormalVector(IN, float3( 0, 0, 1 ) );
                        float2 uv = IN.worldPos.xz;
                       
                        if( abs( correctWorldNormal.x ) > 0.5 ) uv = IN.worldPos.yz;
                        if( abs( correctWorldNormal.z ) > 0.5 ) uv = IN.worldPos.xy;
 
                        uv.x *= _ScaleX;
                        uv.y *= _ScaleY;
                       
                        fixed4 tex = tex2D( _MainTex, uv );
                        o.Albedo = IN.color * tex.rgb * _Color.rgb;
                        o.Specular = _Shininess;
                        o.Normal = UnpackNormal( tex2D( _BumpMap, uv ) );
                }
               
                ENDCG
        }
FallBack "Bumped Specular"
}