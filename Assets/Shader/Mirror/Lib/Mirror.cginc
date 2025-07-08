#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
    float3 normal : NORMAL;
};

struct v2f
{
    float4 vertex : SV_POSITION;
    float2 uv : TEXCOORD0;
    float3 wPos : TEXCOORD1;
    float3 worldNormal : TEXCOORD2;
};

struct v2f_m {
    float4 vertex : SV_POSITION;
    float2 uv : TEXCOORD0;
    float4 normal : TEXCOORD1;
    float4 wPos : TEXCOORD2;
};

sampler2D _MainTex;

float _MirrorRange;
float3 n, p; // 镜面法线，镜面任意点
float4 _Color;
float4 _DarkColor;
float4 _MirrorColor;


v2f vert_normal (appdata v)
{
    v2f o;
    o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.worldNormal = normalize( mul((float3x3)unity_ObjectToWorld,v.normal) );
    o.uv = v.uv;
    return o;
}
fixed4 frag_normal (v2f i) : SV_Target
{
    float3 dir = i.wPos.xyz - p;                // 平面与插值点的指向
    half d = dot(dir, n);                       // 与反向镜面的距离
    if (d < 0) discard;                         // 如果平面背面，那就丢弃

    
    fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
    float isShadow = step(dot(i.worldNormal,worldLightDir),0);

    return lerp(tex2D(_MainTex, i.uv) * _Color ,tex2D(_MainTex, i.uv) * _Color * _DarkColor,isShadow);
   // return tex2D(_MainTex, i.uv) * _Color;
}


v2f_m vert_mirror (appdata v)
{
    v2f_m o;

    o.wPos = mul(unity_ObjectToWorld, v.vertex);

    float3 nn = -n;                 // 法线反向
    float3 dp = o.wPos.xyz - p;     // 平面点与世界空间的点的向量（即：从平面的点指向世界空间点的方向）
    half nd = dot(n, dp);           // 计算出点与平面的垂直距离
    o.wPos.xyz += nn * (nd * 2);    // 将垂直距离反向2倍的距离，就是镜像的位置
    
    o.vertex = mul(unity_MatrixVP, o.wPos);
    o.normal.xyz = UnityObjectToWorldNormal(v.normal);

    fixed t = nd / _MirrorRange;       // 将位置与镜面最大范围比利作为fade alpha的插值系数
   
    o.wPos.w = nd;      // 距离存于o.wPos.w
    o.uv = v.uv;
    
    return o;
}


fixed4 frag_mirror (v2f_m i) : SV_Target
{
    if (i.wPos.w > _MirrorRange) discard;       // 超过镜像范围也丢弃

    float3 dir = i.wPos.xyz - p;                // 平面与插值点的指向
    half d = dot(dir, n);                       // 与反向镜面的距离
    if (d > 0) discard;                         // 如果超过了平面，那就丢弃
    
    fixed4 col = tex2D(_MainTex, i.uv) * _Color;     // 加上扰动UV后再采样主纹理
    return fixed4(col.rgb, i.normal.w) * _DarkColor * _MirrorColor; // 返回颜色与透明度;
}