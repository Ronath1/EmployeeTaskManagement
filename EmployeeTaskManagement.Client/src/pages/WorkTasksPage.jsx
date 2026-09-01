import { useEffect, useState } from "react";
import {
  createWorkTask,
  deleteWorkTask,
  getWorkTasks,
  updateWorkTask,
} from "../api/workTasksApi";
import { getEmployees } from "../api/employeesApi";
import { getProjects } from "../api/projectsApi";
import { getUser } from "../utils/authStorage";

function WorkTasksPage() {
  const user = getUser();
  const canManage = user?.role === "Admin" || user?.role === "Manager";

  const [tasks, setTasks] = useState([]);
  const [employees, setEmployees] = useState([]);
  const [projects, setProjects] = useState([]);

  // Filters
  const [search, setSearch] = useState("");
  const [statusFilter, setStatusFilter] = useState("");
  const [priorityFilter, setPriorityFilter] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // Form & Edit state
  const [editingId, setEditingId] = useState(null);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [taskStatus, setTaskStatus] = useState("To Do");
  const [taskPriority, setTaskPriority] = useState("Medium");
  const [dueDate, setDueDate] = useState("");
  const [employeeId, setEmployeeId] = useState("");
  const [projectId, setProjectId] = useState("");
  const [submitting, setSubmitting] = useState(false);

  async function loadTasks() {
    try {
      setLoading(true);
      setError("");

      const data = await getWorkTasks({
        search,
        status: statusFilter,
        priority: priorityFilter,
      });

      setTasks(data);
    } catch (err) {
      setError("Failed to load tasks.");
    } finally {
      setLoading(false);
    }
  }

  async function loadDropdowns() {
    try {
      const [empData, projData] = await Promise.all([
        getEmployees({ pageSize: 100 }),
        getProjects(),
      ]);
      setEmployees(empData.items || []);
      setProjects(projData || []);
    } catch (err) {
      // Non-blocking for dropdowns
    }
  }

  useEffect(() => {
    loadTasks();
    if (canManage) {
      loadDropdowns();
    }
  }, []);

  function handleFilterSubmit(event) {
    event.preventDefault();
    loadTasks();
  }

  function resetForm() {
    setEditingId(null);
    setTitle("");
    setDescription("");
    setTaskStatus("To Do");
    setTaskPriority("Medium");
    setDueDate("");
    setEmployeeId("");
    setProjectId("");
  }

  function handleStartEdit(task) {
    setEditingId(task.id);
    setTitle(task.title);
    setDescription(task.description || "");
    setTaskStatus(task.status || "To Do");
    setTaskPriority(task.priority || "Medium");
    setDueDate(task.dueDate ? task.dueDate.substring(0, 10) : "");
    setEmployeeId(task.employeeId || "");
    setProjectId(task.projectId || "");
    window.scrollTo({ top: 0, behavior: "smooth" });
  }

  async function handleSubmit(event) {
    event.preventDefault();

    try {
      setSubmitting(true);
      setError("");

      const payload = {
        title,
        description: description || null,
        status: taskStatus,
        priority: taskPriority,
        dueDate: dueDate ? new Date(dueDate).toISOString() : null,
        employeeId: employeeId ? parseInt(employeeId, 10) : null,
        projectId: projectId ? parseInt(projectId, 10) : null,
      };

      if (editingId) {
        await updateWorkTask(editingId, payload);
      } else {
        await createWorkTask(payload);
      }

      resetForm();
      await loadTasks();
    } catch (err) {
      const backendMessage =
        err.response?.data?.message ||
        err.response?.data ||
        "Failed to save task.";
      setError(
        typeof backendMessage === "string"
          ? backendMessage
          : "Failed to save task."
      );
    } finally {
      setSubmitting(false);
    }
  }

  async function handleDelete(id, taskTitle) {
    const confirmed = window.confirm(
      `Are you sure you want to delete task "${taskTitle}"?`
    );
    if (!confirmed) return;

    try {
      setError("");
      await deleteWorkTask(id);
      if (editingId === id) {
        resetForm();
      }
      await loadTasks();
    } catch (err) {
      const backendMessage =
        err.response?.data?.message ||
        err.response?.data ||
        "Failed to delete task.";
      setError(
        typeof backendMessage === "string"
          ? backendMessage
          : "Failed to delete task."
      );
    }
  }

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Tasks</h1>
          <p>View task assignments, project links, status, and priority.</p>
        </div>
      </div>

      {canManage && (
        <form className="form-panel" onSubmit={handleSubmit}>
          <h3>{editingId ? "Edit Task" : "Add New Task"}</h3>
          <div
            className="form-row"
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(auto-fit, minmax(180px, 1fr))",
              gap: "12px",
              marginBottom: "12px",
            }}
          >
            <div>
              <label>Task Title *</label>
              <input
                type="text"
                required
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                placeholder="Title"
              />
            </div>

            <div>
              <label>Status *</label>
              <select
                value={taskStatus}
                onChange={(e) => setTaskStatus(e.target.value)}
                style={{
                  width: "100%",
                  padding: "10px 12px",
                  border: "1px solid #cbd5e1",
                  borderRadius: "6px",
                  background: "white",
                }}
              >
                <option value="To Do">To Do</option>
                <option value="In Progress">In Progress</option>
                <option value="Done">Done</option>
                <option value="Blocked">Blocked</option>
              </select>
            </div>

            <div>
              <label>Priority *</label>
              <select
                value={taskPriority}
                onChange={(e) => setTaskPriority(e.target.value)}
                style={{
                  width: "100%",
                  padding: "10px 12px",
                  border: "1px solid #cbd5e1",
                  borderRadius: "6px",
                  background: "white",
                }}
              >
                <option value="Low">Low</option>
                <option value="Medium">Medium</option>
                <option value="High">High</option>
                <option value="Critical">Critical</option>
              </select>
            </div>

            <div>
              <label>Assignee</label>
              <select
                value={employeeId}
                onChange={(e) => setEmployeeId(e.target.value)}
                style={{
                  width: "100%",
                  padding: "10px 12px",
                  border: "1px solid #cbd5e1",
                  borderRadius: "6px",
                  background: "white",
                }}
              >
                <option value="">Unassigned</option>
                {employees.map((emp) => (
                  <option key={emp.id} value={emp.id}>
                    {emp.firstName} {emp.lastName}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label>Project</label>
              <select
                value={projectId}
                onChange={(e) => setProjectId(e.target.value)}
                style={{
                  width: "100%",
                  padding: "10px 12px",
                  border: "1px solid #cbd5e1",
                  borderRadius: "6px",
                  background: "white",
                }}
              >
                <option value="">No Project</option>
                {projects.map((proj) => (
                  <option key={proj.id} value={proj.id}>
                    {proj.name}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label>Due Date</label>
              <input
                type="date"
                value={dueDate}
                onChange={(e) => setDueDate(e.target.value)}
              />
            </div>

            <div style={{ gridColumn: "1 / -1" }}>
              <label>Description</label>
              <input
                type="text"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                placeholder="Task description or notes"
              />
            </div>
          </div>

          <div style={{ display: "flex", gap: "10px" }}>
            <button type="submit" disabled={submitting}>
              {submitting
                ? "Saving..."
                : editingId
                ? "Save Changes"
                : "Create Task"}
            </button>

            {editingId && (
              <button
                type="button"
                onClick={resetForm}
                style={{
                  background: "#e2e8f0",
                  color: "#334155",
                  border: "none",
                  borderRadius: "6px",
                  padding: "11px 16px",
                  fontWeight: "700",
                  cursor: "pointer",
                }}
              >
                Cancel
              </button>
            )}
          </div>
        </form>
      )}

      <form className="toolbar" onSubmit={handleFilterSubmit}>
        <input
          type="search"
          placeholder="Search tasks..."
          value={search}
          onChange={(event) => setSearch(event.target.value)}
        />

        <select
          value={statusFilter}
          onChange={(event) => setStatusFilter(event.target.value)}
        >
          <option value="">All statuses</option>
          <option value="To Do">To Do</option>
          <option value="In Progress">In Progress</option>
          <option value="Done">Done</option>
          <option value="Blocked">Blocked</option>
        </select>

        <select
          value={priorityFilter}
          onChange={(event) => setPriorityFilter(event.target.value)}
        >
          <option value="">All priorities</option>
          <option value="Low">Low</option>
          <option value="Medium">Medium</option>
          <option value="High">High</option>
          <option value="Critical">Critical</option>
        </select>

        <button type="submit">Filter</button>
      </form>

      {error && (
        <div className="error-message" style={{ marginBottom: "16px" }}>
          {error}
        </div>
      )}

      <div className="table-panel">
        {loading ? (
          <p style={{ padding: "16px" }}>Loading tasks...</p>
        ) : tasks.length === 0 ? (
          <p style={{ padding: "16px", color: "#64748b" }}>No tasks found.</p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Title</th>
                <th>Status</th>
                <th>Priority</th>
                <th>Employee</th>
                <th>Project</th>
                <th>Due Date</th>
                {canManage && <th>Actions</th>}
              </tr>
            </thead>
            <tbody>
              {tasks.map((task) => (
                <tr key={task.id}>
                  <td>
                    <strong>{task.title}</strong>
                    {task.description && (
                      <div style={{ fontSize: "12px", color: "#64748b" }}>
                        {task.description}
                      </div>
                    )}
                  </td>
                  <td>{task.status}</td>
                  <td>{task.priority}</td>
                  <td>{task.employeeName || "Not assigned"}</td>
                  <td>{task.projectName || "Not assigned"}</td>
                  <td>
                    {task.dueDate
                      ? new Date(task.dueDate).toLocaleDateString()
                      : "-"}
                  </td>
                  {canManage && (
                    <td>
                      <div style={{ display: "flex", gap: "6px" }}>
                        <button
                          type="button"
                          onClick={() => handleStartEdit(task)}
                          style={{
                            padding: "6px 10px",
                            fontSize: "13px",
                            background: "#f1f5f9",
                            color: "#0f172a",
                            border: "1px solid #cbd5e1",
                            borderRadius: "4px",
                            cursor: "pointer",
                          }}
                        >
                          Edit
                        </button>
                        <button
                          type="button"
                          onClick={() =>
                            handleDelete(task.id, task.title)
                          }
                          style={{
                            padding: "6px 10px",
                            fontSize: "13px",
                            background: "#fee2e2",
                            color: "#991b1b",
                            border: "1px solid #fecaca",
                            borderRadius: "4px",
                            cursor: "pointer",
                          }}
                        >
                          Delete
                        </button>
                      </div>
                    </td>
                  )}
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}

export default WorkTasksPage;