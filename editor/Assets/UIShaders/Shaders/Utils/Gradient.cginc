
float4 gradient2(float2 uv, float4 color1, float4 color2, float inverse, float _mode, float _balance) {

    float4 invColor1 = color1*(1.0-inverse) + color2*inverse;
    float4 invColor2 = color2*(1.0-inverse) + color1*inverse;
    color1 = invColor1;
    color2 = invColor2;

    float t;
    if (_mode == 0){
        t = ( (length(float2(uv.x, 0) - float2(0, 0))) + (length(float2(0, uv.y) - float2(0, 0))) );
        //t = pow(t, _density);
        t += lerp(-1.0, 1.0, _balance/10.0);
    }
       else if (_mode == 1){
        t = length(float2(uv.x, 0));
        t = pow(t, _balance);
    }
    else if (_mode == 2){
        t = length(float2(0, uv.y) - float2(0, 0));
        t = pow(t, _balance);
    }
    else if (_mode == 3){
        t = length(uv - float2(.5, .5)) * _balance;
        t = clamp(t*_balance, 0.0, 1.0);
    }
    else if (_mode == 3){
        t = ( (length(float2(uv.x, 0) - float2(.5, _balance*.18))) + (length(float2(0, uv.y) - float2(_balance*.18, .5))) );
    }
    return lerp(color1, color2, t);
}