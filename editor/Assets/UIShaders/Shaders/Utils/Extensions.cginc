
float mod(float x, float y){
      return x - y * floor(x/y);
}

float translateRange(float value, float leftMin, float leftMax, float rightMin, float rightMax) {
    float leftSpan = leftMax - leftMin;
    float rightSpan = rightMax - rightMin;
    float valScaled = float(value - leftMin) / float(leftSpan);
    return rightMin + (valScaled * rightSpan);
}


float boolToSign(float toggleValue) {
    return (-1.0 + (2.0*toggleValue));  //BOOL (false, true) TO FLOAT (-1, 1)
}