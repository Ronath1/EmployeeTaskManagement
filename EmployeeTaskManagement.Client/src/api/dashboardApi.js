import { getDepartments } from "./departmentsApi";
import { getEmployees } from "./employeesApi";
import { getProjects } from "./projectsApi";
import { getWorkTasks } from "./workTasksApi";

export async function getDashboardSummary() {
  const [employeesResult, departments, projects, tasks] = await Promise.all([
    getEmployees({ pageNumber: 1, pageSize: 1 }),
    getDepartments(),
    getProjects(),
    getWorkTasks(),
  ]);

  return {
    employeeCount: employeesResult.totalCount,
    departmentCount: departments.length,
    projectCount: projects.length,
    taskCount: tasks.length,
  };
}