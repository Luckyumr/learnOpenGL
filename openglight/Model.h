#pragma once

#include <glad/glad.h>
#include "glm/glm.hpp"
#include "glm/gtc/matrix_transform.hpp"
#include "stb_image.h"

#include <assimp/Importer.hpp>
#include <assimp/scene.h>
#include <assimp/postprocess.h>

#include "Shader.h"
#include "Mesh.h"

#include <iostream>
#include <string>
#include <vector>
#include <fstream>
#include <sstream>

using namespace std;

class Model
{
public:
	//����
	Model(char * path);
	~Model();

	void Draw(Shader Shader);

private:
	//ģ������
	vector<Mesh> meshes;
	string directory;
	vector<Texture> textures_loaded;
	//����
	void LoadModel(string path);
	void processNode(aiNode *node, const aiScene *scene);
	Mesh processMesh(aiMesh * mesh, const aiScene *scene);

	vector<Texture> loadMaterialTextures(aiMaterial *mat, aiTextureType type, string typeName);
	unsigned int Model::TextureFromFile(const char* path, string &directory, bool gamma = false);
};

