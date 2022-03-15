#version 440 core

layout(location = 0) in vec2 pos;
layout(location = 1) in vec2 uv;

uniform sampler2D tex1;
uniform sampler2D tex2;

out float outValue;

void main()
{
    outValue = float(texture(tex1, uv).x == 0.0f || texture(tex2, uv).x == 0.0f);
}
