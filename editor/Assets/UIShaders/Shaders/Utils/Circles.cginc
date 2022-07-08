float4 circle(float2 uv, float2 pos, float rad_start, float rad_end, float4 color, float _sharpness) {
    float d1 = length(pos - uv) - rad_end;
    float d2 = length(pos - uv) - rad_start;
    float t1 = clamp(d1*_sharpness, 0.0, 1.0);
    float t2 = clamp(d2*_sharpness, 0.0, 1.0);
    return float4(color.r, color.g, color.b, color.a * (1.0 + t2 - (1+t1)));
}