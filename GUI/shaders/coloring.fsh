#version 440 core

layout(location = 0) in vec2 pos;
layout(location = 1) in vec2 uv;

uniform sampler2D tex;
uniform vec4 backgroundColor;
uniform vec4 color;

out vec4 outColor;

void main()
{
    float value = texture(tex, uv).r;
    outColor = float(value != 0) * color + float(value == 0) * backgroundColor;
}