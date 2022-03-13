#version 440 core

layout(location = 0) out vec4 frag_color;

uniform sampler2D tex1;
uniform sampler2D tex2;
uniform vec2 tex1_size;
uniform vec2 tex2_size;

void main()
{
    float merger = float(texture(tex1, gl_FragCoord.xy / tex1_size) == vec4(0.0f, 0.0f, 0.0f, 1.0f) ||
                         texture(tex2, gl_FragCoord.xy / tex2_size) == vec4(0.0f, 0.0f, 0.0f, 1.0f));
    frag_сolor = vec4(merger, merger, merger, 1.0f);
}
