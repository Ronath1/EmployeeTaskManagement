import apiClient from "./apiClient";

export async function getProjects(params = {}) {
  const response = await apiClient.get("/Projects", {
    params,
  });

  return response.data;
}

export async function createProject(project) {
  const response = await apiClient.post("/Projects", project);

  return response.data;
}

export async function updateProject(id, project) {
  await apiClient.put(`/Projects/${id}`, project);
}

export async function deleteProject(id) {
  await apiClient.delete(`/Projects/${id}`);
}