import { getDepartments } from "./departmentsApi";
import { getEmployees } from "./employeesApi";
import { getProjects } from "./projectsApi";
import { getWorkTasks } from "./workTasksApi";
import { getUser } from "../utils/authStorage";

export async function getDashboardSummary() {
  const user = getUser();
  const isAdmin = user?.role === "Admin";

  // Fetch departments only if user is Admin
  const deptPromise = isAdmin
    ? getDepartments().catch(() => [])
    : Promise.resolve(null);

  const [employeesResult, departments, projects, tasks] = await Promise.all([
    getEmployees({ pageNumber: 1, pageSize: 1 }).catch(() => ({ totalCount: 0 })),
    deptPromise,
    getProjects().catch(() => []),
    getWorkTasks().catch(() => []),
  ]);

  return {
    employeeCount: employeesResult.totalCount || 0,
    departmentCount: departments !== null ? departments.length : null,
    projectCount: projects.length,
    taskCount: tasks.length,
    inProgressTasks: tasks.filter((t) => t.status === "In Progress").length,
    completedTasks: tasks.filter((t) => t.status === "Done").length,
    activeProjects: projects.filter((p) => p.status === "In Progress").length,
  };
}