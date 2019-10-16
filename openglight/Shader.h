#pragma once
#include <glad/glad.h>

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <string>
#include <fstream>
#include <sstream>
#include <iostream>

using namespace glm;

//struct Light
//{
//	// vec3 position; // 使用定向光就不再需要了
//	vec3 direction;
//
//	vec3 ambient;
//	vec3 diffuse;
//	vec3 specular;
//}l;


class Shader
{
public:
	unsigned int ID;		//程序ID

	//读取并创建着色器
	Shader(const GLchar* vertexPath, const GLchar* fragmentPath);
	~Shader();

	//使用/激活程序
	void use();

	//uniform工具函数
	void setBool(const std::string &name, bool value) const;
	void setInt(const std::string &name, int value) const;
	void setFloat(const std::string &name, float value) const;
	void setVec3(const std::string &name, glm::vec3 value) const;
	void setMat4(const std::string &name, glm::mat4 value) const;
	void setLight();
};