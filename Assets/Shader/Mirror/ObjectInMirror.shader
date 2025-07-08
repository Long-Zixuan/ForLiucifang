Shader "Mirror/ObjectInMirror"
{
    Properties 
    {
        _Color ("Color", Color) = (1,1,1,1)
        _DarkColor("DarkColor", Color) = (0.6,0.6,0.6,1)
        _MirrorColor("MirrorColor",Color) = (0.2,0.2,0.7,1)
        [NoScaleOffset] _MainTex ("MainTex", 2D) = "white" {}               // 主纹理
        _MirrorRange ("MirrorRange", Range(0, 100)) = 1                       // 镜面范围（最大范围，超出该范围就不反射
    }
    CGINCLUDE

    ENDCG
    SubShader 
    {
        Tags { "Queue"="Geometry+2" "RenderType"="Opaque" }
        Pass 
        {
           
            //Cull front
            //ZTest Always
            //ZWrite Off
            //Blend SrcAlpha OneMinusSrcAlpha
            Stencil 
            {
                Ref 1
                Comp Equal
            }
            CGPROGRAM
            #include "Lib/Mirror.cginc"
            #pragma vertex vert_mirror
            #pragma fragment frag_mirror
            ENDCG
        }


        Pass 
        {
            CGPROGRAM
            #include "Lib/Mirror.cginc"
            #pragma vertex vert_normal
            #pragma fragment frag_normal
            ENDCG
        }
    }
    Fallback "Diffuse"
}

