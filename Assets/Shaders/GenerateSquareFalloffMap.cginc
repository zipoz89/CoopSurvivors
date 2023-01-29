float Evaluate(float value) 
{
    float a = 3;
    float b = 2.2f;

    return pow(value,a)/(pow(value,a)+ pow(b-b*value,a));
}

void GenerateSquareFalloffMap_float(float r,float4 UV, out float3 fallOffMap) {
    fallOffMap = float4(0,UV.y,0,0);

    float xc = UV.x - 0.5;
    float yc = UV.y - 0.5;

    float3 res;
    float2 pos = float2(xc,yc);

    float t = lerp(xc,yc,0.5);
    
    if (abs(xc) <= r &&  abs(yc) <= r)
    {
        res = Evaluate(max(abs(xc), abs(yc)));
    }
    else
    {
        res = float3(1,1,1);
    }
    
    fallOffMap = res;
}