#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float2 Center;
float RadiusOuter;
float RadiusInner;
float WarpCoefficient;
float2 ScreenRatio;

matrix WorldViewProjection;
sampler s0;

struct VertexShaderInput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TexCoords : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position: SV_Position;
    float4 Color : COLOR0;
    float2 TexCoords : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.Color = input.Color;
    output.TexCoords = input.TexCoords;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR0
{
    float2 uv = input.TexCoords;
    float2 centeredUV = uv * 2.0 - 1.0;
    float2 v = ScreenRatio * centeredUV - Center;
    float2 v2 = centeredUV - Center;

    float phi = atan2(v.y, v.x);
    float len = length(v);
    float len2 = length(v2);
    float d = max(RadiusOuter - len, 0.0);
    float rho = phi + d * lerp(0.0, WarpCoefficient, (len2 - RadiusOuter) / (RadiusOuter - RadiusInner));
    float2 uvWarp = float2(cos(rho), sin(rho)) * len * 0.5 + 0.5;

    float2 coords = len < RadiusOuter ? uvWarp : uv;

    return tex2D(s0, coords);
}

technique Swirl
{
    pass
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
}