import axios from "axios";
import { getToken } from "../utils/authStorage";

const apiClient = axios.create({
  baseURL: "https://localhost:7138/api",
});

apiClient.interceptors.request.use((config) => {
  const token = getToken();

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

export default apiClient;