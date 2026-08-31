const TOKEN_KEY = "employee_task_management_token";
const USER_KEY = "employee_task_management_user";

export function saveAuth(token, user) {
  localStorage.setItem(TOKEN_KEY, token);
  localStorage.setItem(USER_KEY, JSON.stringify(user));
}

export function getToken() {
  return localStorage.getItem(TOKEN_KEY);
}

export function getUser() {
  const userJson = localStorage.getItem(USER_KEY);

  if (!userJson) {
    return null;
  }

  return JSON.parse(userJson);
}

export function clearAuth() {
  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(USER_KEY);
}

export function isAuthenticated() {
  return Boolean(getToken());
}