

sampler TextureSampler : register(s0);

float InFloat;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 Color;
    
    texCoord.y = texCoord.y + (sin(texCoord.x*InFloat)*0.1);
	Color = tex2D( TextureSampler, texCoord.xy);
	
	Color.rgb = texCoord.y;

    return Color;
}

technique Wave
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
