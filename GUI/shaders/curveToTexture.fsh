#version 440 core

layout(location = 0) in vec2 pos;
layout(location = 1) in vec2 uv;

out float outValue;
uniform float coeffs[6];

void main()
{
	outValue = float(coeffs[0] * pos.x * pos.x + 
                     coeffs[1] * pos.y * pos.y + 
                     coeffs[2] * pos.x * pos.y + 
                     coeffs[3] * pos.x + 
                     coeffs[4] * pos.y + 
                     coeffs[5] < 0.0f);
}