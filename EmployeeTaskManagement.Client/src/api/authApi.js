import apiClient from "./apiClient";

export async function login(email, password) {
  const response = await apiClient.post("/Auth/login", {
    email,
    password,
  });

  return response.data;
}

export async function getCurrentUser() {
  const response = await apiClient.get("/Auth/me");

  return response.data;
}