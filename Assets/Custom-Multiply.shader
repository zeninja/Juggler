Shader "Custom/Multiply" {
     Properties {
         _MainTex ("Base (RGB)", 2D) = "white" {}
         _Alpha ("Alpha", Float) = 0.5
     }
     SubShader {
         Tags { "RenderType"="Opaque" }
         LOD 200
         
         CGPROGRAM
         #pragma surface surf Lambert alpha
 
         sampler2D _MainTex;
         uniform half _Alpha;
         
         struct Input {
             half Alpha;
             float2 uv_MainTex;
         };
 
         void surf (Input IN, inout SurfaceOutput o) {
             half4 c = tex2D (_MainTex, IN.uv_MainTex);
             half4 a = _Alpha;
             o.Albedo = c.rgb;
             o.Alpha =  (1.0f-c.r) * _Alpha;
         }
         ENDCG
     } 
     FallBack "Diffuse"
 }