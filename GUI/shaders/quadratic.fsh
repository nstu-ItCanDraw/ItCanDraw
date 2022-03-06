#version 440 core

layout(location = 0) out vec4 frag_color;
uniform float a1, a2, a3, a4, a5, a6;

void main()
{
	float c = 0.0;
	if (a1 * gl_FragCoord.x * gl_FragCoord.x + a2 * gl_FragCoord.y * gl_FragCoord.y + a3 * gl_FragCoord.x * gl_FragCoord.y + a4 * gl_FragCoord.x + a5 * gl_FragCoord.y + a6 < 0.0)
		c = 1.0;
		
    frag_сolor = vec4(c,c,c,1.0);
}