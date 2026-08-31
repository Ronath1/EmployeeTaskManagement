import apiClient from "./apiClient";

export async function getWorkTasks(params = {}) {
  const response = await apiClient.get("/WorkTasks", {
    params,
  });

  return response.data;
}

export async function createWorkTask(task) {
  const response = await apiClient.post("/WorkTasks", task);

  return response.data;
}

export async function updateWorkTask(id, task) {
  await apiClient.put(`/WorkTasks/${id}`, task);
}

export async function deleteWorkTask(id) {
  await apiClient.delete(`/WorkTasks/${id}`);
}