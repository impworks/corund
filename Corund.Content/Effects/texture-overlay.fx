#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

texture OverlayTexture;
float2 OverlayOrigin;
float2 OverlayScale;
float OverlayOpacity;

matrix WorldViewProjection;

sampler2D s0;
sampler2D TextureSampler = sampler_state
{
    Texture = <OverlayTexture>;
    AddressU = Wrap;
    AddressV = Wrap;
};

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
    float2 overlayUV = (input.TexCoords - OverlayOrigin) * OverlayScale;

    float4 original = tex2D(s0, input.TexCoords);
    float4 overlay = tex2D(TextureSampler, overlayUV);

    float4 result = 0;
    result.rgb = lerp(original.rgb, overlay.rgb, OverlayOpacity) * original.a;
    result.a = original.a;

    return result;
}

technique TextureOverlay
{
    pass
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
}