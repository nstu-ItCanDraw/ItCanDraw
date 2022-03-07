#version 440 core

layout(location = 0) out vec2 pos;

void main()
{
    float x = float(((uint(gl_VertexID)+2u) / 3u) % 2u) * 2.0f - 1.0f;
    float y = float(((uint(gl_VertexID)+1u) / 3u) % 2u) * 2.0f - 1.0f;

    pos = vec2(x, y);

    gl_Position = vec4(pos, 0.0f, 1.0f);
}