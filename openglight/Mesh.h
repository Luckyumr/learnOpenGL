#pragma once

#include <glad/glad.h>
#include "glm/glm.hpp"
#include "glm/gtc/matrix_transform.hpp"
#include "stb_image.h"

#include <assimp/Importer.hpp>
#include <assimp/scene.h>
#include <assimp/postprocess.h>

#include "Shader.h"

#include <iostream>
#include <string>
#include <vector>
#include <fstream>
#include <sstream>

using namespace std;

struct Vertex
{
	glm::vec3 Position;
	glm::vec3 Normal;
	glm::vec2 TexCoords;
};

struct Texture
{
	unsigned int id;
	string type;
	aiString path;
};

class Mesh
{
public:
	//��������
	vector<Vertex> vertices;
	vector<unsigned int> indices;
	vector<Texture> textures;
	//����
	Mesh(vector<Vertex> vertices, vector<unsigned int> indices, vector<Texture> textures);
	void Draw(Shader shader);

	~Mesh();
private:
	//��Ⱦ����
	unsigned int VAO, VBO, EBO;
	//����
	void setupMesh();
};

