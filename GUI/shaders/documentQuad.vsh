#version 440 core

layout(location = 0) out vec2 pos;
layout(location = 1) out vec2 uv;

uniform mat3 view;
uniform mat3 curveView;
uniform float documentWidth;
uniform float documentHeight;

void main()
{
    float x = float(((uint(gl_VertexID)+2u) / 3u) % 2u);
    float y = float(((uint(gl_VertexID)+1u) / 3u) % 2u);

    pos = vec2(x - 0.5f, y - 0.5f) * vec2(documentWidth, documentHeight);

    gl_Position = vec4((view * vec3(pos, 1.0f)).xy, 0.0f, 1.0f);

    uv = gl_Position.xy / 2.0f + 0.5f;
    pos = (curveView * vec3(pos, 1.0f)).xy;
}