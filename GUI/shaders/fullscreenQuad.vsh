#version 440 core

layout(location = 0) out vec2 pos;
layout(location = 1) out vec2 uv;

uniform mat3 curveView;
uniform float quadWidth;
uniform float quadHeight;

void main()
{
    float x = float(((uint(gl_VertexID)+2u) / 3u) % 2u);
    float y = float(((uint(gl_VertexID)+1u) / 3u) % 2u);

    uv = vec2(x, y);
    pos = (uv - 0.5f) * vec2(quadWidth, quadHeight);
    pos = (curveView * vec3(pos, 1.0f)).xy;

    gl_Position = vec4(uv * 2.0f - 1.0f, 0.0f, 1.0f);
}