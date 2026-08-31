import apiClient from "./apiClient";

export async function getDepartments() {
  const response = await apiClient.get("/Departments");

  return response.data;
}

export async function createDepartment(department) {
  const response = await apiClient.post("/Departments", department);

  return response.data;
}

export async function updateDepartment(id, department) {
  await apiClient.put(`/Departments/${id}`, department);
}

export async function deleteDepartment(id) {
  await apiClient.delete(`/Departments/${id}`);
}