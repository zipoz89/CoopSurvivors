void Pixelate_float(float4 UV,float step, out float4 PixelatedUV) {
    PixelatedUV = floor(UV*step)/step;
}