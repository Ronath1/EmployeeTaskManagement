import apiClient from "./apiClient";

export async function getEmployees(params = {}) {
  const response = await apiClient.get("/Employees", {
    params,
  });

  return response.data;
}

export async function createEmployee(employee) {
  const response = await apiClient.post("/Employees", employee);

  return response.data;
}

export async function updateEmployee(id, employee) {
  await apiClient.put(`/Employees/${id}`, employee);
}

export async function deleteEmployee(id) {
  await apiClient.delete(`/Employees/${id}`);
}