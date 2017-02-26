#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

#define SAMPLE_COUNT 10

float2 Center;
float Amount;

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
    float2 uv = input.TexCoords - Center;
    
    float precompute = Amount / float(SAMPLE_COUNT - 1);
    float4 color = 0;
    
    for (int i = 0; i < SAMPLE_COUNT; i++)
    {
        float scale = 1.0 + (float(i) * precompute);
        color += tex2D(s0, uv * scale + Center);
    }
    
    color /= float(SAMPLE_COUNT);
    
    return color;
}


technique GaussianBlur
{
    pass
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
}