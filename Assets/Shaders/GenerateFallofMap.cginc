float Evaluate(float value) 
{
    float a = 3;
    float b = 2.2f;

    return pow(value,a)/(pow(value,a)+ pow(b-b*value,a));
}

void GenerateFallofMap_float(float r,float4 UV, out float3 fallOffMap) {
    fallOffMap = float4(0,UV.y,0,0);

    float xc = UV.x - 0.5;
    float yc = UV.y - 0.5;

    float3 res;
    float2 pos = float2(xc,yc);

    float t = lerp(xc,yc,0.5);
    
    
    if (xc * xc + yc * yc < r * r)
    {
        res = Evaluate(distance(pos,float2(0,0))/r);
    }
    else
    {
        res = float3(1,1,1);
    }
    
    fallOffMap = res;
}