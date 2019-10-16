#version 330 core
out vec4 FragColor;

struct Material 
{
	sampler2D specular;
	sampler2D diffuse;
	float shininess;
};

struct Light
{
	vec3 position;
	vec3 direction;
	
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	
	float constant;
	float linear;
	float quadratic;
	float cutOff;
	float outerCutOff;
};

//定向光
struct DirLight 
{
	vec3 direction;
	
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

//点光源
struct PointLight
{
	vec3 position;
	
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	
	float constant;
	float linear;
	float quadratic;
};
#define NR_POINT_LIGHTS 4

//聚光
struct SpotLight
{
	vec3 position;
	vec3 direction;
	
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	
	float constant;
	float linear;
	float quadratic;
	float cutOff;
	float outerCutOff;
};

uniform DirLight dirLight;
uniform PointLight pointLights[NR_POINT_LIGHTS];
uniform SpotLight spotLight;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;

uniform Material material;
//uniform Light light;
uniform vec3 viewPos;

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
void main()
{
/*	vec3 lightDir = normalize(light.position - FragPos);
	float theta = dot(lightDir, normalize(-light.direction));
	float epsilon = light.cutOff - light.outerCutOff;
	float instensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

	//计算环境光照
	vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
	
	//计算漫反射
	vec3 norm = normalize(Normal);					//法向量
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
	
	//计算镜面反射
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir),0.0), material.shininess);
	vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb;
	
	// emission
	//vec3 emission = texture(material.emission, TexCoords).rgb;
	
	diffuse *= instensity;
	specular *= instensity;
	
	//衰减
	float distance = length(light.position - FragPos);
	float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
	
	ambient  *= attenuation; 
	diffuse  *= attenuation;
	specular *= attenuation;
*/
	//属性
	vec3 norm = normalize(Normal);
	vec3 viewDir = normalize(viewPos - FragPos);
	
	//定向光
	vec3 result = CalcDirLight(dirLight, norm, viewDir);
	//点光源
	for(int i =0; i < NR_POINT_LIGHTS ; i++)
		result += CalcPointLight(pointLights[i], norm, FragPos, viewDir);
	//聚光
	result += CalcSpotLight(spotLight, norm, FragPos, viewDir);

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
	vec3 ambient 	= light.ambient  * vec3(texture(material.diffuse, TexCoords));
	vec3 diffuse	= light.diffuse  * diff * vec3(texture(material.diffuse, TexCoords));
	vec3 specular 	= light.specular * spec * vec3(texture(material.specular, TexCoords));
	
	return (ambient + diffuse + specular);
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
	vec3 lightDir = normalize(light.position - FragPos);

	//计算环境光照
	vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
	
	//计算漫反射
	vec3 norm = normalize(normal);					//法向量
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
	
	//计算镜面反射
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir),0.0), material.shininess);
	vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb;

	//衰减
	float distance = length(light.position - FragPos);
	float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
	
	ambient  *= attenuation; 
	diffuse  *= attenuation;
	specular *= attenuation;

	return (ambient + diffuse + specular);
}

vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
	vec3 lightDir = normalize(light.position - FragPos);
	float theta = dot(lightDir, normalize(-light.direction));
	float epsilon = light.cutOff - light.outerCutOff;
	float instensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

	//计算环境光照
	vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
	
	//计算漫反射
	vec3 norm = normalize(normal);					//法向量
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
	
	//计算镜面反射
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir),0.0), material.shininess);
	vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb;
	
	ambient  *= instensity; 
	diffuse *= instensity;
	specular *= instensity;
	
	//衰减
	float distance = length(light.position - FragPos);
	float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
	
	ambient  *= attenuation; 
	diffuse  *= attenuation;
	specular *= attenuation;

	return (ambient + diffuse + specular);
}