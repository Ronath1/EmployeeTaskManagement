import { useEffect, useState } from "react";
import {
  createProject,
  deleteProject,
  getProjects,
  updateProject,
} from "../api/projectsApi";
import { getEmployees } from "../api/employeesApi";
import { getUser } from "../utils/authStorage";

function ProjectsPage() {
  const user = getUser();
  const canManage = user?.role === "Admin" || user?.role === "Manager";

  const [projects, setProjects] = useState([]);
  const [employees, setEmployees] = useState([]);
  const [search, setSearch] = useState("");
  const [status, setStatus] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // Form & Edit state
  const [editingId, setEditingId] = useState(null);
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [projectStatus, setProjectStatus] = useState("Planning");
  const [managerId, setManagerId] = useState("");
  const [submitting, setSubmitting] = useState(false);

  async function loadProjects() {
    try {
      setLoading(true);
      setError("");

      const data = await getProjects({
        search,
        status,
      });

      setProjects(data);
    } catch (err) {
      setError("Failed to load projects.");
    } finally {
      setLoading(false);
    }
  }

  async function loadEmployees() {
    try {
      const data = await getEmployees({ pageSize: 100 });
      setEmployees(data.items || []);
    } catch (err) {
      setEmployees([]);
    }
  }

  useEffect(() => {
    loadProjects();
    if (canManage) {
      loadEmployees();
    }
  }, []);

  function handleFilterSubmit(event) {
    event.preventDefault();
    loadProjects();
  }

  function resetForm() {
    setEditingId(null);
    setName("");
    setDescription("");
    setStartDate("");
    setEndDate("");
    setProjectStatus("Planning");
    setManagerId("");
  }

  function handleStartEdit(project) {
    setEditingId(project.id);
    setName(project.name);
    setDescription(project.description || "");
    setStartDate(project.startDate ? project.startDate.substring(0, 10) : "");
    setEndDate(project.endDate ? project.endDate.substring(0, 10) : "");
    setProjectStatus(project.status || "Planning");
    setManagerId(project.managerId || "");
    window.scrollTo({ top: 0, behavior: "smooth" });
  }

  async function handleSubmit(event) {
    event.preventDefault();

    try {
      setSubmitting(true);
      setError("");

      const payload = {
        name,
        description: description || null,
        startDate: startDate
          ? new Date(startDate).toISOString()
          : new Date().toISOString(),
        endDate: endDate ? new Date(endDate).toISOString() : null,
        status: projectStatus,
        managerId: managerId ? parseInt(managerId, 10) : null,
      };

      if (editingId) {
        await updateProject(editingId, payload);
      } else {
        await createProject(payload);
      }

      resetForm();
      await loadProjects();
    } catch (err) {
      const backendMessage =
        err.response?.data?.message ||
        err.response?.data ||
        "Failed to save project.";
      setError(
        typeof backendMessage === "string"
          ? backendMessage
          : "Failed to save project."
      );
    } finally {
      setSubmitting(false);
    }
  }

  async function handleDelete(id, projectName) {
    const confirmed = window.confirm(
      `Are you sure you want to delete project "${projectName}"?`
    );
    if (!confirmed) return;

    try {
      setError("");
      await deleteProject(id);
      if (editingId === id) {
        resetForm();
      }
      await loadProjects();
    } catch (err) {
      setError("Failed to delete project.");
    }
  }

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Projects</h1>
          <p>View projects, managers, status, and task counts.</p>
        </div>
      </div>

      {canManage && (
        <form className="form-panel" onSubmit={handleSubmit}>
          <h3>{editingId ? "Edit Project" : "Add New Project"}</h3>
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
              <label>Project Name *</label>
              <input
                type="text"
                required
                value={name}
                onChange={(e) => setName(e.target.value)}
                placeholder="Project name"
              />
            </div>

            <div>
              <label>Status *</label>
              <select
                value={projectStatus}
                onChange={(e) => setProjectStatus(e.target.value)}
                style={{
                  width: "100%",
                  padding: "10px 12px",
                  border: "1px solid #cbd5e1",
                  borderRadius: "6px",
                  background: "white",
                }}
              >
                <option value="Planning">Planning</option>
                <option value="In Progress">In Progress</option>
                <option value="Completed">Completed</option>
                <option value="On Hold">On Hold</option>
              </select>
            </div>

            <div>
              <label>Project Manager</label>
              <select
                value={managerId}
                onChange={(e) => setManagerId(e.target.value)}
                style={{
                  width: "100%",
                  padding: "10px 12px",
                  border: "1px solid #cbd5e1",
                  borderRadius: "6px",
                  background: "white",
                }}
              >
                <option value="">No Manager Assigned</option>
                {employees.map((emp) => (
                  <option key={emp.id} value={emp.id}>
                    {emp.firstName} {emp.lastName} ({emp.position})
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label>Start Date *</label>
              <input
                type="date"
                required
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
              />
            </div>

            <div>
              <label>End Date</label>
              <input
                type="date"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
              />
            </div>

            <div style={{ gridColumn: "1 / -1" }}>
              <label>Description</label>
              <input
                type="text"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                placeholder="Brief project scope or description"
              />
            </div>
          </div>

          <div style={{ display: "flex", gap: "10px" }}>
            <button type="submit" disabled={submitting}>
              {submitting
                ? "Saving..."
                : editingId
                ? "Save Changes"
                : "Create Project"}
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
          placeholder="Search projects..."
          value={search}
          onChange={(event) => setSearch(event.target.value)}
        />

        <select
          value={status}
          onChange={(event) => setStatus(event.target.value)}
        >
          <option value="">All statuses</option>
          <option value="Planning">Planning</option>
          <option value="In Progress">In Progress</option>
          <option value="Completed">Completed</option>
          <option value="On Hold">On Hold</option>
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
          <p style={{ padding: "16px" }}>Loading projects...</p>
        ) : projects.length === 0 ? (
          <p style={{ padding: "16px", color: "#64748b" }}>No projects found.</p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Name</th>
                <th>Status</th>
                <th>Manager</th>
                <th>Tasks</th>
                <th>Start Date</th>
                <th>End Date</th>
                {canManage && <th>Actions</th>}
              </tr>
            </thead>
            <tbody>
              {projects.map((project) => (
                <tr key={project.id}>
                  <td>
                    <strong>{project.name}</strong>
                    {project.description && (
                      <div style={{ fontSize: "12px", color: "#64748b" }}>
                        {project.description}
                      </div>
                    )}
                  </td>
                  <td>{project.status}</td>
                  <td>{project.managerName || "Not assigned"}</td>
                  <td>{project.taskCount}</td>
                  <td>{new Date(project.startDate).toLocaleDateString()}</td>
                  <td>
                    {project.endDate
                      ? new Date(project.endDate).toLocaleDateString()
                      : "-"}
                  </td>
                  {canManage && (
                    <td>
                      <div style={{ display: "flex", gap: "6px" }}>
                        <button
                          type="button"
                          onClick={() => handleStartEdit(project)}
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
                            handleDelete(project.id, project.name)
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

export default ProjectsPage;