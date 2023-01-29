void PixelateUvAspect_float(float4 UV,float step,float2 aspect, out float4 PixelatedUV) {

    
    
    float offset = (max(aspect.x,aspect.y) - min(aspect.x,aspect.y))/2 / max(aspect.x,aspect.y);
    
    float pixalateX = floor((UV.x - (aspect.x > aspect.y ? offset : 0))  * (aspect.x/aspect.y) *step)/step;
    float pixalateY = floor((UV.y - (aspect.y > aspect.x ? offset : 0)) *step)/step;
    float pixalateZ = floor(UV.z *step)/step;
    float pixalateW = floor(UV.w *step)/step;
    
    PixelatedUV = float4(pixalateX,pixalateY,pixalateZ,pixalateW);
}