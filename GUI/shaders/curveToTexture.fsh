#version 440 core

struct Curve
{
    float coeffs[6];
};

layout(location = 0) in vec2 pos;
layout(location = 1) in vec2 uv;

out float outValue;
uniform Curve curves[16];
uniform int curvesCount;

void main()
{
    outValue = float(curvesCount > 0);
    for (int i = 0; i < curvesCount; i++)
        outValue *= float(curves[i].coeffs[0] * pos.x * pos.x + 
                          curves[i].coeffs[1] * pos.y * pos.y + 
                          curves[i].coeffs[2] * pos.x * pos.y + 
                          curves[i].coeffs[3] * pos.x + 
                          curves[i].coeffs[4] * pos.y + 
                          curves[i].coeffs[5] < 0.0f);
}