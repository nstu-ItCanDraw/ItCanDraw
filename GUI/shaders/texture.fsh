#version 440 core

layout(location = 0) out vec4 frag_color;
uniform sampler2D tex;
uniform vec2 tex_size;

void main()
{
    frag_сolor = texture(tex, gl_FragCoord.xy / tex_size);
}

