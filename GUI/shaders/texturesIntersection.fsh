#version 440 core

layout(location = 0) in vec2 pos;
layout(location = 1) in vec2 uv;

uniform sampler2D tex1;
uniform sampler2D tex2;

out vec4 outValue;

void main()
{
    outValue = vec4(float(texture(tex1, uv).r * texture(tex2, uv).r != 0.0f), 0.0f, 0.0f, 1.0f);
}