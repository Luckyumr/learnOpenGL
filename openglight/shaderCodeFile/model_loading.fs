#version 330 core
out vec4 FragColor;

struct Material 
{
	sampler2D specular;
	sampler2D diffuse;
	float shininess;
};

//定向光
struct DirLight 
{
	vec3 direction;
	
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

in vec2 TexCoords;
in vec3 FragPos;
in vec3 Normal;

uniform Material material;
uniform vec3 viewPos;
uniform sampler2D texture_diffuse1;
uniform sampler2D texture_specular1;
uniform DirLight dirLight;
vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);

void main()
{    
	vec3 normal = normalize(Normal);
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 result = CalcDirLight(dirLight, normal, viewDir);
    FragColor = vec4(result, 1.0);
}

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{

	vec3 lightDir = normalize(-light.direction);
	//漫反射着色
	float diff = max(dot(normal, lightDir), 0.0);
	//镜面光着色
	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	//合并
	vec3 ambient 	= light.ambient  * vec3(texture(texture_diffuse1, TexCoords));
	vec3 diffuse	= light.diffuse  * diff * vec3(texture(texture_diffuse1, TexCoords));
	vec3 specular 	= light.specular * spec * vec3(texture(texture_specular1, TexCoords));
	
	return (ambient + diffuse + specular);
}

