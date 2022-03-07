// passthrough vertex shader (doing nothing)

#version 440 core

layout(location = 0) in vec4 in_position;

void main()
{
    gl_Position = in_position;
}