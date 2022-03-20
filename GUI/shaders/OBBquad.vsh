#version 440 core

layout(location = 0) out vec2 pos;
layout(location = 1) out vec2 uv;

uniform mat3 model;
uniform mat3 view;
uniform vec2 bottomLeft;
uniform vec2 topRight;

void main()
{
    float x = float((uint(gl_VertexID)+1u) / 2u % 2u);
    float y = float(uint(gl_VertexID) / 2u);
    
    uv = vec2(x, y);
    pos = topRight * (uv - 0.5f) + bottomLeft * (0.5f - uv);
    
    gl_Position = vec4((view * model * vec3(pos, 1.0f)).xy, 0.0f, 1.0f);
}