#include "../Utils/Constants.cginc"
#include "../Utils/Extensions.cginc"

//Produces and angle over _time. It needs a mode and a series of modifiers
float produceAngle(float _time, float _mode, float _mod1, float _mod2, float _mod3) {
    if(_mode<=3) {
        return -_time * PI*2.0*_mod1 + sin(_time*PI*2.0*_mod2) * (_mode) * _mod3;
    }
    else if (_mode==4) {
        return -((_time*_mod1) - (sin(_time + PI)*_mod2) * (cos(_time)*_mod3)) - _time; 
    }
    return 0.0;
}

float2 rotationCoord(float2 _mode, float2 _speed, float _offset, float _time, float _mod1, float _mod2, float _mod3) {
    float wantedAngle = produceAngle(_time, _mode, _mod1, _mod2, _mod3);
    float x = (sin(wantedAngle*_speed) * _offset) + .5;
    float y = (cos(wantedAngle*_speed) * _offset) + .5;
    return float2(x, y);
}

float2 rotationAngle(float _offset, float4 angle) {
    float x = sin(angle)*_offset + .5;
    float y = cos(angle)*_offset + .5;
    return float2(x, y);
}


float2 uvRotation(float2 _uv, float _mode, float _speed, float _counterClock, float _time, float _mod1, float _mod2, float _mod3) {
    float wantedAngle = produceAngle(_time, _mode, _mod1, _mod2, _mod3);
    wantedAngle *=  boolToSign(_counterClock) * _speed;
    float2x2 rot = float2x2(cos(wantedAngle),sin(wantedAngle),-sin(wantedAngle),cos(wantedAngle));
    return mul(_uv, rot); 
}