#version 440 core

layout(location = 0) in vec2 pos;
layout(location = 1) in vec2 uv;

uniform sampler2D tex;
uniform vec4 color;

out vec4 outColor;

void main()
{
    outColor = texture(tex, uv).r * color;
}