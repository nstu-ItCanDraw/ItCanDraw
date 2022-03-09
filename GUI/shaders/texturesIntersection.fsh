#version 440 core

layout(location = 0) in vec2 pos;

uniform sampler2D tex1;
uniform sampler2D tex2;

out vec4 outValue;

void main()
{
    outValue = vec4(float(texture(tex1, pos / 2.0f + 0.5f).r * texture(tex2, pos / 2.0f + 0.5f).r != 0.0f), 0.0f, 0.0f, 1.0f);
}